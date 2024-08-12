using System.ComponentModel.DataAnnotations;

namespace Company_X.Models
{
    public class UserSession
    {
        [Key]
        public string SessionID { get; set; }
        public int EmployeID { get; set; }
        public string Level { get;set; }
        public DateTime Expiration { get; set; }

    }
}
