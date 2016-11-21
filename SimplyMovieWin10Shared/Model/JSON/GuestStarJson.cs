using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "gueststar")]
    public class GuestStarJson
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string credit_id { get; set; }
        [DataMember]
        public string character { get; set; }
        [DataMember]
        public int order { get; set; }
        [DataMember]
        public string profile_path { get; set; }

    }
}
