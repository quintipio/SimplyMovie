using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "result")]
    public class ResultSearchGenJson
    {
        [DataMember]
        public string poster_path { get; set; }
        [DataMember]
        public double popularity { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string overview { get; set; }
        [DataMember]
        public string backdrop_path { get; set; }
        [DataMember]
        public double vote_average { get; set; }
        [DataMember]
        public string media_type { get; set; }
        [DataMember]
        public string first_air_date { get; set; }
        [DataMember]
        public List<string> origin_country { get; set; }
        [DataMember]
        public List<object> genre_ids { get; set; }
        [DataMember]
        public string original_language { get; set; }
        [DataMember]
        public int vote_count { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string original_name { get; set; }
        [DataMember]
        public bool? adult { get; set; }
        [DataMember]
        public string release_date { get; set; }
        [DataMember]
        public string original_title { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public bool? video { get; set; }

        public BitmapImage affiche { get; set; }
    }
}
