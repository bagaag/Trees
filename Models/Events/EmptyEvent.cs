using Trees.Models.Stateful;
using Trees.Services;

namespace Trees.Models.Events
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