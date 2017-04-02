using System.Collections.Generic;
using Trees.Models;

namespace Trees.Services
{
    public interface IGameData
    {
        List<Tree> Trees { get; }

        List<Event> Events { get; }

        List<Land> Lands { get; }
    }
}
