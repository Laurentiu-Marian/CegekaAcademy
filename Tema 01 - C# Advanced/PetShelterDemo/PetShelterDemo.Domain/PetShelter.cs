using PetShelterDemo.DAL;

namespace PetShelterDemo.Domain;

public class PetShelter
{
    private readonly IRegistry<Pet> petRegistry;
    private readonly IRegistry<Donations> donorRegistry;
    private readonly IEventRegistry<Fundraiser> fundraiserRegistry;
    private int donationsInRon = 0;
    private int donationsInEuro = 0;

    public PetShelter()
    {
        donorRegistry = new Registry<Donations>(new Database());
        petRegistry = new Registry<Pet>(new Database());
        fundraiserRegistry = new EventRegistry<Fundraiser>(new Database());
    }

    public void RegisterFundraise(Fundraiser fundraiser)
    {
        fundraiserRegistry.Register(fundraiser);
    }
    public IReadOnlyList<Fundraiser> GetAllFundraisers()
    {
        return fundraiserRegistry.GetAll().Result;
    }

    public Fundraiser GetFundraiserByName(string title)
    {
        return fundraiserRegistry.GetByName(title).Result;
    }

    public Donations GetDonorByName(string name)
    {
        return donorRegistry.GetByName(name).Result;
    }

    public Donations GetDonorById(string id)
    {
        return donorRegistry.GetById(id).Result;
    }

    public void RegisterPet(Pet pet)
    {
        petRegistry.Register(pet);
    }

    public IReadOnlyList<Pet> GetAllPets()
    {
        return petRegistry.GetAll().Result;  // Actually blocks thread until the result is available.
    }

    public Pet GetByName(string name)
    {
        return petRegistry.GetByName(name).Result;
    }

    public void SetDonationAmounts(int amountInRON, int amountInEURO)
    {
        donationsInRon += amountInRON;
        donationsInEuro += amountInEURO;
    }

    public void Donate(Donations donor, int amountInRON, int amountInEURO)
    {
        donorRegistry.Register(donor);
        SetDonationAmounts(amountInRON, amountInEURO);
    }

    public int GetTotalDonationsInRON()
    {
        return donationsInRon;
    }

    public int GetTotalDonationsInEURO()
    {
        return donationsInEuro;
    }

    public IReadOnlyList<Donations> GetAllDonors()
    {
        return donorRegistry.GetAll().Result;
    }
}
