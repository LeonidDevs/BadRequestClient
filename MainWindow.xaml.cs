using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VittaClient.Tools;
using VittaClient.ViewModels;
using VittaClient.Views;

namespace VittaClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangePage(new AuthView());

        }
        public void ChangePage(Page page)
        {
            ContentPresenter.Content = page;
            ContentPresenter.NavigationService.RemoveBackEntry();
        }
    }
}