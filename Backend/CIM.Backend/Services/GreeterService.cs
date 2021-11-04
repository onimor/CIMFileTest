using CIM.Protos;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CIM.Backend
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

       
        public override async Task<StatusRequest> UploadFile(IAsyncStreamReader<FileRequest> requestStream, ServerCallContext context)
        {
            List<byte> newFile = new();
            string fileName = "";
            await foreach (var b in requestStream.ReadAllAsync())
            {
                fileName = b.FileName;
                newFile.AddRange(b.FileBytes.ToByteArray());
            }
            string filename = @$"Resources\FileTest\{fileName}";
            File.WriteAllBytes(filename, newFile.ToArray());
            return await Task.FromResult(new StatusRequest
            {
                Status = @$"Файл {fileName} загружен)"
            });
        }
        public override async Task GetFilesInfo(HelloRequest request, IServerStreamWriter<FilesInfoRequest> responseStream, ServerCallContext context)
        {
            var dir = new DirectoryInfo(@"Resources\FileTest"); // папка с файлами 

            foreach (FileInfo file in dir.GetFiles())
            {
                await responseStream.WriteAsync(new FilesInfoRequest
                {
                     FileName = file.Name,
                     FilePath = file.FullName
                });
            }
           
           
        }
        private const int _maxSize = 3 * 1024 * 1024;
        private byte[] _part = new byte[_maxSize];
        public override async Task DownloadFile(FilesInfoRequest request, IServerStreamWriter<FileRequest> responseStream, ServerCallContext context)
        {
            using (FileStream fstream = new FileStream(request.FilePath,FileMode.Open))
            {
                while (fstream.Length - fstream.Position > 0)
                {
                    if (fstream.Length - fstream.Position <= _maxSize)
                        _part = new byte[fstream.Length - fstream.Position];

                    await fstream.ReadAsync(_part, 0, _part.Length);

                    await responseStream.WriteAsync(new FileRequest
                    {
                        FileName = fstream.Name,
                        FileBytes = UnsafeByteOperations.UnsafeWrap(_part)
                    });

                }

                   
            }
        }
    }
}
