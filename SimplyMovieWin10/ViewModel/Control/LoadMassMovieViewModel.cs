using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// ViewModel de la vue du chargement de masse
    /// </summary>
    public partial class LoadMassMovieViewModel : AbstractViewModel
    {
        private MovieDbBusiness _movieDbBusiness;

        private FilmBusiness _filmBusiness;

        private int _idData;

        /// <summary>
        /// Constructeur
        /// </summary>
        public LoadMassMovieViewModel()
        {
            Initialization = InitializeAsync();
            TextChanged = false;
        }

        public sealed override async Task InitializeAsync()
        {
            _movieDbBusiness = new MovieDbBusiness();
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
        }

        /// <summary>
        /// Scan un fichier pour en connaître la liste des films
        /// </summary>
        /// <param name="folder">le dossier racine</param>
        /// <returns></returns>
        public async Task ScanFolder(StorageFolder folder)
        {
            var listeFilms = new List<MassLoad>();
            _idData = 0;
            listeFilms = await Scan(folder);

            //pour chaque film trouvé, on recherche sur internet, si un résultat, on récupère lef ilm, si plusieurs ont garde la liste des trouvailles
            foreach (var film in listeFilms)
            {
                try
                {
                    var res = await _movieDbBusiness.RechercheFilm(film.Name);
                    Task.Delay(250).Wait();//délai d'attente pour éviter de dépasser les 40 requetes par 10 secondes
                    if (res.total_results == 1)
                    {
                        film.Movie = await GetFilm(res.results[0].id);
                        film.Resultat = film.Movie.Titre;
                        film.IsOk = true;
                        film.Color = "ForestGreen";
                    }
                    else if (res.total_results > 1)
                    {
                        film.Results = res;
                        film.Resultat = ResourceLoader.GetForCurrentView().GetString("PlusieursRes");
                        film.PlusieursRes = true;
                        film.Color = "OrangeRed";
                    }
                    else
                    {
                        film.Resultat = ResourceLoader.GetForCurrentView().GetString("AucunRes");
                        film.Color = "Firebrick";
                    }
                }
                catch (Exception)
                {

                }
            }
            ListeData = listeFilms;
        }

        /// <summary>
        /// Retourne un film à partir de son id internet
        /// </summary>
        /// <param name="idInternet">l'id internet du film</param>
        /// <returns>le film</returns>
        private async Task<Film> GetFilm(int idInternet)
        {
            var mov = await _movieDbBusiness.GetFilm(idInternet);
            mov.Posseder = true;
            mov.Type = (int) TypeFilmEnum.FILM;
            mov.MaNote = 3;
            mov.Id = 0;
            mov.SouhaitVoir = false;
            mov.TypeSupport = (int) TypeSupportEnum.FICHIER;
            mov.Voir = true;
            mov.Saison = null;
            return mov;
        }

        /// <summary>
        /// Méthode récursive pour récupérer tout les fichiers
        /// </summary>
        /// <param name="folder">le dossier à scanner</param>
        /// <returns>la liste de fichier trouvé dans le dossier</returns>
        private async Task<List<MassLoad>> Scan(IStorageFolder folder)
        {
            var liste = (from file in await folder.GetFilesAsync() where ContexteStatic.ListeExtension.Contains(file.FileType) select new MassLoad {Id = ++_idData, Path = file.Path, Name = file.DisplayName, MaNote = 3, IsFilmSelected = true, IsOk = false, IsAnimSelected = false, IsDocuSelected = false, IsSpecSelected = false}).ToList();

            foreach (var fold in await folder.GetFoldersAsync())
            {
                liste.AddRange(await Scan(fold));
            }

            return liste;
        }

        /// <summary>
        /// Efface un film de la liste
        /// </summary>
        /// <param name="id">l'id du massLoad à supprimer</param>
        public void DeleteMovie(int id)
        {
            ListeData.Remove(ListeData.First(x => x.Id == id));
            UpdateListeData();
        }

        /// <summary>
        /// Charge les infos nécéssaire pour faire apparaitre le dlg
        /// </summary>
        /// <param name="id">l'id du massLoad</param>
        public async Task<bool> LoadDlg(int id)
        {
            var retour = true;
            try
            {
                ListeFilmDlg?.Clear();
                //si il n'y a pas de liste, on cherche
                if (ListeData.First(x => x.Id == id).Results == null ||
                    ListeData.First(x => x.Id == id).Results?.total_results == 0 || TextChanged)
                {
                    var film = ListeData.First(x => x.Id == id);
                    var res = await _movieDbBusiness.RechercheFilm(film.Name);
                    if (res.total_results == 1)
                    {
                        film.Movie = await GetFilm(res.results[0].id);
                        film.Resultat = film.Movie.Titre;
                        film.IsOk = true;
                        film.PlusieursRes = false;
                        film.Color = "ForestGreen";
                        film.Results = res;
                    }
                    else if (res.total_results > 1)
                    {
                        film.Results = res;
                        film.Resultat = ResourceLoader.GetForCurrentView().GetString("PlusieursRes");
                        film.PlusieursRes = true;
                        film.Color = "OrangeRed";
                    }
                    else
                    {
                        film.Resultat = ResourceLoader.GetForCurrentView().GetString("AucunRes");
                        film.Color = "Firebrick";
                        film.IsOk = false;
                        film.PlusieursRes = false;
                        retour = false;
                    }
                    TextChanged = false;
                }

                if (ListeData.Count(x => x.Id == id) > 0)
                {
                    var tmp = ListeData.First(x => x.Id == id);
                    if (tmp.Results != null)
                    {
                       ListeFilmDlg = new List<ResultSearchMovieJson>(ListeData.First(x => x.Id == id).Results.results);
                    }
                    SelectedMassLoad = id;
                }
                return retour;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Récupère un film d'internet à partir de la dlg
        /// </summary>
        /// <param name="idFilmInternet">l'id internet</param>
        public async Task SelectMovieFmDlg(int idFilmInternet)
        {
            var film = await GetFilm(idFilmInternet);

            var massLoad = ListeData.First(x => x.Id == SelectedMassLoad);
            massLoad.Movie = film;
            massLoad.Resultat = film.Titre;
            massLoad.IsOk = true;
            massLoad.Color = "ForestGreen";
            UpdateListeData();
        }

        /// <summary>
        /// Sauvegarde la liste des films
        /// </summary>
        /// <returns>les erreurs s'il y en a</returns>
        public async Task<string> Sauvegarder()
        {
            var retour = Validate();
            if (string.IsNullOrWhiteSpace(retour))
            {
                foreach (var massLoad in ListeData)
                {
                    massLoad.Movie.MaNote = massLoad.MaNote;
                    if (massLoad.IsFilmSelected)
                    {
                        massLoad.Movie.Type = (int) TypeFilmEnum.FILM;
                    }
                    if (massLoad.IsDocuSelected)
                    {
                        massLoad.Movie.Type = (int)TypeFilmEnum.DOCUMENTAIRE;
                    }
                    if (massLoad.IsAnimSelected)
                    {
                        massLoad.Movie.Type = (int)TypeFilmEnum.ANIMATION;
                    }
                    if (massLoad.IsSpecSelected)
                    {
                        massLoad.Movie.Type = (int)TypeFilmEnum.SPECTACLE;
                    }
                    massLoad.Movie.Lien = massLoad.Path;

                    await _filmBusiness.SaveFilm(massLoad.Movie);
                }
            }
            return retour;
        }

        /// <summary>
        /// Vérifie les données avant la sauvegarde
        /// </summary>
        /// <returns>les erreurs</returns>
        public string Validate()
        {
            var retour = "";

            if (!ListeData.All(massLoad => massLoad.IsOk))
            {
                retour += ResourceLoader.GetForCurrentView().GetString("errFilmNoOk") +"\r\n";
            }
            return retour;
        }

        /// <summary>
        /// Met à jour la liste des données
        /// </summary>
        private void UpdateListeData()
        {
            var tmp = ListeData.ToList();
            ListeData.Clear();
            ListeData = tmp;
        }
    }
}
