using Windows.UI.Xaml.Controls;
using SimplyMovieWin10Shared.Context;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// La page A propos de...
    /// </summary>
    public sealed partial class AboutView : Page
    {
        public AboutView()
        {
            InitializeComponent();
            
            TitreText.Text = ContexteStatic.NomAppli;
            Developpeur.Text = ContexteStatic.Developpeur;
            Version.Text = ContexteStatic.Version;
        }
    }
}
