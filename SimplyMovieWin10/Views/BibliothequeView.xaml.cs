
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Text;
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
    /// Vue de la bibliothèque
    /// </summary>
    public sealed partial class BibliothequeView : IView<BibliothequeViewModel>
    {

        /// <summary>
        /// Controleur
        /// </summary>
        public BibliothequeViewModel ViewModel { get; set; }

        public bool LinkEnabled { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public BibliothequeView()
        {
            InitializeComponent();

            NewFilmButton.IsEnabled = true;
            ExportButton.IsEnabled = true;
            LinkEnabled = true;
            WaitRing.IsActive = false;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ChangerEtatAttente();
            ViewModel = new BibliothequeViewModel();
            await ViewModel.Initialization;

            if (e.Parameter is SearchFilter)
            {
                var filter = (SearchFilter) e.Parameter;

                switch (filter.TypeA)
                {
                        case FilterBibliothequeEnum.GENRE:
                        GridLink.Visibility = Visibility.Collapsed;
                        break;

                        case FilterBibliothequeEnum.PERSONNE:
                        GridLink.Visibility = Visibility.Collapsed;
                        break;

                        case FilterBibliothequeEnum.FILMNONPOSSEDE:
                        HyperlinkButtonNoPossess.FontWeight = FontWeights.Bold;
                        break;

                        case FilterBibliothequeEnum.FILMNONPOSSEDEAVOIR:
                        HyperlinkButtonToSeeNoPOssess.FontWeight = FontWeights.Bold;
                        break;

                        case  FilterBibliothequeEnum.FILMPOSSEDEAVOIR:
                        HyperlinkButtonToSeePossess.FontWeight = FontWeights.Bold;
                        break;

                        case FilterBibliothequeEnum.FILMPOSSEDE:
                        HyperlinkButtonPossess.FontWeight = FontWeights.Bold;
                        break;

                        default:
                        HyperlinkButtonAll.FontWeight = FontWeights.Bold;
                        break;
                }

                await SearchMovie(filter);
            }
            else
            {
                ViewModel.FiltreA = FilterBibliothequeEnum.NONE;
                ViewModel.FiltreB = FilterBibliothequeEnum.NONE;
                HyperlinkButtonAll.FontWeight = FontWeights.Bold;
                await ViewModel.GetMovieBySearch();
            }
            ChangerEtatAttente();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel?.ListeFilms?.Clear();
            base.OnNavigatedFrom(e);
        }

        private async Task SearchMovie(SearchFilter data)
        {
            await ViewModel.GetMovieBySearch(data);
        }


        private void View_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(ConsulterFilmView), ((AppBarButton)sender).Tag as Film);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(AjouterFilmView), ((AppBarButton)sender).Tag);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var film = ((AppBarButton)sender).Tag as Film;
            if (film != null)
            {
                if (await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("confirmSupFilm"), ResourceLoader.GetForCurrentView().GetString("avert"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await ViewModel.SupprimerFilm(film);
                }
            }
            
        }

        private void ChangerEtatAttente()
        {
            NewFilmButton.IsEnabled = !NewFilmButton.IsEnabled;
            ExportButton.IsEnabled = !ExportButton.IsEnabled;
            LinkEnabled = !LinkEnabled;
            WaitRing.IsActive = !WaitRing.IsActive;
        }

        private async void ClickGenre(object sender, RoutedEventArgs e)
        {
            var genre = (Genre)((HyperlinkButton) sender).Tag;
            await SearchMovie(new SearchFilter {RechercheGenre = genre, TypeA = FilterBibliothequeEnum.GENRE});
        }

        private void AjouterFilm_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(AjouterFilmView));
        }

        private async void TrierBiblio_Click(object sender, RoutedEventArgs e)
        {
            if (LinkEnabled)
            {
                ChangerEtatAttente();
                var num = (string)((HyperlinkButton)sender).Tag;
                switch (num)
                {
                    case "1":
                        ViewModel.FiltreA = FilterBibliothequeEnum.NONE;
                        ViewModel.FiltreB = FilterBibliothequeEnum.NONE;
                        HyperlinkButtonAll.FontWeight = FontWeights.Bold;

                        HyperlinkButtonAnim.FontWeight = FontWeights.Normal;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Normal;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Normal;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Normal;

                        HyperlinkButtonPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonNoPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeePossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeeNoPOssess.FontWeight = FontWeights.Normal;
                        break;
                    case "2":
                        ViewModel.FiltreA = FilterBibliothequeEnum.FILMPOSSEDE;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonPossess.FontWeight = FontWeights.Bold;
                        HyperlinkButtonNoPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeePossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeeNoPOssess.FontWeight = FontWeights.Normal;
                        break;
                    case "3":
                        ViewModel.FiltreA = FilterBibliothequeEnum.FILMNONPOSSEDE;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonNoPossess.FontWeight = FontWeights.Bold;
                        HyperlinkButtonToSeePossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeeNoPOssess.FontWeight = FontWeights.Normal;
                        break;
                    case "4":
                        ViewModel.FiltreA = FilterBibliothequeEnum.FILMPOSSEDEAVOIR;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonNoPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeePossess.FontWeight = FontWeights.Bold;
                        HyperlinkButtonToSeeNoPOssess.FontWeight = FontWeights.Normal;
                        break;
                    case "5":
                        ViewModel.FiltreA = FilterBibliothequeEnum.FILMNONPOSSEDEAVOIR;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonNoPossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeePossess.FontWeight = FontWeights.Normal;
                        HyperlinkButtonToSeeNoPOssess.FontWeight = FontWeights.Bold;
                        break;

                    case "10":
                        ViewModel.FiltreB = FilterBibliothequeEnum.COLLECTION;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonAnim.FontWeight = FontWeights.Normal;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Normal;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Normal;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Bold;
                        break;
                    case "11":
                        ViewModel.FiltreB = FilterBibliothequeEnum.FILM;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonAnim.FontWeight = FontWeights.Normal;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Normal;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Bold;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Normal;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Normal;
                        break;
                    case "12":
                        ViewModel.FiltreB = FilterBibliothequeEnum.SERIE;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonAnim.FontWeight = FontWeights.Normal;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Normal;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Bold;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Normal;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Normal;
                        break;
                    case "13":
                        ViewModel.FiltreB = FilterBibliothequeEnum.DOCUMENTAIRE;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonAnim.FontWeight = FontWeights.Normal;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Black;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Normal;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Normal;
                        break;
                    case "14":
                        ViewModel.FiltreB = FilterBibliothequeEnum.ANIMATION;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonAnim.FontWeight = FontWeights.Bold;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Normal;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Normal;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Normal;
                        break;
                    case "15":
                        ViewModel.FiltreB = FilterBibliothequeEnum.SPECTACLE;
                        HyperlinkButtonAll.FontWeight = FontWeights.Normal;
                        HyperlinkButtonAnim.FontWeight = FontWeights.Normal;
                        HyperlinkButtonDocu.FontWeight = FontWeights.Normal;
                        HyperlinkButtonFilms.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSeries.FontWeight = FontWeights.Normal;
                        HyperlinkButtonSpectacles.FontWeight = FontWeights.Bold;
                        HyperlinkButtonCollection.FontWeight = FontWeights.Normal;
                        break;
                }
                await ViewModel.GetMovieBySearch();
                ChangerEtatAttente();
            }
            
        }

        private async void Exporter_Click(object sender, RoutedEventArgs e)
        {
            var fileSavePicker = new FileSavePicker
            {
                CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                SuggestedFileName = "export",
                SuggestedStartLocation = PickerLocationId.Downloads,
                DefaultFileExtension = ".csv",

            };

            fileSavePicker.FileTypeChoices.Add("SimplyMovieDb", new List<string> { ".csv" });
            var fichierTmp = await fileSavePicker.PickSaveFileAsync();
            if (fichierTmp != null)
            {
                var data = ViewModel.ExporterBibliotheque();
                await FileIO.WriteTextAsync(fichierTmp, data);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ChangePage(null,true,false);
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ChangePage(null,false,true);
        }

        private async void ImportPass_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
                FileTypeFilter = { ".avi", ".wmv", ".mpg", ".mp4", ".mkv" }
            };

            var fichier = await folderPicker.PickSingleFolderAsync();
            if (fichier != null)
            {
                App.AppShell.NavigateFrame(typeof(LoadMassMovie),fichier);
            }
        }
    }
}
