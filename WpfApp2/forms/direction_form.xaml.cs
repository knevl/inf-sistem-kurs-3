using System.Windows;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для direction_form.xaml
/// </summary>
public partial class direction_form : Window
{
    public direction_form()
    {
        InitializeComponent();
    }

    private void Button_Click_Save_dir(object sender, RoutedEventArgs e)
    {
    }

    private void Button_Click_cancel_dir(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
            "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) Close();
    }
}