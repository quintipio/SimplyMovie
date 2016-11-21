using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract]
    public class CreditJson
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public List<CastJson> cast { get; set; }
        [DataMember]
        public List<CrewJson> crew { get; set; }
    }
}
