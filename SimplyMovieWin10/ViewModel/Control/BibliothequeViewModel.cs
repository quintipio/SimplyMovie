using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Controleur de la bibliothèque
    /// </summary>
    public partial class BibliothequeViewModel : AbstractViewModel
    {
        private FilmBusiness _filmBusiness;

        private const string CaractereSeparateur = ";";

        /// <summary>
        /// Constructeur
        /// </summary>
        public BibliothequeViewModel()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// init
        /// </summary>
        /// <returns></returns>
        public sealed override async Task InitializeAsync()
        {
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
        }

        /// <summary>
        /// Lance une recherche à partir des filtres pré remplis
        /// </summary>
        /// <returns></returns>
        public async Task GetMovieBySearch()
        {
            await GetMovieBySearch(new SearchFilter {TypeA = FiltreA, TypeB = FiltreB});
        }


        /// <summary>
        /// Récupère les films à partir d'une recherche
        /// </summary>
        /// <param name="data">les données de recherches</param>
        /// <returns></returns>
        public async Task GetMovieBySearch(SearchFilter data)
        {
            //récupération de tout les films
            _listeFilmTotal = await SearchGeneral(data.TypeA, data.TypeB, data.RechercheGenre, data.RecherchePersonne);

            //affichage des saison
            foreach (var source in _listeFilmTotal.Where(x => x.Type == (int)TypeFilmEnum.SERIE))
            {
                 source.Titre += "(" + source.Saison + ")";
            }

            //préparation de la visu et calcul du nombre de page total
            VisibleGridNextBack = _listeFilmTotal.Count > NbOccurencesMax;
            _nombrePage = _listeFilmTotal.Count / NbOccurencesMax;
            if (_nombrePage == 0)
            {
                _nombrePage = 1;
            }
            else
            {
                if (NbOccurencesMax % _listeFilmTotal.Count > 0)
                {
                    _nombrePage++;
                }
            }
            

            //affichage de la première page
            ChangePage(1,false,false);
        }



        /// <summary>
        /// Lance une recherche en base avec des critères spécifiques
        /// </summary>
        /// <param name="filterA">le prmeier des deux filtres</param>
        /// <param name="filterB">le deuxième filtre</param>
        /// <param name="genreSearch">si recherche par genre, le genre</param>
        /// <param name="personneSearch">si rechercher par perosnne, la personne recherchée</param>
        /// <returns></returns>
        private async Task<List<Film>> SearchGeneral(FilterBibliothequeEnum filterA, FilterBibliothequeEnum filterB, Genre genreSearch, Personne personneSearch)
        {
            //si aucun filtre on récupère toute la bibliothèque
            if (filterA == FilterBibliothequeEnum.NONE && filterB == FilterBibliothequeEnum.NONE)
            {
                return await _filmBusiness.GetBibliotheque();
            }

            //si c'est un filtre de personne ou de genre
            else if (filterA == FilterBibliothequeEnum.GENRE && genreSearch != null)
            {
                return await _filmBusiness.GetFilm(genreSearch);
            }

            else if (filterA == FilterBibliothequeEnum.PERSONNE && personneSearch != null)
            {
                return await _filmBusiness.GetFilm(personneSearch);
            }

            //si c'est n'importe quel autre filtre
            else
            {
                return await _filmBusiness.GetFilm(filterA,filterB);
            }
        }

        public void ChangePage(int? page, bool goToPrevious, bool goToNext)
        {
            if (page != null)
            {
                if (page <= _nombrePage && page > 0)
                {
                    _pageEnCours = page.Value;
                }
            }
            else
            {
                if (goToPrevious)
                {
                    _pageEnCours--;
                }

                if (goToNext)
                {
                    _pageEnCours++;
                }
            }

            //Changement de la visibilité
            EnableBack = _pageEnCours > 1;
            EnableNext = _pageEnCours < _nombrePage;

            //affichage de la nouvelle liste de films
            ListeFilms = new ObservableCollection<Film>(_listeFilmTotal.Skip((_pageEnCours - 1) * NbOccurencesMax).Take(NbOccurencesMax));
        }


        /// <summary>
        /// Supprime un film de la base de donnée et de la bibliothèque
        /// </summary>
        /// <param name="film">le film à supprimer</param>
        /// <returns></returns>
        public async Task SupprimerFilm(Film film)
        {
            await _filmBusiness.SupprimerFilm(film);
            ListeFilms.Remove(film);
            ListeFilms = new ObservableCollection<Film>(ListeFilms);
        }

        /// <summary>
        /// Converti en string la liste des films à exporter
        /// </summary>
        /// <returns>la chaine à mettre dans le fichier</returns>
        public string ExporterBibliotheque()
        {
            var retour = "";
            foreach (var film in _listeFilmTotal)
            {
                retour += film.NomCollection + CaractereSeparateur;
                retour += film.Titre + CaractereSeparateur;
                retour += film.Annee + CaractereSeparateur;
                retour = (film.Genres.Aggregate(retour, (current, genre) => current + (genre.Nom + " ")))+CaractereSeparateur;
                retour = (film.Acteurs.Aggregate(retour, (current, acteur) => current + (acteur.Nom + " "))) + CaractereSeparateur;
                retour += film.Duree + " min " + CaractereSeparateur;
                retour += "\r\n";
            }

            return retour;
        }
    }
}
