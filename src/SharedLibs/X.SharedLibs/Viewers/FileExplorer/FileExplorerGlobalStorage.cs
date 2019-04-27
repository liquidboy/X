using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using X.Services.Data;

namespace X.Viewer.FileExplorer
{
    public class FileExplorerGlobalStorage
    {

        private static FileExplorerGlobalStorage _storage;
        public static FileExplorerGlobalStorage Current
        {
            get
            {
                if (_storage == null) _storage = new FileExplorerGlobalStorage();
                return _storage;
            }
            private set { }
        }

        public void InitializeGlobalStorage(string connectionString)
        {
            AzureGlobalStorage.Current.InitializeGlobalStorage(connectionString);
        }

        public async Task<bool> ClearAll()
        {
            var foundTable = AzureGlobalStorage.Current.TableClient.GetTableReference("GlobalMediaFiles");
            await foundTable.DeleteIfExistsAsync();
            return true;
        }

        private async Task<StorageFolder> GetWorkingFolder() {
            //return ApplicationData.Current.LocalFolder;
            //return ApplicationData.Current.TemporaryFolder;
            var foundFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            var rootFolder = await foundFolder.CreateFolderAsync("XAssets", CreationCollisionOption.OpenIfExists);
            return rootFolder;
        }

        public async Task<StorageFile> CreateFileAndReplaceIfExists(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            return await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }

        public async Task<(bool FileExists, bool FileDoesNotExist, StorageFile FileThatWasFound)> DoesFileExist(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = false;
            StorageFile foundFile = null;
            try {
                foundFile = await storageFolder.GetFileAsync(fileName);
                doesFileExist = foundFile != null;
            }
            catch (Exception ex) {
                doesFileExist = false;
            }
            return (doesFileExist, !doesFileExist, foundFile);
        }

        public async Task DeleteFile(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var foundFile = await CreateFileAndReplaceIfExists(fileName);
            await foundFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }


        public async void WriteToFile(StorageFile file, string data) => await FileIO.WriteTextAsync(file, data);
        public async void WriteToFile(StorageFile file, IBuffer data) => await FileIO.WriteBufferAsync(file, data);



        public async Task<string> ReadStringFromFile(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = await DoesFileExist(fileName);
            if (doesFileExist.FileDoesNotExist) return string.Empty;
            return await FileIO.ReadTextAsync(doesFileExist.FileThatWasFound);
        }
        public async Task<string> ReadStringFromFileViaBuffer(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = await DoesFileExist(fileName);
            if (doesFileExist.FileDoesNotExist) return string.Empty;
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(doesFileExist.FileThatWasFound);
            string text;
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                text = dataReader.ReadString(buffer.Length);
            }
            return text;
        }
        public async Task<string> ReadStringFromFileViaStream(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = await DoesFileExist(fileName);
            if (doesFileExist.FileDoesNotExist) return string.Empty;
            var stream = await doesFileExist.FileThatWasFound.OpenAsync(Windows.Storage.FileAccessMode.Read);
            ulong size = stream.Size;
            string text;
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    text = dataReader.ReadString(numBytesLoaded);
                }
            }
            return text;
        }



        public async Task<IBuffer> ReadBufferFromFile(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = await DoesFileExist(fileName);
            if (doesFileExist.FileDoesNotExist) return null;
            return await FileIO.ReadBufferAsync(doesFileExist.FileThatWasFound);
        }
        public async Task<IBuffer> ReadBufferFromFileViaBuffer(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = await DoesFileExist(fileName);
            if (doesFileExist.FileDoesNotExist) return null;
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(doesFileExist.FileThatWasFound);
            IBuffer data;
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                data = dataReader.ReadBuffer(buffer.Length);
            }
            return data;
        }
        public async Task<IBuffer> ReadBufferFromFileViaStream(string fileName)
        {
            var storageFolder = await GetWorkingFolder();
            var doesFileExist = await DoesFileExist(fileName);
            if (doesFileExist.FileDoesNotExist) return null;
            var stream = await doesFileExist.FileThatWasFound.OpenAsync(FileAccessMode.Read);
            ulong size = stream.Size;
            IBuffer data;
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new DataReader(inputStream))
                {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    data = dataReader.ReadBuffer(numBytesLoaded);
                }
            }
            return data;
        }


        public async void TestStorage()
        {
            var fileName = "testing-storage.txt";

            var doesFileExist = await DoesFileExist(fileName);
            //Assert wasFileCreated.FileDoesNotExist = true

            var newFile = await CreateFileAndReplaceIfExists(fileName);
            var wasFileCreated = await DoesFileExist(fileName);
            //Assert wasFileCreated.FileExists = true

            newFile = await CreateFileAndReplaceIfExists(fileName);
            WriteToFile(newFile, "test1");
            var existingFile = await ReadStringFromFile(fileName);
            //Assert existingFile == "test1";

            newFile = await CreateFileAndReplaceIfExists(fileName);
            var buffer = CryptographicBuffer.ConvertStringToBinary("test2", BinaryStringEncoding.Utf8);
            WriteToFile(newFile, buffer);
            existingFile = await ReadStringFromFile(fileName);
            //Assert existingFile == "test2";

            newFile = await CreateFileAndReplaceIfExists(fileName);
            WriteToFile(newFile, "test3");
            existingFile = await ReadStringFromFileViaBuffer(fileName);
            //Assert existingFile == "test3";

            newFile = await CreateFileAndReplaceIfExists(fileName);
            WriteToFile(newFile, "test4");
            existingFile = await ReadStringFromFileViaStream(fileName);
            //Assert existingFile == "test4";

            newFile = await CreateFileAndReplaceIfExists(fileName);
            WriteToFile(newFile, "test5");
            var existingFileBuffer = await ReadBufferFromFile(fileName);
            existingFile = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, existingFileBuffer);
            //Assert existingFile == "test5";

            newFile = await CreateFileAndReplaceIfExists(fileName);
            WriteToFile(newFile, "test6");
            existingFileBuffer = await ReadBufferFromFileViaBuffer(fileName);
            existingFile = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, existingFileBuffer);
            //Assert existingFile == "test6";

            newFile = await CreateFileAndReplaceIfExists(fileName);
            WriteToFile(newFile, "test7");
            existingFileBuffer = await ReadBufferFromFileViaStream(fileName);
            existingFile = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, existingFileBuffer);
            //Assert existingFile == "test7";

            await DeleteFile(fileName);
            wasFileCreated = await DoesFileExist(fileName);
            //Assert wasFileCreated.FileDoesNotExist == true
        }


    }
}

// https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files
//C:\Users\fajar\AppData\Local\Packages\06cbfd52-5681-4c73-b610-b20b50467b75_1v77q6cebkz10\LocalState

// known folders
// https://docs.microsoft.com/en-us/uwp/api/windows.storage.knownfolders


// documents lib
// https://blogs.msdn.microsoft.com/wsdevsol/2013/05/09/dealing-with-documents-how-not-to-use-the-documentslibrary-capability-in-windows-store-apps/