using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Utils;

namespace SimplyMovieWin10Shared.Business
{
    /// <summary>
    /// Business pour les films
    /// </summary>
    public class FilmBusiness : AbstractBusiness
    {
        /// <summary>
        /// nombre de film max à afficher dans une liste sur la page d'acceuil de l'appli
        /// </summary>
        private const int MaxFilmAcceuil = 6;

        #region Personnes

        /// <summary>
        /// Recherche une personne en base à partir du nom et prénom
        /// </summary>
        /// <param name="nom">le nom à rechercher</param>
        /// <returns>la personne trouvé sinon une nouvelle</returns>
        public async Task<Personne> GetPersonneFromBase(string nom)
        {
            Personne personne;
            if (await Bdd.Connection.Table<Personne>().Where(x => x.Nom.ToLower() == nom.ToLower()).CountAsync() > 0)
            {
                personne = await Bdd.Connection.Table<Personne>().Where(x => x.Nom.ToLower() == nom.ToLower()).FirstAsync();
            }
            else
            {
                personne = new Personne
                {
                    Id = 0,
                    Nom = nom
                };
            }
            return personne;
        }


        /// <summary>
        /// Retoune une liste de personnes complete
        /// </summary>
        /// <returns>la liste des personnes</returns>
        public async Task<List<Personne>> GetListePersonnes()
        {
            return await Bdd.Connection.Table<Personne>().ToListAsync();
        }


        /// <summary>
        /// donne un nouvel Id pour les personnes
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetNewIdPersonne()
        {
            var idMax = await Bdd.Connection.Table<Personne>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (idMax != null)
            {
                id = idMax.Id + 1;
            }
            return id;
        }

        /// <summary>
        /// Créer une personne en base de donnée
        /// </summary>
        /// <param name="personne">la personne à créer</param>
        /// <returns>la personne ajouté en base avec l'id</returns>
        private async Task<Personne> AddPersonne(Personne personne)
        {
            personne.Id = await GetNewIdPersonne();
            await Bdd.AjouterDonnee(personne);
            return personne;
        }

        /// <summary>
        /// Recherche une personne à partir de son nom prénom 
        /// </summary>
        /// <param name="nom">le nom de la personne</param>
        /// <returns>null si aucun sinon la personne</returns>
        private async Task<Personne> SearchPersonne(string nom)
        {
            var res = await Bdd.Connection.Table<Personne>().Where(x => x.Nom.ToLower() == nom.ToLower()).ToListAsync();
            return res.Count > 0 ? res[0] : null;
        }

        /// <summary>
        /// Supprime les personnes d'un film
        /// </summary>
        /// <param name="film">le film dont les genres sont à effacer</param>
        /// <returns></returns>
        public async Task RemovePersonneFilm(Film film)
        {
            var personneFilms = await Bdd.Connection.Table<PersonneFilm>().Where(x => x.IdFilm == film.Id).ToListAsync();
            if (personneFilms != null && personneFilms.Count > 0)
            {
                await Bdd.DeleteListeDonnee(personneFilms);
            }
        }


        /// <summary>
        /// donne un nouvel Id pour les personnes des films
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetNewIdPersonneFilm()
        {
            var idMax = await Bdd.Connection.Table<PersonneFilm>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (idMax != null)
            {
                id = idMax.Id + 1;
            }
            return id;
        }
        #endregion


        #region Film

        /// <summary>
        /// Fait passer un film à vu
        /// </summary>
        /// <param name="id">l'id du film</param>
        /// <returns></returns>
        public async Task PasserFilmVu(int id)
        {
            var film =await  Bdd.Connection.Table<Film>().Where(x => x.Id == id).FirstAsync();
            film.SouhaitVoir = false;
            film.Voir = true;
            await Bdd.UpdateDonnee(film);
        }


        /// <summary>
        /// Fait passer un film à acheter
        /// </summary>
        /// <param name="id">l'id du film</param>
        /// <returns></returns>
        public async Task PasserFilmAcheter(int id)
        {
            var film = await Bdd.Connection.Table<Film>().Where(x => x.Id == id).FirstAsync();
            film.Posseder = true;
            film.Souhait = false;
            await Bdd.UpdateDonnee(film);
        }

