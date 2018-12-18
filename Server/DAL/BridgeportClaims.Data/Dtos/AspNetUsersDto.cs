using SQLinq;

namespace BridgeportClaims.Data.Dtos
{
    [SQLinqTable("AspNetUsers")]
    public class AspNetUsersDto
    {
        [SQLinqColumn("Id")]
        public string Id { get; set; }
        [SQLinqColumn("FirstName")]
        public string FirstName { get; set; }
        [SQLinqColumn("LastName")]
        public string LastName { get; set; }
    }
}