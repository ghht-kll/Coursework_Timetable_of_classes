using System;
using System.Data.SqlClient;
using System.Windows;


namespace Timetable_of_classes
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    //public enum ObjectName { Classroom, Group, LessonNumber, Teacher, Subject }

    public partial class MainWindow : Window
    {
        Authorization authorization;
        GroupChange groupChange;
        ChangingTheLessonNumber changingTheLessonNumber;
        SubjectChange subjectChange;
        TeacherChange teacherChange;
        ClassroomChange classroomChange;
        DateChange dateChange;
        private string sqlExpression;

        public MainWindow()
        {
            InitializeComponent();
            this.authorization = new Authorization(this);
            this.authorization.Show();
            Connection.GroupGrid = this.GroupGrid;
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.GroupGrid.ItemsSource = Connection.SelectFromGroup();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.GroupGrid.ItemsSource = Connection.SelectfromLessonNumber();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) // ?????????????????
        {
            this.GroupGrid.ItemsSource = Connection.SelectfromSubject();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.GroupGrid.ItemsSource = Connection.SelectFromTeacher();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.GroupGrid.ItemsSource = Connection.SelectFromClassroom();
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            this.GroupGrid.ItemsSource = Connection.SelectFromDate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Connection.sqlConnection.Close();
        }

        private void GroupGrid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Connection.objectName == ObjectName.Classroom)
            {
                if (GroupGrid.SelectedItem is Classroom)
                {
                    Classroom path = GroupGrid.SelectedItem as Classroom;
                    MessageBox.Show("id: " + path.id.ToString());
                    Connection.currentId = path.id;
                    Connection.objectName = ObjectName.Classroom;
                    return;
                }
            }
            if (Connection.objectName == ObjectName.Group)
            {
                if (GroupGrid.SelectedItem is Group)
                {
                    Group path = GroupGrid.SelectedItem as Group;
                    MessageBox.Show("id: " + path.Id.ToString());
                    Connection.currentId = path.Id;
                    Connection.objectName = ObjectName.Group;
                    return;
                }
            }
            if (Connection.objectName == ObjectName.LessonNumber)
            {
                if (GroupGrid.SelectedItem is LessonNumber)
                {
                    LessonNumber path = GroupGrid.SelectedItem as LessonNumber;
                    MessageBox.Show("id: " + path.id.ToString());
                    Connection.currentId = path.id;
                    Connection.objectName = ObjectName.LessonNumber;
                    return;
                }
            }
            if (Connection.objectName == ObjectName.Subject)
            {
                if (GroupGrid.SelectedItem is Subject)
                {
                    Subject path = GroupGrid.SelectedItem as Subject;
                    MessageBox.Show("id: " + path.id.ToString());
                    Connection.currentId = path.id;
                    Connection.objectName = ObjectName.Subject;
                    return;
                }
            }
            if (Connection.objectName == ObjectName.Teacher)
            {
                if (GroupGrid.SelectedItem is Teacher)
                {
                    Teacher path = GroupGrid.SelectedItem as Teacher;
                    MessageBox.Show("id: " + path.id.ToString());
                    Connection.currentId = path.id;
                    Connection.objectName = ObjectName.Teacher;
                    return;
                }
            }
            if (Connection.objectName == ObjectName.Date)
            {
                if (GroupGrid.SelectedItem is DateT)
                {
                    DateT path = GroupGrid.SelectedItem as DateT;
                    MessageBox.Show("id: " + path.Id.ToString());
                    Connection.currentId = path.Id;
                    Connection.objectName = ObjectName.Date;
                    return;
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (Connection.objectName == ObjectName.Group)
            {
                this.groupChange = new GroupChange();
                this.groupChange.Owner = this;
                this.groupChange.Show();
            }
            if (Connection.objectName == ObjectName.LessonNumber)
            {
                this.changingTheLessonNumber = new ChangingTheLessonNumber();
                this.changingTheLessonNumber.Owner = this;
                this.changingTheLessonNumber.Show();
            }
            if (Connection.objectName == ObjectName.Subject)
            {
                this.subjectChange = new SubjectChange();
                this.subjectChange.Owner = this;
                this.subjectChange.Show();
            }
            if (Connection.objectName == ObjectName.Teacher)
            {
                this.teacherChange = new TeacherChange();
                this.teacherChange.Owner = this;
                this.teacherChange.Show();
            }
            if (Connection.objectName == ObjectName.Classroom)
            {
                this.classroomChange = new ClassroomChange();
                this.classroomChange.Owner = this;
                this.classroomChange.Show();
            }
            if (Connection.objectName == ObjectName.Date)
            {
                this.dateChange = new DateChange();
                this.dateChange.Owner = this;
                this.dateChange.Show();
            }
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Connection.objectName == ObjectName.Classroom)
                {
                    if (GroupGrid.SelectedItem is Classroom)
                    {
                        Classroom path = GroupGrid.SelectedItem as Classroom;
                        Connection.currentId = path.id;
                        Connection.objectName = ObjectName.Classroom;

                        sqlExpression = $"EXEC DelClassroom {path.id}";
                        SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                        await command.ExecuteNonQueryAsync();

                        Connection.GroupGrid.ItemsSource = Connection.SelectFromClassroom();
                        return;
                    }
                }
                if (Connection.objectName == ObjectName.Date)
                {
                    if (GroupGrid.SelectedItem is DateT)
                    {
                        DateT path = GroupGrid.SelectedItem as DateT;
                        Connection.currentId = path.Id;
                        Connection.objectName = ObjectName.Date;

                        sqlExpression = $"EXEC DelDate {path.Id}";
                        SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                        await command.ExecuteNonQueryAsync();

                        Connection.GroupGrid.ItemsSource = Connection.SelectFromDate();
                        return;
                    }
                }
                if (Connection.objectName == ObjectName.Group)
                {
                    if (GroupGrid.SelectedItem is Group)
                    {
                        Group path = GroupGrid.SelectedItem as Group;
                        Connection.currentId = path.Id;
                        Connection.objectName = ObjectName.Group;

                        sqlExpression = $"EXEC DelGroup {path.Id}";
                        SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                        await command.ExecuteNonQueryAsync();

                        Connection.GroupGrid.ItemsSource = Connection.SelectFromGroup();
                        return;
                    }
                }
                if (Connection.objectName == ObjectName.LessonNumber)
                {
                    if (GroupGrid.SelectedItem is LessonNumber)
                    {
                        LessonNumber path = GroupGrid.SelectedItem as LessonNumber;
                        Connection.currentId = path.id;
                        Connection.objectName = ObjectName.LessonNumber;

                        sqlExpression = $"EXEC DelLesson_Number {path.id}";
                        SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                        await command.ExecuteNonQueryAsync();

                        Connection.GroupGrid.ItemsSource = Connection.SelectfromLessonNumber();
                        return;
                    }
                }
                if (Connection.objectName == ObjectName.Subject)
                {
                    if (GroupGrid.SelectedItem is Subject)
                    {
                        Subject path = GroupGrid.SelectedItem as Subject;
                        Connection.currentId = path.id;
                        Connection.objectName = ObjectName.Subject;

                        sqlExpression = $"EXEC DelSubjetc {path.id}";
                        SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                        await command.ExecuteNonQueryAsync();

                        Connection.GroupGrid.ItemsSource = Connection.SelectfromSubject();
                        return;
                    }
                }
                if (Connection.objectName == ObjectName.Teacher)
                {
                    if (GroupGrid.SelectedItem is Teacher)
                    {
                        Teacher path = GroupGrid.SelectedItem as Teacher;
                        Connection.currentId = path.id; 
                        Connection.objectName = ObjectName.Teacher;

                        sqlExpression = $"EXEC DelTeacher {path.id}";
                        SqlCommand command = new SqlCommand(sqlExpression, Connection.sqlConnection);
                        await command.ExecuteNonQueryAsync();

                        Connection.GroupGrid.ItemsSource = Connection.SelectFromTeacher();
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Connection.GroupGrid.ItemsSource = Connection.A2();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Connection.GroupGrid.ItemsSource = Connection.B2();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            Connection.GroupGrid.ItemsSource = Connection.C2();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            Connection.GroupGrid.ItemsSource = Connection.histories();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            Connection.GroupGrid.ItemsSource = Connection.E2();
        }
    }
}
