using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    ///^propriétés de la page d'acceuil
    /// </summary>
    public partial class AcceuilViewModel
    {

        #region Mabibliothèque
        private ObservableCollection<Film> _filmFavoris;

        /// <summary>
        /// la liste des films favoris de l'utilisateur
        /// </summary>
        public ObservableCollection<Film> FilmFavoris
        {
            get { return _filmFavoris; }

            set
            {
                _filmFavoris = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _filmVoir;

        /// <summary>
        /// La liste des films à voir
        /// </summary>
        public ObservableCollection<Film> FilmVoir
        {
            get { return _filmVoir; }

            set
            {
                _filmVoir = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _filmPosseder;

        /// <summary>
        /// La liste des film à posséder
        /// </summary>
        public ObservableCollection<Film> FilmPosseder
        {
            get { return _filmPosseder; }

            set
            {
                _filmPosseder = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Film> _filmSuggestion;

        public ObservableCollection<Film> FilmSuggestion
        {
            get { return _filmSuggestion; }

            set
            {
                _filmSuggestion = value;
                OnPropertyChanged();
            }
        }

        private bool _visibleAVoir;

        public bool VisibleAVoir
        {
            get { return _visibleAVoir; }

            set
            {
                _visibleAVoir = value;
                OnPropertyChanged();
            }
        }

        private bool _visibleAcheter;

        public bool VisibleAcheter
        {
            get { return _visibleAcheter; }

            set
            {
                _visibleAcheter = value;
                OnPropertyChanged();
            }
        }

        private bool _visibleFavoris;

        public bool VisibleFavoris
        {
            get { return _visibleFavoris; }

            set
            {
                _visibleFavoris = value;
                OnPropertyChanged();
            }
        }

        private bool _visibleSuggestion;

        public bool VisibleSuggestion
        {
            get { return _visibleSuggestion; }

            set
            {
                _visibleSuggestion = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region internet

        private ObservableCollection<ResultSearchMovieJson> _listeNowPlaying;

        public ObservableCollection<ResultSearchMovieJson> ListeNowPlaying
        {
            get { return _listeNowPlaying; }

            set
            {
                _listeNowPlaying = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisibleNowPlaying;

        public bool IsVisibleNowPlaying
        {
            get { return _isVisibleNowPlaying; }

            set
            {
                _isVisibleNowPlaying = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultSearchMovieJson> _listeFilmPopulaire;

        public ObservableCollection<ResultSearchMovieJson> ListeFilmPopulaire
        {
            get { return _listeFilmPopulaire; }

            set
            {
                _listeFilmPopulaire = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisibleFilmPopulaire;

        public bool IsVisibleFilmPopulaire
        {
            get { return _isVisibleFilmPopulaire; }

            set
            {
                _isVisibleFilmPopulaire = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultSearchTvJson> _listeTvMoment;

        public ObservableCollection<ResultSearchTvJson> ListeTvMoment
        {
            get { return _listeTvMoment; }

            set
            {
                _listeTvMoment = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisibleTvMoment;

        public bool IsVisibleTvMoment
        {
            get { return _isVisibleTvMoment; }

            set
            {
                _isVisibleTvMoment = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultSearchTvJson> _listeTvPopular;

        public ObservableCollection<ResultSearchTvJson> ListeTvPopular
        {
            get { return _listeTvPopular; }

            set
            {
                _listeTvPopular = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisibleTvPopular;

        public bool IsVisibleTvPopular
        {
            get { return _isVisibleTvPopular; }

            set
            {
                _isVisibleTvPopular = value;
                OnPropertyChanged();
            }
        }
        #endregion


    }
}
