using System.Collections.Generic;
using System.Xml.Serialization;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Model pour la sauvegarde ou le chargement des données
    /// </summary>
    [XmlRoot("simplymoviedata")]
    public class SaveLoad
    {
        [XmlElement("films")]
        public List<Film> ListeFilm { get; set; }

        [XmlElement("genres")]
        public List<Genre> ListeGenre { get; set; }

        [XmlElement("personnes")]
        public List<Personne> ListePersonne { get; set; }

        [XmlElement("genrefilms")]
        public List<GenreFilm> ListeGenreFilm { get; set; }

        [XmlElement("personnefilms")]
        public List<PersonneFilm> ListePersonneFilm { get; set; }

        [XmlElement("collections")]
        public List<Collection> ListeCollection { get; set; }
        

        public SaveLoad()
        {
            ListeFilm = new List<Film>();
            ListeGenre = new List<Genre>();
            ListePersonne = new List<Personne>();
            ListeGenreFilm = new List<GenreFilm>();
            ListePersonneFilm = new List<PersonneFilm>();
            ListeCollection = new List<Collection>();
        }
    }
}
