using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "crew")]
    public class CrewJson
    {
        [DataMember]
        public string credit_id { get; set; }
        [DataMember]
        public string department { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string job { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string profile_path { get; set; }

        public BitmapImage affiche { get; set; }
    }
}
