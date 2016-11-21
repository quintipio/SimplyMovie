using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10.Views;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Context;

namespace SimplyMovieWin10
{
    /// <summary>
    /// Classe de démarrage
    /// </summary>
    sealed partial class App
    {

        /// <summary>
        /// page principale
        /// </summary>
        public static Shell AppShell;

        /// <summary>
        /// Constructeur
        /// </summary>
        public App()
        {
            InitializeComponent();
           Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;
            

            await ContexteAppli.Init(false);

            //chargement de la page d'acceuil
            rootFrame = new Frame();
            Window.Current.Content = rootFrame;
            rootFrame.NavigationFailed += OnNavigationFailed;
            rootFrame.Navigated += OnNavigated;

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;

            var paramBusiness = new ParamBusiness();
            await paramBusiness.Initialization;
            if (await paramBusiness.NeedUpdate())
            {
                rootFrame.Navigate(typeof(UpdateVersion));
                Window.Current.Activate();
            }
            else
            {
                OpenAppli();
            }

           
        }

        /// <summary>
        /// Ouvre la page d'acceuil normal de l'appli
        /// </summary>
        public static void OpenAppli()
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Shell));
            AppShell = rootFrame.Content as Shell;
            AppShell.NavigateFrame(typeof(AcceuilView));
            Window.Current.Activate();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null && !rootFrame.CanGoBack) return;
            e.Handled = true;
            rootFrame?.GoBack();
        }
    }
}
