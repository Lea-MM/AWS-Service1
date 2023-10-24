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
        // Create an instance of the AmazonS3Client with AWS credentials and region
        private AmazonS3Client s3Client = new AmazonS3Client(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"], RegionEndpoint.CACentral1);

        public BucketWindow()
        {
            InitializeComponent();
            LoadCollectionData();
        }

        private async void LoadCollectionData()
        {
            List<Bucket> buckets = new List<Bucket>();

            ListBucketsResponse bucketList = await s3Client.ListBucketsAsync();

            foreach (S3Bucket s3Bucket in bucketList.Buckets)
            {
                buckets.Add(new Bucket
                {
                    BucketName = s3Bucket.BucketName,
                    CreationDate = s3Bucket.CreationDate
                });
            }

            dgBuckets.ItemsSource = buckets;
        }

        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string bucketName = txtbxBucketName.Text;
                s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true,
                }).Wait();

                LoadCollectionData();
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show("Error creating bucket: " + ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Please specify a bucket name and try again.");
            }
        }

        private void btnDeleteBucket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string bucketToDelete = txtbxBucketName.Text != null? txtbxBucketName.Text : throw new Exception();
                s3Client.DeleteBucketAsync(bucketToDelete).Wait();
                LoadCollectionData();
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show("Error deleting bucket: " + ex.Message);
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
                Process.Start("https://github.com/Lea-MM/AWS-Service1");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the URL: {ex.Message}");
            }
        }
    }
}
