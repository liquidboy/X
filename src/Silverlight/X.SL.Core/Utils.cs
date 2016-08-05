using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace X.SL.Core
{
    public static class Utils
    {
        public static async Task<StorageFolder> CreateTempDir(string fileName) {

            var tempDir = $@"c:\temp\";
            var tempFileDir =  $@"{tempDir}{fileName}";
            
            var tempFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(tempDir);
            var tempFileFolder = await tempFolder.CreateFolderAsync(tempFileDir);
            
            return tempFileFolder;

        }
    }
}
