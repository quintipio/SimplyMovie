using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10.ViewModel;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Interface;
using SimplyMovieWin10Shared.Model;
using SimplyPasswordWin10Shared.Utils;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue pour afficher un film
    /// </summary>
    public sealed partial class ConsulterFilmView : IView<ConsulterFilmViewModel>
    {
        /// <summary>
        /// ViewModel 
        /// </summary>
        public ConsulterFilmViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConsulterFilmView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = new ConsulterFilmViewModel();
            await ViewModel.Initialization;

            var film = e.Parameter as Film;
            if (film != null)
            {
                await ViewModel.ChargerFilm(film);
                SaisonStack.Visibility = (ViewModel.FilmAffiche.Type == (int) TypeFilmEnum.SERIE)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                if (ViewModel.FilmAffiche.Posseder != null || ViewModel.FilmAffiche.Souhait != null)
                {
                    HaveImage.Visibility = Visibility.Visible;
                    if (ViewModel.FilmAffiche.Souhait != null && ViewModel.FilmAffiche.Souhait.Value)
                    {
                        ButtonAcheter.Visibility = Visibility.Visible;
                        HaveText.Text = ResourceLoader.GetForCurrentView().GetString("SouhaitAvoirText");
                    }

                    if (ViewModel.FilmAffiche.Posseder != null && ViewModel.FilmAffiche.Posseder.Value)
                    {
                        HaveText.Text = ResourceLoader.GetForCurrentView().GetString("PossederText");
                        switch (ViewModel.FilmAffiche.TypeSupport)
                        {
                            case (int)TypeSupportEnum.DVD:
                                HaveText.Text += " ( " + ResourceLoader.GetForCurrentView().GetString("DvdText") + " )";
                                break;

                            case (int)TypeSupportEnum.BLURAY:
                                HaveText.Text += " ( "+ResourceLoader.GetForCurrentView().GetString("BluRayText")+" )";
                                break;

                            case (int)TypeSupportEnum.FICHIER:
                                HaveText.Text += " ( " + ViewModel.FilmAffiche.Lien + " )";
                                break;

                            case (int)TypeSupportEnum.WEB:
                                HaveText.Visibility = Visibility.Collapsed;
                                LinkWeb.Visibility = Visibility.Visible;
                                break;

                            case (int)TypeSupportEnum.AUTRE:
                                HaveText.Text += " ( " +ResourceLoader.GetForCurrentView().GetString("AutreText") + ViewModel.FilmAffiche.Lien + " )";
                                break;

                            default:
                                HaveText.Text = ResourceLoader.GetForCurrentView().GetString("PossederText");
                                break;
                        }
                    }
                }

                if (ViewModel.FilmAffiche.SouhaitVoir != null || ViewModel.FilmAffiche.Voir != null)
                {
                    SeeImage.Visibility = Visibility.Visible;
                    if (ViewModel.FilmAffiche.Voir != null && ViewModel.FilmAffiche.Voir.Value)
                    {
                        SeeText.Text = ResourceLoader.GetForCurrentView().GetString("VuText");
                    }

                    if (ViewModel.FilmAffiche.SouhaitVoir != null && ViewModel.FilmAffiche.SouhaitVoir.Value)
                    {
                        ButtonVu.Visibility = Visibility.Visible;
                        SeeText.Text = ResourceLoader.GetForCurrentView().GetString("SouhaitVoirText");
                    }
                }
                await ViewModel.ChargerFilmInternet();
            }
        }
        

        private void OpenGenre_Click(object sender, RoutedEventArgs e)
        {
            var genre = (Genre)((HyperlinkButton)sender).Tag;
            App.AppShell.NavigateFrame(typeof(BibliothequeView), new SearchFilter { RechercheGenre = genre, TypeA = FilterBibliothequeEnum.GENRE });
        }

        private void OpenPersonne_Click(object sender, RoutedEventArgs e)
        {
            var personne = (Personne)((HyperlinkButton)sender).Tag;
            App.AppShell.NavigateFrame(typeof(BibliothequeView), new SearchFilter { RecherchePersonne = personne, TypeA = FilterBibliothequeEnum.PERSONNE });
        }

        private async void ChoixFilmSimilaire(object sender, ItemClickEventArgs e)
        {
            var film = e.ClickedItem as Film;
            if (film != null)
            {
                var filmTrouve = await ViewModel.IsFilmPresent(film,film.Type);
                if (filmTrouve != null)
                {
                    App.AppShell.NavigateFrame(typeof(ConsulterFilmView), filmTrouve);
                }
                else
                {
                    App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = film.IdInternet, TypeData = (TypeFilmEnum)film.Type });
                }
            }
        }

        private async void OuvrirLien(object sender, RoutedEventArgs e)
        {
            try
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(ViewModel.FilmAffiche.Lien));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void OuvrirFicheInternet(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView),new SearchDataInternet {Id = ViewModel.FilmAffiche.IdInternet, TypeData = (TypeFilmEnum)ViewModel.FilmAffiche.Type});
        }

        private async void Vu_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.PasserFilmVu();
            SeeText.Text = ResourceLoader.GetForCurrentView().GetString("VuText");
            ButtonVu.Visibility = Visibility.Collapsed;
        }

        private async void Acheter_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.PasserFilmAcheter();
            HaveText.Text = ResourceLoader.GetForCurrentView().GetString("PossederText");
            ButtonAcheter.Visibility = Visibility.Collapsed;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(AjouterFilmView),ViewModel.FilmAffiche);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("confirmSupFilm"), ResourceLoader.GetForCurrentView().GetString("avert"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await ViewModel.SupprimerFilm();
                App.AppShell.NavigateFrame(typeof(BibliothequeView));
            }
        }

        private void ChoixFilmCollection(object sender, ItemClickEventArgs e)
        {
            var film = e.ClickedItem as Film;
            if (film != null)
            {
                if (film.Id > 0)
                {
                    App.AppShell.NavigateFrame(typeof(ConsulterFilmView), film);
                }
                else
                {
                    if (film.IdInternet > 0)
                    {
                        App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = film.IdInternet, TypeData = (TypeFilmEnum)film.Type });
                    }
                }
            }
        }
    }
}
