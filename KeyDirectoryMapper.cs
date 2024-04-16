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
        private readonly Dictionary<string, (string, long)> _index = new Dictionary<string, (string, long)>();

        public void Add(string key, (string, long) position)
        {
            _index[key] = position;
        }

        public (string?, long) GetPosition(string key)
        {
            return _index.ContainsKey(key) ? _index[key] : (null, -1);
        }

        public List<string> GetAllKeys()
        {
            return _index.Keys.ToList();
        }

        public void Update(string directoryPath)
        {
            _index.Clear();

            var allFiles = Directory.GetFiles(directoryPath, "*.data");
            foreach (var file in allFiles)
            {
                using (var stream = File.OpenRead(file))
                {
                    var reader = new StreamReader(stream);
                    string line;
                    long position = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':');
                        string key = parts[0];
                        _index[key] = (file, position);
                        position += Encoding.UTF8.GetByteCount(line) + 1; // +1 for the newline character
                    }
                }
            }
        }
    }
}
