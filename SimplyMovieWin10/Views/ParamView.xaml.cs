
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SimplyMovieWin10.ViewModel;
using SimplyMovieWin10Shared.Interface;
using SimplyMovieWin10Shared.Strings;
using SimplyPasswordWin10Shared.Utils;

namespace SimplyMovieWin10.Views
{
    /// <summary>
    /// Vue pour les paramètres de l'appli
    /// </summary>
    public sealed partial class ParamView : IView<ParametreViewModel>
    {

        private bool _canChangeLangue;

        /// <summary>
        /// Controleur
        /// </summary>
        public ParametreViewModel ViewModel { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ParamView()
        {
            InitializeComponent();
            _canChangeLangue = true;
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = new ParametreViewModel();
            await ViewModel.Initialization;

            //mise en place des langues
            ComboListeLangue.ItemsSource = ViewModel.ListeLangues;
            ComboListeLangue.SelectedItem = ListeLangues.GetLangueEnCours();
        }

        private void comboListeLangue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_canChangeLangue && ComboListeLangue.SelectedItem is ListeLangues.LanguesStruct)
            {
                _canChangeLangue = false;
                ViewModel.ChangeLangueApplication((ListeLangues.LanguesStruct)ComboListeLangue.SelectedItem);
                _canChangeLangue = true;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            LoadButton.IsEnabled = false;
            WaitRing.IsActive = true;
            try
            {
                var fileSavePicker = new FileSavePicker
                {
                    CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                    SuggestedFileName = "SimplyMovieDb",
                    SuggestedStartLocation = PickerLocationId.Downloads,
                    DefaultFileExtension = ".spmv",

                };

                fileSavePicker.FileTypeChoices.Add("SimplyMovieDb", new List<string> { ".spmv" });
                var fichierTmp = await fileSavePicker.PickSaveFileAsync();
                if (fichierTmp != null)
                {
                    await ViewModel.Save(fichierTmp);
                    await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("exportOk"));
                }
            }
            catch (Exception)
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("errExport"));
            }
            SaveButton.IsEnabled = true;
            LoadButton.IsEnabled = true;
            WaitRing.IsActive = false;

        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            LoadButton.IsEnabled = false;
            WaitRing.IsActive = true;
            try
            {
                if (await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("avertissementImport"),
                ResourceLoader.GetForCurrentView().GetString("avert"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var fileOpenPicker = new FileOpenPicker
                    {
                        CommitButtonText = ResourceLoader.GetForCurrentView().GetString("phraseOK"),
                        ViewMode = PickerViewMode.List,
                        SuggestedStartLocation = PickerLocationId.Downloads,
                        FileTypeFilter = { ".spmv" },
                    };


                    var fichier = await fileOpenPicker.PickSingleFileAsync();
                    if (fichier != null)
                    {
                        await ViewModel.Load(fichier);
                        await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("importOk"));
                    }
                }
            }
            catch (Exception)
            {
                await MessageBox.ShowAsync(ResourceLoader.GetForCurrentView().GetString("errImport"));
            }

            SaveButton.IsEnabled = true;
            LoadButton.IsEnabled = true;
            WaitRing.IsActive = false;

        }
    }
}
