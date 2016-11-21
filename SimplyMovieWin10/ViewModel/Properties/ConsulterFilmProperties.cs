using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Model;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Propriétés du controleur de consultation des films
    /// </summary>
    public partial class ConsulterFilmViewModel
    {
        private Film _filmAffiche;

        /// <summary>
        /// le film à afficher
        /// </summary>
        public Film FilmAffiche
        {
            get { return _filmAffiche; }

            set
            {
                _filmAffiche = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _filmSimilaire;

        /// <summary>
        /// la liste des films similaires
        /// </summary>
        public ObservableCollection<Film> FilmSimilaire
        {
            get
            {
                return _filmSimilaire;
            }

            set
            {
                _filmSimilaire = value;
                OnPropertyChanged();
            }
        }
        
        private bool _filmSimilaireVisible;

        /// <summary>
        /// Indique si la grid des films similaire est visible ou non?
        /// </summary>
        public bool FilmSimilaireVisible
        {
            get { return _filmSimilaireVisible; }

            set
            {
                _filmSimilaireVisible = value;
                OnPropertyChanged();
            }
        }


        private bool _filmCollectionVisible;

        public bool FilmCollectionVisible
        {
            get { return _filmCollectionVisible; }

            set
            {
                _filmCollectionVisible = value;
                OnPropertyChanged();
            }
        }

        private string _titreCollection;

        public string TitreCollection
        {
            get { return _titreCollection; }

            set
            {
                _titreCollection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _filmCollection;

        /// <summary>
        /// la liste des films similaires
        /// </summary>
        public ObservableCollection<Film> FilmCollection
        {
            get
            {
                return _filmCollection;
            }

            set
            {
                _filmCollection = value;
                OnPropertyChanged();
            }
        }


        private bool _consulterInternetVisible;

        public bool ConsulterInternetVisible
        {
            get { return _consulterInternetVisible; }

            set
            {
                _consulterInternetVisible = value;
                OnPropertyChanged();
            }
        }

    
}
}
