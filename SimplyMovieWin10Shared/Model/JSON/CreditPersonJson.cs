using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    public class CreditPersonJson
    {
        [DataMember]
        public List<CastPersonJson> cast { get; set; }
        [DataMember]
        public List<CrewPersonJson> crew { get; set; }
        [DataMember]
        public int id { get; set; }
    }
}
