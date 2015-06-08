using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Auth;
using Amazon.Auth.AccessControlPolicy;

using log4net;

using System;
using System.IO;
using System.Threading;


using System.IO.Compression;

namespace MessageGears.EventQueue
{
    /// <summary>
    /// MessageGears C# SDK main entry point. 
    /// </summary>
    public class MessageGearsAwsClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MessageGearsAwsClient));
        MessageGearsAwsProperties properties = null;
        AmazonSQS sqs = null;
        AmazonS3 s3 = null;
        
        /// <summary>
        /// Used to create a new instance of the MessageGears client.
        /// </summary>
        /// <param name="props">
        /// Contains the credentials needed to access MessageGears, Amazon S3, and Amazon SQS.<see cref="MessageGearsProperties"/>
        /// </param>
        public MessageGearsAwsClient(MessageGearsAwsProperties props)
        {
            this.properties = props;
            this.sqs = new AmazonSQSClient(properties.MyAWSAccountKey, properties.MyAWSSecretKey);
            this.s3 = new AmazonS3Client(properties.MyAWSAccountKey, properties.MyAWSSecretKey);
            log.Info("MessageGears AWS client initialized");
        }
        
        /// <summary>
        /// Used to compress a given file into a .gz file.
        /// </summary>
        /// <param name="inputFileName">
        /// The path to and file name to be compressed.
        /// </param>
        /// <returns>
        /// The path to and file name of the newly created compressed file.
        /// </returns>
        public String CompressFile(String inputFileName) {
            String outputFileName = inputFileName.Replace(".xml",".gz");
            FileStream inStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            FileStream outStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            GZipStream compressedStream = new GZipStream(outStream, CompressionMode.Compress);
            CopyStream(inStream, compressedStream);
            compressedStream.Close();
            return outputFileName;
        }        

        /// <summary>
        /// Copies an InputStream to Amazon S3 and grants access to MessageGears.
        /// </summary>
        /// <param name="stream">
        /// An input stream for the data to be copied to S3.
        /// </param>
        /// <param name="bucketName">
        /// The name of the S3 bucket where the file will be copied.
        /// </param>
        /// <param name="key">
        /// The S3 key of the file to be created.
        /// </param>
        public void PutS3File(Stream stream, String bucketName, String key)
        {
            // Check to see if the file already exists in S3
            ListObjectsRequest listRequest = new ListObjectsRequest().WithBucketName(bucketName).WithPrefix(key);
            ListObjectsResponse listResponse = listFiles(listRequest);
            if(listResponse.S3Objects.Count > 0)
            {
                String message = "File " + key + " already exists.";
                log.Warn("PutS3File failed: " + message);
                throw new ApplicationException(message);
            }
            
            // Copy a file to S3
            PutObjectRequest request = new PutObjectRequest().WithKey(key).WithBucketName(bucketName).WithTimeout(properties.S3PutTimeoutSecs * 1000);
            request.WithInputStream(stream);
            putWithRetry(request);
            setS3PermissionsWithRetry(bucketName, key);
            
            log.Info("PutS3File successful: " + key);
        }
        
        /// <summary>
        /// Copies a file to Amazon S3 and grants READ-ONLY access to MessageGears.
        /// </summary>
        /// <param name="fileName">
        /// The fully qualified name of the file to be copied to S3.
        /// </param>
        /// <param name="bucketName">
        /// The name of the S3 bucket where the file will be copied.
        /// </param>
        /// <param name="key">
        /// The S3 key of the file to be created.
        /// </param>
        public void PutS3File(String fileName, String bucketName, String key)
        {
            // Check to see if the file already exists in S3
            ListObjectsRequest listRequest = new ListObjectsRequest().WithBucketName(bucketName).WithPrefix(key);
            ListObjectsResponse listResponse = listFiles(listRequest);
            if(listResponse.S3Objects.Count > 0)
            {
                String message = "File " + fileName + " already exists.";
                log.Warn("PutS3File failed: " + message);
                throw new ApplicationException(message);
            }
            
            // Copy a file to S3
            PutObjectRequest request = new PutObjectRequest().WithKey(key).WithFilePath(fileName).WithBucketName(bucketName).WithTimeout(properties.S3PutTimeoutSecs * 1000);
            putWithRetry(request);
            setS3PermissionsWithRetry(bucketName, key);
            
            log.Info("PutS3File successful: " + fileName);
        }
        
        /// <summary>
        /// Deletes a file from Amazon S3.
        /// </summary>
        /// <param name="bucketName">
        /// The name of the bucket where the file resides.
        /// </param>
        /// <param name="key">
        /// The key of the file to be deleted.
        /// </param>
        public void DeleteS3File(String bucketName, String key)
        {
            // Copy a file to S3
            DeleteObjectRequest request = new DeleteObjectRequest().WithBucketName(bucketName).WithKey(key);
            try 
            {
                deleteWithRetry(request);
                log.Info("DeleteS3File successful: " + bucketName + "/" + key);
            } 
            catch(Exception e) 
            {
                log.Info("DeleteS3File failed: " + bucketName + "/" + key + " : " + e.ToString());    
            }
        }
        
        /// <summary>
        /// Creates a new queue in Amazon SQS and grants SendMessage only access to MessageGears.
        /// </summary>
        /// <param name="queueName">
        /// The name of the queue to be created.
        /// </param>
        /// <returns>
        /// The full url of the newly created queue.
        /// </returns>
        public String CreateQueue(String queueName)
        {
            CreateQueueRequest request = new CreateQueueRequest()
                .WithQueueName(queueName)
                .WithDefaultVisibilityTimeout(properties.SQSVisibilityTimeoutSecs);

            CreateQueueResponse response = sqs.CreateQueue(request);
            
            addQueuePermission(response.CreateQueueResult.QueueUrl);
            setMaximumMessageSize(response.CreateQueueResult.QueueUrl);
            
            log.Info("Create queue successful: " + queueName);
            
            return response.CreateQueueResult.QueueUrl;
        }
        
        private void setMaximumMessageSize(String queueUrl)
        {
            Amazon.SQS.Model.Attribute[] attrs = new Amazon.SQS.Model.Attribute[1];
            attrs[0] = new Amazon.SQS.Model.Attribute().WithName("MaximumMessageSize").WithValue("65536");
            SetQueueAttributesRequest request = new SetQueueAttributesRequest().WithQueueUrl(queueUrl).WithAttribute(attrs);
            sqs.SetQueueAttributes(request);
        }
        
        private void addQueuePermission(String queueUrl)
        {
            AddPermissionRequest permissionRequest = new AddPermissionRequest()
                .WithActionName("SendMessage")
                .WithAWSAccountId(properties.MessageGearsAWSAccountId)
                .WithLabel("MessageGears Send Permission")
                .WithQueueUrl(queueUrl);
        
            sqs.AddPermission(permissionRequest);
        }
        
        private ListObjectsResponse listFiles(ListObjectsRequest request) 
        {
            // Retry list request up to five times
            for (int i = 0; i<properties.S3MaxErrorRetry; i++) 
            {
                try 
                {
                    // Attempt to put the object
                    ListObjectsResponse response = s3.ListObjects(request);
                    return response;
                } 
                catch (AmazonS3Exception exception) 
                {
                    log.Info("Failed to retrieve file list from S3: " + exception.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
                catch (Exception e) {
                    log.Info("Failed to retrieve file list from S3: " + e.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
            }
            
            throw new AmazonS3Exception("Failed list objects on S3");
        }
        
        private void putWithRetry(PutObjectRequest request) 
        {
            // Retry put request up to five times
            for (int i = 0; i<properties.S3MaxErrorRetry; i++) 
            {
                try 
                {
                    // Attempt to put the object
                    s3.PutObject(request);
                    log.Info("Successfully uploaded file to S3: " + request.ToString());
                    return;
                } 
                catch (AmazonS3Exception exception) 
                {
                    log.Info("Failed to upload file to S3: " + exception.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
                catch (Exception e) {
                    log.Info("Failed to upload file to S3: " + e.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
            }
            
            throw new AmazonS3Exception("Failed to upload file to S3");
        }
        
        private void deleteWithRetry(DeleteObjectRequest request) 
        {
            // Retry delete request up to five times
            for (int i = 0; i<properties.S3MaxErrorRetry; i++) 
            {
                try 
                {
                    // Attempt to delete the object
                    s3.DeleteObject(request);
                    log.Info("Successfully deleted file on S3: " + request.ToString());
                    return;
                } 
                catch (AmazonS3Exception exception) 
                {
                    log.Debug("Failed to deleted file from S3: " + exception.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
                catch (Exception e) {
                    log.Debug("Failed to deleted file from S3: " + e.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
            }
            
            throw new AmazonS3Exception("Failed to deleted file from S3");
        }
        
        private void setS3PermissionsWithRetry(String bucketName, String key) 
        {
            // Retry setting permissions up to five times
            for (int i = 0; i<properties.S3MaxErrorRetry; i++) 
            {
                try 
                {
                    // Set permissions
                    setS3Permission(bucketName, key);
                    return;
                } 
                catch (AmazonS3Exception exception) 
                {
                    log.Info("Failed to set permissions for file on S3: " + exception.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
                catch (Exception e) {
                    log.Info("Failed to set permissions for file to S3: " + e.ToString());
                    Thread.Sleep(properties.S3RetryDelayInterval);
                }
            }
        }
        
        private void setS3Permission(String bucketName, String key)
        {
            // Get the ACL for the file and retrieve the owner ID (not sure how to get it otherwise).
            GetACLRequest getAclRequest = new GetACLRequest().WithBucketName(bucketName).WithKey(key);
            GetACLResponse aclResponse = s3.GetACL(getAclRequest);
            Owner owner = aclResponse.AccessControlList.Owner;
            
            // Create a grantee as the MessageGears account
            S3Grantee grantee = new S3Grantee().WithCanonicalUser(properties.MessageGearsAWSCanonicalId, "MessageGears");  
            
            // Grant MessageGears Read-only access
            S3Permission messageGearsPermission = S3Permission.READ;
            S3AccessControlList acl = new S3AccessControlList().WithOwner(owner);
            acl.AddGrant(grantee, messageGearsPermission);
            
            // Create a new ACL granting the owner full control.
            grantee = new S3Grantee().WithCanonicalUser(owner.Id, "MyAWSId");
            acl.AddGrant(grantee, S3Permission.FULL_CONTROL);
            SetACLRequest aclRequest = new SetACLRequest().WithACL(acl).WithBucketName(bucketName).WithKey(key);
            s3.SetACL(aclRequest);
        }
        
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ( (len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }    
        }
    }        
}