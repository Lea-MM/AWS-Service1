using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
using System.Windows.Shapes;

namespace lab1_api_aws
{
    /// <summary>
    /// Interaction logic for ObjectWindow.xaml
    /// </summary>
    public partial class ObjectWindow : Window
    {
        AwsS3Operations operations = new AwsS3Operations();

        public ObjectWindow()
        {
            InitializeComponent();
            operations.LoadBucketDataToComboBox(cbBucketNames);
        }

        private void cbBucketNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboBoxValue = cbBucketNames.SelectedItem.ToString();

            if (selectedComboBoxValue != null)
            {
                operations.LoadObjectData(selectedComboBoxValue, dgObjects);
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                txtbxObjectName.Text = filename;
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var bucketName = cbBucketNames.SelectedItem.ToString();
            var objectName = txtbxObjectName.Text;

            try
            {
                if (bucketName != null && objectName != null)
                {
                    operations.UploadObjectData(bucketName, dgObjects, objectName, S3CannedACL.BucketOwnerFullControl);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bucket and/or a file to upload and try again.");
            }
            finally
            {
                txtbxObjectName.Text = "";
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
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
