using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        // Create an instance of the AmazonS3Client with AWS credentials and region
        private AmazonS3Client s3Client = new AmazonS3Client(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"], RegionEndpoint.CACentral1);

        public ObjectWindow()
        {
            InitializeComponent();
            LoadCollectionData();
        }

        private async void LoadCollectionData()
        {
            ListBucketsResponse bucketList = await s3Client.ListBucketsAsync();

            foreach (S3Bucket s3Bucket in bucketList.Buckets)
            {
                cbBucketNames.(new Bucket
                {
                    BucketName = s3Bucket.BucketName,
                    CreationDate = s3Bucket.CreationDate
                });
            }

            dgBuckets.ItemsSource = buckets;
        }
    }
}
