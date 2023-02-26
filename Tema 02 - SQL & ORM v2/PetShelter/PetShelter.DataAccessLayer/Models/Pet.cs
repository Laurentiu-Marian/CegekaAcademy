namespace PetShelter.DataAccessLayer.Models;

public class Pet: IEntity
{
    /// <summary>
    ///     Identifier
    /// </summary>
    public int Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    /// <summary>
    ///     e.g. Cat, Dog etc.
    /// </summary>
    public string Type { get; set; }

    public string? Description { get; set; }

    public DateTime Birthdate { get; set; }
    
    public bool IsHealthy { get; set; }
    
    public decimal WeightInKg { get; set; }

    /// <summary>
    ///     Can be used for soft delete. The pet can be marked as not sheltered anymore if it was adopted or it died.
    /// </summary>
    public bool IsSheltered { get; set; }

    /// <summary>
    ///     FK to a person
    /// </summary>
    public int? RescuerId { get; set; }

    /// <summary>
    ///     FK to a person
    /// </summary>
    public int? AdopterId { get; set; }

    public Person? Adopter { get; set; }
    public Person Rescuer { get; set; }

    public Pet()
    {

    }

    public Pet(string name, string imageUrl, string type, string? description, DateTime birthdate, bool isHealthy, decimal weightInKg, bool isSheltered, int? rescuerId, int? adopterId, Person? adopter, Person rescuer)
    {
        Name = name;
        ImageUrl = imageUrl;
        Type = type;
        Description = description;
        Birthdate = birthdate;
        IsHealthy = isHealthy;
        WeightInKg = weightInKg;
        IsSheltered = isSheltered;
        RescuerId = rescuerId;
        AdopterId = adopterId;
        Adopter = adopter;
        Rescuer = rescuer;
    }

}