        /// <summary>
        /// Sauvegarde un film en base
        /// </summary>
        /// <param name="film">le film à sauvegarder</param>
        /// <returns></returns>
        public async Task SaveFilm(Film film)
        {
            var isCreate = film.Id == 0;
            //si création, récup d'un nouvel ID
            if (isCreate)
            {
                film.Id = await GetNewIdFilm();
            }
            else
            {
                //si modif, on efface les tables de jointures genre/ personne du film
                await RemoveGenreFilm(film);
                await RemovePersonneFilm(film);

                //et on efface l'id de la collection éventuelle à laquelle il peut appartenir
                if (film.IdCollection > 0)
                {
                    await SupprimerFilmCollection(film.Id);
                }
            }

            //Création des genres s'ils n'existent pas, puis jointure avec le film
            foreach (var genre in film.Genres)
            {
                if (genre.Id == 0)
                {
                    var newGenre = await AddGenre(genre);
                    genre.Id = newGenre.Id;
                }

                var joinFilmGenre = new GenreFilm
                {
                    Id = await GetNewIdGenreFilm(),
                    IdFilm = film.Id,
                    IdGenre = genre.Id
                };
                await Bdd.AjouterDonnee(joinFilmGenre);
            }

            //Création de la liste des acteurs
            if (film.Acteurs != null && film.Acteurs.Count > 0)
            {
                foreach (var personne in film.Acteurs)
                {
                    var res = await SearchPersonne(personne.Nom);
                    if (res == null)
                    {
                        var newPersonne = await AddPersonne(personne);
                        personne.Id = newPersonne.Id;
                    }
                    else
                    {
                        personne.Id = res.Id;
                    }
                    var personneFilm = new PersonneFilm
                    {
                        Id = await GetNewIdPersonneFilm(),
                        IdFilm = film.Id,
                        IdPersonne = personne.Id,
                        Role = (int)TypePersonneEnum.ACTEUR
                    };
                    await Bdd.AjouterDonnee(personneFilm);
                }
            }

            //Création de la liste des producteurs
            if (film.Producteurs != null && film.Producteurs.Count > 0)
            {
                foreach (var personne in film.Producteurs)
                {
                    var res = await SearchPersonne(personne.Nom);
                    if (res == null)
                    {
                        var newPersonne = await AddPersonne(personne);
                        personne.Id = newPersonne.Id;
                    }
                    else
                    {
                        personne.Id = res.Id;
                    }
                    var personneFilm = new PersonneFilm
                    {
                        Id = await GetNewIdPersonneFilm(),
                        IdFilm = film.Id,
                        IdPersonne = personne.Id,
                        Role = (int)TypePersonneEnum.PRODUCTEUR
                    };
                    await Bdd.AjouterDonnee(personneFilm);
                }
            }

            //Création de la liste des réalisateurs
            if (film.Realisateurs != null && film.Realisateurs.Count > 0)
            {
                foreach (var personne in film.Realisateurs)
                {
                    var res = await SearchPersonne(personne.Nom);
                    if (res == null)
                    {
                        var newPersonne = await AddPersonne(personne);
                        personne.Id = newPersonne.Id;
                    }
                    else
                    {
                        personne.Id = res.Id;
                    }
                    var personneFilm = new PersonneFilm
                    {
                        Id = await GetNewIdPersonneFilm(),
                        IdFilm = film.Id,
                        IdPersonne = personne.Id,
                        Role = (int)TypePersonneEnum.REALISATEUR
                    };
                    await Bdd.AjouterDonnee(personneFilm);
                }
            }

            //Création du film en base
            if (isCreate)
            {
                await Bdd.AjouterDonnee(film);
            }
            else
            {
                await Bdd.UpdateDonnee(film);
            }
            
            //si le film doit appartenir à une collection, lien avec cette dernière
            if (!string.IsNullOrEmpty(film.NomCollection))
            {
                await AjouterFilmCollection(film);
            }
        }

        /// <summary>
        /// Interroge la base pour récupérer une liste de titre des films
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<Film>> GetListeTitreFilm()
        {
            return (await Bdd.Connection.Table<Film>().ToListAsync()).Select(film => new Film {Id = film.Id, Titre = film.Titre+ " - "+film.Annee}).ToList();
        }

        /// <summary>
        /// Retourne un nouvel id pour les films
        /// </summary>
        /// <returns>le nouvel Id</returns>
        private async Task<int> GetNewIdFilm()
        {
            var idMax = await Bdd.Connection.Table<Film>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (idMax != null)
            {
                id = idMax.Id + 1;
            }
            return id;
        }

