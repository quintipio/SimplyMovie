using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using SimplyMovieWin10Shared.Abstract;
using SimplyMovieWin10Shared.Business;
using SimplyMovieWin10Shared.Strings;

namespace SimplyMovieWin10.ViewModel
{
    /// <summary>
    /// ViewModel des paramètres
    /// </summary>
    public partial class ParametreViewModel : AbstractViewModel
    {

        private ParamBusiness _paramBusiness;


        /// <summary>
        /// Constructeur
        /// </summary>
        public ParametreViewModel()
        {
            Initialization = InitializeAsync();
        }

        /// <summary>
        /// init
        /// </summary>
        /// <returns></returns>
        public sealed override async Task InitializeAsync()
        {
            _paramBusiness = new ParamBusiness();
            await _paramBusiness.Initialization;
            ListeLangues = new ObservableCollection<ListeLangues.LanguesStruct>(SimplyMovieWin10Shared.Strings.ListeLangues.GetListesLangues());

        }

        /// <summary>
        /// Change la langue de l'application
        /// </summary>
        /// <param name="langue">la langue à appliquer</param>
        public void ChangeLangueApplication(ListeLangues.LanguesStruct langue)
        {
            SimplyMovieWin10Shared.Strings.ListeLangues.ChangeLangueAppli(langue);

        }

        /// <summary>
        /// Charge un fichier
        /// </summary>
        /// <param name="fichier">le fichier qui contient les données à charger</param>
        public async Task Load(StorageFile fichier)
        {
            var data = await FileIO.ReadTextAsync(fichier);
            await _paramBusiness.LoadData(data);
        }

        /// <summary>
        /// Sauvegarde un fichier à partir de la base
        /// </summary>
        /// <param name="fichier">le fichier de destination</param>
        public async Task Save(StorageFile fichier)
        {
            var data = await _paramBusiness.CreateDataToSave();
            await FileIO.WriteTextAsync(fichier, data);
        }
    }
}
