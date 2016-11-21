using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace SimplyMovieWin10Shared.Model.JSON
{
    public class PersonJson
    {
        [DataMember]
        public bool adult { get; set; }
        [DataMember]
        public string biography { get; set; }
        [DataMember]
        public string birthday { get; set; }
        [DataMember]
        public string deathday { get; set; }
        [DataMember]
        public int gender { get; set; }
        [DataMember]
        public string homepage { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string imdb_id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string place_of_birth { get; set; }
        [DataMember]
        public double popularity { get; set; }
        [DataMember]
        public string profile_path { get; set; }
        public BitmapImage affiche { get; set; }
    }
}
