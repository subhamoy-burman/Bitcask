using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcask.Console
{
    /// <summary>
    /// KeyDirectoryMapper is the in memory index that maps the keys to their position in the datafile
    /// </summary>
    internal class KeyDirectoryMapper
    {
        private readonly Dictionary<string, long> _index = new Dictionary<string, long>();

        public void Add(string key, long position)
        {
            _index[key] = position;
        }

        public long GetPosition(string key)
        {
            //TODO
            return _index.ContainsKey(key) ? _index[key] : -1;
        }

        public List<string> GetAllKeys()
        {
            return _index.Keys.ToList();
        }
    }
}
