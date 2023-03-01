using FluentAssertions;
using Moq;
using PetShelter.BusinessLayer.ExternalServices;
using PetShelter.BusinessLayer.Validators;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;

namespace PetShelter.BusinessLayer.Tests;

public class AddDonationTests
{


    [Fact]
    public async Task GivenValidRequest_WhenAddDonation_DonationIsAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var mockPersonRepository = new Mock<IPersonRepository>();
        var mockIdNumberValidator = new Mock<IIdNumberValidator>();

        var personService = new PersonService(mockPersonRepository.Object, mockIdNumberValidator.Object, new PersonValidator());
        var donationServiceSut = new DonationService(personService, mockDonationRepository.Object, new AddDonationRequestValidator());

        

        var request = new AddDonationRequest
        {
            Amount = 10,
            Person = new BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.ParseExact("2000-05-08", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture),
                IdNumber = "1111222233334",
                Name = "TestName"
            }
        };
        //await donationServiceSut.AddDonation(request);
        await Assert.ThrowsAsync<ArgumentException>(() => donationServiceSut.AddDonation(request));

        mockDonationRepository.Verify(x => x.Add(It.Is<Donation>(d => d.Amount == request.Amount)), Times.Never);
    }

    [Fact]
    public async Task GivenRequestWithMissingAmount_WhenAddDonation_DonationIsNotAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var mockPersonRepository = new Mock<IPersonRepository>();
        var mockIdNumberValidator = new Mock<IIdNumberValidator>();

        var personService = new PersonService(mockPersonRepository.Object, mockIdNumberValidator.Object, new PersonValidator());
        var donationServiceSut = new DonationService(personService, mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest
        {
            
            Person = new BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.ParseExact("2000-05-08", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture),
                IdNumber = "1111222233334",
                Name = "TestName"
            }
        };
        await Assert.ThrowsAsync<ArgumentException>(() => donationServiceSut.AddDonation(request));

        mockDonationRepository.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }

    [Fact]
    public async Task GivenRequestWithPositiveAmount_WhenAddDonation_DonationIsNotAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var mockPersonRepository = new Mock<IPersonRepository>();
        var mockIdNumberValidator = new Mock<IIdNumberValidator>();

        var personService = new PersonService(mockPersonRepository.Object, mockIdNumberValidator.Object, new PersonValidator());
        var donationServiceSut = new DonationService(personService, mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest
        {
            Amount = 10,
            Person = new BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.ParseExact("2000-05-08", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture),
                IdNumber = "1111222233334",
                Name = "TestName"
            }
        };

        //await donationServiceSut.AddDonation(request);
        await Assert.ThrowsAsync<ArgumentException>(() => donationServiceSut.AddDonation(request));
        
        mockDonationRepository.Verify(x => x.Add(It.Is<Donation>(d => d.Amount > 0)), Times.Never);
    }

    [Fact]
    public async Task GivenRequestWithWrongIdNumber_WhenAddDonation_DonationIsNotAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var mockPersonRepository = new Mock<IPersonRepository>();
        var mockIdNumberValidator = new Mock<IIdNumberValidator>();

        var personService = new PersonService(mockPersonRepository.Object, mockIdNumberValidator.Object, new PersonValidator());
        var donationServiceSut = new DonationService(personService, mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest
        {
            Amount = 10,
            Person = new BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.ParseExact("2000-05-08", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture),
                IdNumber = "1111222233334",
                Name = "TestName"
            }
        };

        mockIdNumberValidator.Setup(x => x.Validate(It.IsAny<string>())).ReturnsAsync(false);

        //await donationServiceSut.AddDonation(request);
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => donationServiceSut.AddDonation(request));
        exception.Message.Should().Be("CNP format is invalid");

        mockDonationRepository.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
}
