using System;

namespace SimplyMovieWin10Shared.Utils
{
    /// <summary>
    /// Outils pour gérer les dates
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Converti une chaine de caractère en date
        /// </summary>
        /// <param name="chaine">la chaine</param>
        /// <returns>la date ou si echec null</returns>
        public static DateTime? ConvertStringToDateTime(string chaine)
        {
            DateTime result;
            if (DateTime.TryParse(chaine, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Donne l'année d'une date à partir d'une chaine de caractères
        /// </summary>
        /// <param name="chaine">la chaine</param>
        /// <returns>l'année nullable</returns>
        public static int? GetYearFromString(string chaine)
        {
            if (!string.IsNullOrEmpty(chaine))
            {
                return ConvertStringToDateTime(chaine)?.Year;
            }
            return null;
        }
    }
}
