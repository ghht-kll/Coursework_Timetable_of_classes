using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Timetable_of_classes
{
    /// <summary>
    /// Логика взаимодействия для ClassroomChange.xaml
    /// </summary>
    public partial class ClassroomChange : Window
    {
        Dictionary<int, string> keyValueGroup;
        Dictionary<int, string> keyValueteacher;
        Dictionary<int, string> keyValuelessonNumber;
        Dictionary<int, string> keyValueDate;
        Dictionary<int, string> keyValueSubject;
        string groupValue, teacherValue, lessonNumberValue, datevalue, subjectValue;
        int groupKey, teacherKey, lessonNumberKey, dateKey, subjectKey;
        private string sqlExpression;


        public ClassroomChange()
        {
            InitializeComponent();
            this.keyValueGroup = new Dictionary<int, string>();
            string sqlExpression = "SELECT * FROM [Group]";
            SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string name = (string)reader.GetValue(1);

                this.keyValueGroup.Add(id, name);
            }
            reader.Close();

            this.comboBoxGroup.ItemsSource = this.keyValueGroup.Values;

            this.keyValueteacher = new Dictionary<int, string>();
            sqlExpression = "SELECT * FROM [Teacher]";
            command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string first_name = (string)reader.GetValue(1);
                string patronymic = (string)reader.GetValue(2);
                string Last_name = (string)reader.GetValue(3);

                this.keyValueteacher.Add(id, first_name + " " + patronymic + " " + Last_name);
            }
            reader.Close();
            this.comboBoxTeacher.ItemsSource = this.keyValueteacher.Values;
            this.keyValueDate = new Dictionary<int, string>();
            sqlExpression = "SELECT * FROM Date";
            command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string time = (string)reader.GetValue(1).ToString(); 
                this.keyValueDate.Add(id, time);
            }
            reader.Close();
            this.comboBoxDate.ItemsSource = this.keyValueDate.Values;
        }

        private void comboBoxDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.datevalue = this.comboBoxDate.SelectedItem.ToString();

            this.dateKey = keyValueDate.FirstOrDefault(x => x.Value == this.datevalue).Key;

            this.keyValuelessonNumber = new Dictionary<int, string>();
            sqlExpression = "select Lesson_Number.Id, Lesson_Number.[start], Lesson_Number.[end] " +
                            "from[Date], Lesson_Number " +
                            $"where[Date].[time] = '{this.datevalue}' and Lesson_Number.Id =  [Date].lesson_number_id ";
            SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string start = (string)reader.GetValue(1).ToString();
                string end = (string)reader.GetValue(2).ToString();

                this.keyValuelessonNumber.Add(id, start + " : " + end);
            }
            reader.Close();
            this.comboBoxLessonNumber.ItemsSource = this.keyValuelessonNumber.Values;
            
        }

        private void comboBoxSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void comboBoxGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.groupValue = this.comboBoxGroup.SelectedItem.ToString();
        }

        private void comboBoxTeacher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.teacherValue = this.comboBoxTeacher.SelectedItem.ToString();
        }

        private void comboBoxLessonNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.lessonNumberValue = this.comboBoxLessonNumber.SelectedItem.ToString();
        }                                                                           

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("EXEC UpdateClassroom");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("EXEC AddClassroom");
        }

        private async void UpdateOrAdd(string EXEC)
        {
            this.groupKey = keyValueGroup.FirstOrDefault(x => x.Value == this.groupValue).Key;
            this.teacherKey = keyValueteacher.FirstOrDefault(x => x.Value == this.teacherValue).Key;
            this.lessonNumberKey = keyValuelessonNumber.FirstOrDefault(x => x.Value == this.lessonNumberValue).Key;

            sqlExpression = "SELECT [Date].Id FROM [Date] " +
                          $"WHERE [Date].[time] = '{this.datevalue}' AND [Date].lesson_number_id = {this.lessonNumberKey}";

            SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                this.dateKey = (int)reader.GetValue(0);
            }
            reader.Close();

            if(EXEC == "EXEC UpdateClassroom")
            {
                sqlExpression = $"{EXEC} {Connection.currentId}, '{this.classroomNameTextBox.Text}', {this.classroomCapacityTextBox.Text}, {this.groupKey}, {this.teacherKey}, {this.dateKey} ";
            }
            else if(EXEC == "EXEC AddClassroom")
            {
                sqlExpression = $"{EXEC} '{this.classroomNameTextBox.Text}', {this.classroomCapacityTextBox.Text}, {this.groupKey}, {this.teacherKey}, {this.dateKey} ";
            }
            command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            await command.ExecuteNonQueryAsync();
            Connection.GroupGrid.ItemsSource = Connection.SelectFromClassroom();
            this.Close();
        }
    }
}
