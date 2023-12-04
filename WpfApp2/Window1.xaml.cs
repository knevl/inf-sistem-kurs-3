
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private bool isStudentsTabLoaded = false;
        private bool isListsTabLoaded = false;
        private bool isGroupsTabLoaded = false;

        private void MStudents_Click(object sender, RoutedEventArgs e)
        {
            if (!isStudentsTabLoaded)
            {
                LoadStudentsTab();
                isStudentsTabLoaded = true;
            }
            if (!isGroupsTabLoaded)
            {
                LoadGroupsTab();
                isGroupsTabLoaded = true;
            }
            if (!isListsTabLoaded)
            {
                LoadListsTab();
                isListsTabLoaded = true;
            }
        }

        private void LoadStudentsTab()
        {
            TabItem studentsTab = new TabItem();
            studentsTab.Header = "Ученики";
            studentsTab.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            studentsTab.Background = new SolidColorBrush(Color.FromRgb(255, 211, 211));

            Grid studentsGrid = new Grid();
            studentsGrid.Background = new SolidColorBrush(Colors.White);

            TextBox searchbox_students = new TextBox();
            searchbox_students.Name = "searchbox_students";
            searchbox_students.Margin = new Thickness(10, 10, 0, 0);
            searchbox_students.TextWrapping = TextWrapping.Wrap;
            searchbox_students.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            searchbox_students.SelectionBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            searchbox_students.MaxLength = 99;
            searchbox_students.MaxLines = 3;
            searchbox_students.Height = 20;
            searchbox_students.VerticalAlignment = VerticalAlignment.Top;
            searchbox_students.HorizontalAlignment = HorizontalAlignment.Left;
            searchbox_students.Width = 217;

            Button add_students = new Button();
            add_students.Name = "add_students";
            add_students.Content = "Добавить";
            add_students.Margin = new Thickness(0, 10, 190, 0);
            add_students.Background = new SolidColorBrush(Color.FromRgb(255, 220, 206));
            add_students.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 139, 120));
            add_students.Height = 20;
            add_students.VerticalAlignment = VerticalAlignment.Top;
            add_students.HorizontalAlignment = HorizontalAlignment.Right;
            add_students.Width = 80;
         

            Button edit_students = new Button();
            edit_students.Name = "edit_students";
            edit_students.Content = "Редактировать";
            edit_students.Margin = new Thickness(0, 10, 95, 0);
            edit_students.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            edit_students.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            edit_students.Height = 20;
            edit_students.VerticalAlignment = VerticalAlignment.Top;
            edit_students.HorizontalAlignment = HorizontalAlignment.Right;
            edit_students.Width = 90;
            

            Button del_students = new Button();
            del_students.Name = "del_students";
            del_students.Content = "Удалить";
            del_students.Margin = new Thickness(0, 10, 10, 0);
            del_students.Background = new SolidColorBrush(Color.FromRgb(255, 147, 147));
            del_students.BorderBrush = new SolidColorBrush(Color.FromRgb(197, 56, 56));
            del_students.Height = 20;
            del_students.VerticalAlignment = VerticalAlignment.Top;
            del_students.HorizontalAlignment = HorizontalAlignment.Right;
            del_students.Width = 80;
            // Добавьте обработчик события для кнопки "Удалить" при необходимости

            Button search_students = new Button();
            search_students.Name = "search_students";
            search_students.Content = "Найти";
            search_students.Margin = new Thickness(232, 10, 0, 0);
            search_students.Background = new SolidColorBrush(Color.FromRgb(255, 236, 237));
            search_students.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            search_students.Height = 20;
            search_students.VerticalAlignment = VerticalAlignment.Top;
            search_students.HorizontalAlignment = HorizontalAlignment.Left;
            search_students.Width = 63;
            // Добавьте обработчик события для кнопки "Найти" при необходимости


            DataGrid grid_students = new DataGrid();
            grid_students.Name = "grid_students";
            grid_students.Margin = new Thickness(10, 39, 10, 10);
            grid_students.AutoGenerateColumns = true;
            grid_students.ItemsSource = Enumerable.Range(1, 5).Select(i => new { Column1 = $"Item {i}", Column2 = $"Value {i}" }).ToList();
            // Добавьте свойства для DataGrid

            studentsGrid.Children.Add(searchbox_students);
            studentsGrid.Children.Add(add_students);
            studentsGrid.Children.Add(edit_students);
            studentsGrid.Children.Add(del_students);
            studentsGrid.Children.Add(search_students);
            studentsGrid.Children.Add(grid_students);

            studentsTab.Content = studentsGrid;
            mainTabControl.Items.Add(studentsTab);
        }


        private void LoadListsTab()
        {
            TabItem listsTab = new TabItem();
            listsTab.Header = "Списки групп";

            Grid listsGrid = new Grid();
            listsGrid.Background = Brushes.White;

            TextBox a1 = new TextBox();
            a1.Name = "a1";
            a1.Margin = new Thickness(10, 10, 0, 0);
            a1.TextWrapping = TextWrapping.Wrap;
            a1.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            a1.SelectionBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            a1.MaxLength = 99;
            a1.MaxLines = 3;
            a1.Height = 20;
            a1.VerticalAlignment = VerticalAlignment.Top;
            a1.HorizontalAlignment = HorizontalAlignment.Left;
            a1.Width = 217;

            Button a2 = new Button();
            a2.Name = "a2";
            a2.Content = "Добавить";
            a2.Margin = new Thickness(0, 10, 190, 0);
            a2.Background = new SolidColorBrush(Color.FromRgb(220, 243, 206));
            a2.BorderBrush = new SolidColorBrush(Color.FromRgb(139, 171, 120));
            a2.Height = 20;
            a2.VerticalAlignment = VerticalAlignment.Top;
            a2.HorizontalAlignment = HorizontalAlignment.Right;
            a2.Width = 80;
           

            Button a3 = new Button();
            a3.Name = "a3";
            a3.Content = "Редактировать";
            a3.Margin = new Thickness(0, 10, 95, 0);
            a3.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            a3.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            a3.Height = 20;
            a3.VerticalAlignment = VerticalAlignment.Top;
            a3.HorizontalAlignment = HorizontalAlignment.Right;
            a3.Width = 90;
            

            Button a4 = new Button();
            a4.Name = "a4";
            a4.Content = "Удалить";
            a4.Margin = new Thickness(0, 10, 10, 0);
            a4.BorderBrush = new SolidColorBrush(Color.FromRgb(197, 56, 56));
            a4.Background = new SolidColorBrush(Color.FromRgb(255, 147, 147));
            
            a4.Height = 20;
            a4.VerticalAlignment = VerticalAlignment.Top;
            a4.HorizontalAlignment = HorizontalAlignment.Right;
            a4.Width = 80;

            Button a5 = new Button();
            a5.Name = "a5";
            a5.Content = "Найти";
            a5.Margin = new Thickness(232, 10, 0, 0);
            a5.Background = new SolidColorBrush(Color.FromRgb(255, 236, 237));
            a5.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            a5.Height = 20;
            a5.VerticalAlignment = VerticalAlignment.Top;
            a5.HorizontalAlignment = HorizontalAlignment.Left;
            a5.Width = 63;

            DataGrid a6 = new DataGrid();
            a6.Name = "a6";
            a6.Margin = new Thickness(10, 39, 10, 10);
            a6.AutoGenerateColumns = true;
            a6.ItemsSource = Enumerable.Range(1, 5).Select(i => new { Column1 = $"Item {i}", Column2 = $"Value {i}" }).ToList();
            a6.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            a6.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));

            listsGrid.Children.Add(a1);
            listsGrid.Children.Add(a2);
            listsGrid.Children.Add(a3);
            listsGrid.Children.Add(a4);
            listsGrid.Children.Add(a5);
            listsGrid.Children.Add(a6);

            listsTab.Content = listsGrid;
            mainTabControl.Items.Add(listsTab);
        }

        private void LoadGroupsTab()
        {
            TabItem groupsTab = new TabItem();
            groupsTab.Header = "Группы";

            Grid groupsGrid = new Grid();
            groupsGrid.Background = Brushes.White;

            TextBox b1 = new TextBox();
            b1.Name = "b1";
            b1.Margin = new Thickness(10, 10, 0, 0);
            b1.TextWrapping = TextWrapping.Wrap;
            b1.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            b1.SelectionBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            b1.MaxLength = 99;
            b1.MaxLines = 3;
            b1.Height = 20;
            b1.VerticalAlignment = VerticalAlignment.Top;
            b1.HorizontalAlignment = HorizontalAlignment.Left;
            b1.Width = 217;

            Button b2 = new Button();
            b2.Name = "b2";
            b2.Content = "Добавить";
            b2.Margin = new Thickness(0, 10, 190, 0);
            b2.Background = new SolidColorBrush(Color.FromRgb(220, 243, 206));
            b2.BorderBrush = new SolidColorBrush(Color.FromRgb(139, 171, 120));
            b2.Height = 20;
            b2.VerticalAlignment = VerticalAlignment.Top;
            b2.HorizontalAlignment = HorizontalAlignment.Right;
            b2.Width = 80;
            

            Button b3 = new Button();
            b3.Name = "b3";
            b3.Content = "Редактировать";
            b3.Margin = new Thickness(0, 10, 95, 0);
            b3.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            b3.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            b3.Height = 20;
            b3.VerticalAlignment = VerticalAlignment.Top;
            b3.HorizontalAlignment = HorizontalAlignment.Right;
            b3.Width = 90;
            

            Button b4 = new Button();
            b4.Name = "b4";
            b4.Content = "Удалить";
            b4.Margin = new Thickness(0, 10, 10, 0);
            b4.BorderBrush = new SolidColorBrush(Color.FromRgb(197, 56, 56));
            b4.Background = new SolidColorBrush(Color.FromRgb(255, 147, 147));
           
            b4.Height = 20;
            b4.VerticalAlignment = VerticalAlignment.Top;
            b4.HorizontalAlignment = HorizontalAlignment.Right;
            b4.Width = 80;

            Button b5 = new Button();
            b5.Name = "b5";
            b5.Content = "Найти";
            b5.Margin = new Thickness(232, 10, 0, 0);
            b5.Background = new SolidColorBrush(Color.FromRgb(255, 236, 237));
            b5.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 176, 176));
            b5.Height = 20;
            b5.VerticalAlignment = VerticalAlignment.Top;
            b5.HorizontalAlignment = HorizontalAlignment.Left;
            b5.Width = 63;

            DataGrid b6 = new DataGrid();
            b6.Name = "b6";
            b6.Margin = new Thickness(10, 39, 10, 10);
            b6.AutoGenerateColumns = true;
            b6.ItemsSource = Enumerable.Range(1, 5).Select(i => new { Column1 = $"Item {i}", Column2 = $"Value {i}" }).ToList();
            b6.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            b6.Background = new SolidColorBrush(Color.FromRgb(247, 247, 247));

            groupsGrid.Children.Add(b1);
            groupsGrid.Children.Add(b2);
            groupsGrid.Children.Add(b3);
            groupsGrid.Children.Add(b4);
            groupsGrid.Children.Add(b5);
            groupsGrid.Children.Add(b6);

            groupsTab.Content = groupsGrid;
            mainTabControl.Items.Add(groupsTab);
        }
    }
}
