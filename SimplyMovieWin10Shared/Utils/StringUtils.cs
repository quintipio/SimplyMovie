namespace SimplyMovieWin10Shared.Utils
{
    /// <summary>
    /// Classe comprenant des outils pour gérer les strings
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Met en majuscule la première lettre d'une chaine de caractère
        /// </summary>
        /// <param name="chaine">la chaine  à modifié</param>
        /// <returns>le résultat</returns>
        public static string FirstLetterUpper(string chaine)
        {
            if (!string.IsNullOrWhiteSpace(chaine))
            {
                return chaine[0].ToString().ToUpper() + chaine.Substring(1).ToLower();
            }
            return "";
        }

        ///<summary> 
        /// vérifie que la chaine ne contient que des chiffres 
        ///</summary> 
        ///<param name="sReceive">la chaine à vérifier</param> 
        ///<returns>true si que des chiffres</returns> 
        public static bool IsDigit(string sReceive)
        {
            bool bResult;
            bResult = true;
            var cWork = sReceive.ToCharArray();
            var enumeratorcWork = cWork.GetEnumerator();
            while (enumeratorcWork.MoveNext())
            {
                if (char.IsDigit((char)enumeratorcWork.Current) == false)
                {
                    bResult = false;
                }
            }
            return bResult;
        }

        /// <summary>
        /// Controle deux numéro de version et dit si oldVersion est plus vieux que new Version
        /// </summary>
        /// <param name="oldVersion">le plus vieux des deux numéros de version</param>
        /// <param name="newVersion">le plus récent des deux numéro de version</param>
        /// <returns>true si old est bien plus vieux</returns>
        public static bool CheckVersion(string oldVersion, string newVersion)
        {
            var oldA = Getnumber(oldVersion, 0);
            var oldB = Getnumber(oldVersion, 1);
            var oldC = Getnumber(oldVersion, 2);

            var newA = Getnumber(newVersion, 0);
            var newB = Getnumber(newVersion, 1);
            var newC = Getnumber(newVersion, 2);

            if (oldA < newA)
            {
                return true;
            }
            if (oldB < newB)
            {
                return true;
            }
            if (oldC < newC)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retourne un chiffre du numéro de version
        /// </summary>
        /// <param name="version">la chaine de la version</param>
        /// <param name="pos">la position du point de départ(0,1,2)</param>
        /// <returns>le numéro converti</returns>
        private static int Getnumber(string version, int pos)
        {
            int start;
            switch (pos)
            {
                case 0:
                    start = 0;
                    break;

                case 1:
                    start = version.IndexOf('.');
                    break;

                case 2:
                    start = version.LastIndexOf('.');
                    break;

                default:
                    start = 0;
                    break;
            }
            start++;
            string number = "";
            do
            {
                number += version[start];
                start++;
            } while (start < version.Length && version[start] != '.');

            int retour;
            int.TryParse(number, out retour);

            return retour;
        }
    }
}
