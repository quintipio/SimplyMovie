using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10.ViewModel;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Interface;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;
using SimplyPasswordWin10Shared.Utils;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue pour afficher les données provenant d'internet
    /// </summary>
    public sealed partial class ConsulterDataInternetView : IView<ConsulterDataInternetViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public ConsulterDataInternetViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConsulterDataInternetView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            GridData.Visibility =  Visibility.Collapsed;
            WaitRing.IsActive = true;
            GridWait.Visibility = Visibility.Visible;
            base.OnNavigatedTo(e);
            GridAjouterFilm.Visibility = Visibility.Collapsed;
            GridAjouterSerie.Visibility = Visibility.Collapsed;
            ViewModel = new ConsulterDataInternetViewModel();
            await ViewModel.Initialization;

            var data = e.Parameter as SearchDataInternet;

            if (data != null)
            {
                try
                {
                    await ViewModel.SearchData(data);
                }
                catch (Exception)
                {
                    await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
                    App.AppShell.NavigateFrame(typeof(AcceuilView));
                }
            }
            GridAjouterFilm.Visibility = Visibility.Visible;
            GridAjouterSerie.Visibility = Visibility.Visible;
            GridWait.Visibility = Visibility.Collapsed;
            WaitRing.IsActive = false;
            GridData.Visibility = Visibility.Visible;
        }

        private void OpenCast_Click(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as CastJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView),new SearchDataInternet {Id = el.id, TypeData = TypeFilmEnum.PERSONNE});
            }
        }

        private void OpenCrew_Click(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as CrewJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = el.id, TypeData = TypeFilmEnum.PERSONNE });
            }
        }

        private void OpenSimilarMovie_Click(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as ResultSearchMovieJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = el.id, TypeData = TypeFilmEnum.FILM });
            }
        }

        private void OpenSimilarSerie_Click(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as ResultSearchTvJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = el.id, TypeData = TypeFilmEnum.SERIE });
            }
        }

        private void OpenMovie_PersonCast(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as CastPersonJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = el.id, TypeData = SearchDataInternet.GetTypeData(el.media_type) });
            }
        }

        private void OpenMovie_PersonCrew(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as CrewPersonJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = el.id, TypeData = SearchDataInternet.GetTypeData(el.media_type) });
            }
        }

        private void AjouterFilm_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(AjouterFilmView),new SearchDataInternet { Id = ViewModel.DataToDisplay.Film.id, TypeData = TypeFilmEnum.FILM});
        }

        private void AjouterSerie_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(AjouterFilmView), new SearchDataInternet { Id = ViewModel.DataToDisplay.Tv.id, TypeData = TypeFilmEnum.SERIE });
        }

        private void ConsulterBiblio_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.FilmBibliotheque != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterFilmView), ViewModel.FilmBibliotheque);
            }
        }

        private async void ChoisirSaison_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.GetSeason((int)((Button)sender).Tag);
            }
            catch (Exception)
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
            }
        }

        private void ChoisirEpisode_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Episode = (EpisodeJson) ((Button) sender).Tag;
        }

        private void OpenCollectionMovie_Click(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as PartJson;
            if (el != null)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = el.id, TypeData = TypeFilmEnum.FILM });
            }
        }
    }
}
