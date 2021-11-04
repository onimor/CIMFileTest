using System.Collections.Generic;
using System.Linq;

namespace CIM.FileHelper
{
    public static class ArrSplit
    {
        public static IEnumerable<IEnumerable<byte>> Split(this byte[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }
}
