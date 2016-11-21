using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Context;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Controleur pour la vue de la recherche
    /// </summary>
    public partial class RechercheViewModel : AbstractViewModel
    {
        private MovieDbBusiness _movieDbBusiness;

        private FilmBusiness _filmBusiness;

        /// <summary>
        /// Constructeur
        /// </summary>
        public RechercheViewModel()
        {
            Initialization = InitializeAsync();
        }

        public sealed override async Task InitializeAsync()
        {
            _movieDbBusiness = new MovieDbBusiness();
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
            ListeFilm = new ObservableCollection<ResultSearchGenJson>();
            ListeSerie = new ObservableCollection<ResultSearchGenJson>();
            ListePersonne = new ObservableCollection<ResultSearchGenJson>();
            pageEnCours = 0;
            DispoMaBibliotheque = false;
            DispoFilms = false;
            DispoSeries = false;
            DispoPersonnes = false;
            DispoPlusResult = false;
        }

        /// <summary>
        /// Charge les premières données lors de l'ouverture de la vue
        /// </summary>
        /// <param name="search">les donénes trouvées</param>
        public void ChargerDonnees(SearchGeneralJson search)
        {
            pageEnCours = 1;
            Titre = search.query;
            query = search.query;
            nbPageMax = search.total_pages;
            DispoPlusResult = search.total_pages > 1;
            PreparerDonneeRecherche(search);
        }

        /// <summary>
        /// Réparti les données trouvées dans les trois liste
        /// </summary>
        /// <param name="search">le résulat des recherches de movie Db</param>
        public void PreparerDonneeRecherche(SearchGeneralJson search)
        {
            var imageVide = new BitmapImage(ContexteStatic.UriAfficheDefaut);
            foreach (var res in search.results.Where(x => x.media_type == "movie"))
            {
                if (res.affiche == null)
                {
                    res.affiche = imageVide;
                }
                if (string.IsNullOrEmpty(res.title))
                {
                    res.title = res.name;
                }
                ListeFilm.Add(res);
            }
            if (ListeFilm.Count > 0)
            {
                DispoFilms = true;
            }
            foreach (var res in search.results.Where(x => x.media_type == "tv"))
            {
                if (res.affiche == null)
                {
                    res.affiche = imageVide;
                }
                if (string.IsNullOrEmpty(res.title))
                {
                    res.title = res.name;
                }
                ListeSerie.Add(res);
            }
            if (ListeSerie.Count > 0)
            {
                DispoSeries = true;
            }
            foreach (var res in search.results.Where(x => x.media_type == "person"))
            {
                if (res.affiche == null)
                {
                    res.affiche = imageVide;
                }
                if (string.IsNullOrEmpty(res.title))
                {
                    res.title = res.name;
                }
                ListePersonne.Add(res);
            }
            if (ListePersonne.Count > 0)
            {
                DispoPersonnes = true;
            }
        }

        /// <summary>
        /// Donne plus de résultat
        /// </summary>
        /// <returns></returns>
        public async Task GetPlusResultat()
        {
            if (DispoPlusResult)
            {
                pageEnCours++;
                var result = await _movieDbBusiness.RechercheGenerale(query, pageEnCours);
                PreparerDonneeRecherche(result);

                if (pageEnCours+1 >= nbPageMax)
                {
                    DispoPlusResult = false;
                }
            }
            
        }

        /// <summary>
        /// Recherche une donnée sur internet
        /// </summary>
        /// <param name="newQuery">le mot clé</param>
        /// <returns>les résultats</returns>
        public async Task<SearchGeneralJson> RechercheInternet(string newQuery)
        {
            return await _movieDbBusiness.RechercheGenerale(newQuery, 1);
        }

        /// <summary>
        /// Recherche en bas de donnée tout les films ayant un rapport avec la query
        /// </summary>
        /// <param name="query">la chaine de recherche</param>
        /// <returns></returns>
        public async Task RechercheBase(string query)
        {
            ListeBibliotheque = new ObservableCollection<Film>(await _filmBusiness.RechercheFilmToutCritere(query));
            if (ListeBibliotheque.Count > 0)
            {
                DispoMaBibliotheque = true;
            }
        }
        
    }
}
