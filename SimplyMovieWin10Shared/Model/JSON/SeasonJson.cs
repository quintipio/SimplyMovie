using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "season")]
    public class SeasonJson
    {
        [DataMember]
        public string air_date { get; set; }
        [DataMember]
        public int episode_count { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public int season_number { get; set; }
    }
}
