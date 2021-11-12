using System;
using System.Data.SqlClient;
using System.Windows;

namespace Timetable_of_classes
{
    /// <summary>
    /// Логика взаимодействия для GroupChange.xaml
    /// </summary>
    public partial class GroupChange : Window
    {
        string nameGroup;
        string speciality;
        int numberOfStudent;
        int semester;
        private string sqlExpression;

        public GroupChange()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("EXEC UpdateGroup");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateOrAdd("EXEC AddGroup");
        }
        private async void UpdateOrAdd(string EXEC)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.nameGroupTextBox.Text))
                {
                    this.nameGroup = this.nameGroupTextBox.Text;
                }
                else
                {
                    MessageBox.Show("name group is empty");
                    return;
                }
                if (!string.IsNullOrEmpty(this.specialityTextBox.Text))
                {
                    this.speciality = this.specialityTextBox.Text;
                }
                else
                {
                    MessageBox.Show("speciality is empty");
                    return;
                }
                if (!int.TryParse(this.numberOfStudentTextBox.Text, out numberOfStudent))
                {
                    MessageBox.Show("number Of Student is empty");
                    return;
                }
                if (!int.TryParse(this.semesterTextBox.Text, out semester))
                {
                    MessageBox.Show("semester is empty");
                    return;
                }
                if (EXEC == "EXEC UpdateGroup")
                {
                    sqlExpression = $"{EXEC} {Connection.currentId},'{this.nameGroup}', '{this.speciality}',{this.numberOfStudent},{this.semester}";
                }
                else if (EXEC == "EXEC AddGroup")
                {
                    sqlExpression = $"{EXEC} '{this.nameGroup}', '{this.speciality}',{this.numberOfStudent},{this.semester}";
                }
                SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                await command.ExecuteNonQueryAsync();
                Connection.GroupGrid.ItemsSource = Connection.SelectFromGroup();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
