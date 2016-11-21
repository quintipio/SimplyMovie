using SimplyMovieWin10Shared.Enum;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Permet de fournir les informations à rechercher pour la page de consultation par internet
    /// </summary>
    public class SearchDataInternet
    {
        /// <summary>
        /// l'id de l'objet
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// le type de donnée de l'id (1 = film, 2 = série, 3 = personne)
        /// </summary>
        public TypeFilmEnum TypeData { get; set; }

        /// <summary>
        /// Donne le numéro pour type data à partir de la string
        /// </summary>
        /// <param name="chaine">la chaine tv, movie ou person</param>
        /// <returns>le numéro associé</returns>
        public static TypeFilmEnum GetTypeData(string chaine)
        {
            switch (chaine)
            {
                case "movie":
                    return TypeFilmEnum.FILM;

                case "tv":
                    return TypeFilmEnum.SERIE;

                case "person":
                    return TypeFilmEnum.PERSONNE;
            }
            return 0;
        }

    }
}
