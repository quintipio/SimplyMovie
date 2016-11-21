using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Utils;

namespace SimplyMovieWin10Shared.Business
{
    /// <summary>
    /// Classe de communication avec la BDD pour l'import export
    /// </summary>
    public class ParamBusiness : AbstractBusiness
    {
        /// <summary>
        /// récupère les données en base à sauvegarder
        /// </summary>
        /// <returns>le model à sérializer</returns>
        public async Task<string> CreateDataToSave()
        {
            var data = await GetDataFmBdd();
            var xs = new XmlSerializer(typeof (SaveLoad));
            var wr = new StringWriter();
            xs.Serialize(wr, data);
            return wr.ToString();
        }

        /// <summary>
        /// Efface la base de donnée et charge les nouvelles données
        /// </summary>
        /// <param name="data">les données à charger</param>
        /// <returns></returns>
        public async Task LoadData(string data)
        {
            var xsb = new XmlSerializer(typeof (SaveLoad));
            var rd = new StringReader(data);
            var newData = xsb.Deserialize(rd) as SaveLoad;

            if (newData != null)
            {
                var oldData = await GetDataFmBdd();

                await Bdd.DropDb();
                await Bdd.CreateDb();
                try
                {
                    await SetDataFmBdd(newData);
                }
                catch (Exception)
                {
                    //en cas d'échec, restauration des données
                    await SetDataFmBdd(oldData);
                    throw;
                }
            }
        }

        /// <summary>
        /// Récupère toute les données de la base de donnée dans un saveLoad
        /// </summary>
        /// <returns></returns>
        private async Task<SaveLoad> GetDataFmBdd()
        {
            var data = new SaveLoad();
            data.ListeFilm.AddRange(await Bdd.Connection.Table<Film>().ToListAsync());
            data.ListeGenre.AddRange(await Bdd.Connection.Table<Genre>().ToListAsync());
            data.ListePersonne.AddRange(await Bdd.Connection.Table<Personne>().ToListAsync());
            data.ListePersonneFilm.AddRange(await Bdd.Connection.Table<PersonneFilm>().ToListAsync());
            data.ListeGenreFilm.AddRange(await Bdd.Connection.Table<GenreFilm>().ToListAsync());
            data.ListeCollection.AddRange(await Bdd.Connection.Table<Collection>().ToListAsync());
            return data;
        }

        /// <summary>
        /// Met en base toute les données d'un saveload
        /// </summary>
        /// <param name="data">les données à enregistrer</param>
        /// <returns></returns>
        private async Task SetDataFmBdd(SaveLoad data)
        {
            await Bdd.AjouterListeDonnee(data.ListeGenre);
            await Bdd.AjouterListeDonnee(data.ListeFilm);
            await Bdd.AjouterListeDonnee(data.ListePersonne);
            await Bdd.AjouterListeDonnee(data.ListeGenreFilm);
            await Bdd.AjouterListeDonnee(data.ListePersonneFilm);
            await Bdd.AjouterListeDonnee(data.ListeCollection);
        }

        /// <summary>
        /// Méthode pour vérifier si une mise à jour des données est nécéssaire
        /// </summary>
        /// <returns>true si nécéssaire</returns>
        public async Task<bool> NeedUpdate()
        {
            if ((await Bdd.Connection.Table<Application>().Where(x => x.Id >= 1).CountAsync()) <= 0)
            {
                return true;
            }
            else
            {
                var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
                return res.Version != ContexteStatic.Version;
            }
        }

