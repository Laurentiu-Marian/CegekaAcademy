using FluentAssertions;
using Moq;

using PetShelter.Domain.Services;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using PetShelter.Api.Validators;

using PetShelter.Domain.Services;
using PetShelter.Api.Resources;

namespace PetShelter.BusinessLayer.Tests
{
    public class RescuePetTests
    {
        private readonly IPetService _petServiceSut;

        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<IPetRepository> _mockPetRepository;

        public RescuePetTests()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockPetRepository = new Mock<IPetRepository>();


            _petServiceSut = new PetService(_mockPetRepository.Object, _mockPersonRepository.Object);
        }

        private void SetupHappyPath()
        {
            //_mockIdNumberValidator.Setup(x => x.Validate(It.IsAny<string>())).ReturnsAsync(true);

            //_request = new RescuePetRequest
            //{
            //    PetName = "Max",
            //    Type = Constants.PetType.Dog,
            //    Description = "Nice dog",
            //    IsHealthy = true,
            //    ImageUrl = "test",
            //    WeightInKg = 10,
            //    Person = new BusinessLayer.Models.Person
            //    {
            //        DateOfBirth = DateTime.Now.AddYears(-Constants.PersonConstants.AdultMinAge),
            //        IdNumber = "1111222233334",
            //        Name = "TestName"
            //    }
            //};
        }

        [Fact]
        public async void GivenValidName_WhenCreatePerson()
        {
            var _roPersonBuilder = Mock.Of<PetShelter.Api.Resources.RoPersonBuilder>();
            _roPersonBuilder.AddName("Laurentiu");

            Assert.Equal("Laurentiu", _roPersonBuilder.GetPerson().Name);

        }

        [Fact]
        public async void GivenValidDateOfBirth_WhenCreatePerson()
        {
            var _roPersonBuilder = Mock.Of<PetShelter.Api.Resources.RoPersonBuilder>();

            var dateOfBirth = DateTime.Now;

            _roPersonBuilder.AddDateOfBirth(dateOfBirth);

            Assert.Equal(dateOfBirth, _roPersonBuilder.GetPerson().DateOfBirth);

        }

        [Fact]
        public async void GivenValidIdNumber_WhenCreatePerson()
        {
            var _roPersonBuilder = Mock.Of<PetShelter.Api.Resources.RoPersonBuilder>();
            
            _roPersonBuilder.AddIdNumber("1234567891234");

            Assert.Equal("1234567891234", _roPersonBuilder.GetPerson().IdNumber);

        }

        [Fact]
        public async void GivenValidData_WhenCreatePerson()
        {
            var _roPersonBuilder = Mock.Of<PetShelter.Api.Resources.RoPersonBuilder>();

            var dateOfBirth = DateTime.Now;
            _roPersonBuilder.AddAllAtributes("Alexandru", dateOfBirth, "4321987654321");

            Assert.Equal("Alexandru", _roPersonBuilder.GetPerson().Name);
            //Assert.Equal(dateOfBirth, _roPersonBuilder.GetPerson().DateOfBirth);
            //Assert.Equal("1234567891234", _roPersonBuilder.GetPerson().IdNumber);

        }

        
        [Fact]
        public async void GivenValidData_WhenRescuePet_PetIsAdded()
        {
            //Arrange
            SetupHappyPath();

            //Act
            //await _petServiceSut.AdoptPetAsync(_request);

            //Assert
            //_mockPetRepository.Verify(x => x.Add(It.Is<Pet>(p => p.Name == _request.PetName)), Times.Once);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(-5)]
        [InlineData(0)]
        public async Task GiventWeightIsInvalid_WhenRescuePet_ThenThrowsArgumentException_And_PetIsNotAdded(decimal weight)
        {
            // Arrange
            //SetupHappyPath();
            //_request.WeightInKg = weight;

            ////Act
            //await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));

            ////Assert
            //_mockPetRepository.Verify(x => x.Add(It.Is<Pet>(p => p.Name == _request.PetName)), Times.Never);
        }


        [Fact]
        public async Task GivenIdNumberIsInvalid_WhenRescuePet_ThenThowsArgumentException()
        {
            //    //Arrange
            //    SetupHappyPath();

            //_mockIdNumberValidator.Setup(x => x.Validate(It.IsAny<string>())).ReturnsAsync(false);
            
            //Act
            //var exception = await Assert.ThrowsAsync<ArgumentException>(() => _petServiceSut.RescuePet(_request));

            //exception.Message.Should().Be("CNP format is invalid");

            //Assert
            //_mockPetRepository.Verify(x => x.Add(It.Is<Pet>(p => p.Name == _request.PetName)), Times.Never);
        }
        
    }
}