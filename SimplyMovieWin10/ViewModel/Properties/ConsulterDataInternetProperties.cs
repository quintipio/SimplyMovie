
using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Les propriétés de la page d'affichage des données d'internet
    /// </summary>
    public partial class ConsulterDataInternetViewModel
    {
        private DataFromInternet _dataToDisplay;

        public DataFromInternet DataToDisplay
        {
            get { return _dataToDisplay; }

            set
            {
                _dataToDisplay = value;
                OnPropertyChanged();
            }
        }

        private string _titre;

        public string Titre
        {
            get { return _titre; }

            set
            {
                _titre = value;
                OnPropertyChanged();
            }
        }

        private bool _filmVisible;

        public bool FilmVisible
        {
            get { return _filmVisible; }

            set
            {
                _filmVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _serieVisible;

        public bool SerieVisible
        {
            get { return _serieVisible; }

            set
            {
                _serieVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _personVisible;

        public bool PersonVisible
        {
            get { return _personVisible; }

            set
            {
                _personVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _elementPresent;

        public bool ElementPresent
        {
            get { return _elementPresent; }

            set
            {
                _elementPresent = value;
                OnPropertyChanged();
            }
        }

        public Film FilmBibliotheque { get; set; }

        private ObservableCollection<int> _listeSaison;

        public ObservableCollection<int> ListeSaison
        {
            get { return _listeSaison; }

            set
            {
                _listeSaison = value;
                OnPropertyChanged();
            }
        }

        private SearchSeasonJson _season;

        public SearchSeasonJson Season
        {
            get { return _season; }

            set
            {
                _season = value;
                OnPropertyChanged();
            }
        }

        private EpisodeJson _episode;

        public EpisodeJson Episode
        {
            get { return _episode; }

            set
            {
                _episode = value;
                OnPropertyChanged();
            }
        }
    }
}