        /// <summary>
        /// Met en place les infos du film
        /// </summary>
        /// <param name="film">le film à modifier</param>
        /// <param name="chargePersonne">indique si il faut charger les personnes</param>
        /// <param name="chargerImage">indique si il faut charger les images </param>
        /// <returns>le film modifié</returns>
        private async Task<Film> SetInfosFilm(Film film, bool chargePersonne,bool chargerImage)
        {
            //init
            film.Acteurs = new List<Personne>();
            film.Producteurs = new List<Personne>();
            film.Realisateurs = new List<Personne>();
            film.Genres = new List<Genre>();

            if (chargePersonne)
            {
                //Acteurs
                var listeActeurs = await Bdd.Connection.Table<PersonneFilm>().Where(x => x.IdFilm == film.Id && x.Role == (int)TypePersonneEnum.ACTEUR).ToListAsync();
                foreach (var acteur in listeActeurs)
                {
                    if (acteur.IdPersonne > 0)
                    {
                        var personne = await Bdd.Connection.Table<Personne>().Where(x => x.Id == acteur.IdPersonne).FirstAsync();
                        if (personne != null)
                        {
                            personne.Role = TypePersonneEnum.ACTEUR;
                            personne.NomScene = acteur.NomScene;
                            film.Acteurs.Add(personne);
                        }
                    }

                }

                //Producteurs
                var listeProd = await Bdd.Connection.Table<PersonneFilm>().Where(x => x.IdFilm == film.Id && x.Role == (int)TypePersonneEnum.PRODUCTEUR).ToListAsync();
                foreach (var producteur in listeProd)
                {
                    if (producteur.IdPersonne > 0)
                    {
                        var personne = await Bdd.Connection.Table<Personne>().Where(x => x.Id == producteur.IdPersonne).FirstAsync();
                        if (personne != null)
                        {
                            personne.Role = TypePersonneEnum.PRODUCTEUR;
                            film.Producteurs.Add(personne);
                        }
                    }

                }

                //Réalisateur
                var listeReal = await Bdd.Connection.Table<PersonneFilm>().Where(x => x.IdFilm == film.Id && x.Role == (int)TypePersonneEnum.REALISATEUR).ToListAsync();
                foreach (var realisateur in listeReal)
                {
                    if (realisateur.IdPersonne > 0)
                    {
                        var personne = await Bdd.Connection.Table<Personne>().Where(x => x.Id == realisateur.IdPersonne).FirstAsync();
                        if (personne != null)
                        {
                            personne.Role = TypePersonneEnum.REALISATEUR;
                            film.Realisateurs.Add(personne);
                        }
                    }
                }
            }
            

            //Genre
            var listeGenre = await Bdd.Connection.Table<GenreFilm>().Where(x => x.IdFilm == film.Id).ToListAsync();
            foreach (var genre in listeGenre)
            {
                var genr = await Bdd.Connection.Table<Genre>().Where(x => x.Id == genre.IdGenre).FirstAsync();
                if (genr != null)
                {
                    film.Genres.Add(genr);
                }
            }

            //affiche
            if (chargerImage && film.Affiche != null && film.Affiche.Length > 0)
            {
                film.AfficheImage = await ObjectUtils.ConvertBytesToBitmap(film.Affiche);
            }
            else
            {
                film.AfficheImage = new BitmapImage(ContexteStatic.UriAfficheDefaut);
            }
            film.AfficheImage = ObjectUtils.ResizedImage(film.AfficheImage, (int)ContexteStatic.MaxSizeXAffiche,
                (int)ContexteStatic.MaxSizeYAffiche);
            return film;
        }

        /// <summary>
        /// Charge un film à partir de son ID
        /// </summary>
        /// <param name="idFilm">l'id du film à charger</param>
        /// <returns>le film</returns>
        public async Task<Film> GetFilm(int idFilm)
        {
            var film = await Bdd.Connection.Table<Film>().Where(x => x.Id == idFilm).FirstAsync();
            if (film != null)
            {
                return await SetInfosFilm(film,true,true);
            }
            return null;
        }
        

        /// <summary>
        /// Supprime un film de la base de donnée
        /// </summary>
        /// <param name="film">le film à supprimer</param>
        /// <returns></returns>
        public async Task SupprimerFilm(Film film)
        {
            await RemoveGenreFilm(film);
            await RemovePersonneFilm(film);
            await SupprimerFilmCollection(film.Id);
            await Bdd.DeleteDonnee(film);
        }

        /// <summary>
        /// regarde si un film avec un idinternet est présent en base
        /// </summary>
        /// <param name="idInternet">l'id internet</param>
        /// <param name="type">le type de données recherchée (film ou série)</param>
        /// <returns>le film trouvé sinon null</returns>
        public async Task<Film> IsFilmInternetPresentEnBase(int idInternet,int type)
        {
            var listeIdType = new List<int>();
            if (type == (int)TypeFilmEnum.FILM || type == (int)TypeFilmEnum.ANIMATION || type == (int)TypeFilmEnum.DOCUMENTAIRE || type == (int)TypeFilmEnum.SPECTACLE )
            {
                listeIdType.Add((int)TypeFilmEnum.FILM);
                listeIdType.Add((int)TypeFilmEnum.ANIMATION);
                listeIdType.Add((int)TypeFilmEnum.SPECTACLE);
                listeIdType.Add((int)TypeFilmEnum.DOCUMENTAIRE);
            }
            if (type == (int)TypeFilmEnum.SERIE)
            {
                listeIdType.Add((int)TypeFilmEnum.SERIE);
            }

            if (await Bdd.Connection.Table<Film>().Where(x => x.IdInternet == idInternet && listeIdType.Contains(x.Type)).CountAsync() > 0)
            {
                var filmTrouve = await Bdd.Connection.Table<Film>().Where(x => x.IdInternet == idInternet && listeIdType.Contains(x.Type)).FirstAsync();
                return await SetInfosFilm(filmTrouve,false,false);
            }
            return null;
        }


