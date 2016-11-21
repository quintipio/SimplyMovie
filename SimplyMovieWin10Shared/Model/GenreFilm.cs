using System.Xml.Serialization;
using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// les genres de chaque film
    /// </summary>
    [Table("genrefilm")]
    [XmlRoot("GenreFilm")]
    public class GenreFilm
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("Id")]
        public int Id { get; set; }

        [Column("idfilm"), NotNull]
        [XmlElement("IdFilm")]
        public int IdFilm { get; set; }

        [Column("idgenre"), NotNull]
        [XmlElement("IdGenre")]
        public int IdGenre { get; set; }
    }
}
