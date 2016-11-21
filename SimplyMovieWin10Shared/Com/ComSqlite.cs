using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Model;
using SQLite;

namespace SimplyMovieWin10Shared.Com
{
    /// <summary>
    /// Singleton pour la connexion à la base de donnée SQLite (nécéssite le paquet qlite-net)
    /// </summary>
    public class ComSqlite
    {
        /// <summary>
        /// objet du singleton
        /// </summary>
        private static ComSqlite _comSqlite;

        /// <summary>
        /// connexion à la base
        /// </summary>
        public SQLiteAsyncConnection Connection { get; }

        private static string _path;


        /// <summary>
        /// retourne une instance de l'objet de connexion à la base de donnée
        /// </summary>
        /// <returns>l'instance de connexion</returns>
        public static async Task<ComSqlite> GetComSqlite()
        {
            if (_comSqlite == null)
            {
                _path = Path.Combine(ApplicationData.Current.LocalFolder.Path, ContexteStatic.FichierBdd);
                _comSqlite = new ComSqlite(_path);
                await _comSqlite.CreateDb();
            }
            return _comSqlite;
        }
        


        /// <summary>
        /// Constructeur créant la base si elle n'existe et vérifie si il faut effectuer des mises à jour
        /// </summary>
        /// <param name="nomFichier">le nom du fichier de la base de donnée</param>
        private ComSqlite(string nomFichier)
        {
            Connection = new SQLiteAsyncConnection(nomFichier);
        }
        
        
        /// <summary>
        /// Créer la base de donnée
        /// </summary>
        public async Task CreateDb()
        {
            await Connection.CreateTableAsync<Film>();
            await Connection.CreateTableAsync<Personne>();
            await Connection.CreateTableAsync<Genre>();
            await Connection.CreateTableAsync<GenreFilm>();
            await Connection.CreateTableAsync<PersonneFilm>();
            await Connection.CreateTableAsync<Application>();
            await Connection.CreateTableAsync<Collection>();
        }

        /// <summary>
        /// Supprime la base de donnée
        /// </summary>
        public async Task DropDb()
        {
            await Connection.DropTableAsync<Film>();
            await Connection.DropTableAsync<Personne>();
            await Connection.DropTableAsync<Genre>();
            await Connection.DropTableAsync<GenreFilm>();
            await Connection.DropTableAsync<PersonneFilm>();
            await Connection.DropTableAsync<Collection>();
        }

        /// <summary>
        /// ajoute une donnée en base
        /// </summary>
        /// <typeparam name="T">le type de donnée à ajouter</typeparam>
        /// <param name="data">la donnée</param>
        public async Task AjouterDonnee<T>(T data)
        {
            await Connection.InsertAsync(data);
        }

        /// <summary>
        /// Ajoute une liste de donnée à la base
        /// </summary>
        /// <typeparam name="T">le type de donnée à ajouter</typeparam>
        /// <param name="data">la liste des données</param>
        public async Task AjouterListeDonnee<T>(IEnumerable<T> data)
        {
            await Connection.InsertAllAsync(data);
        }

        /// <summary>
        /// met à jour une donnée
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la donnée</param>
        public async Task<int> UpdateDonnee<T>(T data)
        {
            return await Connection.UpdateAsync(data);
        }

        /// <summary>
        /// Met à jour plusieurs données
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la liste des données</param>
        public async Task<int> UpdateListeDonnee<T>(IEnumerable<T> data)
        {
            return await Connection.UpdateAllAsync(data);
        }

        /// <summary>
        /// efface une donnée
        /// </summary>
        /// <typeparam name="T">le type de donnée</typeparam>
        /// <param name="data">la donnée</param>
        public async Task<int> DeleteDonnee<T>(T data)
        {
            return await Connection.DeleteAsync(data);
        }

        /// <summary>
        /// Efface une liste de données
        /// </summary>
        /// <typeparam name="T">le type de données à effacer</typeparam>
        /// <param name="data">la liste de données à effacer</param>
        /// <returns>le nombre de ligne effacé</returns>
        public async Task<int> DeleteListeDonnee<T>(IEnumerable<T> data)
        {
            var i = 0;
            foreach (var dataToDelete in data)
            {
                await Connection.DeleteAsync(dataToDelete);
                i++;
            }
            return i;
        }


    }
}
