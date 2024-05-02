using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeMAUI.Model
{
    public class FileUpload
    {
        public async Task<Stream> FileResultToStream(FileResult result)
        {
            if (result == null) return null;
            return await result.OpenReadAsync();
        }
        public Stream ByteArrayToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }
        public string ByteBase64ToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        public byte[] StringToByteBase64(string text)
        {
            return Convert.FromBase64String(text);
        }
        public async Task<ImageFile> Upload(FileResult fileResult)
        {
            byte[] bytes;
            try
            {
                using (var ms = new MemoryStream())
                {
                    var stream = await FileResultToStream(fileResult);
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                }
                return new ImageFile
                {
                    ByteBase64 = ByteBase64ToString(bytes),
                    ContentType = fileResult.ContentType,
                    FileName = fileResult.FileName
                };
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

