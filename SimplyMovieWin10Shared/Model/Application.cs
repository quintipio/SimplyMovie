using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Objet application
    /// </summary>
    [Table("application")]
    public class Application
    {
        [PrimaryKey, Column("id"), NotNull]
        public int Id { get; set; }

        /// <summary>
        /// Numéro de version actuel de l'application
        /// </summary>

        [Column("version"), NotNull]
        public string Version { get; set; }
    }
}
