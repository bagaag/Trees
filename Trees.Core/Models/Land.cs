namespace Trees.Core.Models 
{
    public class Land 
    {
        public Land(string name, int spaces, Habitat habitat) {
            Habitat = habitat;
            Spaces = spaces;
            Name = name;
        }

        public string Name { get; private set; }
        public int Spaces { get; set; }
        public Habitat Habitat { get; private set; }
    }
}