using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcask.Console
{
    internal class Bitcask
    {
        private readonly DataFile _dataFile;
        private readonly KeyDirectoryMapper _keyDirectoryMapper;

        public Bitcask(string directoryPath)
        {
            
        }

        public void Put(string key, string value)
        {
            // TODO: 
        }

        public string Get(string key)
        {
            throw new NotImplementedException();
        }
    }
}
