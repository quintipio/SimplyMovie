using System.Runtime.Serialization;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "productioncompany")]
    public class ProductionCompanyJson
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int id { get; set; }
    }
}