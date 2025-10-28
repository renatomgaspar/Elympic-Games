using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elympic_Games.Mobile.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GameTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
