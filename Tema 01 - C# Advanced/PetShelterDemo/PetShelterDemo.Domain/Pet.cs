namespace PetShelterDemo.Domain
{
    public class Pet : INamedEntity
    {
        public string Name { get; }
        public string IdNumber { get; }
        public string Description { get; }

        public Pet(string name, string id, string description)
        {
            Name = name;
            IdNumber = id;
            Description = description;
        }
    }
}
