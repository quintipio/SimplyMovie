using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "poster")]
    public class PosterJson
    {
        [DataMember]
        public double aspect_ratio { get; set; }
        [DataMember]
        public string file_path { get; set; }
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public string iso_639_1 { get; set; }
        [DataMember]
        public double vote_average { get; set; }
        [DataMember]
        public int vote_count { get; set; }
        [DataMember]
        public int width { get; set; }
    }
}
