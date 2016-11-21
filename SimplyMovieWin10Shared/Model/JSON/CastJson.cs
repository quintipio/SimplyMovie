using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "cast")]
    public class CastJson
    {
        [DataMember]
        public int cast_id { get; set; }
        [DataMember]
        public string character { get; set; }
        [DataMember]
        public string credit_id { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int order { get; set; }
        [DataMember]
        public string profile_path { get; set; }

        public BitmapImage affiche { get; set; }
    }
}
