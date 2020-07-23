using System.Collections.Generic;
using System.IO;

namespace WordNet
{
    public class IdMapping
    {
        private readonly Dictionary<string, string> _map;

        /**
         * <summary>Constructor to load ID mappings from specific file "mapping.txt" to a {@link HashMap}.</summary>
         */
        public IdMapping()
        {
            var assembly = typeof(IdMapping).Assembly;
            var stream = assembly.GetManifestResourceStream("WordNet.mapping.txt");
            _map = new Dictionary<string, string>();
            var streamReader = new StreamReader(stream);
            var s = streamReader.ReadLine();
            while (s != null)
            {
                var mapInfo = s.Split("->");
                _map[mapInfo[0]] = mapInfo[1];
                s = streamReader.ReadLine();
            }
        }

        /**
         * <summary>Constructor to load ID mappings from given file to a {@link HashMap}.</summary>
         *
         * <param name="fileName">string file name input that will be read</param>
         */
        public IdMapping(string fileName)
        {
            var streamReader = new StreamReader(fileName);
            var s = streamReader.ReadLine();
            while (s != null)
            {
                var mapInfo = s.Split("->");
                _map[mapInfo[0]] = mapInfo[1];
                s = streamReader.ReadLine();
            }
        }

        /**
         * <summary>Returns a {@link Set} view of the keys contained in this map.</summary>
         *
         * <returns>a set view of the keys contained in this map</returns>
         */
        public List<string> KeySet()
        {
            return new List<string>(_map.Keys);
        }


        /**
         * <summary>Returns the value to which the specified key is mapped,
         * or {@code null} if this map contains no mapping for the key.</summary>
         *
         * <param name="id">string id of a key</param>
         * <returns>value of the specified key</returns>
         */
        public string Map(string id)
        {
            if (!_map.ContainsKey(id))
            {
                return null;
            }

            var mappedId = _map[id];
            while (_map.ContainsKey(mappedId))
            {
                mappedId = _map[mappedId];
            }

            return mappedId;
        }


        /**
         * <summary>Returns the value to which the specified key is mapped.</summary>
         *
         * <param name="id">string id of a key</param>
         * <returns>value of the specified key</returns>
         */
        public string SingleMap(string id)
        {
            return _map[id];
        }

        /**
         * <summary>Associates the specified value with the specified key in this map.</summary>
         *
         * <param name="key">  key with which the specified value is to be associated</param>
         * <param name="value">value to be associated with the specified key</param>
         */
        public void Add(string key, string value)
        {
            _map[key] = value;
        }

        /**
         * <summary>Removes the mapping for the specified key from this map if present.</summary>
         *
         * <param name="key">key whose mapping is to be removed from the map</param>
         */
        public void Remove(string key)
        {
            _map.Remove(key);
        }

        /**
         * <summary>Saves map to the specified file.</summary>
         *
         * <param name="fileName">string file to write map</param>
         */
        public void Save(string fileName)
        {
            var streamWriter = new StreamWriter(fileName);
            foreach (var key in _map.Keys)
            {
                streamWriter.WriteLine(key + "->" + _map[key]);
            }
            streamWriter.Close();
        }
    }
}