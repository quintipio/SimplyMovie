
using System.Collections.Generic;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media.Imaging;
using SQLite;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Le model du film
    /// </summary>
    [Table("film")]
    [XmlRoot("Film")]
    public class Film
    {
        /// <summary>
        /// l'id
        /// </summary>
        [PrimaryKey, Column("id"), NotNull]
        [XmlElement("Id")]
        public int Id { get; set; }

        /// <summary>
        /// le titre du film
        /// </summary>
        [Column("titre"), NotNull]
        [XmlElement("Titre")]
        public string Titre { get; set; }

        /// <summary>
        /// L'année de sortie du film
        /// </summary>
        [Column("annee")]
        [XmlElement("Annee")]
        public int? Annee { get; set; }

        /// <summary>
        /// Les producteurs
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public List<Personne> Producteurs { get; set; }

        /// <summary>
        /// Les réalisateurs
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public List<Personne> Realisateurs { get; set; }

        /// <summary>
        /// les acteurs
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public List<Personne> Acteurs { get; set; }

        /// <summary>
        /// Les genres
        /// </summary>
        [Ignore]
        [XmlIgnore]
        public List<Genre> Genres { get; set; }

        /// <summary>
        /// l'histoire du film
        /// </summary>
        [Column("synopsis")]
        [XmlElement("Synopsis")]
        public string Synopsis { get; set; }
        
        /// <summary>
        /// l'image du film
        /// </summary>
        [Column("affiche")]
        [XmlElement("Affiche")]
        public byte[] Affiche { get; set; }

        [Column("type"), NotNull]
        [XmlElement("Type")]
        public int Type { get; set; }

        [Column("notegen")]
        [XmlElement("NoteGen")]
        public double NoteGen { get; set; }

        [Column("manote")]
        [XmlElement("MaNote")]
        public double MaNote { get; set; }


        [Column("saison")]
        [XmlElement("Saison")]
        public string Saison { get; set; }

        [Column("duree")]
        [XmlElement("Duree")]
        public int? Duree { get; set; }

        [Column("souhait")]
        [XmlElement("Souhait")]
        public bool? Souhait { get; set; }

        [Column("Posseder")]
        [XmlElement("Posseder")]
        public bool? Posseder { get; set; }

        [Column("Voir")]
        [XmlElement("Voir")]
        public bool? Voir { get; set; }


        [Column("SouhaitVoir")]
        [XmlElement("SouhaitVoir")]
        public bool? SouhaitVoir { get; set; }

        [Column("TypeSupport")]
        [XmlElement("TypeSupport")]
        public int TypeSupport { get; set; }
        
        [Column("Lien")]
        [XmlElement("Lien")]
        public string Lien { get; set; }

        [Ignore]
        [XmlIgnore]
        public BitmapImage AfficheImage { get; set; }


        [Column("IdInternet")]
        [XmlElement("IdInternet")]
        public int IdInternet { get; set; }

        [Column("idcollection")]
        [XmlElement("idcollection")]
        public int IdCollection { get; set; }


        [Column("ordrecollection")]
        [XmlElement("ordrecollection")]
        public int OrdreCollection { get; set; }

        [Ignore]
        [XmlIgnore]
        public int IdCollectionInternet { get; set; }

        [Ignore]
        [XmlIgnore]
        public string NomCollection { get; set; }


        [Ignore]
        [XmlIgnore]
        public string Opacity { get; set; }

        public override string ToString()
        {
            return Titre;
        }
    }
}
