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




        public async Task<StorageFile> CreateFileAsync(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            return await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }

        public async void WriteToFile(StorageFile file, string data) => await FileIO.WriteTextAsync(file, data);
        public async void WriteToFile(StorageFile file, IBuffer data) => await FileIO.WriteBufferAsync(file, data);



        public async Task<string> ReadStringFromFile(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var foundFile = await storageFolder.GetFileAsync(fileName);
            return await FileIO.ReadTextAsync(foundFile);
        }
        public async Task<string> ReadStringFromFileViaBuffer(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var foundFile = await storageFolder.GetFileAsync(fileName);
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(foundFile);
            string text;
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                text = dataReader.ReadString(buffer.Length);
            }
            return text;
        }
        public async Task<string> ReadStringFromFileViaStream(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var foundFile = await storageFolder.GetFileAsync(fileName);
            var stream = await foundFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
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
            var storageFolder = ApplicationData.Current.LocalFolder;
            var foundFile = await storageFolder.GetFileAsync(fileName);
            return await FileIO.ReadBufferAsync(foundFile);
        }
        public async Task<IBuffer> ReadBufferFromFileViaBuffer(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var foundFile = await storageFolder.GetFileAsync(fileName);
            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(foundFile);
            IBuffer data;
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                data = dataReader.ReadBuffer(buffer.Length);
            }
            return data;
        }
        public async Task<IBuffer> ReadBufferFromFileViaStream(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var foundFile = await storageFolder.GetFileAsync(fileName);
            var stream = await foundFile.OpenAsync(FileAccessMode.Read);
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
            var fileName = "sample.txt";

            var newFile = await CreateFileAsync(fileName);
            WriteToFile(newFile, "test1");

            //var existingFile = await ReadStringFromFile(fileName);
            //Assert existingFile == "test1";

            newFile = await CreateFileAsync(fileName);
            var buffer = CryptographicBuffer.ConvertStringToBinary("test2", BinaryStringEncoding.Utf8);
            WriteToFile(newFile, buffer);
            //var existingFile = await ReadStringFromFile(fileName);
            //Assert existingFile == "test2";

        }


    }
}

// https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files
//C:\Users\fajar\AppData\Local\Packages\06cbfd52-5681-4c73-b610-b20b50467b75_1v77q6cebkz10\LocalState