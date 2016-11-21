using System;
using System.Collections.Generic;

namespace SimplyMovieWin10Shared.Context
{
    /// <summary>
    /// Classe contenant des informations fixe et ne devant pas être modifié nécéssaire dans toute l'appli
    /// </summary>
    public static class ContexteStatic
    {
        /// <summary>
        /// le nom de l'application
        /// </summary>
        public const string NomAppli = "Simply movie";

        /// <summary>
        /// adresse de support
        /// </summary>
        public const string Support = "xxxx@xxx.fr";

        /// <summary>
        /// Nom du développeur
        /// </summary>
        public const string Developpeur = "XXXX";

        /// <summary>
        /// version de l'application
        /// </summary>
        public const string Version = "1.2.2";

        /// <summary>
        ///  Nom du fichier de la Bdd
        /// </summary>
        public const string FichierBdd = "database.sqlite";

        /// <summary>
        /// Largeur max d'une affiche
        /// </summary>
        public const double MaxSizeXAffiche = 200.0;

        /// <summary>
        /// Hauteur max d'une affiche
        /// </summary>
        public const double MaxSizeYAffiche = 300.0;

        /// <summary>
        /// Image de film par défaut
        /// </summary>
        public static readonly Uri UriAfficheDefaut = new Uri(@"ms-appx:/Rsc/AfficheDefaut.png");

        /// <summary>
        /// Lien pour interroger internet
        /// </summary>
        public const string ApiKeyMovieDb =  "clé de my movie db";

        /// <summary>
        /// liste des extensions autorisé pour les films
        /// </summary>
        public static readonly List<string> ListeExtension = new List<string> { ".avi", ".wmv", ".mpg", ".mp4", ".mkv" };
    }
}
