using Microsoft.AspNetCore.Http.HttpResults;
using PetShelter.DataAccessLayer;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

var petShelterContext = new PetShelterContext();

IDonationRepository donationRepository = new DonationRepository(petShelterContext);
IDonationFundraiserRepository donationFundraiserRepository = new DonationFundraiserRepository(petShelterContext);
IFundraiserRepository fundraiserRepository = new FundraiserRepository(petShelterContext);
IPetRepository petRepository = new PetRepository(petShelterContext);
IPersonRepository personRepository = new PersonRepository(petShelterContext);

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();


/////////////////////////////
///Bonus
/////////////////////////////

var exit = false;
try
{
    while (!exit)
    {
        PresentOptions(
            "\nHere's what you can do.. ",
            new Dictionary<string, Action>
            {
                { "Register a newly rescued pet", RegisterPet },
                { "Register a newly helper", RegisterHelper },
                { "Current donation amount for a fundraiser", CurrentDonationAmount },
                { "Start a fundraiser", StartFundraiser },
                { "See all fundraisers and contribute", SeeFundraisers },
                { "See our residents", SeePets },
                { "See our helpers", SeeHelpers },
                { "Break our database connection", BreakDatabaseConnection },
                { "Leave:(", Leave }
            }
        );
    }
}
catch (Exception e)
{
    Console.WriteLine($"Unfortunately we ran into an issue: {e.Message}.");
    Console.WriteLine("Please try again later.");
}

bool CheckPetValidValues(string valueToCheck)
{
    if (valueToCheck == "0") return false;

    return true;
}

void RegisterPet()
{
    //IsHealthy and IsSheltered always default valuea True, i dont know why

    var name = ReadString("Name?");
    var imageUrl = ReadString("Link to a photo");
    var description = ReadString("Description?");
    var date = ReadString("Birthdate (format : yyyy-mm-dd)");
    DateTime birthdate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
    var type = ReadString("Type?");

    var healthy = ReadString("Is healthy? (0 or 1)");
    bool isHealthy = CheckPetValidValues(healthy);

    Console.WriteLine("Weight?");
    var weightInKgInteger = ReadInteger();
    decimal weightInKg = weightInKgInteger;

    var sheltered = ReadString("Is sheltered? (0 or 1)");
    bool isSheltered = CheckPetValidValues(sheltered);

    var rescuer = ReadString("IdNumber of rescuer ( xxxx format )");
    var personRescuer = personRepository.GetPersonByIdNumber(rescuer).Result;

    var adopter = ReadString("IdNumber of adopter ( xxxx format )");
    var personAdopter = personRepository.GetPersonByIdNumber(adopter).Result;

    var pet = new Pet()
    { 
        Name= name,
        ImageUrl= imageUrl, 
        Type= type,
        Description= description,
        Birthdate= birthdate,
        IsHealthy= isHealthy,
        WeightInKg= weightInKg,
        IsSheltered= isSheltered,
        RescuerId= personRescuer.Id,
        AdopterId= personAdopter.Id,
        Adopter= personAdopter,
        Rescuer= personRescuer
    };
    petRepository.Add(pet);

}

