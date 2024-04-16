using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcask.Console
{
    internal class Bitcask
    {
        private DataFile _dataFile;
        private readonly KeyDirectoryMapper _keyDirectoryMapper;

        public long MaxFileSize { get; private set; }

        public Bitcask(string directoryPath)
        {
            _dataFile = new DataFile(directoryPath);
            _keyDirectoryMapper = new KeyDirectoryMapper();
        }

        public void Put(string key, string value)
        {
            (string filePath, long position) = _dataFile.Append(new KeyValuePair<string, string>(key, value));
            _keyDirectoryMapper.Add(key, (filePath, position));
        }

        public string Get(string key)
        {
            (string? filePath, long position) = _keyDirectoryMapper.GetPosition(key);
            return _dataFile.Read(filePath!, position);
        }

        public void Compact()
        {
            string tempFilePath = Path.GetTempFileName();
            DataFile tempDataFile = new DataFile(tempFilePath);

            foreach (var key in _keyDirectoryMapper.GetAllKeys())
            {
                string value = Get(key);
                (string filePath, long newPosition) = tempDataFile.Append(new KeyValuePair<string, string>(key, value));
                _keyDirectoryMapper.Add(key, (filePath, newPosition));
            }

            //_dataFile.Dispose();
            File.Delete(_dataFile.GetCurrentFilePath());
            File.Move(tempFilePath, _dataFile.GetCurrentFilePath());

            _dataFile = new DataFile(_dataFile.GetCurrentFilePath());
        }

        public void Merge()
        {
            string tempDirectoryPath = Path.GetTempPath();
            DataFile tempDataFile = null;
            long currentFileSize = 0;

            var allFiles = Directory.GetFiles(_dataFile.GetCurrentFilePath(), "*.data");
            foreach (var file in allFiles)
            {
                var fileInfo = new FileInfo(file);
                if (tempDataFile == null || currentFileSize + fileInfo.Length > MaxFileSize)
                {
                    //tempDataFile?.Dispose();
                    tempDataFile = new DataFile(tempDirectoryPath);
                    currentFileSize = 0;
                }

                // Copy the contents of the old file to the new file
                using (var sourceStream = File.OpenRead(file))
                {
                    sourceStream.CopyTo(tempDataFile.GetFileStream());
                }
                currentFileSize += fileInfo.Length;
            }

            // Delete old data files
            foreach (var filePath in allFiles)
            {
                File.Delete(filePath);
            }

            // Move new data files to original directory
            foreach (var filePath in Directory.GetFiles(tempDirectoryPath, "*.data"))
            {
                File.Move(filePath, Path.Combine(_dataFile.GetCurrentFilePath(), Path.GetFileName(filePath)));
            }

            // Update the key directory mapper
            _keyDirectoryMapper.Update(_dataFile.GetCurrentFilePath());
        }

    }
}
