using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for BucketWindow.xaml
    /// </summary>
    public partial class BucketWindow : Window
    {
        AwsS3Operations operations = new AwsS3Operations();
        
        public BucketWindow()
        {
            InitializeComponent();
            operations.LoadCollectionDataToGrid(dgBuckets);
        }

        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string bucketName = txtbxBucketName.Text;

                if (string.IsNullOrEmpty(bucketName))
                {
                    throw new Exception();
                }
                else
                {
                    operations.CreateBucket(bucketName, dgBuckets);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please check the bucket name and try again.");
            }
        }

        private void btnDeleteBucket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string bucketToDelete = txtbxBucketName.Text != null ? txtbxBucketName.Text : throw new Exception();
                operations.DeleteBucket(bucketToDelete, dgBuckets);
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a non-null bucket for deletion.");
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUrl_Click(object sender, RoutedEventArgs e)
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