        #endregion

        #region Collection de films

        /// <summary>
        /// Recupère une collection en base
        /// </summary>
        /// <param name="id">l'id de la collection</param>
        /// <returns>la collection</returns>
        public async Task<Collection> GetCollection(int id)
        {
            if (id > 0)
            {
                var collection = await Bdd.Connection.Table<Collection>().Where(x => x.Id == id).FirstAsync();
                var listeTmp = await Bdd.Connection.Table<Film>().Where(x => x.IdCollection == id).ToListAsync();
                collection.FilmCollection = new List<Film>();
                foreach (var film in listeTmp)
                {
                    collection.FilmCollection.Add(await SetInfosFilm(film, false, true));
                }
                return collection;
            }
            return null;
        }

        /// <summary>
        /// Retourne toute les colelctions en base
        /// </summary>
        /// <returns>la liste des collections</returns>
        public async Task<List<Collection>> GetListeCollection()
        {
            return await Bdd.Connection.Table<Collection>().ToListAsync();
        }
        
        /// <summary>
        /// Retourne un nouvel id d'une collection
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetNewIdCollection()
        {
            var idMax = await Bdd.Connection.Table<Collection>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (idMax != null)
            {
                id = idMax.Id + 1;
            }
            return id;
        }
        
        /// <summary>
        /// Retourne une collection à partir de son titre sinon
        /// </summary>
        /// <param name="titre">le titre de la collection recherchée</param>
        /// <returns>la collection sinon null</returns>
        private async Task<Collection> GetCollectionByTitre(string titre)
        {
            if ((await Bdd.Connection.Table<Collection>().Where(x => x.NomCollection == titre).CountAsync()) > 0)
            {
                return await Bdd.Connection.Table<Collection>().Where(x => x.NomCollection == titre).FirstAsync();
            }
            return null;
        }

        /// <summary>
        /// Ajoute un film à une collection, et créer la collection si elle n'existe pas
        /// </summary>
        /// <param name="film">le film contenant es informations pour la collection (le titre de la collection et l'id internet de la collection si elle n'existe pas encore)</param>
        /// <returns></returns>
        public async Task AjouterFilmCollection(Film film)
        {
            //on ajoute le film à la collection où on la crée si elle n'existe pas
            var collection = await GetCollectionByTitre(film.NomCollection);
            if (collection != null)
            {
                //mise à jour du film
                var f = await Bdd.Connection.Table<Film>().Where(x => x.Id == film.Id).FirstAsync();
                f.IdCollection = collection.Id;
                await Bdd.UpdateDonnee(f);

                //mise à jour de l'ordre des films
                await ReorganizeOrdreCollection(collection.Id);
            }
            else
            {
                //Création de la collection
                var newCollec = new Collection
                {
                    Id = await GetNewIdCollection(),
                    NomCollection = film.NomCollection,
                    IdCollectionInternet = film.IdCollectionInternet
                };
                await Bdd.AjouterDonnee(newCollec);
                var f = await Bdd.Connection.Table<Film>().Where(x => x.Id == film.Id).FirstAsync();
                f.IdCollection = newCollec.Id;
                await Bdd.UpdateDonnee(f);
            }
        }

        /// <summary>
        /// Reorganise les nuémros d'ordre de la collection
        /// </summary>
        /// <param name="idCollection">l'id de la colection</param>
        /// <returns></returns>
        public async Task ReorganizeOrdreCollection(int idCollection)
        {
            var liste = (await Bdd.Connection.Table<Film>().Where(x => x.IdCollection == idCollection).ToListAsync()).OrderBy(x => x.OrdreCollection).ThenBy(x => x.Annee).ThenBy(x => x.IdInternet);
            var lastOrdre = 0;

            foreach (var film in liste.Where(film => film.OrdreCollection == 0))
            {
                film.OrdreCollection++;
                await UpdateOrdreFilm(film.Id);
            }

            liste = liste.OrderBy(x => x.OrdreCollection).ThenBy(x => x.Annee).ThenBy(x => x.IdInternet);

            foreach (var film in liste)
            {

                if (film.OrdreCollection == lastOrdre)
                {
                    film.OrdreCollection = await UpdateOrdreFilm(film.Id);
                }
                lastOrdre = film.OrdreCollection;
            }
        }

        /// <summary>
        /// met à jour l'ordre d'un film
        /// </summary>
        /// <param name="idFilm">l'id du film</param>
        /// <returns>le nouvel ordre</returns>
        private async Task<int> UpdateOrdreFilm(int idFilm)
        {
            var film = await Bdd.Connection.Table<Film>().Where(x => x.Id == idFilm).FirstAsync();
            film.OrdreCollection++;
            await Bdd.UpdateDonnee(film);
            return film.OrdreCollection;
        }


