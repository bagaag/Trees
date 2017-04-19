using Trees.Core.Models;
using Trees.Core.Models.Events;
using Trees.Core.Models.Stateful;

namespace Trees.Core.Services
{
    public interface IGameData
    {
        Deck<Tree> Trees { get; }

        Deck<BaseEvent> Events { get; }

        Deck<Land> Lands { get; }
    }
}
