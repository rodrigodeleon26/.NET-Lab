using Amazon.S3.Model;
using Amazon.S3;
using Amazon.Runtime;

namespace HistoriaClinicaWebApi.Controllers
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var options = configuration.GetAWSOptions();
            options.Credentials = new BasicAWSCredentials(
                configuration["AWS:AccessKey"],
                configuration["AWS:SecretKey"]
            );

            _s3Client = options.CreateServiceClient<IAmazonS3>();
            _bucketName = configuration["AWS:BucketName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName, // Nombre del archivo en S3
                InputStream = fileStream,
                ContentType = contentType,
                AutoCloseStream = true
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // Devolver la URL del archivo en S3
                return $"http://{_bucketName}.s3.amazonaws.com/{fileName}";
            }

            throw new Exception("Error al subir archivo a S3");
        }
    }
}