        /// <summary>
        /// Vérifie la dernière version installée, et sinon met à jour
        /// </summary>
        /// <returns></returns>
        public async Task CheckVersion()
        {
            //on vérifie l'existence de la ligne appli
            var nbLigne = await Bdd.Connection.Table<Application>().Where(x => x.Id >= 1).CountAsync();

            if (nbLigne < 1)
            {
                var app = new Application {Id = 1,Version = "1.0.0"};
                await Bdd.AjouterDonnee(app);
            }

            var res = await Bdd.Connection.Table<Application>().Where(x => x.Id == 1).FirstOrDefaultAsync();
            if (res.Version != ContexteStatic.Version)
            {
                //si c'est une maj vers la version 1.2.0 recherche des collections, et maj des types
                if (StringUtils.CheckVersion(res.Version, "1.2.0"))
                {
                    try
                    {
                        await UpdateFilmForCollection();
                        await UpdateTypeFilm();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                res.Version = ContexteStatic.Version;
                await Bdd.UpdateDonnee(res);

            }
        }


        #region maj

        /// <summary>
        /// Met à jour les différents types de films
        /// </summary>
        /// <returns></returns>
        private async Task UpdateTypeFilm()
        {
            //récupération des id des genres
            var listeDocuLib = new List<string> {"documentaire","documental", "Documentário","documentario","documentary" };
            var listeAnimLib = new List<string> {"animation" ,"animated movie", "cartoon", "película de animación", "pelicula de animacion", "dibujos animados", "animación", "animacion", "animação", "animacao", "animaçao", "desenhos animados", "filme de animação", "filme de animacao", "filme de animaçao" };
            var listeSpecLib = new List<string> { "spectacle musical", "spectacle comique", "spectacle", "concert", "musical show", "comedy show", "show", "concert", "show musical", "mostrar", "concierto", "espectáculo", "espectaculo","musique", "music" , "música", "musica" };
            
            //récupération de tout les genres
            var listeGenre = await Bdd.Connection.Table<Genre>().ToListAsync();

            //récupération des genres par catégories
            var listeDocuId = listeGenre.Where(x => listeDocuLib.Contains(x.Nom.ToLower())).Select(x=> x.Id).ToList();
            var listeAnimId = listeGenre.Where(x => listeAnimLib.Contains(x.Nom.ToLower())).Select(x => x.Id).ToList();
            var listeSpecId = listeGenre.Where(x => listeSpecLib.Contains(x.Nom.ToLower())).Select(x => x.Id).ToList();

            //récupération des id de films
            var listeFilmDocu = (await Bdd.Connection.Table<GenreFilm>().Where(x => listeDocuId.Contains(x.IdGenre)).ToListAsync()).Select(x => x.IdFilm);
            var listeFilmAnim = (await Bdd.Connection.Table<GenreFilm>().Where(x => listeAnimId.Contains(x.IdGenre)).ToListAsync()).Select(x => x.IdFilm);
            var listeFilmSpec = (await Bdd.Connection.Table<GenreFilm>().Where(x => listeSpecId.Contains(x.IdGenre)).ToListAsync()).Select(x => x.IdFilm);

            //maj des films
            var listeFilmDocuObj = await Bdd.Connection.Table<Film>().Where(x => listeFilmDocu.Contains(x.Id) && x.Type == (int)TypeFilmEnum.FILM).ToListAsync();
            foreach (var x in listeFilmDocuObj)
            {
                x.Type = (int) TypeFilmEnum.DOCUMENTAIRE;
            }
            await Bdd.UpdateListeDonnee(listeFilmDocuObj);

            var listeFilmAnimObj = await Bdd.Connection.Table<Film>().Where(x => listeFilmAnim.Contains(x.Id) && x.Type == (int)TypeFilmEnum.FILM).ToListAsync();
            foreach (var x in listeFilmAnimObj)
            {
                x.Type = (int)TypeFilmEnum.ANIMATION;
            }
            await Bdd.UpdateListeDonnee(listeFilmAnimObj);

            var listeFilmSpecObj = await Bdd.Connection.Table<Film>().Where(x => listeFilmSpec.Contains(x.Id) && x.Type == (int)TypeFilmEnum.FILM).ToListAsync();
            foreach (var x in listeFilmSpecObj)
            {
                x.Type = (int)TypeFilmEnum.SPECTACLE;
            }
            await Bdd.UpdateListeDonnee(listeFilmSpecObj);
        }

        /// <summary>
        /// Ajoute les id de collections à tout les films
        /// </summary>
        /// <returns></returns>
        private async Task UpdateFilmForCollection()
        {
            var movieDbBusiness = new MovieDbBusiness();
            var filmBusiness = new FilmBusiness();
            await filmBusiness.Initialization;

            var listeFilm = await Bdd.Connection.Table<Film>().Where(x => x.IdInternet > 0 && x.Type == (int)TypeFilmEnum.FILM).ToListAsync();
            foreach (var film in listeFilm)
            {
                try
                {
                    //récupère les infos d'internet
                    var tuple = await movieDbBusiness.GetIdCollectionInternet(film.IdInternet);
                    Task.Delay(250).Wait();//délai d'attente pour éviter de dépasser les 40 requetes par 10 secondes
                    var idCollectionInternet = tuple.Item2;
                    var nomCollection = tuple.Item1;

                    //si il y a bien une collection pour ce film, on ajoute le film à la collection
                    if (idCollectionInternet > 0)
                    {
                        film.IdCollectionInternet = idCollectionInternet;
                        film.NomCollection = nomCollection;
                        

                        //ajout de l'id de la collection local
                        var idCollection = await filmBusiness.GetIdCollectionFromIdInternet(idCollectionInternet);
                        //si la collection n'existait pas on la créer
                        if (idCollection == -1)
                        {
                            film.OrdreCollection = 1;
                            await filmBusiness.AjouterFilmCollection(film);
                        }
                        else //si elle existe on récupère l'id et met à jour les ordres
                        {
                            film.IdCollection = idCollection;
                            var ordre =  await  filmBusiness.GetNumeroSuivantCollectionInternet(idCollectionInternet, film.Annee ?? -1,film.IdInternet);
                            film.OrdreCollection = ordre;
                            await Bdd.UpdateDonnee(film);
                            await filmBusiness.ReorganizeOrdreCollection(idCollection);
                        }

                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        #endregion
    }
}
