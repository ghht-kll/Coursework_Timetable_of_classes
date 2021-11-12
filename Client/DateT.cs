using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable_of_classes
{
    public class DateT
    {
        public DateT(int Id, string date, string startTime, string endTime)
        {
            this.Id = Id;
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
        }
        public int Id { get; set; }
        public string date { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
    }
}
