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

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue pour la recherche d'information sur internet
    /// </summary>
    public sealed partial class RechercheView : IView<RechercheViewModel>
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        public RechercheViewModel ViewModel { get; set; }

        public RechercheView()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            RingWait.IsActive = true;
            ViewModel = new RechercheViewModel();
            var query = e.Parameter as string;

            if (query != null)
            {
                //recherche internet
                try
                {
                    var result = await ViewModel.RechercheInternet(query);
                    SearchBoxTitle.Text = result.query;
                    ViewModel.ChargerDonnees(result);
                }
                catch (Exception)
                {
                    ErrorTextBlock.Text = ResourceLoader.GetForCurrentView().GetString("erreurInternet");
                }

                //recherche en base
               await ViewModel.RechercheBase(query);
                RingWait.IsActive = false;
            }

            //si aucun résultat
            if (!ViewModel.DispoFilms && !ViewModel.DispoSeries && !ViewModel.DispoPersonnes &&
                !ViewModel.DispoMaBibliotheque)
            {
                ErrorTextBlock.Text += "\r\n"+ ResourceLoader.GetForCurrentView().GetString("AucunResultat");
            }
        }

        private void SelectFilm_Click(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Film)
            {
                App.AppShell.NavigateFrame(typeof(ConsulterFilmView),(Film)e.ClickedItem);
            }

            if (e.ClickedItem is ResultSearchGenJson)
            {
                var data = (ResultSearchGenJson)e.ClickedItem;
                TypeFilmEnum typeData = TypeFilmEnum.FILM;
                switch (data.media_type)
                {
                    case "movie":
                        typeData = TypeFilmEnum.FILM;
                        break;

                    case "tv":
                        typeData = TypeFilmEnum.SERIE;
                        break;

                    case "person":
                        typeData = TypeFilmEnum.PERSONNE;
                        break;
                }

                App.AppShell.NavigateFrame(typeof (ConsulterDataInternetView), new SearchDataInternet {Id = data.id,TypeData = typeData});
            }
        }
        

        private async void PlusResultat_Click(object sender, RoutedEventArgs e)
        {
            RingWait.IsActive = true;
            await ViewModel.GetPlusResultat();
            RingWait.IsActive = false;
        }

        private void SearchBoxTitle_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                App.AppShell.NavigateFrame(typeof(RechercheView), args.QueryText);
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBoxTitle.Text))
            {
                App.AppShell.NavigateFrame(typeof(RechercheView), SearchBoxTitle.Text);
            }
        }
    }
}
