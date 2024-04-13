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

        public DataFile(string filePath)
        {
            _filePath = filePath;
        }

        public void Append(KeyValuePair<string, string> item)
        {
            //TODO: 
        }

        public string Read(string key)
        {
            return string.Empty;
            //TODO: 
        }
    }
}
