//using System.Collections.Generic;
//using System.Web.Http;

//namespace BridgeportClaims.Web.Controllers
//{
//    [AllowAnonymous]
//    public class ValuesController : ApiController
//    {
//        static List<string> data = initList();

//        private static List<string> initList()
//        {
//            var ret = new List<string>();
//            ret.Add("value1");
//            ret.Add("value2");

//            return ret;
//        }

//        // GET api/values
//        public IEnumerable<string> Get()
//        {
//            return data;
//        }

//        // GET api/values/5
//        public string Get(int id)
//        {
//            return data[id];
//        }

//        // POST api/values
//        public void Post([FromBody]string value)
//        {
//            data.Add(value);
//        }

//        // PUT api/values/5
//        public void Put(int id, [FromBody]string value)
//        {
//            data[id] = value;
//        }

//        // DELETE api/values/5
//        public void Delete(int id)
//        {
//            data.RemoveAt(id);
//        }
//    }
//}
