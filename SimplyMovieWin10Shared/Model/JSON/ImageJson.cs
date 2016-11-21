using System.Collections.Generic;

namespace SimplyMovieWin10Shared.Model.JSON
{
    public class ImageJson
    {
        public int id { get; set; }
        public List<BackdropJson> backdrops { get; set; }
        public List<PosterJson> posters { get; set; }
    }
}
