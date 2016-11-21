using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "spokenlanguage")]
    public class SpokenLanguageJson
    {
        [DataMember]
        public string iso_639_1 { get; set; }
        [DataMember]
        public string name { get; set; }
    }
}