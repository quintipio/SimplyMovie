using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10Shared.Business;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue permettant de patienter pendant une mise à jour
    /// </summary>
    public sealed partial class UpdateVersion
    {
        private readonly ParamBusiness _paramBusiness;

        public UpdateVersion()
        {
            InitializeComponent();
            _paramBusiness = new ParamBusiness();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await _paramBusiness.Initialization;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await _paramBusiness.CheckVersion();

            App.OpenAppli();
        }
    }
}
