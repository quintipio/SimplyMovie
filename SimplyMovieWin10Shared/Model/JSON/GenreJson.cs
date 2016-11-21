using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "genre")]
    public class GenreJson
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
    }
}