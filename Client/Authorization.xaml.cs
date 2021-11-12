using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace Timetable_of_classes
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        MainWindow window;
        public Authorization(MainWindow mainWindow)
        {
            InitializeComponent();
            window = mainWindow;
            Connection.sqlConnection = new SqlConnection(Connection.connectionString);
        }

        private void authorizationButoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connection.connectionString = $"Data Source=DESKTOP-CTL9SPM;Initial Catalog=Timetable_of_classes;Integrated Security=False; User ID={textBoxLogin.Text};Password={textBoxPassword.Password}";
                Connection.sqlConnection = new SqlConnection(Connection.connectionString);

                Connection.sqlConnection.Open();
                if (Connection.sqlConnection.State == ConnectionState.Open) 
                {
                    this.Close();
                    window.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
