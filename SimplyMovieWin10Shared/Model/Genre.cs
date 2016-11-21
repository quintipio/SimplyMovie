
using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Model du genre du film
    /// </summary>
    [Table("genre")]
    [XmlRoot("Genre")]
    public class Genre
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("Id")]
        public int Id { get; set; }

        /// <summary>
        /// le nom du genre
        /// </summary>
        [Column("nom"), NotNull]
        [XmlElement("Nom")]
        public string Nom { get; set; }

        [Ignore]
        [XmlIgnore]
        public List<Film> ListeFilms { get; set; }


        public override string ToString()
        {
            return Nom;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(Genre other)
        {
            return Id == other.Id && string.Equals(Nom, other.Nom);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Nom?.GetHashCode() ?? 0;
            }
        }
    }
}
