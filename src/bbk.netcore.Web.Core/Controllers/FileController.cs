using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using bbk.netcore.Dto;
using bbk.netcore.Net.MimeTypes;
using bbk.netcore.Storage;
using bbk.netcore.Storage.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace bbk.netcore.Controllers
{
    public class FileController : netcoreControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        //private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            //IBinaryObjectManager binaryObjectManager,
            IFileSystemBlobProvider fileSystemBlobProvider
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            //_binaryObjectManager = binaryObjectManager;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        //[DisableAuditing]
        //public ActionResult DownloadTempFile(FileDto file)
        //{
        //    var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
        //    if (fileBytes == null)
        //    {
        //        return NotFound(L("RequestedFileDoesNotExists"));
        //    }

        //    return File(fileBytes, file.FileType, file.FileName);
        //}

        //[DisableAuditing]
        //public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        //{
        //    var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
        //    if (fileObject == null)
        //    {
        //        return StatusCode((int)HttpStatusCode.NotFound);
        //    }

        //    return File(fileObject.Bytes, contentType, fileName);
        //}

        [DisableAuditing]
        public async Task<ActionResult> DownloadReportFile(FileDto file)
        {
            //var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            //if (fileBytes == null)
            //{
            //    return NotFound(L("RequestedFileDoesNotExists"));
            //}
            file.FileToken = Guid.NewGuid().ToString();
            file.FileType = MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet;

            var fileStream = (MemoryStream)await _fileSystemBlobProvider.GetOrNullAsync(new StorageProviderGetArgs(file.FileName));

            return File(fileStream.ToArray(), file.FileType, file.FileName);
        }
    }
}
