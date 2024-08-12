using Microsoft.AspNetCore.Mvc;
using Company_X.Models;
using Company_X.Data;
using Microsoft.AspNetCore.Authorization;

namespace Company_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly APIContext _context;

        public CompanyController(APIContext context)
        {
            _context = context;
        }

        //create and edit data
        [HttpPost]
        public JsonResult CreateEdit(Employe model)
        {
            if (model.EmployeID == 0)
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                _context.Employes.Add(model);
            }
            else 
            {
                var dataEmploye = _context.Employes.FirstOrDefault(e => e.EmployeID == model.EmployeID);
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                if (dataEmploye == null) 
                    return new JsonResult(NotFound());

                dataEmploye = model;
            }

            _context.SaveChanges();
            return new JsonResult(Ok(model));
        }
        
        //Search id data user
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Employes.Find(id);
            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Employes.Find(id);

            if(result == null) return new JsonResult(NotFound());

            _context.Employes.Remove(result);
            _context.SaveChanges();

            return new JsonResult(NoContent());
        }

        [HttpGet("/Getall")]
        public JsonResult ListData()
        {
            var allData = _context.Employes.ToList();
            return new JsonResult(Ok(allData));
        }
    }
}
