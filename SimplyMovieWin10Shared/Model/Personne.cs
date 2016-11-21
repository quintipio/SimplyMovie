using System.Collections.Generic;
using System.Xml.Serialization;
using SimplyMovieWin10Shared.Enum;
using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Model des personnes
    /// </summary>
    [Table("personne")]
    [XmlRoot("Personne")]
    public class Personne
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("Id")]
        public int Id { get; set; }

        [Column("nom"), NotNull]
        [XmlElement("Nom")]
        public string Nom { get; set; }

        [Ignore]
        [XmlIgnore]
        public List<Film> ListeFilm { get; set; }
        
        [Ignore]
        [XmlIgnore]
        public List<TypePersonneEnum> ListeRole { get; set; }

        [Ignore]
        [XmlIgnore]
        public TypePersonneEnum Role { get; set; }

        [Ignore]
        [XmlIgnore]
        public string NomScene { get; set; }

        public override string ToString()
        {
            return Nom;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(Personne other)
        {
            return Id == other.Id && string.Equals(Nom, other.Nom);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id*397) ^ (Nom != null ? Nom.GetHashCode() : 0);
            }
        }
    }
}
