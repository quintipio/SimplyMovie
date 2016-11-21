
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    public partial class AjouterFilmViewModel
    {
        
        private Film _film;

        /// <summary>
        /// Le film à ajouter
        /// </summary>
        public Film Film
        {
            get { return _film; }

            set
            {
                _film = value;
                OnPropertyChanged();
            }
        }

        private List<Personne> _listePersonnes;

        public List<Personne> ListePersonnes
        {
            get { return _listePersonnes; }

            set
            {
                _listePersonnes = value;
                OnPropertyChanged();
            }
        }
        

        private List<Personne> _selectedActeurListe;

        public List<Personne> SelectedActeurListe
        {
            get { return _selectedActeurListe; }

            set
            {
                _selectedActeurListe = value;
                OnPropertyChanged();
            }
        }
        

        private List<Collection> _listeCollection;

        public List<Collection> ListeCollection
        {
            get { return _listeCollection; }

            set
            {
                _listeCollection = value;
                OnPropertyChanged();
            }
        }

        private Collection _selectedCollection;

        public Collection SelectedCollection
        {
            get { return _selectedCollection; }

            set
            {
                _selectedCollection = value;
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

        private List<Personne> _selectedProducteursListe;

        public List<Personne> SelectedProducteursListe
        {
            get { return _selectedProducteursListe; }

            set
            {
                _selectedProducteursListe = value;
                OnPropertyChanged();
            }
        }

        private List<Personne> _selectedRealisateursListe;

        public List<Personne> SelectedRealisateursListe
        {
            get { return _selectedRealisateursListe; }

            set
            {
                _selectedRealisateursListe = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _affiche;

        public BitmapImage Affiche
        {
            get { return _affiche; }

            set
            {
                _affiche = value;
                OnPropertyChanged();
            }
        }

        private List<Genre> _listeGenre;

        public List<Genre> ListeGenre
        {
            get { return _listeGenre; }

            set
            {
                _listeGenre = value;
                OnPropertyChanged();
            }
        }

        private List<Genre> _listeGenreSelected;

        public List<Genre> ListeGenreSelected
        {
            get { return _listeGenreSelected; }

            set
            {
                _listeGenreSelected = value;
                OnPropertyChanged();
            }
        }

        private SearchMovieJson _filmsJson;

        public SearchMovieJson FilmsJson
        {
            get { return _filmsJson; }

            set
            {
                _filmsJson = value;
                OnPropertyChanged();
            }
        }

        private SearchTvJson _searchTvJson;

        public SearchTvJson SearchTvJson
        {
            get { return _searchTvJson; }

            set
            {
                _searchTvJson = value;
                OnPropertyChanged();
            }
        }
    }
}
