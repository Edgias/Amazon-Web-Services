using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edgias.Study.AWS.AmazonS3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                RegionEndpoint _bucketRegion = RegionEndpoint.USEast2;
                string _bucketName = "edgiasfirstbucket";

                CredentialProfileStoreChain chain = new CredentialProfileStoreChain();
                bool profileFound = chain.TryGetProfile("basic_profile", out CredentialProfile basicProfile);

                // If profile doesn't exist try to create it.
                if (!profileFound)
                {
                    RegisterProfile(_bucketRegion);
                    bool profileFoundAfterRegistration = chain.TryGetProfile("basic_profile", out basicProfile);

                    if (!profileFoundAfterRegistration)
                    {
                        Console.WriteLine("Profile was not found");
                        return;
                    }
                }

                using (var client = new AmazonS3Client(basicProfile.Options.AccessKey, basicProfile.Options.SecretKey, _bucketRegion))
                {
                    BucketManager bucketManager = new BucketManager(client);

                    Console.WriteLine("Listing objects stored in a bucket, please wait...");

                    IEnumerable<S3Object> objects = await bucketManager.GetObjectsAsync(_bucketName);

                    foreach (S3Object entry in objects)
                    {
                        Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                    }
                }

                Console.WriteLine("Done");

                Console.Read();
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

        /// <summary>
        /// Register a profile to access a bucket using your IAM credentials
        /// </summary>
        /// <param name="regionEndpoint">Region to use</param>
        static void RegisterProfile(RegionEndpoint regionEndpoint)
        {
            CredentialProfileOptions options = new CredentialProfileOptions
            {
                AccessKey = "access_key",
                SecretKey = "secret_key"
            };

            CredentialProfile profile = new CredentialProfile("basic_profile", options)
            {
                Region = regionEndpoint
            };

            NetSDKCredentialsFile netSDKFile = new NetSDKCredentialsFile();

            netSDKFile.RegisterProfile(profile);
        }
    }
}
