//// See https://aka.ms/new-console-template for more information
//// Syntactic sugar: Starting with .Net 6, Program.cs only contains the code that is in the Main method.
//// This means we no longer need to write the following code, but the compiler still creates the Program class with the Main method:
//// namespace PetShelterDemo
//// {
////    internal class Program
////    {
////        static void Main(string[] args)
////        { actual code here }
////    }
//// }

using PetShelterDemo.DAL;
using PetShelterDemo.Domain;

var shelter = new PetShelter();

Console.WriteLine("Hello, Welcome the the Pet Shelter!");

var exit = false;
try
{
    while (!exit)
    {
        PresentOptions(
            "Here's what you can do.. ",
            new Dictionary<string, Action>
            {
                { "Register a newly rescued pet", RegisterPet },
                { "Donate", Donate },
                { "Start a fundraiser", StartFundraiser },
                { "See all fundraisers and contribute", SeeFundraisers },
                { "See current donations total", SeeDonations },
                { "See our residents", SeePets },
                { "See our donors", SeeDonors },
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

void StartFundraiser()
{
    Console.WriteLine("Name the fundraise: ");
    var title = ReadString();

    Console.WriteLine("Briefly describe for future donors: ");
    var description = ReadString();

    Console.WriteLine("What's the target amount? (RON)");
    var targetDonation = ReadInteger();

    var fundraiser = new Fundraiser(title, description, targetDonation);

    shelter.RegisterFundraise(fundraiser);
    Console.WriteLine("Fundraise created");
}

void SeeFundraisers()
{
    var fundraisers = shelter.GetAllFundraisers();

    var fundraisersOptions = new Dictionary<string, Action>();
    foreach (var fundraiser in fundraisers)
    {
        if(fundraiser.DonationTarget<=fundraiser.CurrentDonation)
        {
            fundraisersOptions.Add(fundraiser.Title + " ; " + SeeFundraiserDetailsByTitle(fundraiser.Title), () => { Console.WriteLine("Already closed \n\n");
                                                                                                                        var donations = fundraiser.GetAllFundraiserDonations();
                                                                                                                        Console.WriteLine("We thank the donors:");
                                                                                                                        foreach (var donation in donations)
                                                                                                                        {
                                                                                                                            Console.WriteLine($"{donation.Name}: {donation.RONDonation}RON & {donation.EURODonation}EURO");
                                                                                                                        }
                                                                                                                    });
        }
        else
        {
            fundraisersOptions.Add(fundraiser.Title + " ; " + SeeFundraiserDetailsByTitle(fundraiser.Title), () => ContributeToFundraiser(fundraiser));
        }
    }
    fundraisersOptions.Add("Back, no donation :( ", () => Console.WriteLine("see ya \n\n"));

    PresentOptions("Choose where you contribute ", fundraisersOptions);
}

string SeeFundraiserDetailsByTitle(string title)
{
    var fundraiser = shelter.GetFundraiserByName(title);
    //Console.WriteLine($"A few words about {fundraiser.Title}: {fundraiser.Description}"+"\n\n");
    return ($"A few words about {fundraiser.Title}: {fundraiser.Description}, {fundraiser.CurrentDonation}/{fundraiser.DonationTarget} RON");
}

void ContributeToFundraiser(Fundraiser fundraiser)
{
    Console.WriteLine("What's your personal ID? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();

    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();

    Console.WriteLine("Enter the amount of money for -> "+ fundraiser.Title + "<- \n RON or/and EURO");

    Console.WriteLine("RON: ");
    var moneyAmountRON = ReadInteger();

    Console.WriteLine("EURO: ");
    var moneyAmountEURO = ReadInteger();

    var donations = fundraiser.GetAllFundraiserDonations();

    int ok = 0;
    foreach (var donation in donations)
    {
        if (donation.Name.Equals(name) && donation.IdNumber.Equals(id))
        {
            ok = 1;
            Console.WriteLine("You have already helped in this fundraiser");
            break;
        }
    }

    if (ok == 0)
    {
        var person = new Donations(name, id, moneyAmountRON, moneyAmountEURO); ;
        fundraiser.CurrentDonation += moneyAmountRON + 5 * moneyAmountEURO;
        fundraiser.RegisterFundraise(person);
    }

    Console.WriteLine("We thank the other donors:");
    foreach (var donation in donations)
    {
        Console.WriteLine($"{donation.Name}: {donation.RONDonation}RON & {donation.EURODonation}EURO");
    }
}

////////////////////////////////////////////////////
///STIU CA ESTE COD DUPLICAT, NU MI-A VENIT IN MINTE CUM SA GRUPEZ
///METODELE, DAR SA PASTREZ SPECIFICUL, UNA PENTRU DONATII GENERALE,
///ALTA PENTRU STRANGERI DE FONDURI
////////////////////////////////////////////////////

void Donate()
{
    Console.WriteLine("You are going to donate for the entire shelter...");

    Console.WriteLine("What's your personal ID? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();

    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();

    Console.WriteLine("Enter the amount of money \n RON or/and EURO");

    Console.WriteLine("RON: ");
    var moneyAmountRON = ReadInteger();

    Console.WriteLine("EURO: ");
    var moneyAmountEURO = ReadInteger();

    var donors = shelter.GetAllDonors();
    int ok = 0;
    foreach (var donor in donors)
    {
        if (donor.Name.Equals(name) && donor.IdNumber.Equals(id))
        {
            ok = 1;
            donor.RONDonation += moneyAmountRON;
            donor.EURODonation += moneyAmountEURO;
            shelter.SetDonationAmounts(moneyAmountRON, moneyAmountEURO);
            break;
        }
    }

    if (ok == 0)
    {
        var person = new Donations(name, id, moneyAmountRON, moneyAmountEURO); ;

        shelter.Donate(person, moneyAmountRON, moneyAmountEURO);
    }

}

void RegisterPet()
{
    var name = ReadString("Name?");
    var id = ReadString("ID?");
    var description = ReadString("Description?");

    var pet = new Pet(name, id, description);

    shelter.RegisterPet(pet);
}

void SeeDonors()
{
    var donors = shelter.GetAllDonors();

    var donorOptions = new Dictionary<string, Action>();
    foreach (var donor in donors)
    {
        donorOptions.Add(donor.Name + " ; " + SeeDonorDetailsByName(donor.Name), () => Console.WriteLine("see ya \n\n"));
    }
    donorOptions.Add("Back", () => Console.WriteLine("see ya \n\n"));

    PresentOptions("Good people: ", donorOptions);
}

string SeeDonorDetailsByName(string title)
{
    var donor = shelter.GetDonorByName(title);
    //Console.WriteLine($"A few words about {fundraiser.Title}: {fundraiser.Description}"+"\n\n");
    return ($"A few words about {donor.Name }: {donor.RONDonation} RON & {donor.EURODonation} EURO");
}

void SeeDonations()
{
    Console.WriteLine($"Our current direct donation for shelter (no foundraisers) is {shelter.GetTotalDonationsInRON()}RON & {shelter.GetTotalDonationsInEURO()}EURO");
    Console.WriteLine($"With a total of: {shelter.GetTotalDonationsInRON()+5*shelter.GetTotalDonationsInEURO()} RON (1EURO = 5RON)");
    Console.WriteLine("Special thanks to our donors:");
    var donors = shelter.GetAllDonors();
    foreach (var donor in donors)
    {
        Console.WriteLine(donor.Name);
    }
}

void SeePets()
{
    var pets = shelter.GetAllPets();

    var petOptions = new Dictionary<string, Action>();
    foreach (var pet in pets)
    {
        petOptions.Add(pet.Name, () => SeePetDetailsByName(pet.Name));
    }

    PresentOptions("We got..", petOptions);
}

void SeePetDetailsByName(string name)
{
    var pet = shelter.GetByName(name);
    Console.WriteLine($"A few words about {pet.Name}: {pet.Description}\n\n");
}

void BreakDatabaseConnection()
{
    Database.ConnectionIsDown = true;
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