        /// <summary>
        /// Supprime un film d'une collection et la collection avec si le film est le dernier y appartemant
        /// </summary>
        /// <param name="idFilm">l'id du film concerné</param>
        /// <returns></returns>
        private async Task SupprimerFilmCollection(int idFilm)
        {
            if (await Bdd.Connection.Table<Film>().Where(x => x.Id == idFilm).CountAsync() > 0)
            {
                var film = await Bdd.Connection.Table<Film>().Where(x => x.Id == idFilm).FirstAsync();
                //si il n'y a plus qu'une seule occurence du film lié à la la collection, on supprime la collection avant de supprimer le film de la collection
                if (await Bdd.Connection.Table<Film>().Where(x => x.IdCollection == film.IdCollection).CountAsync() == 1)
                {
                    if (await Bdd.Connection.Table<Collection>().Where(x => x.Id == film.IdCollection).CountAsync() > 0)
                    {
                        var collec = await Bdd.Connection.Table<Collection>().Where(x => x.Id == film.IdCollection).FirstAsync();
                        await Bdd.DeleteDonnee(collec);
                    }
                }
                
                //supression du film de la liste des films de la collection
                film.IdCollection = 0;
                film.OrdreCollection = 0;
                await Bdd.UpdateDonnee(film);
            }
        }
        
        /// <summary>
        /// Retourne le numéro d'ordre théorique du film dans sa collection
        /// </summary>
        /// <param name="idCollectionInternet">l'id internet de la collection</param>
        /// <param name="yearFilm">l'année de sortie du film sinon -1</param>
        /// <param name="idInternet">l'id internet du film</param>
        /// <returns></returns>
        public async Task<int> GetNumeroSuivantCollectionInternet(int idCollectionInternet, int yearFilm,int idInternet)
        {
            if (idCollectionInternet > 0)
            {
                if ((await  Bdd.Connection.Table<Collection>() .Where(x => x.IdCollectionInternet == idCollectionInternet).CountAsync()) >= 1)
                {
                    var collec = await Bdd.Connection.Table<Collection>() .Where(x => x.IdCollectionInternet == idCollectionInternet).FirstAsync();
                    int res;
                    if (yearFilm != -1)
                    {
                        res = await Bdd.Connection.Table<Film>().Where(x => x.IdCollection == collec.Id && x.Annee <= yearFilm).CountAsync();
                    }
                    else
                    {
                        res = await Bdd.Connection.Table<Film>().Where(x => x.IdCollection == collec.Id && x.IdInternet < idInternet).CountAsync();
                    }
                    return res+1;
                }
                return 1;
            }
            return 0;
        }
        
        /// <summary>
        /// Retoune l'id d'une collection à partir de son id internet
        /// </summary>
        /// <param name="idCollectionInternet">l'id internet de la collection</param>
        /// <returns>l'id de la collection</returns>
        public async Task<int> GetIdCollectionFromIdInternet(int idCollectionInternet)
        {
            var collec = await Bdd.Connection.Table<Collection>().Where(x => x.IdCollectionInternet == idCollectionInternet).ToListAsync();
            if (collec == null ||(collec.Count == 0))
            {
                return -1;
            }
            else
            {
                return collec[0].Id;
            }  
        }
        
        #endregion

        #region Genre

        /// <summary>
        /// Recherche un genre en base à partir du nom
        /// </summary>
        /// <param name="nom">le nom à rechercher</param>
        /// <returns>le genre trouvé sinon un nouveau</returns>
        public async Task<Genre> GetGenreFromBase(string nom)
        {
            Genre genre;
            if (await Bdd.Connection.Table<Genre>().Where(x => x.Nom.ToLower() == nom.ToLower()).CountAsync() > 0)
            {
                genre = await Bdd.Connection.Table<Genre>().Where(x => x.Nom.ToLower() == nom.ToLower()).FirstAsync();
            }
            else
            {
                genre = new Genre
                {
                    Id = 0,
                    Nom = nom
                };
            }
            return genre;
        }

        /// <summary>
        /// Retourne la liste complète des genres
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<Genre>> GetListeGenre()
        {
            return await Bdd.Connection.Table<Genre>().ToListAsync();
        }

        /// <summary>
        /// Supprime les genres d'un film
        /// </summary>
        /// <param name="film">le film dont les genres sont à effacer</param>
        /// <returns></returns>
        public async Task RemoveGenreFilm(Film film)
        {
            var genreFilm = await Bdd.Connection.Table<GenreFilm>().Where(x => x.IdFilm == film.Id).ToListAsync();
            if (genreFilm != null && genreFilm.Count > 0)
            {
                await Bdd.DeleteListeDonnee(genreFilm);
            }
        }

