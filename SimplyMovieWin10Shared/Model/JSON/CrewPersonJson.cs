using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "crew")]
    public class CrewPersonJson
    {
        [DataMember]
        public bool adult { get; set; }
        [DataMember]
        public string credit_id { get; set; }
        [DataMember]
        public string department { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string job { get; set; }
        [DataMember]
        public string original_title { get; set; }
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public string release_date { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string media_type { get; set; }
        [DataMember]
        public int? episode_count { get; set; }
        [DataMember]
        public string first_air_date { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string original_name { get; set; }

    }
}
