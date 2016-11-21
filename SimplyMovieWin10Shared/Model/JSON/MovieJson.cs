using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract]
    public class MovieJson
    {
        [DataMember]
        public bool adult { get; set; }
        [DataMember]
        public string backdrop_path { get; set; }
        [DataMember]
        public BelongsToCollectionJson belongs_to_collection { get; set; }
        [DataMember]
        public int budget { get; set; }
        [DataMember]
        public List<GenreJson> genres { get; set; }
        [DataMember]
        public string homepage { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string imdb_id { get; set; }
        [DataMember]
        public string original_language { get; set; }
        [DataMember]
        public string original_title { get; set; }
        [DataMember]
        public string overview { get; set; }
        [DataMember]
        public double popularity { get; set; }
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public List<ProductionCompanyJson> production_companies { get; set; }
        [DataMember]
        public List<ProductionCountryJson> production_countries { get; set; }
        [DataMember]
        public string release_date { get; set; }
        [DataMember]
        public long revenue { get; set; }
        [DataMember]
        public object runtime { get; set; }
        [DataMember]
        public List<SpokenLanguageJson> spoken_languages { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string tagline { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public bool video { get; set; }
        [DataMember]
        public double vote_average { get; set; }
        [DataMember]
        public int vote_count { get; set; }
    }
}
