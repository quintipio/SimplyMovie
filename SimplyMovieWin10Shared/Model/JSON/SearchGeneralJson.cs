using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{

    public class SearchGeneralJson
    {
        [DataMember]
        public int page { get; set; }
        [DataMember]
        public List<ResultSearchGenJson> results { get; set; }
        [DataMember]
        public int total_results { get; set; }
        [DataMember]
        public int total_pages { get; set; }

        public string query { get; set; }
    }
}
