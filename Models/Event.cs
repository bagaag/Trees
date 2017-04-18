namespace Trees.Models 
{
     public class Event
    {
        public Event(string name, string description) 
        {
            Name = name;
            Description = description;
        }
        
        public string Name { get; set; }

        public string Description { get; set; }
    }
}