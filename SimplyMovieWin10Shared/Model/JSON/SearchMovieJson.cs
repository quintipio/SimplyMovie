using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract]
    public class SearchMovieJson
    {
        [DataMember]
        public int page { get; set; }

        [DataMember]
        public List<ResultSearchMovieJson> results { get; set; }

        [DataMember]
        public int total_results { get; set; }

        [DataMember]
        public int total_pages { get; set; }

        
    }
}
