using System.Collections.Generic;
using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// propriétés de la bibliothèque
    /// </summary>
    public partial class BibliothequeViewModel
    {
        private int _pageEnCours;
        private int _nombrePage;
        
        private List<Film> _listeFilmTotal;

        private const int NbOccurencesMax = 50;

        

        private ObservableCollection<Film> _listeFilms;

        public ObservableCollection<Film> ListeFilms
        {
            get { return _listeFilms; }

            set
            {
                _listeFilms = value;
                OnPropertyChanged();
            }
        }

        private FilterBibliothequeEnum _filtreA;

        public FilterBibliothequeEnum FiltreA
        {
            get { return _filtreA; }

            set
            {
                _filtreA = value;
                OnPropertyChanged();
            }
        }

        private FilterBibliothequeEnum _filtreB;

        public FilterBibliothequeEnum FiltreB
        {
            get { return _filtreB; }

            set
            {
                _filtreB = value;
                OnPropertyChanged();
            }
        }

        private bool _visibleGridNextBack;

        public bool VisibleGridNextBack
        {
            get { return _visibleGridNextBack; }

            set
            {
                _visibleGridNextBack = value;
                OnPropertyChanged();
            }
        }

        private bool _enableNext;

        public bool EnableNext
        {
            get { return _enableNext; }

            set
            {
                _enableNext = value;
                OnPropertyChanged();
            }
        }

        private bool _enableBack;

        public bool EnableBack
        {
            get { return _enableBack; }

            set
            {
                _enableBack = value;
                OnPropertyChanged();
            }
        }
    }
}
