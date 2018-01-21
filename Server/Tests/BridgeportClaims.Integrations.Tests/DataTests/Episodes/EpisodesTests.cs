using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Integrations.Tests.DataTests.Episodes
{
    [TestClass]
    public class EpisodesTests
    {
        //private readonly IRepository<Episode> _episodeRepository;
        //private readonly IRepository<AspNetUsers> _usersRepository;

        public EpisodesTests()
        {
            //_episodeRepository = new Repository<Episode>(FluentSessionProvider.GetSession());
            //_usersRepository = new Repository<AspNetUsers>(FluentSessionProvider.GetSession());
        }

        /*[TestMethod]
        public void CanPullRandomEpisodeFromDb()
        {
            // Arrange, Act.
            var episode = _episodeRepository?.GetAll()?.OrderByDescending(x => x.EpisodeId).Take(1).Select(
                s => new EpisodeTestDto
                {
                    EpisodeId = s.EpisodeId,
                    AcquiredUserId = s.AcquiredUserId,
                    AssignedUserId = s.AssignedUserId,
                    Claim = s.Claim,
                    CreatedDateUtc = s.CreatedDateUtc,
                    CreatedOnUtc = s.CreatedOnUtc,
                    Description = s.Description,
                    EpisodeType = s.EpisodeType,
                    Note = s.Note,
                    ResolvedDateUtc = s.ResolvedDateUtc,
                    UpdatedOnUtc = s.UpdatedOnUtc,
                    RxNumber = s.RxNumber,
                    ResolvedUserId = s.ResolvedUserId,
                    Role = s.Role,
                    Status = s.Status
                }).SingleOrDefault();

            var users = _usersRepository.GetAll()?.OrderBy(x => Guid.NewGuid()).ToArray();

            // Assert.
            Assert.IsNotNull(episode);
            Assert.IsNotNull(users);

            var entity = new Episode
            {
                AcquiredUserId = users[0],
                AssignedUserId = users[1],
                Claim = episode.Claim,
                CreatedDateUtc = episode.CreatedDateUtc,
                CreatedOnUtc = episode.CreatedOnUtc,
                Description = episode.Description,
                EpisodeType = episode.EpisodeType,
                Note = episode.Note,
                ResolvedDateUtc = episode.ResolvedDateUtc,
                UpdatedOnUtc = episode.UpdatedOnUtc,
                RxNumber = episode.RxNumber,
                ResolvedUserId = users[2],
                Role = episode.Role,
                Status = episode.Status
            };

            _episodeRepository.Save(entity);
            
        }*/
    }
}
