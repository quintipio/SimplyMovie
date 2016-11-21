using System.Collections.ObjectModel;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    public partial class RechercheViewModel
    {

        string query { get; set; }

        int pageEnCours { get; set; }

        int nbPageMax { get; set; }

        private bool _dispoPlusResult;

        public bool DispoPlusResult
        {
            get { return _dispoPlusResult; }

            set
            {
                _dispoPlusResult = value;
                OnPropertyChanged();
            }
        }

        private bool _dispoMaBibliotheque;

        public bool DispoMaBibliotheque
        {
            get { return _dispoMaBibliotheque; }

            set
            {
                _dispoMaBibliotheque = value;
                OnPropertyChanged();
            }
        }

        private bool _dispoFilms;

        public bool DispoFilms
        {
            get { return _dispoFilms; }

            set
            {
                _dispoFilms = value;
                OnPropertyChanged();
            }
        }

        private bool _dispoSeries;

        public bool DispoSeries
        {
            get { return _dispoSeries; }

            set
            {
                _dispoSeries = value;
                OnPropertyChanged();
            }
        }

        private bool _dispoPersonnes;

        public bool DispoPersonnes
        {
            get { return _dispoPersonnes; }

            set
            {
                _dispoPersonnes = value;
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

        private ObservableCollection<Film> _listeBibliotheque;

        public ObservableCollection<Film> ListeBibliotheque
        {
            get { return _listeBibliotheque; }

            set
            {
                _listeBibliotheque = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultSearchGenJson> _listeFilm;

        public ObservableCollection<ResultSearchGenJson> ListeFilm
        {
            get { return _listeFilm; }

            set
            {
                _listeFilm = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultSearchGenJson> _listeSerie;

        public ObservableCollection<ResultSearchGenJson> ListeSerie
        {
            get { return _listeSerie; }

            set
            {
                _listeSerie = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultSearchGenJson> _listePersonne;

        public ObservableCollection<ResultSearchGenJson> ListePersonne
        {
            get { return _listePersonne; }

            set
            {
                _listePersonne = value;
                OnPropertyChanged();
            }
        }
    }
}
