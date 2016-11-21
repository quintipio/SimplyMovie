using System;
using SimplyMovieWin10Shared.Model.JSON;

namespace SimplyMovieWin10Shared.Model
{
    public class MassLoad
    {

        public string Path { get; set; }

        public string Name { get; set; }
        public Film Movie { get; set; }
        public string Resultat { get; set; }
        public bool IsOk { get; set; }
        public string Color { get; set; }
        public SearchMovieJson Results { get; set; }
        public bool PlusieursRes { get; set; }
        public int Id { get; set; }
        public int MaNote { get; set; }

        public Boolean IsFilmSelected { get; set; }

        public Boolean IsDocuSelected { get; set; }

        public Boolean IsAnimSelected { get; set; }

        public Boolean IsSpecSelected { get; set; }
    }
}