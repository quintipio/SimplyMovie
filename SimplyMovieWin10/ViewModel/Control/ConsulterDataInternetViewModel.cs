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
    /// ViewModel pour l'affichage des données d'internet
    /// </summary>
    public partial class ConsulterDataInternetViewModel : AbstractViewModel
    {
        private MovieDbBusiness _movieDbBusiness;
        private FilmBusiness _filmBusiness;

        public ConsulterDataInternetViewModel()
        {
            Initialization = InitializeAsync();
        }

        public sealed override async Task InitializeAsync()
        {
            _movieDbBusiness = new MovieDbBusiness();
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
            FilmVisible = false;
            SerieVisible = false;
            PersonVisible = false;
            ElementPresent = false;
        }


        /// <summary>
        /// Cherche les données sur internet pour les afficher
        /// </summary>
        /// <param name="data">l'id et le type de donnée</param>
        /// <returns></returns>
        public async Task SearchData(SearchDataInternet data)
        {
            switch (data.TypeData)
            {
                case TypeFilmEnum.SERIE:
                    DataToDisplay = await _movieDbBusiness.GetSerieJson(data.Id);
                    Titre = DataToDisplay.Tv.name;
                    FilmVisible = false;
                    SerieVisible = true;
                    PersonVisible = false;
                    FilmBibliotheque = await _filmBusiness.IsFilmInternetPresentEnBase(data.Id, (int)data.TypeData);
                    ElementPresent = FilmBibliotheque != null;
                    ListeSaison = new ObservableCollection<int>(DataToDisplay.Tv.seasons.Select(x => x.season_number));
                    break;

                case TypeFilmEnum.PERSONNE:
                    DataToDisplay = await _movieDbBusiness.GetPersonneJson(data.Id);
                    Titre = DataToDisplay.Person.name;
                    FilmVisible = false;
                    SerieVisible = false;
                    PersonVisible = true;
                    ElementPresent = false;
                    break;

                default:
                    DataToDisplay = await _movieDbBusiness.GetFilmJson(data.Id);
                    Titre = DataToDisplay.Film.title;
                    FilmVisible = true;
                    SerieVisible = false;
                    PersonVisible = false;
                    FilmBibliotheque = await _filmBusiness.IsFilmInternetPresentEnBase(data.Id, (int)data.TypeData);
                    ElementPresent = FilmBibliotheque != null;
                    break;
            }
        }

        /// <summary>
        /// Recherche la saison d'une série
        /// </summary>
        /// <param name="saison">la saison</param>
        /// <returns></returns>
        public async Task GetSeason(int saison)
        {
            Season = await _movieDbBusiness.GetSaisonTv(saison, DataToDisplay.Tv.id);
        }
    }
}