void RegisterHelper()
{
    
    var name = ReadString("Name?");

    var date = ReadString("Birthdate (format : yyyy-mm-dd)");
    DateTime birthdate = DateTime.ParseExact(date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
    
    var idNumber = ReadString("IdNumber?");
    
    Person person = new Person(name, birthdate, idNumber);

    personRepository.Add(person);
}

void CurrentDonationAmount()
{
    Console.WriteLine("Id of the fundraiser");
    var fundraiserId = ReadInteger();
    Console.WriteLine($"Fundraiser with Id: {fundraiserId} has a total of: " +
        $"{fundraiserRepository.GetDonationsForFundraiserById(fundraiserId)}");
}

void StartFundraiser()
{
    var title = ReadString("Title?");

    var description = ReadString("Description?");

    Console.WriteLine("Donation Target");
    var donationTarget = ReadInteger();

    var fundraiesr = new Fundraiser(title, description, donationTarget);

    fundraiserRepository.Add(fundraiesr);
}

void SeeFundraisers()
{
    var allFundraisers = fundraiserRepository.GetAll().Result;
    var fundraisersOptions = new Dictionary<string, Action>();

    foreach(var fundraiser in allFundraisers)
    {
        if (fundraiser.DonationTarget <= fundraiserRepository.GetDonationsForFundraiserById(fundraiser.Id))
        {
            fundraisersOptions.Add(fundraiser.Title + " ; " + SeeFundraiserDetailsByTitle(fundraiser.Id), () => {
                    Console.WriteLine("Already closed \n\n");
                    Console.WriteLine("We thank the donors:");
                    var donorsToSelectedFundraiser = fundraiserRepository.GetPersonsWhoDonatedFundraiserById(fundraiser.Id);
                    foreach (var donor in donorsToSelectedFundraiser)
                    {
                        Console.WriteLine($"{donor.Name} (^-^)");
                    }
                });
        }
        else
        {
            fundraisersOptions.Add(fundraiser.Title + " ; " + SeeFundraiserDetailsByTitle(fundraiser.Id),
                                  () => ContributeToFundraiser(fundraiser));
        }
    }
    fundraisersOptions.Add("Back, no donation :( ", () => Console.WriteLine("see ya \n\n"));

    PresentOptions("Choose where you contribute ", fundraisersOptions);


}

string SeeFundraiserDetailsByTitle(int id)
{
    var fundraiser = fundraiserRepository.GetById(id).Result;
    return ($"A few words about {fundraiser.Title}: {fundraiser.Description}," +
        $"{fundraiserRepository.GetDonationsForFundraiserById(fundraiser.Id)}/{fundraiser.DonationTarget}");

}

void ContributeToFundraiser(Fundraiser fundraiserParam)
{
    Console.WriteLine("What's your IdNumber?");
    var idNumber = ReadString();

    Console.WriteLine("Enter the amount of money for -> " + fundraiserParam.Title);
    var moneyAmount = ReadInteger();

    var allDonationFundraisers = donationFundraiserRepository.GetAll().Result;

    int ok = 0;
    foreach (var donationFundraiser in allDonationFundraisers)
    {
        if(donationFundraiser.FundraiserId==fundraiserParam.Id)
        {
            var donation = donationRepository.GetById(donationFundraiser.DonationId).Result;
            if (donation.DonorId == personRepository.GetPersonByIdNumber(idNumber).Result.Id)
            {
                ok = 1;
                Console.WriteLine("You have already helped in this fundraiser");
                break;
            }
        }
    }

    if (ok == 0)
    {
        var donation = new Donation(moneyAmount, personRepository.GetPersonByIdNumber(idNumber).Result.Id);

        petShelterContext.Donations.Add(donation);
        petShelterContext.SaveChanges();

        var donationFundraiser = new DonationFundraiser(donation.Id, fundraiserParam.Id);
        donationFundraiserRepository.Add(donationFundraiser);
    }
}

void SeeHelpers()
{
    var persons = personRepository.GetAll().Result;

    Console.WriteLine("Our helpers (^-^)");

    foreach(var person in persons)
    {
        Console.WriteLine(person.Name);
    }
}

string SeeDonorDetailsByName(string title)
{
    //NOT USED YET
    return "Something";
}

void SeePets()
{
    var pets = petRepository.GetAll().Result;

    Console.WriteLine("Our pets");
    Console.WriteLine(@" /\___/\ ");
    Console.WriteLine("(= ^.^ =)");
    Console.WriteLine(" (\") (\")_/\n");

    foreach (var pet in pets)
    {
        Console.WriteLine(pet.Name);
    }
}

void SeePetDetailsByName(string name)
{
    //NOT USED YET
}

void BreakDatabaseConnection()
{
    PetShelterContext.ConnectionIsDown = true;
}

void Leave()
{
    Console.WriteLine("Good bye!");
    exit = true;
}

void PresentOptions(string header, IDictionary<string, Action> options)
{

    Console.WriteLine(header);

    for (var index = 0; index < options.Count; index++)
    {
        Console.WriteLine(index + 1 + ". " + options.ElementAt(index).Key);
    }

    var userInput = ReadInteger(options.Count);

    options.ElementAt(userInput - 1).Value();
}

string ReadString(string? header = null)
{
    if (header != null) Console.WriteLine(header);

    var value = Console.ReadLine();
    Console.WriteLine("");
    return value;
}

int ReadInteger(int maxValue = int.MaxValue, string? header = null)
{
    if (header != null) Console.WriteLine(header);

    var isUserInputValid = int.TryParse(Console.ReadLine(), out var userInput);
    if (!isUserInputValid || userInput > maxValue)
    {
        Console.WriteLine("Invalid input");
        Console.WriteLine("");
        return ReadInteger(maxValue, header);
    }

    Console.WriteLine("");
    return userInput;
}