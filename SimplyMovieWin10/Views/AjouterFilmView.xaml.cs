using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
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
    /// Vue pour ajouter un film
    /// </summary>
    public sealed partial class AjouterFilmView : IView<AjouterFilmViewModel>
    {
        public AjouterFilmViewModel ViewModel { get; set; }

        private bool _isModif;

        public AjouterFilmView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new AjouterFilmViewModel();
            await ViewModel.Initialization;
            FilmRadioButton.IsChecked = true;
            _isModif = false;

            TitreText.Text = ResourceLoader.GetForCurrentView().GetString("AjouterFilmText");
            if (e.Parameter is Film)
            {
                TitreText.Text = ResourceLoader.GetForCurrentView().GetString("ModifierFilmText");
                await ChargerFilm(e.Parameter as Film);
            }
            if (e.Parameter is SearchDataInternet)
            {
                await ChargerFilm(e.Parameter as SearchDataInternet);
            }
        }

        private async Task ChargerFilm(SearchDataInternet data)
        {
            if (data != null)
            {
               await ViewModel.ChargerFilm(data);

                switch (data.TypeData)
                {
                    case TypeFilmEnum.FILM:
                        FilmRadioButton.IsChecked = ViewModel.Film.Type == (int)TypeFilmEnum.FILM;
                        break;

                    case TypeFilmEnum.SERIE:
                        SerieRadioButton.IsChecked = ViewModel.Film.Type == (int)TypeFilmEnum.SERIE;
                        break;

                }
            }
        }

        private async Task ChargerFilm(Film film)
        {
            if (film != null)
            {
                _isModif = true;
                await ViewModel.ChargerFilm(film);

                VuCheckBox.IsChecked = ViewModel.Film.Voir ?? false;
                SouhaitVoirCheckBox.IsChecked = ViewModel.Film.SouhaitVoir ?? false;
                SouhaitRadioButton.IsChecked = ViewModel.Film.Souhait ?? false;
                PossederRadioButton.IsChecked = ViewModel.Film.Posseder ?? false;

                switch (ViewModel.Film.TypeSupport)
                {
                    case (int)TypeSupportEnum.AUTRE:
                        AutreRadio.IsChecked = true;
                        break;

                    case (int)TypeSupportEnum.FICHIER:
                        FichierRadio.IsChecked = true;
                        break;

                    case (int)TypeSupportEnum.WEB:
                        WebRadio.IsChecked = true;
                        break;

                    case (int)TypeSupportEnum.DVD:
                        DvdRadio.IsChecked = true;
                        break;

                    case (int)TypeSupportEnum.BLURAY:
                        BluRayRadio.IsChecked = true;
                        break;
                }

                SerieRadioButton.IsChecked = ViewModel.Film.Type == (int) TypeFilmEnum.SERIE;
                FilmRadioButton.IsChecked = ViewModel.Film.Type == (int)TypeFilmEnum.FILM;
                DocuRadioButton.IsChecked = ViewModel.Film.Type == (int)TypeFilmEnum.DOCUMENTAIRE;
                DessAnnRadioButton.IsChecked = ViewModel.Film.Type == (int)TypeFilmEnum.ANIMATION;
                SpectacleRadioButton.IsChecked = ViewModel.Film.Type == (int)TypeFilmEnum.SPECTACLE;
                LienSupportTextBox.Text = ViewModel.Film.Lien ?? "";
            }
        }

        #region AutoSugggestion des collections

        private void AutoSuggestBoxCollection_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var match = ViewModel.GetSearchCollection(sender.Text);
                sender.ItemsSource = match.ToList();
            }
        }

        private void AutoSuggestBoxCollection_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var collec = args.SelectedItem as Collection;
            if (collec != null)
            {
                ViewModel.SelectedCollection = collec;
            }

        }

        private void AutoSuggestBoxCollection_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                ViewModel.SelectedCollection = args.ChosenSuggestion as Collection;
            }
            else
            {
                var match = ViewModel.GetSearchCollection(args.QueryText);
                if (match.Any())
                {
                    ViewModel.SelectedCollection = match.FirstOrDefault();
                }
            }
        }
        


        #endregion


        #region AutoSuggestion Personnes

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                 var match = ViewModel.GetSearchPersonnes(sender.Text);
                 sender.ItemsSource = match.ToList();
            }
        }


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var personne = args.SelectedItem as Personne;
            if (personne != null)
            {
                SelectPersonne(personne,sender.Name);
            }

        }
        
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
               SelectPersonne(args.ChosenSuggestion as Personne,sender.Name);
            }
            else  
            {
                var match = ViewModel.GetSearchPersonnes(args.QueryText);
                if (match.Any())
                {
                    SelectPersonne(match.FirstOrDefault(),sender.Name);
                }
            }

        }

        private void SelectPersonne(Personne personne,string nameAutoSuggest)
        {
            if (!string.IsNullOrWhiteSpace(personne?.Nom))
            {
                if (string.Equals(nameAutoSuggest, "AutoSuggestActeur"))
                {
                    if (!ViewModel.CheckPersonnePresent(personne,ViewModel.SelectedActeurListe))
                    {
                        personne.Role = TypePersonneEnum.ACTEUR;
                        ViewModel.SelectedActeurListe.Add(personne);
                        ViewModel.SelectedActeurListe = new List<Personne>(ViewModel.SelectedActeurListe);
                        AutoSuggestActeur.Text = "";
                    }
                }

                if (string.Equals(nameAutoSuggest, "AutoSuggestProducteur"))
                {
                    if (!ViewModel.CheckPersonnePresent(personne, ViewModel.SelectedProducteursListe))
                    {
                        personne.Role = TypePersonneEnum.PRODUCTEUR;
                        ViewModel.SelectedProducteursListe.Add(personne);
                        ViewModel.SelectedProducteursListe = new List<Personne>(ViewModel.SelectedProducteursListe);
                        AutoSuggestProducteur.Text = "";
                    }
                }

                if (string.Equals(nameAutoSuggest, "AutoSuggestRealisateur"))
                {
                    if (!ViewModel.CheckPersonnePresent(personne, ViewModel.SelectedRealisateursListe))
                    {
                        personne.Role = TypePersonneEnum.REALISATEUR;
                        ViewModel.SelectedRealisateursListe.Add(personne);
                        ViewModel.SelectedRealisateursListe = new List<Personne>(ViewModel.SelectedRealisateursListe);
                        AutoSuggestRealisateur.Text = "";
                    }
                }
            }
        }


        private void SupprimerPersonne_Click(object sender, RoutedEventArgs e)
        {
            var personne = ((AppBarButton) sender).Tag as Personne;
            if (personne != null)
            {
                switch (personne.Role)
                {
                    case TypePersonneEnum.ACTEUR:
                        ViewModel.SelectedActeurListe.Remove(personne);
                        ViewModel.SelectedActeurListe = new List<Personne>(ViewModel.SelectedActeurListe);
                        break;

                    case TypePersonneEnum.PRODUCTEUR:
                        ViewModel.SelectedProducteursListe.Remove(personne);
                        ViewModel.SelectedProducteursListe = new List<Personne>(ViewModel.SelectedProducteursListe);
                        break;

                    case TypePersonneEnum.REALISATEUR:
                        ViewModel.SelectedRealisateursListe.Remove(personne);
                        ViewModel.SelectedRealisateursListe = new List<Personne>(ViewModel.SelectedRealisateursListe);
                        break;
                }
            }
        }
        
        private void AjouterPersonne_Click(object sender, RoutedEventArgs e)
        {
            if (string.Equals(((AppBarButton)sender).Name, "AjouterActeurButton"))
            {
                SelectPersonne(ViewModel.GetPersonne(AutoSuggestActeur.Text), "AutoSuggestActeur");
            }

            if (string.Equals(((AppBarButton)sender).Name, "AjouterProducteurButton"))
            {
                SelectPersonne(ViewModel.GetPersonne(AutoSuggestProducteur.Text), "AutoSuggestProducteur");
            }

            if (string.Equals(((AppBarButton)sender).Name, "AjouterRealisateurButton"))
            {
                SelectPersonne(ViewModel.GetPersonne(AutoSuggestRealisateur.Text), "AutoSuggestRealisateur");
            }
        }
        #endregion


        #region AutoSuggestGenre


        private void SupprimerGenre_Click(object sender, RoutedEventArgs e)
        {
            var genre = ((AppBarButton) sender).Tag as Genre;
            if (genre != null)
            {
                ViewModel.ListeGenreSelected.Remove(genre);
                ViewModel.ListeGenreSelected = new List<Genre>(ViewModel.ListeGenreSelected);
            }
        }

        private void AjouterGenre_Click(object sender, RoutedEventArgs e)
        {
            SelectedGenre(ViewModel.GetGenre(AutoSuggestGenre.Text));
        }

        private void AutoSuggestBoxGenre_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var match = ViewModel.GetSearchGenre(sender.Text);
                sender.ItemsSource = match.ToList();
            }
        }

        private void AutoSuggestBoxGenre_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                SelectedGenre(args.ChosenSuggestion as Genre);
            }
            else
            {
                var match = ViewModel.GetSearchGenre(args.QueryText);
                if (match.Any())
                {
                    SelectedGenre(match.FirstOrDefault());
                }
            }
        }

        private void AutoSuggestBoxGenre_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var genre = args.SelectedItem as Genre;
            if (genre != null)
            {
                SelectedGenre(genre);
            }
        }

        private void SelectedGenre(Genre genre)
        {
            if (!string.IsNullOrWhiteSpace(genre?.Nom) && !ViewModel.CheckGenrePresent(genre))
            {
                ViewModel.ListeGenreSelected.Add(genre);
                ViewModel.ListeGenreSelected = new List<Genre>(ViewModel.ListeGenreSelected);
                AutoSuggestGenre.Text = "";
            }
        }

        #endregion


        private void Vu_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.Film.Voir = ((CheckBox) sender).IsChecked;
        }
        
        private void SouhaitVoir_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.Film.SouhaitVoir = ((CheckBox)sender).IsChecked;
        }

        private void ChoixPosseder_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;

            if (rb != null)
            {
                var mode = rb.Tag.ToString();
                switch (mode)
                {
                    case "Souhait":
                        ViewModel.Film.Souhait = true;
                        ViewModel.Film.Posseder = false;
                        GridPossession.Visibility = Visibility.Collapsed;
                        break;
                    case "Posseder":
                        ViewModel.Film.Souhait = false;
                        ViewModel.Film.Posseder = true;
                        GridPossession.Visibility = Visibility.Visible;
                        break;

                    default:
                        ViewModel.Film.Souhait = false;
                        ViewModel.Film.Posseder = false;
                        GridPossession.Visibility = Visibility.Collapsed;
                        break;

                }
            }
        }

        private void ChoixMode_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;

            if (rb != null)
            {
                var mode = rb.Tag.ToString();
                switch (mode)
                {
                    case "Film":
                        ViewModel.Film.Type = (int)TypeFilmEnum.FILM;
                        SaisonText.Visibility = Visibility.Collapsed;
                        SaisonTextBox.Visibility = Visibility.Collapsed;
                        break;

                    case "Serie":
                        ViewModel.Film.Type = (int)TypeFilmEnum.SERIE;
                        SaisonText.Visibility = Visibility.Visible;
                        SaisonTextBox.Visibility = Visibility.Visible;
                        break;

                     case "Docu":
                        ViewModel.Film.Type = (int)TypeFilmEnum.DOCUMENTAIRE;
                        SaisonText.Visibility = Visibility.Collapsed;
                        SaisonTextBox.Visibility = Visibility.Collapsed;
                        break;

                    case "DessAnn":
                        ViewModel.Film.Type = (int)TypeFilmEnum.ANIMATION;
                        SaisonText.Visibility = Visibility.Collapsed;
                        SaisonTextBox.Visibility = Visibility.Collapsed;
                        break;

                    case "Spectacle":
                        ViewModel.Film.Type = (int)TypeFilmEnum.SPECTACLE;
                        SaisonText.Visibility = Visibility.Collapsed;
                        SaisonTextBox.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void ChoixSupport_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb != null)
            {
                var mode = rb.Tag.ToString();
                switch (mode)
                {
                    case "DVD":
                        ViewModel.Film.TypeSupport = (int) TypeSupportEnum.DVD;
                        LienSupportTextBox.Visibility = Visibility.Collapsed;
                        ChargerFichierButton.Visibility = Visibility.Collapsed;
                        LienSupportTextBox.IsEnabled = true;
                        break;
                    case "BluRay":
                        ViewModel.Film.TypeSupport = (int)TypeSupportEnum.BLURAY;
                        LienSupportTextBox.Visibility = Visibility.Collapsed;
                        ChargerFichierButton.Visibility = Visibility.Collapsed;
                        LienSupportTextBox.IsEnabled = true;
                        break;
                    case "Fichier":
                        ViewModel.Film.TypeSupport = (int)TypeSupportEnum.FICHIER;
                        LienSupportTextBox.Visibility = Visibility.Visible;
                        ChargerFichierButton.Visibility = Visibility.Visible;
                        LienSupportTextBox.IsEnabled = false;
                        break;
                    case "Web":
                        ViewModel.Film.TypeSupport = (int)TypeSupportEnum.WEB;
                        LienSupportTextBox.Visibility = Visibility.Visible;
                        ChargerFichierButton.Visibility = Visibility.Collapsed;
                        LienSupportTextBox.IsEnabled = true;
                        break;
                    case "Autre":
                        ViewModel.Film.TypeSupport = (int)TypeSupportEnum.AUTRE;
                        LienSupportTextBox.Visibility = Visibility.Visible;
                        ChargerFichierButton.Visibility = Visibility.Collapsed;
                        LienSupportTextBox.IsEnabled = true;
                        break;
                    default:
                        ViewModel.Film.TypeSupport = (int)TypeSupportEnum.AUTRE;
                        LienSupportTextBox.Visibility = Visibility.Visible;
                        ChargerFichierButton.Visibility = Visibility.Collapsed;
                        LienSupportTextBox.IsEnabled = true;
                        break;

                }
                LienSupportTextBox.Text = "";
            }
        }
        private async void ChargerAffiche_Click(object sender, RoutedEventArgs e)
        {
            var fileOpenPicker = new FileOpenPicker
            {
                CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.Downloads,
                FileTypeFilter = { ".jpeg", ".jpg", ".png", ".bmp" },
            };

            var fichier = await fileOpenPicker.PickSingleFileAsync();
            if (fichier != null)
            {
               var erreur = await ViewModel.SaveImage(fichier);
                if (!string.IsNullOrWhiteSpace(erreur))
                {
                    await MessageBox.ShowAsync(erreur);
                }
            }
        }

        private async void Valider_Click(object sender, RoutedEventArgs e)
        {
            ValiderButton.IsEnabled = false;
            ChargerFichierButton.IsEnabled = false;
            var erreur = await ViewModel.Sauvegarder();
            if (!string.IsNullOrWhiteSpace(erreur))
            {
                await MessageBox.ShowAsync(erreur);
            }
            else
            {
                await MessageBox.ShowAsync((_isModif? ResourceLoader.GetForCurrentView().GetString("FilmModif"): ResourceLoader.GetForCurrentView().GetString("FilmAjoute")));
                App.AppShell.NavigateFrame(typeof(BibliothequeView));
            }
            ValiderButton.IsEnabled = true;
            ChargerFichierButton.IsEnabled = true;
        }

        private async void ChargerFichierButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileOpenPicker = new FileOpenPicker
            {
                CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.Downloads,
                FileTypeFilter = { ".avi", ".wmv", ".mpg", ".mp4", ".mkv" },
            };

            var fichier = await fileOpenPicker.PickSingleFileAsync();
            if (fichier != null)
            {
                LienSupportTextBox.Text = fichier.Path;
                ViewModel.Film.Lien = fichier.Path;
            }
        }

        
        private async void SelectFilmDlg(object sender, ItemClickEventArgs e)
        {
            WaitRing.IsActive = true;
            try
            {
                var film = e.ClickedItem as ResultSearchMovieJson;
                await ViewModel.GetFilmFromInternet(film.id);
                DlgChoixFilm.Hide();
            }
            catch (Exception)
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
            }
            WaitRing.IsActive = false;
        }

        private async void SelectSerieDlg(object sender, ItemClickEventArgs e)
        {
            WaitRing.IsActive = true;
            try
            {
                var film = e.ClickedItem as ResultSearchTvJson;
                await ViewModel.GetFilmFromInternet(film.id);
                DlgChoixSerie.Hide();
            }
            catch (Exception)
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
            }
            WaitRing.IsActive = false;
        }
        

        private void LostFocusLien(object sender, RoutedEventArgs e)
        {
            ViewModel.Film.Lien = LienSupportTextBox.Text;
        }

        private void OnClickAnnulerDlg(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }

        private async void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                FilmSearchBox.IsEnabled = false;
                try
                {
                    switch (await ViewModel.RechercheFilm())
                    {
                        case 0:
                            await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("AucunResultat"));
                            break;

                        case 2:
                            if (ViewModel.Film.Type == (int)TypeFilmEnum.FILM)
                            {
                                await DlgChoixFilm.ShowAsync();
                            }

                            if (ViewModel.Film.Type == (int)TypeFilmEnum.SERIE)
                            {
                                await DlgChoixSerie.ShowAsync();
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
                }
                FilmSearchBox.IsEnabled = true;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FilmSearchBox.Text))
            {
                FilmSearchBox.IsEnabled = false;
                try
                {
                    switch (await ViewModel.RechercheFilm())
                    {
                        case 0:
                            await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("AucunResultat"));
                            break;

                        case 2:
                            if (ViewModel.Film.Type == (int)TypeFilmEnum.FILM || ViewModel.Film.Type == (int)TypeFilmEnum.DOCUMENTAIRE || ViewModel.Film.Type == (int)TypeFilmEnum.SPECTACLE || ViewModel.Film.Type == (int)TypeFilmEnum.ANIMATION )
                            {
                                await DlgChoixFilm.ShowAsync();
                            }

                            if (ViewModel.Film.Type == (int)TypeFilmEnum.SERIE)
                            {
                                await DlgChoixSerie.ShowAsync();
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
                }
                FilmSearchBox.IsEnabled = true;
            }
        }
    }
}
