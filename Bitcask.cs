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

        public Bitcask(string directoryPath)
        {
            _dataFile = new DataFile(directoryPath);
            _keyDirectoryMapper = new KeyDirectoryMapper();
        }

        public void Put(string key, string value)
        {
            long position = _dataFile.Append(new KeyValuePair<string, string>(key, value));
            _keyDirectoryMapper.Add(key, position);
        }

        public string Get(string key)
        {
            long position = _keyDirectoryMapper.GetPosition(key);
            return _dataFile.Read(position);
        }

        public void Compact()
        {
            string tempFilePath = Path.GetTempFileName();
            DataFile tempDataFile = new DataFile(tempFilePath);

            foreach (var key in _keyDirectoryMapper.GetAllKeys())
            {
                string value = Get(key);
                long newPosition = tempDataFile.Append(new KeyValuePair<string, string>(key, value));
                _keyDirectoryMapper.Add(key, newPosition);
            }

            //_dataFile.Dispose();
            File.Delete(_dataFile.GetCurrentFilePath());
            File.Move(tempFilePath, _dataFile.GetCurrentFilePath());

            _dataFile = new DataFile(_dataFile.GetCurrentFilePath());
        }
    }
}
