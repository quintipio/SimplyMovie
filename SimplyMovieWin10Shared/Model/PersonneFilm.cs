
using System.Xml.Serialization;
using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Le role qu'à eu une personne dans un film
    /// </summary>
    [Table("personnefilm")]
    [XmlRoot("PersonneFilm")]
    public class PersonneFilm
    {
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("Id")]
        public int Id { get; set; }

        [Column("idfilm"), NotNull]
        [XmlElement("IdFilm")]
        public int IdFilm { get; set; }

        [Column("idpersonne"), NotNull]
        [XmlElement("IdPersonne")]
        public int IdPersonne { get; set; }

        [Column("nomsScene")]
        [XmlElement("NomScene")]
        public string NomScene { get; set; }

        [Column("role"), NotNull]
        [XmlElement("Role")]
        public  int Role { get; set; }

    }
}
