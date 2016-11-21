using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "productioncountry")]
    public class ProductionCountryJson
    {
        [DataMember]
        public string iso_3166_1 { get; set; }
        [DataMember]
        public string name { get; set; }
    }
}