        /// <summary>
        /// Créer un genre de film en base de donnée
        /// </summary>
        /// <param name="genre">le genre à créer</param>
        /// <returns>le genre ajouté en base avec l'id</returns>
        private async Task<Genre> AddGenre(Genre genre)
        {
            genre.Id = await GetNewIdGenre();
            await Bdd.AjouterDonnee(genre);
            return genre;
        }

        /// <summary>
        /// donne un nouvel Id pour les genres
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetNewIdGenre()
        {
            var idMax = await Bdd.Connection.Table<Genre>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (idMax != null)
            {
                id = idMax.Id + 1;
            }
            return id;
        }

        /// <summary>
        /// donne un nouvel Id pour les genres des films
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetNewIdGenreFilm()
        {
            var idMax = await Bdd.Connection.Table<GenreFilm>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var id = 1;
            if (idMax != null)
            {
                id = idMax.Id + 1;
            }
            return id;
        }
        #endregion


        #region Recherche

        /// <summary>
        /// Retourne la bibliothèque de film
        /// </summary>
        /// <returns>la liste des films</returns>
        public async Task<List<Film>> GetBibliotheque()
        {
            //récupère les films
            var liste = await Bdd.Connection.Table<Film>().ToListAsync();
            var listeRetour = new List<Film>();
            
            liste = await TriCollectionTitre(liste);

            foreach (var film in liste)
            {
                listeRetour.Add(await SetInfosFilm(film, true, false));
            }
            return listeRetour;
        }


        /// <summary>
        /// Recherche les films d'un genre spécifique
        /// </summary>
        /// <param name="genre">le genre de film recherché</param>
        /// <returns>le résultat</returns>
        public async Task<List<Film>> GetFilm(Genre genre)
        {
            var listeId = (await Bdd.Connection.Table<GenreFilm>().Where(x => x.IdGenre == genre.Id).ToListAsync()).Select(genreFilm => genreFilm.IdFilm).ToList();
            var listeFilm =  (await Bdd.Connection.Table<Film>().Where(x => listeId.Contains(x.Id)).ToListAsync()).OrderBy(x => x.Type).ThenBy(x => x.Titre);
            var retour = new List<Film>();
            foreach (var film in listeFilm)
            {
                retour.Add(await SetInfosFilm(film,true,false));
            }
            return retour;
        }

        /// <summary>
        /// Recherche les films avec une personne spécifique
        /// </summary>
        /// <param name="personne">la personne du film recherché</param>
        /// <returns>le résultat</returns>
        public async Task<List<Film>> GetFilm(Personne personne)
        {
            var listeId = (await Bdd.Connection.Table<PersonneFilm>().Where(x => x.IdPersonne == personne.Id).ToListAsync()).Select(personneFilm => personneFilm.IdFilm).ToList();
            var listeFilm = (await Bdd.Connection.Table<Film>().Where(x => listeId.Contains(x.Id)).ToListAsync()).OrderBy(x => x.Type).ThenBy(x => x.Titre);
            var retour = new List<Film>();
            foreach (var film in listeFilm)
            {
                retour.Add(await SetInfosFilm(film,true,false));
            }
            return retour;
        }

        /// <summary>
        /// Retourne une liste de film filtré
        /// </summary>
        /// <param name="filterA">le filtre appliqué</param>
        /// <param name="filterB">le deuxième filtre appliqué</param>
        /// <returns>la liste de film</returns>
        public async Task<List<Film>> GetFilm(FilterBibliothequeEnum filterA,FilterBibliothequeEnum filterB)
        {
            var liste = new List<Film>();
            Expression<Func<Film, bool>> conditionA = null ;
            Expression<Func<Film, bool>> conditionB = null ;
            
            if (filterA == FilterBibliothequeEnum.FILMPOSSEDEAVOIR)
            {
                conditionA = (x => x.Posseder == true && x.SouhaitVoir == true);
            }

            if (filterA == FilterBibliothequeEnum.FILMPOSSEDE)
            {
                conditionA = (x => x.Posseder == true);
            }

            if (filterA == FilterBibliothequeEnum.FILMNONPOSSEDE)
            {
                conditionA = (x => x.Posseder == false);
            }

            if (filterA == FilterBibliothequeEnum.FILMNONPOSSEDEAVOIR)
            {
                conditionA = (x => x.Posseder == false && x.SouhaitVoir == true);
            }

            if (filterA == FilterBibliothequeEnum.FILMFAVORIS)
            {
                conditionA = (x => x.MaNote >= 4);
            }



            if (filterB == FilterBibliothequeEnum.FILM)
            {
                conditionB = x => x.Type == (int) TypeFilmEnum.FILM;
            }

            if (filterB == FilterBibliothequeEnum.SERIE)
            {
                conditionB = x => x.Type == (int)TypeFilmEnum.SERIE;
            }

            if (filterB == FilterBibliothequeEnum.SPECTACLE)
            {
                conditionB = x => x.Type == (int)TypeFilmEnum.SPECTACLE;
            }

            if (filterB == FilterBibliothequeEnum.DOCUMENTAIRE)
            {
                conditionB = x => x.Type == (int)TypeFilmEnum.DOCUMENTAIRE;
            }

            if (filterB == FilterBibliothequeEnum.ANIMATION)
            {
                conditionB = x => x.Type == (int)TypeFilmEnum.ANIMATION;
            }

            if (filterB == FilterBibliothequeEnum.COLLECTION)
            {
                conditionB = x => x.IdCollection > 0;
            }

            if (conditionB != null && conditionA != null)
            {
                 liste = await Bdd.Connection.Table<Film>().Where(conditionA).Where(conditionB).ToListAsync();
            }

            else if (conditionB == null && conditionA != null)
            {
                liste = await Bdd.Connection.Table<Film>().Where(conditionA).ToListAsync();
            }

            else if (conditionB != null && conditionA == null)
            {
                liste = await Bdd.Connection.Table<Film>().Where(conditionB).ToListAsync();
            }
            

            var listeFin = new List<Film>();
            if (liste.Count > 0)
            {
                liste = await TriCollectionTitre(liste);

                foreach (var film in liste)
                {
                    listeFin.Add(await SetInfosFilm(film, true, false));
                }
            }
            return listeFin;
        }

