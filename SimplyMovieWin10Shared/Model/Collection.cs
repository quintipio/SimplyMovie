
using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Model pour les collections
    /// </summary>
    [Table("collection")]
    [XmlRoot("collection")]
    public class Collection
    {

        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("id")]
        public int Id { get; set; }

        /// <summary>
        /// l'id internet de la collection
        /// </summary>
        [Column("IdCollectionInternet")]
        [XmlElement("IdCollectionInternet")]
        public int IdCollectionInternet { get; set; }

        /// <summary>
        /// le nom de la collection
        /// </summary>
        [Column("NomCollection")]
        [XmlElement("NomCollection")]
        public string NomCollection { get; set; }

        [XmlIgnore]
        [Ignore]
        public List<Film> FilmCollection { get; set; }

        public override string ToString()
        {
            return NomCollection;
        }
    }
}
