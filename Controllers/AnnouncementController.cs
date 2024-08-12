using Company_X.Data;
using Company_X.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company_X.Controllers
{
    [Authorize(Policy = "SuperAdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        public readonly APIContext db;
        public AnnouncementController(APIContext context)
        {
            db = context;
        }

        [HttpPost("/UPLOAD ANNOUNCEMENT")]
        public JsonResult UploadAnnouncement(Announcement announc)
        {
            db.Announcements.Add(announc);
            db.SaveChanges();

            return new JsonResult(Ok(new {success = true, Message = $"{announc}, success upload"}));
        }

        [HttpDelete("/DELETE ANNOUNCEMENT")]
        public JsonResult DeleteAnnouncement(int id)
        {
            var result = db.Announcements.Find(id);
            if (result == null)
                return new JsonResult(NotFound(new {success = false}));

            db.Announcements.Remove(result);
            db.SaveChanges();

            return new JsonResult(NoContent());
        }
    }
}
