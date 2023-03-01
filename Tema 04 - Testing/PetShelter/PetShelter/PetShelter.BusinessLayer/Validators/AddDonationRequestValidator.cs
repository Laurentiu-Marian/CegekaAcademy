using FluentValidation;
using PetShelter.BusinessLayer.Tests;
namespace PetShelter.BusinessLayer.Validators;

using PetShelter.BusinessLayer.Constants;
using PetShelter.BusinessLayer.Models;


public class AddDonationRequestValidator: AbstractValidator<AddDonationRequest>
{
	public AddDonationRequestValidator()
	{
        RuleFor(x => x.Amount).NotEmpty();
        
        //////
        RuleFor(x => x.Person).NotEmpty()
            .SetValidator(new PersonValidator());

        RuleFor(x => x.Person.IdNumber).Length(PersonConstants.IdNumberLength);

        RuleFor(x => x.Person.DateOfBirth).LessThan(DateTime.Now.Date.AddYears(-PersonConstants.AdultMinAge))
            .WithMessage("Adopter should be an adult.");
        //////
        
    }
}
