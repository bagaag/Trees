using Trees.Models;
using Trees.Models.Stateful;

namespace Trees.Services
{
    public interface IGameData
    {
        Deck<Tree> Trees { get; }

        Deck<Event> Events { get; }

        Deck<Land> Lands { get; }
    }
}
