using Trees.Core.Models.Stateful;
using Trees.Core.Services;

namespace Trees.Core.Models.Events
{
     public class EmptyEvent : BaseEvent
    {
        public EmptyEvent(string name, string description) : base(name,description)
        {
        }

        public override void Execute(GamePlay game, Table table)
        {            
        }

        public override void Stage(GamePlay game, Table table)
        {

        }
    }
}