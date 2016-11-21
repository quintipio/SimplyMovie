using SimplyMovieWin10Shared.Enum;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Objet servant à filtrer un champ dans la bibliothèque
    /// </summary>
   public  class SearchFilter
    {
        public FilterBibliothequeEnum TypeA { get; set; }

        public FilterBibliothequeEnum TypeB { get; set; }

        /// <summary>
        /// La donnée à recherchée si c'est un genre
        /// </summary>
        public Genre RechercheGenre { get; set; }

        /// <summary>
        /// La donnée à recherchée si c'est une personne
        /// </summary>
        public Personne RecherchePersonne { get; set; }
    }
}
