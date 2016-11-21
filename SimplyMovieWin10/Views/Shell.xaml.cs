using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SimplyMovieWin10Shared.Context;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Shell comportant le menu et l'include des pages
    /// </summary>
    public sealed partial class Shell
    {
        public Shell()
        {
            InitializeComponent();
        }


        #region Liens

        private void MainMenuRadioButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
            CheckSecButtonVisible();
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?PFN=" + Package.Current.Id.FamilyName));
        }

        private async void BugsButton_Click(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=" + ContexteStatic.Support + "&subject=Bugs ou suggestions pour " + ContexteStatic.NomAppli);
            await Launcher.LaunchUriAsync(mailto);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(AboutView));
        }

        private void AcceuilButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(AcceuilView));
        }

        private void BibliothequeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(BibliothequeView));
        }


        private void ParamsRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateFrame(typeof(ParamView));
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                App.AppShell.NavigateFrame(typeof(RechercheView), args.QueryText);
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                App.AppShell.NavigateFrame(typeof(RechercheView), SearchBox.Text);
            }
        }

        #endregion

        #region méthodes

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckSecButtonVisible();
        }

        /// <summary>
        /// Ouvre une frame dans la page
        /// </summary>
        /// <param name="type">le type de page à afficher</param>
        public void NavigateFrame(Type type)
        {
            Frame.Navigate(type);
        }

        /// <summary>
        /// Ouvre une frame dans la page en fournissant un paramètre
        /// </summary>
        /// <param name="type">le type de page à afficher</param>
        /// <param name="parameter">un paramètre à passer à la vue</param>
        public void NavigateFrame(Type type,object parameter)
        {
            Frame.Navigate(type,parameter);
        }

        /// <summary>
        /// Vérifie la visilibté des boutons du bas du menu
        /// </summary>
        private void CheckSecButtonVisible()
        {
            if (!MainSplitView.IsPaneOpen || (MainSplitView.IsPaneOpen && Width < 750))
            {
                RateButton.Visibility = Visibility.Collapsed;
                BugsButton.Visibility = Visibility.Collapsed;
                AboutButton.Visibility = Visibility.Collapsed;
                GridSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                RateButton.Visibility = Visibility.Visible;
                BugsButton.Visibility = Visibility.Visible;
                AboutButton.Visibility = Visibility.Visible;
                GridSearch.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void BackRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
