using Amazon;
using Amazon.S3;
using System;
using System.Threading.Tasks;

namespace Edgias.Study.AWS.AmazonS3
{
    class Program
    {
        private static readonly RegionEndpoint _bucketRegion = RegionEndpoint.USEast2;

        static async Task Main(string[] args)
        {
            MakeS3Request makeS3Request;
            using (var client = new AmazonS3Client(_bucketRegion))
            {
                makeS3Request = new MakeS3Request(client);

                Console.WriteLine("Listing objects stored in a bucket");

                await makeS3Request.ListingObjectsAsync();
            }


            Console.Read();
        }
    }
}
