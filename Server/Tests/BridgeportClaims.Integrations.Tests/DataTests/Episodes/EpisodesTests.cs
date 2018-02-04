using System;
using System.ComponentModel.DataAnnotations;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.SessionFactory;
using BridgeportClaims.Entities.DomainModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Integrations.Tests.DataTests.Episodes
{
    [TestClass]
    public class EpisodesTests
    {
        private readonly IRepository<Episode> _episodeRepository;
        private readonly IRepository<AspNetUsers> _usersRepository;
        private readonly IRepository<EpisodeCategory> _episodeCategoryRepository;

        public EpisodesTests()
        {
            _episodeCategoryRepository = new Repository<EpisodeCategory>(FluentSessionProvider.GetSession());
            _episodeRepository = new Repository<Episode>(FluentSessionProvider.GetSession());
            _usersRepository = new Repository<AspNetUsers>(FluentSessionProvider.GetSession());
        }

        /*[TestMethod]
        public void CanSaveNewEpisode()
        {
            // Arrange, Act.
            var episode = _episodeRepository?.GetAll()?.OrderByDescending(x => x.EpisodeId).Take(1).Select(
                s => new EpisodeTestDto
                {
                    EpisodeId = s.EpisodeId,
                    AcquiredUser = s.AcquiredUser,
                    AssignedUser = s.AssignedUser,
                    Claim = s.Claim,
                    CreatedDateUtc = s.Created,
                    CreatedOnUtc = s.CreatedOnUtc,
                    Description = s.Description,
                    EpisodeType = s.EpisodeType,
                    Note = s.Note,
                    ResolvedDateUtc = s.ResolvedDateUtc,
                    UpdatedOnUtc = s.UpdatedOnUtc,
                    RxNumber = s.RxNumber,
                    ResolvedUser = s.ResolvedUser,
                    ModifiedByUser = s.ModifiedByUser,
                    Role = s.Role,
                    Status = s.Status,
                    EpisodeCategory = s.EpisodeCategory
                }).SingleOrDefault();

            var users = _usersRepository.GetAll()?.OrderBy(x => Guid.NewGuid()).ToArray();

            // Assert.
            Assert.IsNotNull(episode);
            Assert.IsNotNull(users);

            var entity = new Episode
            {
                AcquiredUser = users[0],
                AssignedUser = users[1],
                Claim = episode.Claim,
                Created = episode.CreatedDateUtc,
                CreatedOnUtc = episode.CreatedOnUtc,
                Description = episode.Description,
                EpisodeType = episode.EpisodeType,
                Note = episode.Note,
                ResolvedDateUtc = episode.ResolvedDateUtc,
                UpdatedOnUtc = episode.UpdatedOnUtc,
                RxNumber = episode.RxNumber,
                ResolvedUser = users[2],
                ModifiedByUser = users[3],
                Role = episode.Role,
                Status = episode.Status,
                EpisodeCategory = _episodeCategoryRepository.GetMany(x => x.Code == "IMAGE").Single()
            };

            _episodeRepository.Save(entity);
        }*/

        private class EpisodeTestDto
        {
            [Required]
            public int EpisodeId { get; set; }
            [Required]
            public Claim Claim { get; set; }
            public EpisodeType EpisodeType { get; set; }
            public AspNetUsers AcquiredUser { get; set; }
            public AspNetUsers AssignedUser { get; set; }
            public AspNetUsers ResolvedUser { get; set; }
            public AspNetUsers ModifiedByUser { get; set; }
            public Pharmacy Pharmacy { get; set; }
            public DocumentIndex DocumentIndex { get; set; }
            [Required]
            public EpisodeCategory EpisodeCategory { get; set; }
            [Required]
            [StringLength(8000)]
            public string Note { get; set; }
            [StringLength(25)]
            public string Role { get; set; }
            [StringLength(100)]
            public string RxNumber { get; set; }
            [StringLength(1)]
            public string Status { get; set; }
            public DateTime? CreatedDateUtc { get; set; }
            [StringLength(255)]
            public string Description { get; set; }
            public DateTime? ResolvedDateUtc { get; set; }

            [Required]
            public DateTime CreatedOnUtc { get; set; }

            [Required]
            public DateTime UpdatedOnUtc { get; set; }
        }
    }
}
