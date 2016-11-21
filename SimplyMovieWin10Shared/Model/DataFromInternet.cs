using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10Shared.Model
{
    /// <summary>
    /// Contient les données d'un film, série, ou personne provenant d'internet
    /// </summary>
    public class DataFromInternet
    {
        public CreditJson Casting { get; set; }

        public BitmapImage Affiche { get; set; }

        public MovieJson Film { get; set; }

        public TvJson Tv { get; set; }

        public PersonJson Person { get; set; }

        public CreditPersonJson CreditPerson { get; set; }

        public List<ResultSearchMovieJson> SimilarMovie { get; set; }

        public List<ResultSearchTvJson> SimilarTv { get; set; }

        public ResultCollection Collection { get; set; }
    }
}
