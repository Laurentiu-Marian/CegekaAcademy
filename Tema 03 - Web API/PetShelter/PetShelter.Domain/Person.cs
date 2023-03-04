namespace PetShelter.Domain;

public class Person : INamedEntity
{
    public string Name { get; set; }

    public DateTime? DateOfBirth { get; }

    public string IdNumber { get; }

    public int? FundraiserCreatorId { get; }

    public Person(string idNumber, string name, DateTime? dateOfBirth = null, int? fundraiserCreatorId = null)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        IdNumber = idNumber;
        FundraiserCreatorId = fundraiserCreatorId;
    }
}
