using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CIM.FileHelper
{
    public static class Сonverter
    {
        public static byte[] MergeData(List<byte[]> data)
        {
            List<Package> packages = new List<Package>();
            foreach (var item in data)
            {
                string json = item.GZipDecompress();
                Package package = JsonSerializer.Deserialize<Package>(json);
                packages.Add(package);
            }
            if (packages.Count > 0 && packages[0].t == packages.Count)
            {

                var src_bytes = packages
                    .OrderBy(x => x.c)
                    .Select(x => x.d)
                    .ToArray();
                List<byte> result = new List<byte>();
                for (int i = 0; i < src_bytes.Length; i++)
                {
                    result.AddRange(src_bytes[i]);
                }
                return result.ToArray();
            }
            return default;
        }

        public static async Task<List<byte[]>> SplitDataStream(Stream src_data_stream, int length, Action<string> action)
        {
            if (src_data_stream is null)
            {
                return default;
            }
            action($"src_data:?, length: {length}");
            List<byte[]> result = new List<byte[]>();

            byte[] _part = new byte[length];
            int i = 0;
            while (src_data_stream.Length - src_data_stream.Position > 0)
            {
                if (src_data_stream.Length - src_data_stream.Position <= length)
                    _part = new byte[src_data_stream.Length - src_data_stream.Position];

                await src_data_stream.ReadAsync(_part, 0, _part.Length);

                action($"i: {i++}");
                var package = new Package
                {
                    c = i++,
                    d = _part,
                    t = 0
                };
                byte[] _data = JsonSerializer.Serialize(package).GZipComptess();
                result.Add(_data);
            }

            action($"SplitData: End");
            return result;
        }


        public static List<byte[]> SplitData(byte[] src_data, int length)
        {
            int counter = length;
            var group_data = src_data.Split(length).ToArray();


            List<byte[]> result = new List<byte[]>();
            for (int i = 0; i < group_data.Length; i++)
            {
                var package = new Package
                {
                    c = i,
                    d = group_data[i].ToArray(),
                    t = group_data.Count()
                };
                byte[] data = JsonSerializer.Serialize(package).GZipComptess();
                result.Add(data);
            }
            return result;
        }
    }
}

