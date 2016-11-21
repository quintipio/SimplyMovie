
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Enum;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;
using SimplyMovieWin10Shared.Strings;
using SimplyMovieWin10Shared.Utils;

namespace SimplyMovieWin10Shared.Business
{
    /// <summary>
    /// Classe pour communiquer avec internet
    /// </summary>
    public class MovieDbBusiness
    {
        private const string AdresseRootPhotoActeur = "https://image.tmdb.org/t/p/w640/";
        private const string AddresseRootAffiche = "http://image.tmdb.org/t/p/original";

        #region Film sur internet
        /// <summary>
        /// recherche un film sur movieDb
        /// </summary>
        /// <param name="recherche">le titre du film à rechercher</param>
        /// <returns>la résultat en Json Object</returns>
        public async Task<SearchMovieJson> RechercheFilm(string recherche) 
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.SEARCHMOVIE, recherche,0);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchMovieJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (SearchMovieJson)serializer.ReadObject(ms);
                foreach (var resultSearchMovieJson in result.results)
                {
                    resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                    resultSearchMovieJson.title += " ("+ DateUtils.GetYearFromString(resultSearchMovieJson.release_date) + ")";
                }
                return result;
            }
        }

        /// <summary>
        /// Retourne un film à partir de son id
        /// </summary>
        /// <param name="id">l'id du film</param>
        /// <returns>l'objet Film</returns>
        public async Task<Film> GetFilm(int id)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETMOVIE, id.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(MovieJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var res = (MovieJson)serializer.ReadObject(ms);
                return await DataJsonToFilm(res,null);
            }
        }

        /// <summary>
        /// Retourne un film et toute ses données à partir de l'id
        /// </summary>
        /// <param name="id">l'id du film</param>
        /// <returns>un objet contenant toute les infos utiles liées au film</returns>
        public async Task<DataFromInternet> GetFilmJson(int id)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETMOVIE, id.ToString(), 0);
            using (var client = new HttpClient())
            {
                var retour = new DataFromInternet();
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(MovieJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var film = (MovieJson)serializer.ReadObject(ms);

                retour.Film = film;
                retour.Film.vote_average = (retour.Film.vote_average > 0 )?retour.Film.vote_average/2:0;
                retour.Casting = await GetCasting(film.id, TypeFilmEnum.FILM,true);
                retour.Affiche =(BitmapImage) (await GetAffiche(film.poster_path, false))[0];
                if (film.belongs_to_collection?.id != null && film.belongs_to_collection?.id > 0)
                {
                    retour.Collection = await GetCollectionInternet(film.belongs_to_collection.id);
                    foreach (var partJson in retour.Collection.parts)
                    {
                        partJson.affiche = (BitmapImage) (await GetAffiche(partJson.poster_path, false))[0];
                    }
                }
                retour.SimilarMovie = await GetSimilarMovieLight(film.id);
                return retour;
            }
        }

        /// <summary>
        /// Recherche les films similaire à partir de l'id d'un film en version allégé
        /// </summary>
        /// <param name="idInternet">l'id internet du film</param>
        /// <returns>une liste de films trouvés</returns>
        public async Task<List<ResultSearchMovieJson>> GetSimilarMovieLight(int idInternet)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.DEMANDESIMILARFILM, idInternet.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchMovieJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var retourResult = (SearchMovieJson)serializer.ReadObject(ms);
                var liste = new List<ResultSearchMovieJson>();
                if (retourResult?.results != null && retourResult.results.Count > 0)
                {
                    foreach (var resultSearchMovieJson in retourResult.results)
                    {
                        resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                        liste.Add(resultSearchMovieJson);
                    }
                }
                return liste;
            }
        }


        /// <summary>
        /// Recherche les films similaire à partir de l'id d'un film
        /// </summary>
        /// <param name="idInternet">l'id internet du film</param>
        /// <returns>une liste de films trouvés</returns>
        public async Task<List<Film>> GetSimilarMovie(int idInternet)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.DEMANDESIMILARFILM, idInternet.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchMovieJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var retourResult = (SearchMovieJson)serializer.ReadObject(ms);
                var liste = new List<Film>();
                if (retourResult?.results != null && retourResult.results.Count > 0)
                {
                    foreach (var resultSearchMovieJson in retourResult.results)
                    {
                        liste.Add(await GetFilm(resultSearchMovieJson.id));
                    }
                }
                return liste;
            }
        }

        /// <summary>
        /// Retourne l'objet des json pour les films de la même collections
        /// </summary>
        /// <param name="idCollection">l'id de la collection</param>
        /// <returns>la liste des films</returns>
        private async Task<ResultCollection> GetCollectionInternet(int idCollection)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETCOLLECTION, idCollection.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof (ResultCollection));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                return (ResultCollection) serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// Recherche les films d'une collection à partir de l'id d'une collection
        /// </summary>
        /// <param name="idCollection">l'id de la collection</param>
        /// <returns>une liste de films trouvés</returns>
        public async Task<List<Film>> GetCollectionMovie(int idCollection)
        {
            var retourResult = await GetCollectionInternet(idCollection);
                var liste = new List<Film>();
                if (retourResult?.parts != null && retourResult.parts.Count > 0)
                {
                    foreach (var result in retourResult.parts)
                    {
                        liste.Add(await GetFilm(result.id));
                    }
                }
                return liste;
        }

        #endregion

        #region Series sur internet

        /// <summary>
        /// recherche une série sur movieDb
        /// </summary>
        /// <param name="recherche">le titre de la série à rechercher</param>
        /// <returns>la résultat en Json Object</returns>
        public async Task<SearchTvJson> RechercheSerie(string recherche)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.SEARCHTV, recherche, 0);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchTvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (SearchTvJson)serializer.ReadObject(ms);
                foreach (var resultSearchMovieJson in result.results)
                {
                    resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                }
                return result;
            }

        }

        /// <summary>
        /// Retourne une série à partir de son id
        /// </summary>
        /// <param name="id">l'id de la série</param>
        /// <returns>l'objet Film</returns>
        public async Task<Film> GetSerie(int id)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETTV, id.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(TvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var res = (TvJson)serializer.ReadObject(ms);
                return await DataJsonToFilm(null,res);
            }
        }

        /// <summary>
        /// Retourne une série et toute ses données à partir de l'id
        /// </summary>
        /// <param name="id">l'id de la série</param>
        /// <returns>un objet contenant toute les infos utiles liées à la série</returns>
        public async Task<DataFromInternet> GetSerieJson(int id)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETTV, id.ToString(), 0);
            using (var client = new HttpClient())
            {
                var retour = new DataFromInternet();
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(TvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var tv = (TvJson)serializer.ReadObject(ms);

                retour.Tv = tv;
                retour.Tv.vote_average = (retour.Tv.vote_average > 0)?retour.Tv.vote_average/2:0;
                retour.Casting = await GetCasting(tv.id, TypeFilmEnum.SERIE, true);
                retour.Affiche = (BitmapImage)(await GetAffiche(tv.poster_path, false))[0];
                retour.SimilarTv = await GetSimilarTvLight(tv.id);
                return retour;
            }
        }

        /// <summary>
        /// Recherche les séries similaire à partir de l'id d'une srie en version allégé
        /// </summary>
        /// <param name="idInternet">l'id internet de la série</param>
        /// <returns>une liste des séries trouvés</returns>
        public async Task<List<ResultSearchTvJson>> GetSimilarTvLight(int idInternet)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.DEMANDESIMILARTV, idInternet.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchTvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var retourResult = (SearchTvJson)serializer.ReadObject(ms);
                var liste = new List<ResultSearchTvJson>();
                if (retourResult?.results != null && retourResult.results.Count > 0)
                {
                    foreach (var resultSearchMovieJson in retourResult.results)
                    {
                        resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                        liste.Add(resultSearchMovieJson);
                    }
                }
                return liste;
            }
        }


        /// <summary>
        /// Recherche les séries similaire à partir de l'id d'une série
        /// </summary>
        /// <param name="idInternet">l'id internet de la série</param>
        /// <returns>une liste des séries trouvées</returns>
        public async Task<List<Film>> GetSimilarSerie(int idInternet)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.DEMANDESIMILARTV, idInternet.ToString(), 0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchTvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var retourResult = (SearchTvJson)serializer.ReadObject(ms);
                var liste = new List<Film>();
                if (retourResult?.results != null && retourResult.results.Count > 0)
                {
                    foreach (var resultSearchTvJson in retourResult.results)
                    {
                        liste.Add(await GetSerie(resultSearchTvJson.id));
                    }
                }
                return liste;
            }
        }

        /// <summary>
        /// Retourne les informations d'une saison sur une série
        /// </summary>
        /// <param name="saison">la saison demandé</param>
        /// <param name="idSerie">la série concerné</param>
        /// <returns>les données</returns>
        public async Task<SearchSeasonJson> GetSaisonTv(int saison, int idSerie)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.TVSAISON, idSerie+"/season/"+saison,0);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchSeasonJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var retourResult = (SearchSeasonJson)serializer.ReadObject(ms);
                
                return retourResult;
            }
        }
        #endregion

        #region Personnes

        /// <summary>
        /// Retourne une personne à partir de son id
        /// </summary>
        /// <param name="id">l'id de la personne</param>
        /// <returns>un objet contenant toute les infos utiles liées à la personne</returns>
        public async Task<DataFromInternet> GetPersonneJson(int id)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETPERSON, id.ToString(), 0);
            using (var client = new HttpClient())
            {
                //la personne
                var retour = new DataFromInternet();
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(PersonJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var person = (PersonJson)serializer.ReadObject(ms);

                //ces films
                lien = ConstructeurLien(MovieDbRequeteEnum.GETPERSONCREDIT, id.ToString(), 0);
                response = await client.GetAsync(new Uri(lien));
                jsonString = await response.Content.ReadAsStringAsync();
                serializer = new DataContractJsonSerializer(typeof(CreditPersonJson));
                ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var creditPerson = (CreditPersonJson)serializer.ReadObject(ms);
                creditPerson.cast = creditPerson.cast.OrderByDescending(x => x.release_date).ThenByDescending(x => x.first_air_date).ToList();

                //la photo
                retour.Affiche = new BitmapImage(new Uri(AdresseRootPhotoActeur + person.profile_path));
                
                //retour
                retour.Person = person;
                retour.CreditPerson = creditPerson;

                creditPerson.crew = creditPerson.crew.OrderBy(x => x.media_type).ToList();
                creditPerson.cast = creditPerson.cast.OrderBy(x => x.media_type).ToList();

                foreach (var crewPersonJson in creditPerson.crew)
                {
                    if (string.IsNullOrEmpty(crewPersonJson.title))
                    {
                        crewPersonJson.title = crewPersonJson.name;
                    }

                    if (string.IsNullOrEmpty(crewPersonJson.release_date))
                    {
                        crewPersonJson.release_date = crewPersonJson.first_air_date;
                    }
                }

                foreach (var castPersonJson in creditPerson.cast)
                {
                    if (string.IsNullOrEmpty(castPersonJson.title))
                    {
                        castPersonJson.title = castPersonJson.name;
                    }

                    if (string.IsNullOrEmpty(castPersonJson.release_date))
                    {
                        castPersonJson.release_date = castPersonJson.first_air_date;
                    }
                }
                return retour;
            }
        }

        #endregion

        #region recherche

        /// <summary>
        /// Retourne la liste des films du moment au cinéma
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<ResultSearchMovieJson>> GetNowPlayingMovie()
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.FILMDUMOMENT, null, 0);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(NowPlayingJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (NowPlayingJson)serializer.ReadObject(ms);
                foreach (var resultSearchMovieJson in result.results)
                {
                    resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                }
                return result.results;
            }
        }

        /// <summary>
        /// Retourne la liste des films populaires
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<ResultSearchMovieJson>> GetPopularMovie()
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.FILMPOPULAIRE, null, 0);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(NowPlayingJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (NowPlayingJson)serializer.ReadObject(ms);

                if (!ContexteAppli.IsCortanaActive)
                {
                    foreach (var resultSearchMovieJson in result.results)
                    {
                        resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                    }
                }
                
                return result.results;
            }
        }


        /// <summary>
        /// Retourne la liste des séries du moment à la TV
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<ResultSearchTvJson>> GetNowPlayingTv()
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.TVDUMOMENT, null, 0);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchTvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (SearchTvJson)serializer.ReadObject(ms);
                foreach (var resultSearchMovieJson in result.results)
                {
                    resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                }
                return result.results;
            }
        }

        /// <summary>
        /// Retourne la liste des séries populaires
        /// </summary>
        /// <returns>la liste</returns>
        public async Task<List<ResultSearchTvJson>> GetPopularSerie()
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.TVPOPULAIRE, null, 0);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchTvJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (SearchTvJson)serializer.ReadObject(ms);

                if (!ContexteAppli.IsCortanaActive)
                {
                    foreach (var resultSearchMovieJson in result.results)
                    {
                        resultSearchMovieJson.affiche = (BitmapImage)(await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                    }
                }
                   
                return result.results;
            }
        }


        /// <summary>
        /// Lance une recherche générale sur les séries film et acteurs
        /// </summary>
        /// <param name="query">la recherche</param>
        /// <param name="page">le numéro de page à lire</param>
        /// <returns>le résultat</returns>
        public async Task<SearchGeneralJson> RechercheGenerale(string query,int page)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.RECHERCHEGENERALE, query, page);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(SearchGeneralJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var result = (SearchGeneralJson)serializer.ReadObject(ms);

                foreach (var resultSearchMovieJson in result.results)
                {
                    if (resultSearchMovieJson.media_type != "person")
                    {
                        var image = (BitmapImage) (await GetAffiche(resultSearchMovieJson.poster_path, false))[0];
                        resultSearchMovieJson.affiche = image;
                    }
                    else
                    {
                        var image = new BitmapImage(ContexteStatic.UriAfficheDefaut);
                        resultSearchMovieJson.affiche = image;
                    }
                }
                result.query = query;

                return result;
            }
        }

        #endregion

        #region commun

        /// <summary>
        /// Construit le lien pour interroger le site de film
        /// </summary>
        /// <param name="requete">le type de demande à faire au site</param>
        /// <param name="query">le paramètre de recherche</param>
        /// <param name="page">indique la page de consultation, si non utile 0</param>
        /// <returns>le lien pour le json</returns>
        private string ConstructeurLien(MovieDbRequeteEnum requete, string query,int page)
        {
            var linkBase = "https://api.themoviedb.org/3";

            //emplacement de la requete
            switch (requete)
            {
                case MovieDbRequeteEnum.SEARCHMOVIE:
                    linkBase += "/search/movie";
                    break;
                case MovieDbRequeteEnum.GETMOVIE:
                    linkBase += "/movie/" + query;
                    break;
                case MovieDbRequeteEnum.DEMANDEAFFICHEMOVIE:
                    linkBase += "/movie/" + query + "/images";
                    break;
                case MovieDbRequeteEnum.DEMANDECASTINGMOVIE:
                    linkBase += "/movie/" + query + "/credits";
                    break;
                case MovieDbRequeteEnum.DEMANDESIMILARFILM:
                    linkBase += "/movie/" + query + "/similar";
                    break;


                case MovieDbRequeteEnum.SEARCHTV:
                    linkBase += "/search/tv";
                    break;
                case MovieDbRequeteEnum.GETTV:
                    linkBase += "/tv/" + query;
                    break;
                case MovieDbRequeteEnum.DEMANDEAFFICHETV:
                    linkBase += "/tv/" + query + "/images";
                    break;
                case MovieDbRequeteEnum.DEMANDECASTINGTV:
                    linkBase += "/tv/" + query + "/credits";
                    break;
                case MovieDbRequeteEnum.DEMANDESIMILARTV:
                    linkBase += "/tv/" + query + "/similar";
                    break;
                case MovieDbRequeteEnum.TVSAISON:
                    linkBase += "/tv/" + query;
                    break;

                case MovieDbRequeteEnum.RECHERCHEGENERALE:
                    linkBase += "/search/multi";
                    break;

                case MovieDbRequeteEnum.GETPERSON:
                    linkBase += "/person/"+query;
                    break;
                case MovieDbRequeteEnum.GETPERSONCREDIT:
                    linkBase += "/person/" + query + "/combined_credits";
                    break;

                case MovieDbRequeteEnum.FILMDUMOMENT:
                    linkBase += "/movie/now_playing";
                    break;
                case MovieDbRequeteEnum.FILMPOPULAIRE:
                    linkBase += "/movie/popular";
                    break;
                case MovieDbRequeteEnum.TVDUMOMENT:
                    linkBase += "/tv/on_the_air";
                    break;
                case MovieDbRequeteEnum.TVPOPULAIRE:
                    linkBase += "/tv/popular";
                    break;

                case MovieDbRequeteEnum.GETCOLLECTION:
                    linkBase += "/collection/"+query;
                    break;
            }

            //ajout de l'api key
            linkBase += "?api_key=" + ContexteStatic.ApiKeyMovieDb;

            //infos optionnelles
            switch (requete)
            {
                case MovieDbRequeteEnum.SEARCHMOVIE:
                    linkBase += "&query=" + query.Replace(' ', '+');
                    break;
                case MovieDbRequeteEnum.SEARCHTV:
                    linkBase += "&query=" + query.Replace(' ', '+');
                    break;
                case MovieDbRequeteEnum.RECHERCHEGENERALE:
                    linkBase += "&query=" + query.Replace(' ', '+');
                    if (page > 0)
                    {
                        linkBase += "&page=" + page;
                    }
                    break;
            }


            //ajout de la langue
            linkBase += "&language=" + ListeLangues.GetLangueAppareil();
            linkBase += "&include_image_language=" + ListeLangues.GetLangueAppareil() + ",null";

            /*if (ContexteAppli.IsCortanaActive)
            {
                linkBase += "&language=" + ListeLangues.GetLangueAppareil();
                linkBase += "&include_image_language=" + ListeLangues.GetLangueAppareil() + ",null";
            }
            else
            {
                linkBase += "&language=" + ListeLangues.GetLangueEnCours().diminutif;
                linkBase += "&include_image_language=" + ListeLangues.GetLangueEnCours().diminutif + ",null";
            }*/

            return linkBase;
        }
        
        /// <summary>
        /// retourne les affiches en binaire et en bitmap d'un film
        /// </summary>
        /// <param name="path">le chemin du fichier</param>
        /// <param name="isBinUse">indique si il faut ou non le fichier en binaire</param>
        /// <returns>en 0 l'image en bitmap en 1 l'image en binaire</returns>
        private async Task<List<object>> GetAffiche(string path,bool isBinUse)
        {
            var rootLink = AddresseRootAffiche + path;
            var image =new BitmapImage((!string.IsNullOrEmpty(path)) ? new Uri(rootLink):ContexteStatic.UriAfficheDefaut);
            byte[] imageBin = null;
            var listeRetour = new List<object>();

            if (isBinUse  && !string.IsNullOrEmpty(path))
            {
                //chargement du fichier en binaire
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(rootLink))
                    {
                        response.EnsureSuccessStatusCode();
                        imageBin = await response.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            
            listeRetour.Add(image);
            listeRetour.Add((isBinUse && !string.IsNullOrEmpty(path)) ?imageBin:null);
            return listeRetour;
        }

        /// <summary>
        /// Retourne le casting d'un film
        /// </summary>
        /// <param name="id">:l'id du film</param>
        /// <param name="type">indique si il s'agit d'un film u d'une série</param>
        /// <param name="needImage">true si il faut récupérer l'image</param>
        /// <returns>l'objet Json du casting</returns>
        private async Task<CreditJson> GetCasting(int id, TypeFilmEnum type,bool needImage)
        {
            try
            {
                var lien = ConstructeurLien((type == TypeFilmEnum.FILM) ? MovieDbRequeteEnum.DEMANDECASTINGMOVIE : MovieDbRequeteEnum.DEMANDECASTINGTV, id.ToString(), 0);
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(lien));
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var serializer = new DataContractJsonSerializer(typeof(CreditJson));
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                    var data = (CreditJson)serializer.ReadObject(ms);

                    if (needImage)
                    {
                        foreach (var cr in data.crew)
                        {
                            cr.affiche = new BitmapImage(new Uri(AdresseRootPhotoActeur + cr.profile_path)); 
                        }

                        foreach (var ca in data.cast)
                        {
                            ca.affiche = new BitmapImage(new Uri(AdresseRootPhotoActeur + ca.profile_path));
                        }
                    }
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        #endregion

        #region Conversion objet

        /// <summary>
        /// Converti un film de movie db en film
        /// </summary>
        /// <param name="dataMovie">les données d'un film sinon null</param>
        /// <param name="dataTv">les données d'une série sinon null</param>
        /// <returns>le film</returns>
        public async Task<Film> DataJsonToFilm(MovieJson dataMovie,TvJson dataTv)
        {
            if (dataMovie != null)
            {
                var cast = await GetCasting(dataMovie.id, TypeFilmEnum.FILM,false);
                var resImage = await GetAffiche(dataMovie.poster_path, true);
                var film = new Film
                {
                    Id = 0,
                    Titre = dataMovie.title,
                    Annee = DateUtils.GetYearFromString(dataMovie.release_date),
                    Duree = (int?)dataMovie.runtime,
                    NoteGen = (dataMovie.vote_average > 0 )?dataMovie.vote_average / 2:0,
                    MaNote = (dataMovie.vote_average > 0) ? dataMovie.vote_average / 2 : 0,
                    Synopsis = dataMovie.overview,
                    Genres = await GetGenreMovie(dataMovie.genres),
                    Type = (int)TypeFilmEnum.FILM,
                    IdInternet = dataMovie.id,
                    IdCollectionInternet = dataMovie.belongs_to_collection?.id ?? 0,
                    NomCollection = dataMovie.belongs_to_collection?.name ?? "",
                };

                if (cast != null)
                {
                    film.Producteurs = await GetPersonnesMovie(cast, TypePersonneEnum.PRODUCTEUR);
                    film.Realisateurs = await GetPersonnesMovie(cast, TypePersonneEnum.REALISATEUR);
                    film.Acteurs = await GetPersonnesMovie(cast, TypePersonneEnum.ACTEUR);
                }

                if (resImage != null)
                {
                    film.Affiche = (byte[])resImage[1];
                    film.AfficheImage = (BitmapImage)resImage[0];
                }

                if (film.AfficheImage != null)
                {
                    film.AfficheImage = ObjectUtils.ResizedImage(film.AfficheImage, (int)ContexteStatic.MaxSizeXAffiche,
                        (int)ContexteStatic.MaxSizeYAffiche);
                }

                return film;
            }

            if (dataTv != null)
            {
                var cast = await GetCasting(dataTv.id, TypeFilmEnum.SERIE,false);
                var resImage = await GetAffiche(dataTv.poster_path, true);
                var film = new Film
                {
                    Id = 0,
                    Titre = dataTv.name,
                    Annee = DateUtils.GetYearFromString(dataTv.first_air_date),
                    Duree = (dataTv.episode_run_time.Any())?dataTv.episode_run_time.Min():0,
                    NoteGen = dataTv.vote_average,
                    Synopsis = dataTv.overview,
                    Genres = await GetGenreMovie(dataTv.genres),
                    Type = (int)TypeFilmEnum.SERIE,
                    IdInternet = dataTv.id,
                };

                if (cast != null)
                {
                    film.Producteurs = await GetPersonnesMovie(cast, TypePersonneEnum.PRODUCTEUR);
                    film.Realisateurs = await GetPersonnesMovie(cast, TypePersonneEnum.REALISATEUR);
                    film.Acteurs = await GetPersonnesMovie(cast, TypePersonneEnum.ACTEUR);
                }

                if (resImage != null)
                {
                    film.Affiche = (byte[])resImage[1];
                    film.AfficheImage = (BitmapImage)resImage[0];
                }

                if (film.AfficheImage != null)
                {
                    film.AfficheImage = ObjectUtils.ResizedImage(film.AfficheImage, (int)ContexteStatic.MaxSizeXAffiche,
                        (int)ContexteStatic.MaxSizeYAffiche);
                }
                return film;
            }
            return null;
        }

        /// <summary>
        /// Obtiens les genres du film en recherchant ceux déjà présent en base
        /// </summary>
        /// <param name="liste">la liste des genres à convertir</param>
        /// <returns>les genres en objet</returns>
        private async Task<List<Genre>> GetGenreMovie(IEnumerable<GenreJson> liste)
        {
            var filmBusiness = new FilmBusiness();
            await filmBusiness.Initialization;

            var retour = new List<Genre>();
            foreach (var genreJson in liste)
            {
                retour.Add(await filmBusiness.GetGenreFromBase(genreJson.name));
            }
            return retour;
        }

        private async Task<List<Personne>> GetPersonnesMovie(CreditJson credit, TypePersonneEnum typePersonne)
        {
            var filmBusiness = new FilmBusiness();
            await filmBusiness.Initialization;

            var retour = new List<Personne>();

            switch (typePersonne)
            {
                    case TypePersonneEnum.PRODUCTEUR:
                    if (credit.crew != null)
                    {
                        var found = credit.crew.Where(x => x.job.ToLower().Contains("producer")).Take(5).ToList();
                        foreach (var crewJson in found)
                        {
                            var personne = await filmBusiness.GetPersonneFromBase(crewJson.name);
                            personne.Role = TypePersonneEnum.PRODUCTEUR;
                            retour.Add(personne);
                        }
                    }
                    break;

                case TypePersonneEnum.REALISATEUR:
                    if (credit.crew != null)
                    {
                        var foundB = credit.crew.Where(x => x.job.ToLower() == "director").ToList();
                        foreach (var crewJson in foundB)
                        {
                            var personne = await filmBusiness.GetPersonneFromBase(crewJson.name);
                            personne.Role = TypePersonneEnum.REALISATEUR;
                            retour.Add(personne);
                        }
                    }
                    break;

                case TypePersonneEnum.ACTEUR:
                    if (credit.cast != null)
                    {
                        var foundC = credit.cast.OrderBy(x => x.order).Take(5).ToList();
                        foreach (var castJson in foundC)
                        {
                            var personne = await filmBusiness.GetPersonneFromBase(castJson.name);
                            personne.Role = TypePersonneEnum.ACTEUR;
                            personne.NomScene = castJson.character;
                            retour.Add(personne);
                        }
                    }

                    break;
            }
            return retour;
        }

        #endregion


        #region update

        public struct  RetourCollection
        {
             public string titre { get; set; }
            public int id { get; set; }
            
        }

        /// <summary>
        /// pour un film, retourne l'id de sa collection internet associé s'il existe
        /// </summary>
        /// <param name="idFilm">l'id du film</param>
        /// <returns>l'id de la collection et le titre de cette dernière</returns>
        public async Task<Tuple<string,int>> GetIdCollectionInternet(int idFilm)
        {
            var lien = ConstructeurLien(MovieDbRequeteEnum.GETMOVIE, idFilm.ToString(), 0);
            
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(lien));
                var jsonString = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(MovieJson));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                var res = (MovieJson)serializer.ReadObject(ms);
                return (res.belongs_to_collection?.id != null) ? new Tuple<string,int> ( res.belongs_to_collection.name , res.belongs_to_collection.id ) : new Tuple<string,int> (null,0);
            }
        }

        #endregion
    }
}
