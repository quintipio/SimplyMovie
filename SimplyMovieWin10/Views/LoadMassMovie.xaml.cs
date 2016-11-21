using System;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10.ViewModel;
using SimplyMovieWin10Shared.Interface;
using SimplyMovieWin10Shared.Model.JSON;
using SimplyPasswordWin10Shared.Utils;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue pour le chargement de masse
    /// </summary>
    public sealed partial class LoadMassMovie : IView<LoadMassMovieViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public LoadMassMovieViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public LoadMassMovie()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new LoadMassMovieViewModel();
            await ViewModel.Initialization;
            await ViewModel.ScanFolder(e.Parameter as StorageFolder);
            ViewModel.IsLoaded = true;
        }

        private async void FindMovie_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((AppBarButton) sender).Tag;
            if (await ViewModel.LoadDlg(id))
            {
                await DlgChoixFilm.ShowAsync();
            }
        }

        private void DeleteMovie_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as AppBarButton;
            if (b != null)
            {
                ViewModel.DeleteMovie((int)b.Tag);
            }
        }
        
        private void OnClickAnnulerDlg(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }



        private async void SelectFilmDlg(object sender, ItemClickEventArgs e)
        {
            try
            {
                var film = e.ClickedItem as ResultSearchMovieJson;
                await ViewModel.SelectMovieFmDlg(film.id);
                DlgChoixFilm.Hide();
            }
            catch (Exception)
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("erreurInternet"));
            }
        }

        private async void Valider_Click(object sender, RoutedEventArgs e)
        {
            ValiderButton.IsEnabled = false;
            ViewModel.IsLoaded = false;
            var erreur = await ViewModel.Sauvegarder();
            if (!string.IsNullOrWhiteSpace(erreur))
            {
                await MessageBox.ShowAsync(erreur);
            }
            else
            {
                await MessageBox.ShowAsync( ResourceLoader.GetForCurrentView().GetString("FilmAjoute"));
                App.AppShell.NavigateFrame(typeof(BibliothequeView));
            }
            ViewModel.IsLoaded = true;
            ValiderButton.IsEnabled = true;
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.TextChanged = true;
        }
    }
}
