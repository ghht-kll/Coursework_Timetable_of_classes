using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Timetable_of_classes
{
    public enum ObjectName { Classroom, Group, LessonNumber, Teacher, Subject, Date }

    public static class Connection
    {
        public static ObjectName objectName;
        public static DataGrid GroupGrid;
        public static string connectionString { get; set; }
        public static SqlConnection sqlConnection { get; set; }
        public static int currentId { get; set; }

        public static List<Group> SelectFromGroup()
        {
            List<Group> groups = new List<Group>();
            try
            {
                string sqlExpression = "SELECT * FROM [Group]";
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                SqlDataReader reader =  command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string name = (string)reader.GetValue(1);
                    string specialty = (string)reader.GetValue(2);
                    int number_of_students = (int)reader.GetValue(3);
                    int year = (int)reader.GetValue(4);

                    groups.Add(new Group(id, name, specialty, number_of_students, year));
                }
                reader.Close();
                objectName = ObjectName.Group;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return groups;
        }

        public static List<LessonNumber> SelectfromLessonNumber()
        {
            List<LessonNumber> lessonNumber = new List<LessonNumber>();
            try
            {
                string sqlExpression = "SELECT * FROM Lesson_Number";
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string start = (string)reader.GetValue(1).ToString();
                    string end = (string)reader.GetValue(2).ToString();

                    lessonNumber.Add(new LessonNumber(id, start, end));
                }
                reader.Close();
                objectName = ObjectName.LessonNumber;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return lessonNumber;
        }

        public static List<Subject> SelectfromSubject() 
        {
            List<Subject> subjects = new List<Subject>();
            try
            {
                string sqlExpression = "SELECT * FROM [Subject]";
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string subjectName = (string)reader.GetValue(1);
                    string classroomName = ConvertFromDBVal<string>(reader.GetValue(2).ToString());
                    subjects.Add(new Subject(id, subjectName, classroomName));
                }
                reader.Close();
                objectName = ObjectName.Subject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return subjects;
        }

        public static List<Teacher> SelectFromTeacher()
        {
            List<Teacher> teachers = new List<Teacher>();
            try
            {
                string sqlExpression = "SELECT Teacher.Id, " +
                                       "Teacher.first_name, " +
	                                   "Teacher.patronymic, " +
	                                   "Teacher.last_name, " +
	                                   "Teacher.specialty, " +
	                                   "[Subject].[name] AS[subject] " +
                                       "FROM[Subject] RIGHT JOIN Teacher ON Teacher.subject_id = [Subject].Id";
                
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string first_name = (string)reader.GetValue(1);
                    string patronymic = (string)reader.GetValue(2);
                    string last_name = (string)reader.GetValue(3);
                    string specialty = (string)reader.GetValue(4);
                    string subject = ConvertFromDBVal<string>(reader.GetValue(5));
                    teachers.Add(new Teacher(id, first_name, patronymic, last_name, specialty, subject));
                }
                reader.Close();
                objectName = ObjectName.Teacher;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return teachers;
        }

        public static List<Classroom> SelectFromClassroom()
        {
            List<Classroom> classrooms = new List<Classroom>();
            try
            {
                string sqlExpression = "SELECT " +
                                       "Classroom.Id, " + 
                                       "Classroom.[name] AS Classroom_name, " +
                                       "Classroom.capacity, " +
                                       "[Group].[name] AS Group_name, " +
                                       "Teacher.first_name AS Teacher_first_name, " +
                                       "Teacher.patronymic AS Teacher_patronymic, " +
                                       "Teacher.last_name AS Teacher_last_name, " +
                                       "[Subject].[name] AS Subject_name, " +
                                       "[Date].[time] AS Date_time, " +
                                       "Lesson_Number.[start], " +
                                       "Lesson_Number.[end] " +
                                       "FROM Classroom " +
                                       "JOIN[Group] ON[Group].Id = Classroom.group_id " +
                                       "JOIN Teacher ON Teacher.Id = Classroom.teacher_id JOIN[Subject] ON Teacher.subject_id = [Subject].Id " +
                                       "JOIN [Date] ON[Date].Id = Classroom.date_id JOIN Lesson_Number ON[Date].lesson_number_id = Lesson_Number.Id " +
                                       "ORDER BY[Date].[time]";
                
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string classroom_name = (string)reader.GetValue(1);
                    string classroom_capacity = (string)reader.GetValue(2).ToString();
                    string group_name = (string)reader.GetValue(3);
                    string teacher_first_name = (string)reader.GetValue(4);
                    string teacher_patronymic = (string)reader.GetValue(5);
                    string teacher_last_name = (string)reader.GetValue(6);
                    string subject_name = (string)reader.GetValue(7);
                    string date_time = (string)reader.GetValue(8).ToString();
                    string start_time = (string)reader.GetValue(9).ToString();
                    string end_time = (string)reader.GetValue(10).ToString();

                    classrooms.Add(new Classroom(id, classroom_name, classroom_capacity, group_name, teacher_first_name, teacher_patronymic, teacher_last_name,
                                                 subject_name, date_time, start_time, end_time));
                }
                reader.Close();                
                objectName = ObjectName.Classroom;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return classrooms;
        }

        public static List<DateT> SelectFromDate()
        {
            List<DateT> date = new List<DateT>();
            try
            {
                string sqlExpression = "SELECT [Date].Id, [Date].[time] AS date, Lesson_Number.[start], Lesson_Number.[end] " +
                                       "FROM Lesson_Number RIGHT JOIN[Date] ON[Date].lesson_number_id = Lesson_Number.Id";
                SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string datetime = (string)reader.GetValue(1).ToString();
                    string startTime = (string)reader.GetValue(2).ToString();
                    string endTime = (string)reader.GetValue(3).ToString();
                    date.Add(new DateT(id, datetime, startTime, endTime));
                }
                reader.Close();
                objectName = ObjectName.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return date;
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }
      
        public static List<A2Class> A2()
        {
            List<A2Class> ts = new List<A2Class>();
            try
            {
                
                string sqlExpression = $"SELECT	[Group].[name], " +
                                        "SUM( " +
                                        "CASE " +
                                        "WHEN Classroom.group_id = 2 " +
                                        "THEN    1 " +
                                        "ELSE    0 " +
                                        "END " +
                                        ") AS[group] " +
                                        "FROM " +
                                        "[Group], Classroom " +
                                        "WHERE   group_id = [Group].Id AND[Group].Id = 2 " +
                                        "GROUP BY[Group].[name]";
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = (string)reader.GetValue(0);
                    string group = (string)reader.GetValue(1).ToString();
                    ts.Add(new A2Class(name,group));
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ts;
        }

        public static List<B2Class> B2()
        {
            List<B2Class> ts = new List<B2Class>();
            try
            { 
                string sqlExpression = "SELECT * FROM All_Teacher_All_Subject " +
                                       "ORDER BY quantity";
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int Id = (int)reader.GetValue(0);
                    string name = (string)reader.GetValue(1).ToString();
                    int quantity = (int)reader.GetValue(2);
                    ts.Add(new B2Class(Id, name, quantity));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ts;
        }

        public static List<C2Class> C2()
        {
            List<C2Class> ts = new List<C2Class>();
            try
            {
                string sqlExpression = "SELECT Id, [name] AS 'Classroom name', capacity, " +
                                       "(SELECT[Group].[name] FROM[Group] " +
                                       "WHERE[Group].Id = group_id) AS 'Group name', " +
                                       "(SELECT Teacher.first_name + ' ' + Teacher.patronymic + ' ' + Teacher.last_name " +
                                       "FROM Teacher " +
                                       "WHERE Teacher.Id = teacher_id) AS 'Full name Teacher' " +
                                       "FROM(SELECT * FROM Classroom WHERE group_id != 0) AS needed_Classroom " +
                                       "WHERE capacity< (SELECT sum(capacity)/ count(capacity) FROM Classroom); ";

                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int Id = (int)reader.GetValue(0);
                    string classroomName = (string)reader.GetValue(1).ToString();
                    int capacity = (int)reader.GetValue(2);
                    string groupName = (string)reader.GetValue(3).ToString();
                    string fullNameTeacher = (string)reader.GetValue(4).ToString();
                    ts.Add(new C2Class(Id, classroomName, capacity, groupName, fullNameTeacher));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ts;
        }

        public static List<E2Class> E2()
        {
            List<E2Class> ts = new List<E2Class>();
            try
            {
                string sqlExpression = "SELECT Classroom.[name] " +
                                       "FROM Classroom " +
                                       "WHERE Classroom.group_id = ANY " +
                                       "(SELECT[Group].Id FROM[Group] WHERE[Group].[name] = 'C-4') ";

                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetValue(0).ToString();
                    ts.Add(new E2Class(name));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ts;
        }

        public static List<History> histories()
        {
            List<History> history = new List<History>();
            try
            {
                string sqlExpression = "SELECT * FROM History";

                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int Id = (int)reader.GetValue(0);
                    int groupId = (int)reader.GetValue(1);
                    string operation = (string)reader.GetValue(2);
                    string createAt = (string)reader.GetValue(3).ToString();
                    
                    history.Add(new History(Id, groupId, operation, createAt));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return history;
        }
    }
}
