
namespace Timetable_of_classes
{
    public class LessonNumber
    {
        public LessonNumber(int id, string start, string end)
        {
            this.id = id;
            this.start = start;
            this.end = end;
        }
        public int id;
        public string start { get; set; }
        public string end { get; set; }
    }
}
