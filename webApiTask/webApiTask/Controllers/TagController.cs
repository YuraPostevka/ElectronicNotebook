using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webApiTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/tags")]
    public class TagController : ApiController
    {

        private ITagManager tagManager;
        public TagController(ITagManager tagManager)
        {
            this.tagManager = tagManager;
        }

        // GET: api/Tag
        public IHttpActionResult Get()
        {
            var tags = tagManager.GetAll();
            return Ok(tags);
        }

        // GET: api/Tag
        [Route("getTop10")]
        public IHttpActionResult GetTop10()
        {
            var tags = tagManager.GetTop10();
            return Ok(tags);
        }

        // GET: api/Tag/5
        public IHttpActionResult Get(string tagName)
        {
            return Ok();
        }

        // POST: api/Tag
        [Route("addTag/{tag}/{listId}")]
        public IHttpActionResult Post(string tag, int listId)
        {
            tagManager.AttachToList(tag, listId);
            return Ok();
        }

        // PUT: api/Tag/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }


        [Route("removeTag/{tag}/{listId}")]
        // DELETE: api/Tag/5
        public IHttpActionResult Delete(string tag, int listId)
        {
            tagManager.Delete(tag, listId);
            return Ok();
        }
    }
}
