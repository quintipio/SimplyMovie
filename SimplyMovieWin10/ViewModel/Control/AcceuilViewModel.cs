using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Model;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// Controleur de la page d'aceuil
    /// </summary>
    public partial class AcceuilViewModel : AbstractViewModel
    {
        private FilmBusiness _filmBusiness;
        private MovieDbBusiness _movieDbBusiness;


        /// <summary>
        /// Constructeur
        /// </summary>
        public AcceuilViewModel()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// init
        /// </summary>
        /// <returns></returns>
        public sealed override async Task InitializeAsync()
        {
            _movieDbBusiness = new MovieDbBusiness();
            _filmBusiness = new FilmBusiness();
            await _filmBusiness.Initialization;
            
            //récupération des films de la bibliothèque
            FilmVoir = new ObservableCollection<Film>(await _filmBusiness.GetFilmSuggestionVoir());
            VisibleAVoir = FilmVoir != null && FilmVoir.Any();
            FilmPosseder = new ObservableCollection<Film>(await _filmBusiness.GetFilmSuggestionPosseder());
            VisibleAcheter = FilmPosseder != null && FilmPosseder.Any();
            FilmFavoris = new ObservableCollection<Film>(await _filmBusiness.GetFilmSuggestionFavoris());
            VisibleFavoris =FilmFavoris != null && FilmFavoris.Any();
            FilmSuggestion = new ObservableCollection<Film>(await _filmBusiness.GetFilmSuggestionAleatoire());
            VisibleSuggestion = FilmSuggestion != null && FilmSuggestion.Any();

            //récupértion d'internet
            try
            {
                ListeNowPlaying = new ObservableCollection<ResultSearchMovieJson>(await _movieDbBusiness.GetNowPlayingMovie());
                IsVisibleNowPlaying = ListeNowPlaying != null && ListeNowPlaying.Any();
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                ListeFilmPopulaire = new ObservableCollection<ResultSearchMovieJson>(await _movieDbBusiness.GetPopularMovie());
                IsVisibleFilmPopulaire = ListeFilmPopulaire != null && ListeFilmPopulaire.Any();
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                ListeTvMoment = new ObservableCollection<ResultSearchTvJson>(await _movieDbBusiness.GetNowPlayingTv());
                IsVisibleTvMoment = ListeTvMoment != null && ListeTvMoment.Any();
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                ListeTvPopular = new ObservableCollection<ResultSearchTvJson>(await _movieDbBusiness.GetPopularSerie());
                IsVisibleTvPopular = ListeTvPopular != null && ListeTvPopular.Any();
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
    }
}
