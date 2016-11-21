
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Propriétés de la page de chargement de masse
    /// </summary>
    public partial class LoadMassMovieViewModel
    {
        public bool TextChanged;

        private List<MassLoad> _listeData;

        public List<MassLoad> ListeData
        {
            get { return _listeData; }

            set
            {
                _listeData = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoaded;

        public bool IsLoaded
        {
            get { return _isLoaded; }

            set
            {
                _isLoaded = value;
                OnPropertyChanged();
            }
        }

        private List<ResultSearchMovieJson> _listeFilmDlg;

        public List<ResultSearchMovieJson> ListeFilmDlg
        {
            get { return _listeFilmDlg; }

            set
            {
                _listeFilmDlg = value;
                OnPropertyChanged();
            }
        }

        private int _selectedMassLoad;

        public int SelectedMassLoad
        {
            get { return _selectedMassLoad; }

            set
            {
                _selectedMassLoad = value;
                OnPropertyChanged();
            }
        }
    }
}
