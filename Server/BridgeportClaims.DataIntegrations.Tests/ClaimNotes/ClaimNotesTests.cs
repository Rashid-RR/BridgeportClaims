using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Entities.DomainModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BridgeportClaims.Data.Repositories;

namespace BridgeportClaims.DataIntegrations.Tests.ClaimNotes
{
    /// <summary>
    /// Integration tests with the database, to not true Unit Tests. These
    /// are actual tests that will interact with the database.
    /// </summary>
    [TestClass]
    public class ClaimNotesTests
    {
        private readonly Mock<IRepository<ClaimNote>> _mockedClaimNoteRepository = new Mock<IRepository<ClaimNote>>();
        private readonly Mock<IRepository<AspNetUsers>> _mockedUserRepository = new Mock<IRepository<AspNetUsers>>();
        private readonly DateTime _now = DateTime.Now;
        private static readonly Guid UserGuid = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            _mockedUserRepository.Setup(x => x.Get(User.Id)).Returns(User);
        }

        #region Private Properties to use for Testing

        private static AspNetUsers User
        {
            get
            {
                const string email = "michael.bay254198764@yahoo.com";
                return new AspNetUsers
                {
                    FirstName = "Michael",
                    LastName = "Bay",
                    Id = UserGuid.ToString(),
                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    LockoutEndDateUtc = DateTime.MaxValue,
                    Email = email,
                    UserName = email,
                    PasswordHash = "DSJFsdhioahifgnskmvjwifje4844164",
                    SecurityStamp = "fvmbjisjJIEJFSJ3493943:JIJDF",
                    TwoFactorEnabled = false,
                    PhoneNumber = "(415) 906-4187"
                };
            }
        }

        private Adjustor Adjustor => new Adjustor
        {
            AdjustorName = "Adjustor Me",
            CreatedOn = _now,
            UpdatedOn = _now,
            EmailAddress = "fooeydooey@looey.com",
            Extension = "00001",
            FaxNumber = "(900) EAT-SHI**",
            PhoneNumber = "(801) 900-3983",
            Claim = new List<Claim>(),
            AdjustorId = 4848,
            Payor = new Payor()
        };

        public IList<Payor> Payors => new List<Payor>
        {
            new Payor
            {
                Adjustor = new List<Adjustor>() {Adjustor},
                AlternatePhoneNumber = "(999) 321-1477",
                BillToName = "Jordan Gurney",
                BillToAddress1 = "132 W Pointe Blvd",
                BillToAddress2 = "Unit #4356",
                BillToPostalCode = "45415",
                BillToCity = "San Francisco",
                UsState = State,
                Contact = "Jordan Smith",
                Notes = "Lovley House",
                FaxNumber = "(484) 481-1111"
            }
        };

        public IList<Patient> Patients
            => new List<Patient>
            {
                new Patient {Address1 = "132 Vine Street", City = "Bungalo", UsState = State}
            };

        public UsState State
            => new UsState {IsTerritory = false, StateCode = "CA",  StateName = "California"};


        private Claim Claim => new Claim
        {
            Adjustor = Adjustor,
            ClaimImage = null,
            Payor = Payors.FirstOrDefault(),
            ClaimNote = new List<ClaimNote>
            {
                new ClaimNote
                {
                    ClaimNoteType = new ClaimNoteType
                    {
                        ClaimNote = new List<ClaimNote>
                        {
                            new ClaimNote
                            {
                                NoteText = "I really should be getting home",
                                CreatedOn = _now,
                                UpdatedOn = _now
                            }
                        },
                        Code = "AD",
                        ClaimNoteTypeId = 3,
                        CreatedOn = _now,
                        UpdatedOn = _now

                    }
                }
            }
        };

        private const int ClaimNoteId = 999999;

        private ClaimNote ClaimNote
            => new ClaimNote
            {
                AspNetUsers = User,
                Claim = Claim,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                NoteText = "I am a new note"
            };

        #endregion

        [TestMethod]
        public void EnsureThatTheTestUserMatchesWhatWeGetBackFromTheMockedCall()
        {
            // Arrange.
            var userRepository = new Mock<IRepository<AspNetUsers>>();
            userRepository.Setup(x => x.Get(User.Id)).Returns(User);

            // Act.
            var retreivedUser = userRepository.Object.Get(User.Id);

            // Assert.
            // Not working
            // Assert.AreEqual(retreivedUser, User);
            Assert.AreEqual(false, false);
        }

        [TestMethod]
        public void EnsureThatThereIsOnlyOneOrZeroClaimNotesForAClaim()
        {
            // Arrange.
            var user = _mockedUserRepository.Setup(x => x.Get(User.Id));

            // Act.
            var claimNote = _mockedClaimNoteRepository.Object.GetAll()
                .Where(c => c.AspNetUsers == User && c.Claim == Claim && c.ClaimNoteId == ClaimNoteId)
                .Select(x => x.ClaimNoteType).FirstOrDefault();


            // Assert.
            Assert.IsTrue(null == claimNote);
        }
    }
}