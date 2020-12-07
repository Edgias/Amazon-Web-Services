using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace Edgias.Study.AWS
{
    public class MakeS3Request
    {
        private const string _bucketName = "edgiasfirstbucket";

        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint _bucketRegion = RegionEndpoint.USEast2;
        private static IAmazonS3 _client;

        public MakeS3Request(AmazonS3Client client)
        {
            _client = client;
        }

        //public static void Main()
        //{
        //    using (_client = new AmazonS3Client(_bucketRegion))
        //    {
        //        Console.WriteLine("Listing objects stored in a bucket");
        //       // ListingObjectsAsync().Wait();
        //    }
        //}

        public async Task ListingObjectsAsync()
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = _bucketName,
                    MaxKeys = 2
                };
                do
                {
                    ListObjectsResponse response = await _client.ListObjectsAsync(request);
                    // Process the response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
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
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}
