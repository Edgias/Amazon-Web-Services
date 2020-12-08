using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edgias.Study.AWS
{
    public class BucketManager
    {
        //private const string _bucketName = "edgiasfirstbucket";

        private static IAmazonS3 _client;

        public BucketManager(AmazonS3Client client)
        {
            _client = client;
        }

        public async Task<IEnumerable<S3Object>> GetObjectsAsync(string bucketName)
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    MaxKeys = 10
                };

                List<S3Object> bucketObjects = new List<S3Object>();

                do
                {
                    ListObjectsResponse response = await _client.ListObjectsAsync(request);

                    // Process the response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        bucketObjects.Add(entry);
                    }

                    // If the response is truncated, set the marker to get the next 
                    // set of keys.
                    if (response.IsTruncated)
                    {
                        request.Marker = response.NextMarker;
                    }
                    else
                    {
                        request = null;
                    }
                } while (request != null);

                return bucketObjects;
            }

            catch (AmazonS3Exception)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
