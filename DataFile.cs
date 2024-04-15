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
        private readonly string _filePath;
        private readonly FileStream _fileStream;

        public DataFile(string filePath)
        {
            _filePath = filePath;
            _fileStream = new FileStream(_filePath, FileMode.Append);
        }

        public long Append(KeyValuePair<string, string> item)
        {
            long position = _fileStream.Position;
            byte[] data = Encoding.UTF8.GetBytes($"{item.Key}:{item.Value}\n");
            _fileStream.Write(data, 0, data.Length);
            _fileStream.Flush();
            return position;
        }

        public string Read(long position)
        {
            _fileStream.Position = position;
            StreamReader reader = new StreamReader(_fileStream);
            string line = reader.ReadLine()!;
            return line!.Split(':')[1];
        }

        public string GetCurrentFilePath() { return _filePath; }
    }
}
