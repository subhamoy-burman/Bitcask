using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcask.Console
{
    /// <summary>
    /// This class is responsible for writing key-value pairs to the disk and reading them back
    /// It has methods for appending the key value pair to the file and for reading a value given by key
    /// </summary>
    internal class DataFile
    {
        private const long MaxFileSize = 500 * 1024 * 1024; // 500 MB
        private string _filePath;
        private FileStream _fileStream;

        public DataFile(string directoryPath)
        {
            _filePath = GetNextFilePath(directoryPath);
            _fileStream = new FileStream(_filePath, FileMode.Append);
        }

        public (string, long) Append(KeyValuePair<string, string> item)
        {
            if (_fileStream.Length >= MaxFileSize)
            {
                _fileStream.Close();
                _filePath = GetNextFilePath(Path.GetDirectoryName(_filePath)!);
                _fileStream = new FileStream(_filePath, FileMode.Append);
            }

            long position = _fileStream.Position;
            byte[] data = Encoding.UTF8.GetBytes($"{item.Key}:{item.Value}\n");
            _fileStream.Write(data, 0, data.Length);
            _fileStream.Flush();
            return (_filePath, position);

        }

        public string Read(string filePath, long position)
        {
            if (_filePath != filePath)
            {
                _fileStream.Close();
                _filePath = filePath;
                _fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            }

            _fileStream.Position = position;
            StreamReader reader = new StreamReader(_fileStream);
            string line = reader.ReadLine()!;
            return line!.Split(':')[1];
        }

        public string GetCurrentFilePath() { return _filePath; }

        private string GetNextFilePath(string directoryPath)
        {
            int fileIndex = Directory.GetFiles(directoryPath, "*.data").Length + 1;
            return Path.Combine(directoryPath, $"data{fileIndex}.data");
        }

        internal Stream GetFileStream()
        {
            return _fileStream;
        }
    }
}
