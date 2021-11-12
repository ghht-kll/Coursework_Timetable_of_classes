using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable_of_classes
{
    public class Classroom
    {
        public Classroom(int id, string classroom_name, string classroom_capacity, string group_name, string teacher_first_name, string teacher_patronymic,
                        string teacher_last_name, string subject_name, string date_time, string start_time, string end_time)
        {
            this.id = id;
            this.classroom_name = classroom_name;
            this.group_name = group_name;
            this.classroom_capacity = classroom_capacity;
            this.teacher_first_name = teacher_first_name;
            this.teacher_patronymic = teacher_patronymic;
            this.teacher_last_name = teacher_last_name;
            this.subject_name = subject_name;
            this.date_time = date_time;
            this.start_time = start_time;
            this.end_time = end_time;
        }
        public int id { get; set; }
        public string classroom_name { get; set; }
        public string classroom_capacity { get; set; }
        public string group_name { get; set; }
        public string teacher_first_name { get; set; }
        public string teacher_patronymic { get; set; }
        public string teacher_last_name { get; set; }
        public string subject_name { get; set; }
        public string date_time { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }
}
