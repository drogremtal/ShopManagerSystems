using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataLayer
{
    public class UserInformation
    {
        public int Id { get; set; }

        public int UserID { get; set; }
        public string Email{ get; set; }
        public string PhoneNum{ get; set; }
        public DateTime  BirthDate{ get; set; }
    }
}
