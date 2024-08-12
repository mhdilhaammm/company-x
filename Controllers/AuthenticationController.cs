using Company_X.Data;
using Company_X.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using System.Security;

namespace Company_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly APIContext db;

        public AuthenticationController(APIContext context)
        {
            db = context;
        }

        //POST REGISTRATION
        [HttpPost("/Registration")]
        public JsonResult Regis(Employe model)
        {
            if (model.EmployeID == 0)
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                db.Employes.Add(model);
            }
            else
            {
                var dataEmploye = db.Employes.Find(model.EmployeID);
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                if (dataEmploye == null)
                    return new JsonResult(NotFound());

                dataEmploye = model;
            }

            db.SaveChanges();
            return new JsonResult(Ok(model));
        }

        //LOGIN
        [HttpPost("/Login")]
        public JsonResult Login([FromBody] EmployeLoginDTO model)
        {
            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return new JsonResult(new {status = false});
            }

            //check username has been registerd or not
            var emp = db.Employes.SingleOrDefault(e => e.Username == model.Username);
            if(emp == null)
            {
                return new JsonResult(new { status = false });
            }

            //check password has been registerd or no, whether the password has been used hashing function or not
            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, emp.Password);
            if (!validPassword) 
            {
                return new JsonResult(new { status = false });
            }

            //Create Session Level
            string sessionId = Guid.NewGuid().ToString();
            var session = new UserSession
            {
                SessionID = sessionId,
                EmployeID = emp.EmployeID,
                Level = emp.Level,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            db.UserSessions.Add(session);
            db.SaveChanges();

            return new JsonResult(Ok(new
            {
                success = true,
                message = "Login Successfully",
                sessionId = session.SessionID,
                level = emp.Level,
            }));   
        }

        //LOGOUT
        [HttpPost("/LOGOUT")]
        public JsonResult LogOut([FromBody] string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return new JsonResult(new { success = false, message = "invalid session id" });
            }

            var session = db.UserSessions.SingleOrDefault(s => s.SessionID == sessionId);
            if (session == null)
            {
                return new JsonResult(new { success = false, message = "Session not found" });
            }

            db.UserSessions.Remove(session);
            db.SaveChanges();

            return new JsonResult(Ok(new {success = true, message = "LOGGOUT SUCCESSFULLY"}));
        }
    }
}
