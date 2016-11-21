
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Utils;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Controleur pour l'ajout d'un film
    /// </summary>
    public partial class AjouterFilmViewModel : AbstractViewModel
    {

        private MovieDbBusiness _movieDbBusiness;

        private FilmBusiness _filmBusiness;
        

        /// <summary>
        /// Constructeur
        /// </summary>
        public AjouterFilmViewModel()
        {
            Initialization = InitializeAsync();
            SelectedActeurListe = new List<Personne>();
            SelectedProducteursListe = new List<Personne>();
            SelectedRealisateursListe = new List<Personne>();
            ListeGenreSelected = new List<Genre>();
            Film = new Film
            {
                Id = 0,
                Type = (int) TypeFilmEnum.FILM,
                MaNote = 3,
                NoteGen = 3
            };
        }

        /// <summary>
        /// init
        /// </summary>
        /// <returns></returns>
        public sealed override async Task InitializeAsync()
        {
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
            _movieDbBusiness = new MovieDbBusiness();
            ListePersonnes = await _filmBusiness.GetListePersonnes();
            ListeGenre = await _filmBusiness.GetListeGenre();
            ListeCollection = await _filmBusiness.GetListeCollection();
        }

        /// <summary>
        /// Charge les informations d'un film
        /// </summary>
        /// <param name="film">le film à charger</param>
        /// <returns></returns>
        public async Task ChargerFilm(Film film)
        {
            Film = await _filmBusiness.GetFilm(film.Id);
            if (film.Affiche != null || (film.AfficheImage?.UriSource != null && !film.AfficheImage.UriSource.Equals(ContexteStatic.UriAfficheDefaut)))
            {
                Affiche = Film.AfficheImage;
            }
            ListeGenreSelected = new List<Genre>(Film.Genres);
            SelectedActeurListe = new List<Personne>(Film.Acteurs);
            SelectedProducteursListe = new List<Personne>(Film.Producteurs);
            SelectedRealisateursListe = new List<Personne>(Film.Realisateurs);
            if (Film.IdCollection > 0)
            {
                SelectedCollection = ListeCollection.First(x => x.Id == Film.IdCollection);
                if (SelectedCollection != null)
                {
                    TitreCollection = SelectedCollection.NomCollection;
                }

            }
        }

        /// <summary>
        /// Charge les données provenant d'internet à partir d'un id
        /// </summary>
        /// <param name="data">les données pour récupérer les infos d'internet</param>
        /// <returns></returns>
        public async Task ChargerFilm(SearchDataInternet data)
        {
            switch (data.TypeData)
            {
                case TypeFilmEnum.SERIE:
                    Film = await _movieDbBusiness.GetSerie(data.Id);
                    break;

                default:
                    Film = await _movieDbBusiness.GetFilm(data.Id);
                    break;
            }

            SelectedProducteursListe = new List<Personne>(Film.Producteurs);
            SelectedRealisateursListe = new List<Personne>(Film.Realisateurs);
            SelectedActeurListe = new List<Personne>(Film.Acteurs);
            ListeGenreSelected = new List<Genre>(Film.Genres);
            Affiche = Film.AfficheImage;
            Film.Id = 0;
            Film.MaNote = 3;
            if (Film.IdCollectionInternet > 0)
            {
                TitreCollection = Film.NomCollection;
                if (ListeCollection.Count(x => x.NomCollection == TitreCollection) >= 1)
                {
                    SelectedCollection = ListeCollection.First(x => x.IdCollectionInternet == Film.IdCollectionInternet);
                }
            }
        }

        #region autosuggest

        /// <summary>
        /// Retourne une liste de collection à partir d'une chaine
        /// </summary>
        /// <param name="query">la chaine à rechercher</param>
        /// <returns>la liste des collections trouvés</returns>
        public IEnumerable<Collection> GetSearchCollection(string query)
        {
            return ListeCollection.Where(c => c.NomCollection.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 && c.Id != Film.Id).OrderByDescending(c => c.NomCollection.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Retourne une collection à partir de son nom
        /// </summary>
        /// <param name="nom">le nom</param>
        /// <returns>la collection</returns>
        public Collection GetCollection(string nom)
        {
            if (ListeCollection.Count(x => string.Equals(x.NomCollection.ToLower(), nom.ToLower()) && x.Id != Film.Id) > 0)
            {
                return ListeCollection.FirstOrDefault(x => string.Equals(x.NomCollection.ToLower(), nom.ToLower()));
            }
            return null;
        }

        /// <summary>
        /// Retourne une liste de personnes à partir d'une chaine
        /// </summary>
        /// <param name="query">la chaine à rechercher</param>
        /// <returns>la liste de personne trouvé</returns>
        public IEnumerable<Personne> GetSearchPersonnes(string query)
        {
            return ListePersonnes.Where(c => c.Nom.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1) .OrderByDescending(c => c.Nom.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Recherche un genre parmis la liste
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Genre> GetSearchGenre(string query)
        {
            return ListeGenre.Where(c => c.Nom.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1).OrderByDescending(c => c.Nom.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Retourne une personne à partir de son nom
        /// </summary>
        /// <param name="nom">le nom de la personne saisie</param>
        /// <returns>un objet personne</returns>
        public Personne GetPersonne(string nom)
        {
            var newName = "";
            foreach (var chaine in nom.ToLower().Split(' '))
            {
                newName += StringUtils.FirstLetterUpper(chaine)+" ";
            }
            newName = newName.Trim();

            Personne personne;
            if (ListePersonnes.Count(x => string.Equals(x.Nom.ToLower(), newName.ToLower())) > 0)
            {
                personne =  ListePersonnes.FirstOrDefault(x => string.Equals(x.Nom.ToLower(), newName.ToLower()));
            }
            else
            {
                personne = new Personne
                {
                    Id = 0,
                    Nom = newName
                };
            }
            return personne;
        }

        /// <summary>
        /// Retourne un genre à partir de son nom
        /// </summary>
        /// <param name="nom">le nom</param>
        /// <returns>le genre</returns>
        public Genre GetGenre(string nom)
        {
            var newName = "";
            foreach (var chaine in nom.ToLower().Split(' '))
            {
                newName += StringUtils.FirstLetterUpper(chaine) + " ";
            }
            newName = newName.Trim();

            Genre genre;
            if (ListeGenre.Count(x => string.Equals(x.Nom.ToLower(), newName.ToLower())) > 0)
            {
                genre = ListeGenre.FirstOrDefault(x => string.Equals(x.Nom.ToLower(), newName.ToLower()));
            }
            else
            {
                genre = new Genre
                {
                    Id = 0,
                    Nom = newName
                };
            }
            return genre;
        }

        /// <summary>
        /// Vérifie si un genre est déjà présent dans la liste des genres sélectionné
        /// </summary>
        /// <param name="genre">le genre à controler</param>
        /// <returns>true si présent</returns>
        public bool CheckGenrePresent(Genre genre)
        {
            var toto = ListeGenreSelected.Count(x => string.Equals(x.Nom.ToLower(), genre.Nom.ToLower()));
            return toto > 0;
        }

        /// <summary>
        /// Vérifie si une personne est déjà présente dans une liste
        /// </summary>
        /// <param name="personne">le genre à controler</param>
        /// <param name="liste">la liste à cotnroler</param>
        /// <returns>ture si présent</returns>
        public bool CheckPersonnePresent(Personne personne, IEnumerable<Personne> liste)
        {
            return liste.Count(x => string.Equals(x.Nom.ToLower(), personne.Nom.ToLower())) > 0;
        }

        #endregion

        #region chargement fichier
        /// <summary>
        /// Vérifie la validité d'une image chargé par un utilisateur
        /// </summary>
        /// <param name="image">l'image à vérifier</param>
        /// <returns>les erreurs</returns>
        private async Task<string> ValidateImage(StorageFile image)
        {
            string retour = "";
            if (image == null)
            {
                retour += ResourceLoader.GetForCurrentView().GetString("erreurAucuneImage") + "\r\n";
            }
            else
            {
                if ((await image.Properties.GetImagePropertiesAsync()).Width < ContexteStatic.MaxSizeXAffiche || (await image.Properties.GetImagePropertiesAsync()).Height < ContexteStatic.MaxSizeYAffiche)
                {
                    retour += ResourceLoader.GetForCurrentView().GetString("erreurSizeImage") + " " + ContexteStatic.MaxSizeXAffiche + "x" + ContexteStatic.MaxSizeYAffiche + "\r\n";
                }

                if ((await image.GetBasicPropertiesAsync()).Size > 2048000)
                {
                    retour += ResourceLoader.GetForCurrentView().GetString("erreurPoidImage") + " 2Mb\r\n";
                }
            }


            return retour;
        }


        /// <summary>
        /// Met en mémoire le fichier à afficher
        /// </summary>
        /// <param name="fichier">le fichier à charger</param>
        /// <returns></returns>
        public async Task<string> SaveImage(StorageFile fichier)
        {
            var erreur = await ValidateImage(fichier);
            if (string.IsNullOrWhiteSpace(erreur))
            {
                //si ok conversion du fichier en image
                var bitmapImage = new BitmapImage();
                var stream = (FileRandomAccessStream)await fichier.OpenAsync(FileAccessMode.Read);
                bitmapImage.SetSource(stream);
                Affiche = ObjectUtils.ResizedImage(bitmapImage, (int)ContexteStatic.MaxSizeXAffiche, (int)ContexteStatic.MaxSizeYAffiche);

                //puis mise en mémoire en binaire pour la sauvegarde
                Film.Affiche = await ObjectUtils.ConvertFileToBytes(fichier);
                
            }
            return erreur;
        }


        



        #endregion

        #region validation

        /// <summary>
        /// Vérifie les informations du film avant la sauvegarde
        /// </summary>
        /// <returns>les erreurs à afficher</returns>
        private string Validate()
        {
            var erreur = "";

            if (string.IsNullOrWhiteSpace(Film.Titre))
            {
                erreur += ResourceLoader.GetForCurrentView().GetString("erreurTitreVide")+"\r\n";
            }
            return erreur;
        }

        /// <summary>
        ///  Sauvegarde un film en base
        /// </summary>
        /// <returns>les erreurs</returns>
        public async Task<string> Sauvegarder()
        {
            var erreur = Validate();
            if (string.IsNullOrWhiteSpace(erreur))
            {
                Film.Genres = new List<Genre>(ListeGenreSelected);
                Film.Acteurs = new List<Personne>(SelectedActeurListe);
                Film.Realisateurs = new List<Personne>(SelectedRealisateursListe);
                Film.Producteurs = new List<Personne>(SelectedProducteursListe);
                if (!string.IsNullOrEmpty(TitreCollection))
                {
                    Film.NomCollection = TitreCollection;
                }

                await _filmBusiness.SaveFilm(Film);
            }
            return erreur;
        }
        #endregion


        #region recherche internet
        /// <summary>
        /// Recherche un film sur internet à partir du titre
        /// </summary>
        /// <returns>0 = aucun résultat, 1 = 1 résultat, 2 = plusieurs résultats</returns>
        public async Task<int> RechercheFilm()
        {
            if (Film.Type == (int)TypeFilmEnum.SERIE)
            {
                SearchTvJson = await _movieDbBusiness.RechercheSerie(Film.Titre);
                if (SearchTvJson.total_results > 1)
                {
                    return 2;
                }

                if (SearchTvJson.total_results == 1)
                {
                    await GetFilmFromInternet(SearchTvJson.results[0].id);
                    return 1;
                }

                return 0;
            }
            else
            {
                FilmsJson = await _movieDbBusiness.RechercheFilm(Film.Titre);
                if (FilmsJson.total_results > 1)
                {
                    return 2;
                }

                if (FilmsJson.total_results == 1)
                {
                    await GetFilmFromInternet(FilmsJson.results[0].id);
                    return 1;
                }

                return 0;
            }
        }

        /// <summary>
        /// Charge un film à partir d'internet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task GetFilmFromInternet(int id)
        {
            var newFilm = (Film.Type == (int) TypeFilmEnum.SERIE) ? await _movieDbBusiness.GetSerie(id) : await _movieDbBusiness.GetFilm(id);
            newFilm.Id = Film.Id;
            newFilm.Voir = Film.Voir;
            newFilm.SouhaitVoir = Film.SouhaitVoir;
            newFilm.Souhait = Film.Souhait;
            newFilm.Posseder = Film.Posseder;
            newFilm.TypeSupport = Film.TypeSupport;
            newFilm.MaNote = Film.MaNote;
            newFilm.Lien = Film.Lien;
            newFilm.OrdreCollection = await _filmBusiness.GetNumeroSuivantCollectionInternet(newFilm.IdCollectionInternet, newFilm.Annee ?? -1, newFilm.IdInternet);
            Film = newFilm;
            SelectedProducteursListe = new List<Personne>(Film.Producteurs);
            SelectedRealisateursListe = new List<Personne>(Film.Realisateurs);
            SelectedActeurListe = new List<Personne>(Film.Acteurs);
            ListeGenreSelected = new List<Genre>(Film.Genres);
            Affiche = Film.AfficheImage;
            if (Film.IdCollectionInternet > 0)
            {
                TitreCollection = Film.NomCollection;
                if (ListeCollection.Count(x => x.NomCollection == TitreCollection) >= 1)
                {
                    SelectedCollection = ListeCollection.First(x => x.IdCollectionInternet == Film.IdCollectionInternet);
                }
            }
        }
        #endregion
        
    }
}
