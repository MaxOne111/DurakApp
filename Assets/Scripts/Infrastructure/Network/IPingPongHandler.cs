using System.Threading.Tasks;

namespace Infrastructure.Network
{
    public interface IPingPongHandler
    {
        Task<string> Ping(string message);
    }
}