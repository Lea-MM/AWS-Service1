using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab1_api_aws
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBucket_Click(object sender, RoutedEventArgs e)
        {
            BucketWindow bucketWindow = new BucketWindow();
            this.Visibility = Visibility.Visible;
            bucketWindow.Show();
        }

        private void btnObject_Click(object sender, RoutedEventArgs e)
        {
            ObjectWindow objectWindow = new ObjectWindow();
            this.Visibility = Visibility.Visible;
            objectWindow.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("cmd", $"/c start {"https://github.com/Lea-MM/AWS-Service1"}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the URL: {ex.Message}");
            }
        }
    }
}
