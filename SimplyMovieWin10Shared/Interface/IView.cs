using SimplyMovieWin10Shared.Abstract;

namespace SimplyMovieWin10Shared.Interface
{
    public interface IView<T> where T:AbstractViewModel
    {
        T ViewModel { get; set; }
    }
}
