using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    public class SearchSeasonJson
    {
        [DataMember]
        public string _id { get; set; }
        [DataMember]
        public string air_date { get; set; }
        [DataMember]
        public List<EpisodeJson> episodes { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string overview { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public int season_number { get; set; }

    }
}
