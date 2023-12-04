
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp2.Pages;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Frame.Navigate(new Page_students());

        }

        private void MStudents_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Page_students());
        }

        private void MTeachers_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate (new Page_Teacher());
        }

        private void MDirection_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Page_direction());
        }

        private void MLessons_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Page_classes());
        }

        private void MReports_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Page_report());
        }
    }
}
