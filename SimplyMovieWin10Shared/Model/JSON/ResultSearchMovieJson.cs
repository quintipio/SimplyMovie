using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace SimplyMovieWin10Shared.Model.JSON
{
    [DataContract(Name = "resultsearch")]
    public class ResultSearchMovieJson
    {

        [DataMember]
        public string poster_path { get; set; }

        [DataMember]
        public bool adult { get; set; }

        [DataMember]
        public string overview { get; set; }

        [DataMember]
        public string release_date { get; set; }

        [DataMember]
        public List<int> genre_ids { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string original_title { get; set; }

        [DataMember]
        public string original_language { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string backdrop_path { get; set; }

        [DataMember]
        public double popularity { get; set; }

        [DataMember]
        public int vote_count { get; set; }

        [DataMember]
        public bool video { get; set; }
        
        [DataMember]
        public double vote_average { get; set; }

        public BitmapImage affiche { get; set; }
    }
}