using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "belongstocollection")]
    public class BelongsToCollectionJson
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public string backdrop_path { get; set; }
    }
}