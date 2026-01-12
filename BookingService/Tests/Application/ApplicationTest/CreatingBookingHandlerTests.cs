using Application;
using Application.Booking.Commands;
using Application.Booking.DTO;
using Domain.Booking.Entities;
using Domain.Booking.Ports;
using Domain.Guest.Entites;
using Domain.Guest.Enums;
using Domain.Guest.Ports;
using Domain.Guest.ValueObjects;
using Domain.Room.Entities;
using Domain.Room.Ports;
using Moq;

namespace ApplicationTest
{
    [TestFixture]
    internal class CreateBookingCommandHandlerTests
    {
        private CreatingBookingCommandHandler GetCommandMock(
            Mock<IRoomRepository> roomRepository = null,
            Mock<IGuestRepository> guestRepository = null,
            Mock<IBookingRepository> bookingRepository = null)
        {
            var _bookingRepository = bookingRepository ?? new Mock<IBookingRepository>();
            var _guestRepository = guestRepository ?? new Mock<IGuestRepository>();
            var _roomRepository = roomRepository ?? new Mock<IRoomRepository>();

            var commandHandler = new CreatingBookingCommandHandler(_bookingRepository.Object, _roomRepository.Object, _guestRepository.Object);

            return commandHandler;
        }
            [Test]
            public async Task Should_Not_CreateBooking_If_Room_Is_Missing()
            {
                var command = new CreateBookingCommand
                {
                    BookingDto = new Application.Booking.DTO.BookingDto
                    {
                        // RoomId= 1,
                        GuestId = 1,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(2),
                    }
                };

                var fakeGuest = new Guest
                {
                    Id = command.BookingDto.GuestId,
                    DocumentId = new Domain.Guest.ValueObjects.PersonId
                    {
                        DocumentType = Domain.Guest.Enums.DocumentType.Passport,
                        IdNumber = "abc1234"
                    },
                    Email = "a@a.com",
                    Name = "Fake Guest",
                    Surname = "Fake Guest Surname"
                };

                var guestRepository = new Mock<IGuestRepository>();
                guestRepository.Setup(x => x.GetGuest(command.BookingDto.GuestId))
                    .Returns(Task.FromResult(fakeGuest));

                var fakeRoom = new Room
                {
                    Id = command.BookingDto.RoomId,
                    InMaintenance = false,
                    Price = new Domain.Guest.ValueObjects.Price
                    {
                        Currency = AcceptedCurrencies.Dollar,
                        Value = 100
                    },
                    Name = "Fake Room 01",
                    Level = 1,
                };

                var fakeBooking = new Booking
                {
                    Id = 1
                };

                var bookingRepository = new Mock<IBookingRepository>();
                bookingRepository.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                    .Returns(Task.FromResult(fakeBooking));

                var handler = GetCommandMock(null, guestRepository, bookingRepository);
                var resp = await handler.Handle(command, CancellationToken.None);

                Assert.NotNull(resp);
                Assert.False(resp.Success);
                Assert.AreEqual(resp.ErrorCode, ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION);
                Assert.AreEqual(resp.Message, "Room is a required information");
            }

            [Test]
            public async Task Should_Not_CreateBooking_If_StartDateIsMissing()
            {
                var command = new CreateBookingCommand
                {
                    BookingDto = new BookingDto
                    {
                        RoomId = 1,
                        GuestId = 1,
                        //Start = DateTime.Now,
                        End = DateTime.Now.AddDays(2),
                    }
                };

                var fakeGuest = new Guest
                {
                    Id = command.BookingDto.GuestId,
                    DocumentId = new PersonId
                    {
                        DocumentType = DocumentType.Passport,
                        IdNumber = "abc1234"
                    },
                    Email = "a@a.com",
                    Name = "Fake Guest",
                    Surname = "Fake Guest Surname"
                };

                var guestRepository = new Mock<IGuestRepository>();
                guestRepository.Setup(x => x.GetGuest(command.BookingDto.GuestId))
                    .Returns(Task.FromResult(fakeGuest));

                var fakeRoom = new Room
                {
                    Id = command.BookingDto.RoomId,
                    InMaintenance = false,
                    Price = new Price
                    {
                        Currency = AcceptedCurrencies.Dollar,
                        Value = 100
                    },
                    Name = "Fake Room 01",
                    Level = 1,
                };

                var fakeBooking = new Booking
                {
                    Id = 1
                };

                var bookingRepository = new Mock<IBookingRepository>();
                bookingRepository.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                    .Returns(Task.FromResult(fakeBooking));

                var handler = GetCommandMock(null, guestRepository, bookingRepository);
                var resp = await handler.Handle(command, CancellationToken.None);

                Assert.NotNull(resp);
                Assert.False(resp.Success);
                Assert.AreEqual(resp.ErrorCode, ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION);
                Assert.AreEqual(resp.Message, "Start is a required information");
            }

            [Test]
            public async Task Should_CreateBooking()
            {
                var command = new CreateBookingCommand
                {
                    BookingDto = new BookingDto
                    {
                        RoomId = 1,
                        GuestId = 1,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddDays(2),
                    }
                };

                var fakeGuest = new Guest
                {
                    Id = command.BookingDto.GuestId,
                    DocumentId = new PersonId
                    {
                        DocumentType = DocumentType.Passport,
                        IdNumber = "abc1234"
                    },
                    Email = "a@a.com",
                    Name = "Fake Guest",
                    Surname = "Fake Guest Surname"
                };

                var guestRepository = new Mock<IGuestRepository>();
                guestRepository.Setup(x => x.GetGuest(command.BookingDto.GuestId))
                    .Returns(Task.FromResult(fakeGuest));

                var fakeRoom = new Room
                {
                    Id = command.BookingDto.RoomId,
                    InMaintenance = false,
                    Price = new Price
                    {
                        Currency = AcceptedCurrencies.Dollar,
                        Value = 100
                    },
                    Name = "Fake Room 01",
                    Level = 1,
                };

                var roomRepository = new Mock<IRoomRepository>();
                roomRepository.Setup(x => x.GetRoomWithAggregate(command.BookingDto.RoomId))
                    .Returns(Task.FromResult(fakeRoom));

                var fakeBooking = new Booking
                {
                    Id = 1,
                    Room = fakeRoom,
                    Guest = fakeGuest,

                };

                var bookingRepoMock = new Mock<IBookingRepository>();
                bookingRepoMock.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                    .Returns(Task.FromResult(fakeBooking));
                //bookingRepository.Setup(x => x.Save)

                var handler = GetCommandMock(roomRepository, guestRepository, bookingRepoMock);
                var resp = await handler.Handle(command, CancellationToken.None);

                Assert.NotNull(resp);
                Assert.True(resp.Success);
                Assert.NotNull(resp.Data);
                Assert.AreEqual(resp.Data.Id, command.BookingDto.Id);
            }
    }
}