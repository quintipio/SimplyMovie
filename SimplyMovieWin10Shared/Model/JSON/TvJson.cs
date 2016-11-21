using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    public class TvJson
    {
        [DataMember]
        public string backdrop_path { get; set; }
        [DataMember]
        public List<CreatedByJson> created_by { get; set; }
        [DataMember]
        public List<int> episode_run_time { get; set; }
        [DataMember]
        public string first_air_date { get; set; }
        [DataMember]
        public List<GenreJson> genres { get; set; }
        [DataMember]
        public string homepage { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public bool in_production { get; set; }
        [DataMember]
        public List<string> languages { get; set; }
        [DataMember]
        public string last_air_date { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<NetworkJson> networks { get; set; }
        [DataMember]
        public int number_of_episodes { get; set; }
        [DataMember]
        public int number_of_seasons { get; set; }
        [DataMember]
        public List<string> origin_country { get; set; }
        [DataMember]
        public string original_language { get; set; }
        [DataMember]
        public string original_name { get; set; }
        [DataMember]
        public string overview { get; set; }
        [DataMember]
        public double popularity { get; set; }
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public List<ProductionCompanyJson> production_companies { get; set; }
        [DataMember]
        public List<SeasonJson> seasons { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double vote_average { get; set; }
        [DataMember]
        public int vote_count { get; set; }
    }
}
