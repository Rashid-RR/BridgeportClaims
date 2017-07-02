using System;
using System.Web.Http;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptionnotes")]
    public class PrescriptionNotesController : ApiController
    {
        [HttpPut]
        [Route("savenote")]
        public void AddOrUpdatePrescriptionNote(int prescriptionId, string noteText, DateTime createOn,
            DateTime UpdatedOn)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}