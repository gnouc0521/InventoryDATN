namespace bbk.netcore.mdl.PersonalProfile.Application.Dto
{
    public class FileDto
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }

        public FileDto(string FileUrl)
        {
            try
            {
                this.FileUrl = FileUrl;
                this.FileName = System.IO.Path.GetFileName(FileUrl);
                this.FileExtension = System.IO.Path.GetExtension(FileUrl);
            }
            catch { }
        }
    }
}
