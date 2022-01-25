using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataLayer
{
    public class Check
    {

        public int Id { get; set; }
        public int UserID { get; set; }
        public string Date { get; set; }
        public decimal Summa { get; set; }
        public string? FileLink { get; set; }

    }
}
