using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "episode")]
    public class EpisodeJson
    {
        [DataMember]
        public string air_date { get; set; }
        [DataMember]
        public List<CrewJson> crew { get; set; }
        [DataMember]
        public int episode_number { get; set; }
        [DataMember]
        public List<GuestStarJson> guest_stars { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string overview { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public object production_code { get; set; }
        [DataMember]
        public int season_number { get; set; }
        [DataMember]
        public string still_path { get; set; }
        [DataMember]
        public double vote_average { get; set; }
        [DataMember]
        public int vote_count { get; set; }

    }
}
