using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class UserInformationDTO
    {
        public int UserId { get; set; }
        public int UIID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PatronicName { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string BirthDate { get; set; }
    }
}
