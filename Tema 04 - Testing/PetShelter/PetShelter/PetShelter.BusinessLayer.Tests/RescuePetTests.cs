using FluentAssertions;
using Moq;
using PetShelter.BusinessLayer.Constants;
using PetShelter.BusinessLayer.ExternalServices;
using PetShelter.BusinessLayer.Models;
using PetShelter.BusinessLayer.Validators;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;

namespace PetShelter.BusinessLayer.Tests
{
    public class RescuePetTests
    {
        private readonly IPersonService _personService;
        private readonly PetService _petServiceSut;

        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<IPetRepository> _mockPetRepository;
        private readonly Mock<IIdNumberValidator> _mockIdNumberValidator;


        private RescuePetRequest _request;


        public RescuePetTests()
        {
            _mockPersonRepository= new Mock<IPersonRepository>();
            _mockPetRepository= new Mock<IPetRepository>(); 
            _mockIdNumberValidator = new Mock<IIdNumberValidator>();

            _personService = new PersonService(_mockPersonRepository.Object, _mockIdNumberValidator.Object, new PersonValidator());
            _petServiceSut = new PetService(_personService, _mockPetRepository.Object, new RescuePetRequestValidator(), new AdoptPetRequestValidator());
        }

        private void SetupHappyPath()
        {
            _mockIdNumberValidator.Setup(x => x.Validate(It.IsAny<string>())).ReturnsAsync(true);

            _request = new RescuePetRequest
            {
                PetName = "Max",
                Type = Constants.PetType.Dog,
                Description = "Nice dog",
                IsHealthy = true,
                ImageUrl = "test",
                WeightInKg = 10,
                Person = new BusinessLayer.Models.Person
                {
                    DateOfBirth = DateTime.Now.AddYears(-Constants.PersonConstants.AdultMinAge),
                    IdNumber = "1111222233334",
                    Name = "TestName"
                }
            };
        }

        [Fact]
        public async void GivenValidData_WhenRescuePet_PetIsAdded()
        {
            //Arrange
            SetupHappyPath();

            //Act
            await _petServiceSut.RescuePet(_request);

            //Assert
            _mockPetRepository.Verify(x => x.Add(It.Is<Pet>(p => p.Name == _request.PetName)), Times.Once);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(-5)]
        [InlineData(0)]
        public async Task GiventWeightIsInvalid_WhenRescuePet_ThenThrowsArgumentException_And_PetIsNotAdded(decimal weight)
        {
            // Arrange
            SetupHappyPath();
            _request.WeightInKg = weight;

            //Act
            await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));

            //Assert
            _mockPetRepository.Verify(x => x.Add(It.IsAny<Pet>()), Times.Never);
        }

        

        //  Testare nume corect, minim 3 caractere
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("a")]
        public async Task GivenNameIsInvalid_WhenRescuePet_ThenThrowsArgumentException(string name)
        {
            SetupHappyPath();
            _request.Person.Name = name;

            //Act
            await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));

            //Assert
            _mockPersonRepository.Verify(x => x.Add(It.Is<DataAccessLayer.Models.Person>(p => p.Name == _request.Person.Name)), Times.Never);
        }

        // Testare PetType in enum
        [Fact]
        public async Task GivenTypeIsValid_WhenRescuePet_ThenThrowsArgumentException()
        {
            //Arrange
            SetupHappyPath();
            //_request.Type = type;

            //Act
            //await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));
            Assert.NotNull(_petServiceSut.RescuePet(_request));

            //Assert
            _mockPetRepository.Verify(x => x.Add(It.IsAny<Pet>()));

        }

        [Fact]
        public async Task GivenIdNumberIsInvalid_WhenRescuePet_ThenThrowsArgumentException()
        {
            //Arrange
            SetupHappyPath();

            _mockIdNumberValidator.Setup(x => x.Validate(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));

            exception.Message.Should().Be("CNP format is invalid");

            //Assert
            _mockPetRepository.Verify(x => x.Add(It.Is<Pet>(p => p.Name == _request.PetName)), Times.Never);
        }

        [Fact]
        public async Task GivenPersonIsNull_WhenRescuePet_ThenThrowsArgumentException()
        {
            //Arrange
            SetupHappyPath();
            //DataAccessLayer.Models.Person expectedPerson = new DataAccessLayer.Models.Person
            //{
            //    DateOfBirth = DateTime.Now.AddYears(-Constants.PersonConstants.AdultMinAge),
            //    IdNumber = "1111222233334",
            //    Name = "TestName"
            //};
            _request.Person = null;

            //Act

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));

            //Assert
            Assert.NotNull(exception);

        }

    }
}