        /// <summary>
        /// Tri par collection, par ordre et par titre une liste de films
        /// </summary>
        /// <param name="liste">la liste de films à trier</param>
        /// <returns>la liste trier</returns>
        private async Task<List<Film>> TriCollectionTitre(List<Film> liste)
        {
            var listeCollection = await Bdd.Connection.Table<Collection>().ToListAsync();

            foreach (var film in liste)
            {
                if (film.IdCollection > 0)
                {
                    film.NomCollection =
                    listeCollection.Where(x => x.Id == film.IdCollection)
                        .Select(x => x.NomCollection)
                        .FirstOrDefault();
                }
                else
                {
                    film.NomCollection = film.Titre;
                }
            }

            liste = liste.OrderBy(x => x.NomCollection).ThenBy(x => x.OrdreCollection).ThenBy(x => x.Titre).ToList();

            foreach (var film in liste.Where(film => film.IdCollection == 0))
            {
                film.NomCollection = "";
            }
            return liste;
        }

        /// <summary>
        /// Recherche un film en base à partir d'un mot clé (regarde dans les genres, noms de personnes, et titre du film)
        /// </summary>
        /// <param name="query">le mot clé</param>
        /// <returns>la liste es films trouvés</returns>
        public async Task<List<Film>> RechercheFilmToutCritere(string query)
        {
            var listeIdFilm = new List<int>();
            var listeFilmRetour = new List<Film>();

            //recherche dans les personnes
            var listePersonne =
                (await
                    Bdd.Connection.Table<Personne>().Where(x => x.Nom.ToLower().Contains(query.ToLower())).ToListAsync())
                    .Select(x => x.Id).ToList();

            if (listePersonne != null && listePersonne.Count > 0)
            {
                listeIdFilm.AddRange((await
                Bdd.Connection.Table<PersonneFilm>().Where(x => listePersonne.Contains(x.IdPersonne)).ToListAsync())
                .Select(x => x.IdFilm).ToList());
            }

            //recherche dans les genres

            var listeGenre =
                (await
                    Bdd.Connection.Table<Genre>().Where(x => x.Nom.ToLower().Contains(query.ToLower())).ToListAsync())
                    .Select(x => x.Id).ToList();

            if (listeGenre != null && listeGenre.Count > 0)
            {
                listeIdFilm.AddRange((await
                Bdd.Connection.Table<GenreFilm>().Where(x => listeGenre.Contains(x.IdGenre)).ToListAsync())
                .Select(x => x.IdFilm).ToList());
            }

            //recherche dans les films
            var listeFilmTmp =
                (await
                    Bdd.Connection.Table<Film>().Where(x => x.Titre.ToLower().Contains(query.ToLower())).ToListAsync())
                    .Select(x => x.Id).ToList();

            if (listeFilmTmp != null && listeFilmTmp.Count > 0)
            {
                listeIdFilm.AddRange(listeFilmTmp);
            }

            //récupération des films
            if (listeIdFilm.Count > 0)
            {
                var listeFilms = await Bdd.Connection.Table<Film>().Where(x => listeIdFilm.Contains(x.Id)).ToListAsync();
                foreach (var f in listeFilms)
                {
                    listeFilmRetour.Add(await SetInfosFilm(f, false, true));
                }
            }
            return listeFilmRetour;
        }

        #endregion


        #region recherche Acceuil


