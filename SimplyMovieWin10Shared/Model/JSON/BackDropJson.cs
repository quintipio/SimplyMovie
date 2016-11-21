using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "backdrop")]
    public class BackdropJson
    {
        [DataMember]
        public double aspect_ratio { get; set; }
        [DataMember]
        public string file_path { get; set; }
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public object iso_639_1 { get; set; }
        [DataMember]
        public double vote_average { get; set; }
        [DataMember]
        public int vote_count { get; set; }
        [DataMember]
        public int width { get; set; }
    }
}
