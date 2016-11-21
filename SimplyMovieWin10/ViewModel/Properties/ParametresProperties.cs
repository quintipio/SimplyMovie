

using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Strings;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Propriétés de la page des paramètres
    /// </summary>
    public partial class ParametreViewModel
    {

        private ObservableCollection<ListeLangues.LanguesStruct> _listeLangues;

        /// <summary>
        /// la liste des langues disponibles
        /// </summary>
        public ObservableCollection<ListeLangues.LanguesStruct> ListeLangues
        {
            get { return _listeLangues; }

            set
            {
                _listeLangues = value;
                OnPropertyChanged();
            }
        }
    }
}
