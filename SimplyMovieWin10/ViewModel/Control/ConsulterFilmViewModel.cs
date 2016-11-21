using System;
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
    /// Controleur de consultation des films
    /// </summary>
    public partial class ConsulterFilmViewModel : AbstractViewModel
    {

        private FilmBusiness _filmBusiness;
        private MovieDbBusiness _movieDbBusiness;

        private const string opacityPossess = "1.0";
        private const string opacityNoPossess = "0.75";

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConsulterFilmViewModel()
        {
            FilmSimilaireVisible = false;
            Initialization = InitializeAsync();
        }

        public sealed override async Task InitializeAsync()
        {
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
            _movieDbBusiness = new MovieDbBusiness();
            ConsulterInternetVisible = false;
            FilmCollectionVisible = false;
        }

        /// <summary>
        /// Charge un film complètement pour l'afficher
        /// </summary>
        /// <param name="film">le film à charger</param>
        public async Task ChargerFilm(Film film)
        {
            FilmAffiche = await _filmBusiness.GetFilm(film.Id);
            if (FilmAffiche.IdCollection > 0)
            {
                var collec = await _filmBusiness.GetCollection(FilmAffiche.IdCollection);

                //si il y a une collection, ajout des données locales
                if (collec != null)
                {
                    TitreCollection = collec.NomCollection;
                    FilmCollection = new ObservableCollection<Film>(collec.FilmCollection);
                    foreach (var film1 in FilmCollection)
                    {
                        film1.Opacity = opacityPossess;
                    }
                    FilmCollectionVisible = true;

                    //puis s'il y a des données d'internet, ajout des films provenant d'internet
                    try
                    {
                        if (collec.IdCollectionInternet > 0)
                        {
                            //récupération des films d'internet
                            var collecInternet = new ObservableCollection<Film>(await _movieDbBusiness.GetCollectionMovie(collec.IdCollectionInternet));

                            //ajout des films à la collection déjà faite
                            var listeIdInternet = FilmCollection.Select(x => x.IdInternet).ToList();
                            foreach (var film1 in collecInternet.Where(film1 => !listeIdInternet.Contains(film1.IdInternet)))
                            {
                                film1.Opacity = opacityNoPossess;
                                FilmCollection.Add(film1);
                            }

                            //tri des films
                            FilmCollection = new ObservableCollection<Film>(FilmCollection.OrderBy(x=> x.Annee).ThenBy(x => x.IdInternet));
                        }
                    }
                    catch (Exception)
                    {
                        //Ignored
                    }
                }
            }
        }

        /// <summary>
        /// Charge toute les données provenant d'internet
        /// </summary>
        /// <returns></returns>
        public async Task ChargerFilmInternet()
        {
            if (FilmAffiche.IdInternet > 0)
            {
                ConsulterInternetVisible = true;
                try
                {
                    if (FilmAffiche.Type == (int)TypeFilmEnum.FILM || FilmAffiche.Type == (int)TypeFilmEnum.DOCUMENTAIRE || FilmAffiche.Type == (int)TypeFilmEnum.SPECTACLE || FilmAffiche.Type == (int)TypeFilmEnum.ANIMATION)
                    {
                        FilmSimilaire =
                            new ObservableCollection<Film>(
                                await _movieDbBusiness.GetSimilarMovie(FilmAffiche.IdInternet));

                        foreach (var film in FilmSimilaire)
                        {
                            film.Type = (int)TypeFilmEnum.FILM;
                            film.Opacity =(await _filmBusiness.IsFilmInternetPresentEnBase(film.IdInternet, (int)TypeFilmEnum.FILM) == null)?opacityNoPossess:opacityPossess;
                        }
                    }
                    if (FilmAffiche.Type == (int)TypeFilmEnum.SERIE)
                    {
                        FilmSimilaire =
                            new ObservableCollection<Film>(
                                await _movieDbBusiness.GetSimilarSerie(FilmAffiche.IdInternet));

                        foreach (var film in FilmSimilaire)
                        {
                            film.Type = (int) TypeFilmEnum.SERIE;
                            film.Opacity = (await _filmBusiness.IsFilmInternetPresentEnBase(film.IdInternet, (int)TypeFilmEnum.SERIE) == null) ? opacityNoPossess : opacityPossess;
                        }
                    }

                    if (FilmSimilaire.Count > 0)
                    {
                        FilmSimilaireVisible = true;
                    }
                }
                catch (Exception)
                {
                    FilmSimilaireVisible = false;
                    FilmSimilaire = new ObservableCollection<Film>();
                }
            }
            else
            {
                ConsulterInternetVisible = false;
                FilmSimilaireVisible = false;
            }
        }


        /// <summary>
        /// Supprime un film de la base de donnée et de la bibliothèque
        /// </summary>
        /// <returns></returns>
        public async Task SupprimerFilm()
        {
            await _filmBusiness.SupprimerFilm(FilmAffiche);
        }

        /// <summary>
        /// Vérifie si un film est déjà présent en base ou non (afin de déterminer si il faut ouvrir sa consultation internet)
        /// </summary>
        /// <param name="film">le film</param>
        /// <param name="type">le type (film ou série)</param>
        /// <returns>true si présent</returns>
        public async Task<Film> IsFilmPresent(Film film,int type)
        {
            if (film.IdInternet != 0)
            {
                return await _filmBusiness.IsFilmInternetPresentEnBase(film.IdInternet,type);
            }
            return null;
        }

        /// <summary>
        /// Fait passer le film à l'état vu
        /// </summary>
        /// <returns></returns>
        public async Task PasserFilmVu()
        {
            await _filmBusiness.PasserFilmVu(FilmAffiche.Id);
        }

        /// <summary>
        /// Fait passer le film à l'état posséder
        /// </summary>
        /// <returns></returns>
        public async Task PasserFilmAcheter()
        {
            await _filmBusiness.PasserFilmAcheter(FilmAffiche.Id);
        }
    }
}
