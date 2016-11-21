using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.VoiceCommands;

namespace SimplyMovieWin10Shared.Context
{
    /// <summary>
    /// Classe de contexte au niveau de l'application
    /// </summary>
    public static class ContexteAppli
    {
        /// <summary>
        /// Indique l'application fonctionne par cortana ou non
        /// </summary>
        public static bool IsCortanaActive { get; set; }

        /// <summary>
        /// Initialisation de l'appli
        /// </summary>
        public static async Task Init(bool isCortana)
        {
            //var com = await ComSqlite.GetComSqlite();
            IsCortanaActive = isCortana;
            try
            {
                if (!isCortana)
                {
                    var vcdfile = await Package.Current.InstalledLocation.GetFileAsync(@"VoiceCommands.xml");
                    await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdfile);

                }
            }
            catch (Exception)
            {
                //Ignored
            }
            
        }
    }
}
