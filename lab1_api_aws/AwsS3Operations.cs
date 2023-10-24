using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Configuration;
using Amazon;
using System.Windows;
using System.Xml.Linq;

namespace lab1_api_aws
{
    internal class AwsS3Operations
    {
        // Create an instance of the AmazonS3Client with AWS credentials and region
        private AmazonS3Client s3Client = new AmazonS3Client(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"], RegionEndpoint.CACentral1);

        // loading buckets to the datagrid
        internal async void LoadCollectionDataToGrid(DataGrid dgName)
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

            dgName.ItemsSource = buckets;
        }

        // creating a new bucket
        internal async void CreateBucket(string bucketName, DataGrid dgName)
        {
            try
            {
                await s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true,
                });

                LoadCollectionDataToGrid(dgName);
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show("Error creating bucket: " + ex.Message);
            }
        }

        // deleting the bucket and all its objects (if any)
        internal async void DeleteBucket(string bucketName, DataGrid dgName)
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                };

                ListObjectsV2Response response;
                do
                {
                    response = await s3Client.ListObjectsV2Async(request);
                    var deleteTasks = response.S3Objects.Select(obj => s3Client.DeleteObjectAsync(bucketName, obj.Key));
                    await Task.WhenAll(deleteTasks);

                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                s3Client.DeleteBucketAsync(bucketName).Wait();
                LoadCollectionDataToGrid(dgName);

                MessageBox.Show("Successfully deleted " + bucketName);
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show("Error deleting bucket: " + ex.Message);
            }
        }

        // loading buckets to the combo box
        internal async void LoadBucketDataToComboBox(ComboBox dgName)
        {
            List<string> buckets = new List<string>();

            ListBucketsResponse bucketList = await s3Client.ListBucketsAsync();

            foreach (S3Bucket s3Bucket in bucketList.Buckets)
            {
                buckets.Add(s3Bucket.BucketName);
            }

            dgName.ItemsSource = buckets;
        }

        // loading objects to the datagrid
        internal async void LoadObjectData(string bucketName, DataGrid dgName)
        {
            List<BucketObject> objects = new List<BucketObject>();
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName
                };

                ListObjectsV2Response response;
                do
                {
                    response = await s3Client.ListObjectsV2Async(request);

                    response.S3Objects
                        .ForEach(obj => objects.Add(new BucketObject
                        {
                            ObjectName = obj.Key,
                            Size = obj.Size
                        }));

                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                dgName.ItemsSource = objects;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
            }
        }

        // uploading an object to specific bucket
        internal async void UploadObjectData(string bucketName, DataGrid dgName, string filepath, S3CannedACL s3CannedACL)
        {
            try
            {
                var request = new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = System.IO.Path.GetFileName(filepath),
                    FilePath = filepath,
                    CannedACL = S3CannedACL.BucketOwnerFullControl
                };

                _ = await s3Client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! " + ex.Message);
            }
            finally
            {
                LoadObjectData(bucketName, dgName);
            }
        }
    }
}
