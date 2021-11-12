using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable_of_classes
{
    public class Group
    {
        public Group(int Id, string name, string specialty, int number_of_students, int year)
        {
            this.Id = Id;
            this.name = name;
            this.specialty = specialty;
            this.number_of_students = number_of_students;
            this.year = year;
        }
        public int Id { get; set; }
        public string name { get; set; }
        public string specialty { get; set; }
        public int number_of_students { get; set; }
        public int year { get; set; }
    }
}
