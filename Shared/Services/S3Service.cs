using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Amazon;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service()
        {
            // Obtener las configuraciones de S3 desde la clase GlobalFunctions
            var s3Config = GlobalFunctions.GetS3Config();

            // Establecer las credenciales de AWS del .env
            var key = Environment.GetEnvironmentVariable("S3_KEY");
            var secret = Environment.GetEnvironmentVariable("S3_SECRET");
            Console.WriteLine("S3_KEY: " + key);
            Console.WriteLine("S3_SECRET: " + secret);
            var credentials = new BasicAWSCredentials(key, secret);

            // Convertir la cadena de la región a RegionEndpoint
            var region = RegionEndpoint.GetBySystemName(s3Config["Region"]);

            // Crear el cliente S3 usando las credenciales y región especificadas
            _s3Client = new AmazonS3Client(credentials, region);
            _bucketName = s3Config["BucketName"];
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName, 
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
