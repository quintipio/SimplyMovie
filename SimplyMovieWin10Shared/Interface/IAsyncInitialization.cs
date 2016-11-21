using System.Threading.Tasks;

namespace SimplyMovieWin10Shared.Interface
{
    /// <summary>
    /// Interface pour initialiser une méthode asynchrone avec un constructeur
    /// </summary>
    public interface IAsyncInitialization
    {
        /// <summary>
        /// initialization
        /// </summary>
        Task Initialization { get; }
    }
}
