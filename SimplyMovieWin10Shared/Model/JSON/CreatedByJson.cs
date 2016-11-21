using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name ="createdby")]
    public class CreatedByJson
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string profile_path { get; set; }
    }
}
