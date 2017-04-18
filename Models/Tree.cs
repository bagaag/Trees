namespace Trees.Models 
{
    public class Tree 
    {
        public Tree(string name, string genus, Habitat habitat, Damage damage) 
        {
            Name = name;
            Genus = genus;
            Habitat = habitat;
            Damage = damage;
        }

        public string Name { get; set; }

        public string Genus { get; set; }

        public Habitat Habitat { get; set; }

        public Damage Damage { get; set; }

    }
}