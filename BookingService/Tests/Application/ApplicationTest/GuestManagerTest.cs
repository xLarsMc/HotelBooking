using Application;
using Application.Guest;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Domain.Guest.Entites;
using Domain.Guest.Enums;
using Domain.Guest.Ports;
using Moq;

namespace ApplicationTest
{
    //São mocks manuais.
    class FakeGuestRepository : IGuestRepository
    {
        public Task<int> CreateGuest(Guest guest)
        {
            return Task.FromResult(111);
        }

        public Task<Guest> GetGuest(int id)
        {
            throw new NotImplementedException();
        }
    }

    //Mocks via nuget Mop

    public class Tests
    {
        GuestManager guestManager;

        [SetUp]
        public void Setup()
        {
            //var fakeRepo = new FakeGuestRepository();
        }

        [Test]
        public async Task Should_Return_GuestNotFound_WhenGuestDoesntExists()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.GetGuest(333)).Returns(Task.FromResult<Guest?>(null));

            guestManager = new GuestManager(fakeRepo.Object);

            var response = await guestManager.GetGuest(333);

            Assert.IsNotNull(response);
            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.GUEST_NOT_FOUND);
            Assert.AreEqual(response.Message, "No Guest record has been found with the given id");
        }

        [Test]
        public async Task Should_Return_Guest_WhenGuestExists()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            var fakeGuest = new Guest
            {
                Id = 333,
                Name = "Test",
                DocumentId = new Domain.Guest.ValueObjects.PersonId
                {
                    DocumentType = DocumentType.DriveLicense,
                    IdNumber = "123"
                }
            };

            fakeRepo.Setup(x => x.GetGuest(333)).Returns(Task.FromResult((Guest?)fakeGuest));

            guestManager = new GuestManager(fakeRepo.Object);

            var response = await guestManager.GetGuest(333);

            Assert.IsNotNull(response);
            Assert.True(response.Success);
            Assert.AreEqual(response.Data.Id, fakeGuest.Id);
            Assert.AreEqual(response.Data.Name, fakeGuest.Name);
        }

        [Test]
        public async Task HappyPath()
        {
            int expectedId = 222;

            var guestDto = new GuestDTO
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "fulano@gmail.com",
                IdNumber = "abca",
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto
            };


            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.CreateGuest(
                It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

            guestManager = new GuestManager(fakeRepo.Object);

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.True(response.Success);
            Assert.AreEqual(response.Data.Id, expectedId);
            Assert.AreEqual(response.Data.Name, guestDto.Name);
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task Should_Return_InvalidPersonDocumentIdException_When_DocsAreInvalid(string? docNumber)
        {

            var guestDto = new GuestDTO
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "fulano@gmail.com",
                IdNumber = docNumber,
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto
            };


            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.CreateGuest(
                It.IsAny<Guest>())).Returns(Task.FromResult(222));

            guestManager = new GuestManager(fakeRepo.Object);

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.INVALID_PERSON_ID);
            Assert.AreEqual(response.Message, "The ID passed is not valid");
        }

        [Test]
        [TestCase("", "surnametest", "asdf@gmail.com")]
        [TestCase(null, "surnametest", "asdf@gmail.com")]

        [TestCase("Fulano", null, "asdf@gmail.com")]
        [TestCase("Fulano", "", "asdf@gmail.com")]

        [TestCase("Fulano", "surnametest", null)]
        [TestCase("Fulano", "surnametest", "")]
        public async Task Should_Return_MissingRequiredInformation_WhenNameOrSurnameOrEmailAreInvalid(string? name, string? surname, string? email)
        {

            var guestDto = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "avcdsw",
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto
            };


            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.CreateGuest(
                It.IsAny<Guest>())).Returns(Task.FromResult(222));

            guestManager = new GuestManager(fakeRepo.Object);

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(response.Message, "Missing required information passed");
        }

        [Test]
        [TestCase("b@b.com")]
        public async Task Should_Return_InvalidEmail_EmailAreInvalid(string? email)
        {

            var guestDto = new GuestDTO
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = email,
                IdNumber = "avcdsw",
                IdTypeCode = 1,
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto
            };


            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.CreateGuest(
                It.IsAny<Guest>())).Returns(Task.FromResult(222));

            guestManager = new GuestManager(fakeRepo.Object);

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.False(response.Success);
            Assert.AreEqual(response.ErrorCode, ErrorCodes.INVALID_EMAIL);
            Assert.AreEqual(response.Message, "The given email is not valid");
        }

    }
}