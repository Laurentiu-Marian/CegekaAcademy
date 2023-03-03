using FluentValidation;
using PetShelter.BusinessLayer.Validators;
using PetShelter.DataAccessLayer.Repository;

namespace PetShelter.BusinessLayer.Tests;

public class DonationService
{
    private readonly IDonationRepository _donationRepository;
    private readonly IValidator<AddDonationRequest> _donationValidator;
    private readonly IPersonService _personService;


    public DonationService(IPersonService personService, IDonationRepository donationRepository, 
        IValidator<AddDonationRequest> validator)
    {
        _donationValidator = validator;
        _donationRepository = donationRepository;
        _personService = personService;
    }

    public async Task AddDonation(AddDonationRequest addDonationRequest)
    {
        var validationResult = _donationValidator.Validate(addDonationRequest);
        if(!validationResult.IsValid) { throw new ArgumentException(); }

        var donor = await _personService.GetPerson(addDonationRequest.Person);
        

        await _donationRepository.Add(new DataAccessLayer.Models.Donation
        {
            Amount = addDonationRequest.Amount,
            DonorId = donor.Id,
            Donor = donor
            
        });
    }
}
