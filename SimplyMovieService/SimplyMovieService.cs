using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.VoiceCommands;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Context;

namespace SimplyMovieService
{
    /// <summary>
    /// Service pour l'intégration avec Cortana
    /// </summary>
    public sealed class SimplyMovieService : IBackgroundTask
    {
        private readonly ResourceLoader _resourceLoader = new ResourceLoader();

        private BackgroundTaskDeferral _serviceDeferral;

        private VoiceCommandServiceConnection _voiceCommandServiceConnection;

        /// <summary>
        /// Méthode de lancement de l'appel de cortana
        /// </summary>
        /// <param name="taskInstance"></param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //lancement
            _serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += (sender, reason) => _serviceDeferral?.Complete();
            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            _voiceCommandServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
            _voiceCommandServiceConnection.VoiceCommandCompleted += (sender, args) => _serviceDeferral?.Complete();
            var voicecommand = await _voiceCommandServiceConnection.GetVoiceCommandAsync();

            await ContexteAppli.Init(true);
            var filmBusiness = new FilmBusiness();
            await filmBusiness.Initialization;
            var movieDbBusiness = new MovieDbBusiness();

            var tiles = new List<VoiceCommandContentTile>();
            switch (voicecommand.CommandName)
            {
                case "showVoirFilm":
                    var listeFilmVoir = await filmBusiness.GetFilmSuggestionVoir();
                    tiles.AddRange(listeFilmVoir.Select(film => new VoiceCommandContentTile
                    {
                        ContentTileType = VoiceCommandContentTileType.TitleOnly,
                        Title = film.Titre,
                        AppContext = film
                    }));
                    break;
                case "showFilm":
                    var listeFilm = await filmBusiness.GetFilmSuggestionAleatoire();
                    tiles.AddRange(listeFilm.Select(film => new VoiceCommandContentTile
                    {
                        ContentTileType = VoiceCommandContentTileType.TitleOnly,
                        Title = film.Titre,
                        AppContext = film
                    }));
                    break;
                case "showFilmFav":
                    var listeFilmFav = await filmBusiness.GetFilmSuggestionFavoris();
                    tiles.AddRange(listeFilmFav.Select(film => new VoiceCommandContentTile
                    {
                        ContentTileType = VoiceCommandContentTileType.TitleOnly,
                        Title = film.Titre,
                        AppContext = film
                    }));
                    break;
                case "showAcheter":
                    var listeFilmPosseder = await filmBusiness.GetFilmSuggestionPosseder();
                    tiles.AddRange(listeFilmPosseder.Select(film => new VoiceCommandContentTile
                    {
                        ContentTileType = VoiceCommandContentTileType.TitleOnly,
                        Title = film.Titre,
                        AppContext = film
                    }));
                    break;
                case "showFilmMoment":
                    var listeFilmMoment = (await movieDbBusiness.GetPopularMovie()).Take(9);
                    tiles.AddRange(listeFilmMoment.Select(film => new VoiceCommandContentTile
                    {
                        ContentTileType = VoiceCommandContentTileType.TitleOnly,
                        Title = film.title,
                        AppContext = film
                    }));
                    break;
                case "showSerieMoment":
                    var listeserieMoment = (await movieDbBusiness.GetPopularSerie()).Take(9);
                    tiles.AddRange(listeserieMoment.Select(film => new VoiceCommandContentTile
                    {
                        ContentTileType = VoiceCommandContentTileType.TitleOnly,
                        Title = film.name,
                        AppContext = film
                    }));
                    break;
            }

            var userPrompt = new VoiceCommandUserMessage();
            if (tiles.Count > 0)
            {
                userPrompt.DisplayMessage = GetString("filmTrouve");
                userPrompt.SpokenMessage = GetString("filmTrouve");
            }
            else
            {
                userPrompt.DisplayMessage = GetString("aucunResultat");
                userPrompt.SpokenMessage = GetString("aucunResultat");
            }

            if (tiles.Count == 0)
            {
                var response = VoiceCommandResponse.CreateResponse(userPrompt);
                await _voiceCommandServiceConnection.ReportSuccessAsync(response);
            }
            else
            {
                var response = VoiceCommandResponse.CreateResponse(userPrompt, tiles);
                await _voiceCommandServiceConnection.ReportSuccessAsync(response);
            }
        }
        



        private string GetString(string key)
        {
            return _resourceLoader.GetString(key);
        }
    }
}
