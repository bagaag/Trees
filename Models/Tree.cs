namespace Trees.Models 
{
    public class Tree 
    {
        public Tree(string name, string family, Habitat habitat, Damage damage) 
        {
            Name = name;
            Family = family;
            Habitat = habitat;
            Damage = damage;
        }

        public string Name { get; set; }

        public string Family { get; set; }

        public Habitat Habitat { get; set; }

        public Damage Damage { get; set; }

    }
}