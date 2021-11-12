using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Timetable_of_classes
{
    /// <summary>
    /// Логика взаимодействия для DateChange.xaml
    /// </summary>
    public partial class DateChange : Window
    {
        Dictionary<int, string> keyValueLessonNumber;
        string dateTime = null;
        string currentLessonNumber;
        private string sqlExpression;

        public DateChange()
        {
            InitializeComponent();

            this.keyValueLessonNumber = new Dictionary<int, string>();
            string sqlExpression = "SELECT * FROM Lesson_Number";
            SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string start = (string)reader.GetValue(1).ToString();
                string end = (string)reader.GetValue(2).ToString();
                this.keyValueLessonNumber.Add(id, start + " : " + end);
            }
            reader.Close();
            this.comboBoxLessonId.ItemsSource = this.keyValueLessonNumber.Values;
        }

        private void datePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = (DateTime)calendar.SelectedDate;
            this.dateTime = selectedDate.ToString("yyyy-MM-dd");
            MessageBox.Show(dateTime);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("EXEC UpdateDate");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("EXEC AddDate");
        }
        private async void UpdateOrAdd(string EXEC)
        {
            try
            {
                int subjectId = keyValueLessonNumber.FirstOrDefault(x => x.Value == this.currentLessonNumber).Key;
                if (EXEC == "EXEC UpdateDate")
                {               
                    sqlExpression = $"{EXEC} {Connection.currentId}, '{this.dateTime}', {subjectId}";  
                }
                else if(EXEC == "EXEC AddDate")
                {
                    sqlExpression = $"{EXEC} '{this.dateTime}', {subjectId}";     
                }
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                await command.ExecuteNonQueryAsync();
                Connection.GroupGrid.ItemsSource = Connection.SelectFromDate();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void comboBoxLessonId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.currentLessonNumber = this.comboBoxLessonId.SelectedItem.ToString();
        }
    }
}
