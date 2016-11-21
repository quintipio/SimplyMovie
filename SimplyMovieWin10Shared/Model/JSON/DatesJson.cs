using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "dates")]
    public class DatesJson
    {
        [DataMember]
        public string maximum { get; set; }
        [DataMember]
        public string minimum { get; set; }

    }
}
