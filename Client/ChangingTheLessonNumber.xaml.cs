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
    /// Логика взаимодействия для ChangingTheLessonNumber.xaml
    /// </summary>
    public partial class ChangingTheLessonNumber : Window
    {
        int startHour;
        int startMinute;
        int endHour;
        int endMinute;
        private string sqlExpression;

        public ChangingTheLessonNumber()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("UpdatelessonNumber");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("AddlessonNumber");
        }

        private async void UpdateOrAdd(string EXEC)
        {
            try
            {
                if (!int.TryParse(this.startHourTextBox.Text, out startHour))
                {
                    MessageBox.Show("string is empty");
                    return;
                }
                else
                {
                    if (startHour >= 24 || startHour < 0)
                    {
                        MessageBox.Show("incorrect value entered");
                        return;
                    }
                }
                if (!int.TryParse(this.startMinuteTextBox.Text, out startMinute))
                {
                    MessageBox.Show("string is empty");
                    return;
                }
                else
                {
                    if (startHour > 60 || startHour < 0)
                    {
                        MessageBox.Show("incorrect value entered");
                        return;
                    }
                }
                if (!int.TryParse(this.endHourTextBox.Text, out endHour))
                {
                    MessageBox.Show("string is empty");
                    return;
                }
                else
                {
                    if (endHour >= 24 || endHour < 0)
                    {
                        MessageBox.Show("incorrect value entered");
                        return;
                    }
                }
                if (!int.TryParse(this.endMinuteTextBox.Text, out endMinute))
                {
                    MessageBox.Show("string is empty");
                    return;
                }
                else
                {
                    if (endHour > 60 || endHour < 0)
                    {
                        MessageBox.Show("incorrect value entered");
                        return;
                    }
                }
                if (EXEC == "UpdatelessonNumber")
                {
                    sqlExpression = $"{EXEC} {Connection.currentId},'{this.startHour}:{this.startMinute}' , '{this.endHour}:{this.endMinute}'";
                }
                else if (EXEC == "AddlessonNumber")
                {
                    sqlExpression = $"{EXEC} '{this.startHour}:{this.startMinute}' , '{this.endHour}:{this.endMinute}'";
                }
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                await command.ExecuteNonQueryAsync();
                Connection.GroupGrid.ItemsSource = Connection.SelectfromLessonNumber();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
