using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10.ViewModel;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Interface;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;
namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue de la page d'acceuil
    /// </summary>
    public sealed partial class AcceuilView : IView<AcceuilViewModel>
    {
        /// <summary>
        /// Controleur
        /// </summary>
        public AcceuilViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public AcceuilView()
        {
            InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new AcceuilViewModel();
            await ViewModel.Initialization;
        }

        private void OuvrirFilm_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Film)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterFilmView), e.ClickedItem as Film);
            }

            if (e.ClickedItem is ResultSearchMovieJson)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = ((ResultSearchMovieJson)e.ClickedItem).id, TypeData = TypeFilmEnum.FILM });
            }

            if (e.ClickedItem is ResultSearchTvJson)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterDataInternetView), new SearchDataInternet { Id = ((ResultSearchTvJson)e.ClickedItem).id, TypeData = TypeFilmEnum.SERIE});
            }
        }

        private void VoirPlusFilmAVoir_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(BibliothequeView),new SearchFilter {TypeA = FilterBibliothequeEnum.FILMPOSSEDEAVOIR});
        }

        private void VoirPlusFilmAPosseder_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(BibliothequeView), new SearchFilter { TypeA = FilterBibliothequeEnum.FILMNONPOSSEDE });
        }

        private void VoirPlusFilmFavoris_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(BibliothequeView), new SearchFilter { TypeA = FilterBibliothequeEnum.FILMFAVORIS });
        }

        private void VoirPlusFilmSuggerer_Click(object sender, RoutedEventArgs e)
        {
            App.AppShell.NavigateFrame(typeof(BibliothequeView));
        }
    }
}