        /// <summary>
        /// Retourne une suggestion des film à voir
        /// </summary>
        /// <returns>la liste des films</returns>
        public async Task<List<Film>> GetFilmSuggestionVoir()
        {
            var listeFilm = (await Bdd.Connection.Table<Film>().Where(x => x.Posseder == true && x.SouhaitVoir == true).ToListAsync());
            var max = (listeFilm.Count() < MaxFilmAcceuil) ? listeFilm.Count() : MaxFilmAcceuil;
            var listeFilmFin = new List<Film>();
            for (var i = 0; i < max; i++)
            {
                var passe = false;
                do
                {
                    var toto = Random.Next(0, listeFilm.Count);
                    var filmAleatoire = listeFilm[toto];
                    if (listeFilmFin.Count(x => x.Id == filmAleatoire.Id) == 0)
                    {
                        listeFilmFin.Add(filmAleatoire);
                        passe = true;
                    }
                } while (!passe);
            }

            if (!ContexteAppli.IsCortanaActive)
            {
                var retour = new List<Film>();
                foreach (var film in listeFilmFin)
                {
                    retour.Add(await SetInfosFilm(film, false, true));
                }
                return retour;
            }
            else
            {
                return listeFilmFin;
            }
            
        }

        /// <summary>
        /// Retourne une suggestion des film à voir
        /// </summary>
        /// <returns>la liste des films</returns>
        public async Task<List<Film>> GetFilmSuggestionPosseder()
        {
            var listeFilm = await Bdd.Connection.Table<Film>().Where(x => x.Souhait == true).ToListAsync();
            var max = (listeFilm.Count() < MaxFilmAcceuil) ? listeFilm.Count() : MaxFilmAcceuil;
            var listeFilmFin = new List<Film>();
            for (var i = 0; i < max; i++)
            {
                var passe = false;
                do
                {
                    var toto = Random.Next(0, listeFilm.Count);
                    var filmAleatoire = listeFilm[toto];
                    if (listeFilmFin.Count(x => x.Id == filmAleatoire.Id) == 0)
                    {
                        listeFilmFin.Add(filmAleatoire);
                        passe = true;
                    }
                } while (!passe);
            }

            if (!ContexteAppli.IsCortanaActive)
            {
                var retour = new List<Film>();
                foreach (var film in listeFilmFin)
                {
                    retour.Add(await SetInfosFilm(film, false, true));
                }
                return retour;
            }
            else
            {
                return listeFilmFin;
            }
        }

        /// <summary>
        /// Retourne les films les mieux notés
        /// </summary>
        /// <returns>la liste des films</returns>
        public async Task<List<Film>> GetFilmSuggestionFavoris()
        {
            var listeFilm = (await Bdd.Connection.Table<Film>().Where(x => x.MaNote >= 4).ToListAsync()).OrderByDescending(x => x.MaNote).ToList();
            var max = (listeFilm.Count() < MaxFilmAcceuil) ? listeFilm.Count() : MaxFilmAcceuil;
            var listeFilmFin = new List<Film>();
            for (var i = 0; i < max; i++)
            {
                var passe = false;
                do
                {
                    var toto = Random.Next(0, listeFilm.Count);
                    var filmAleatoire = listeFilm[toto];
                    if (listeFilmFin.Count(x => x.Id == filmAleatoire.Id) == 0)
                    {
                        listeFilmFin.Add(filmAleatoire);
                        passe = true;
                    }
                } while (!passe);
            }

            if (!ContexteAppli.IsCortanaActive)
            {
                var retour = new List<Film>();
                foreach (var film in listeFilmFin)
                {
                    retour.Add(await SetInfosFilm(film, false, true));
                }
                return retour;
            }
            else
            {
                return listeFilmFin;
            }
        }

        /// <summary>
        /// Récupère des films aléatoire de la base de donnée
        /// </summary>
        /// <returns>la liste de films</returns>
        public async Task<List<Film>> GetFilmSuggestionAleatoire()
        {
            var listeFilm = new List<Film>();
            var nbFilm = await Bdd.Connection.Table<Film>().CountAsync();
            if (nbFilm <= MaxFilmAcceuil)
            {
                listeFilm = await Bdd.Connection.Table<Film>().ToListAsync();
            }
            else
            {
                var listeFilmTmp = await Bdd.Connection.Table<Film>().ToListAsync();
                var listeInt = new List<int>();
                for (var i = 0; i < MaxFilmAcceuil; i++)
                {
                    var isPasse = false;
                    do
                    {
                        var al = Random.Next(0, listeFilmTmp.Count);
                        if (!listeInt.Contains(al))
                        {
                            listeInt.Add(al);
                            isPasse = true;
                        }
                    } while (!isPasse);
                    
                }

                foreach (var entier in listeInt)
                {
                    listeFilm.Add(listeFilmTmp[entier]);
                }
            }


            if (!ContexteAppli.IsCortanaActive)
            {
                var retour = new List<Film>();
                foreach (var film in listeFilm)
                {
                    retour.Add(await SetInfosFilm(film, false, true));
                }
                return retour;
            }
            else
            {
                return listeFilm;
            }
        }

        #endregion

        
    }
}
