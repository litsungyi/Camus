using System;
using System.Collections.Generic;
using System.Linq;

namespace Camus.Utilities
{
    public static class RandomHelper
    {
        public static int Next(int max, int min = 0)
        {
            return new Random().Next(min, max);
        }

        public static IList<T> Take<T>(IList<T> datas, int count)
        {
            var total = datas.Count();
            var indexes = new List<int>(total);
            for (int index = 0; index < total; ++index)
            {
                indexes.Add(index);
            }

            var results = new List<T>();
            for (int i = 0; i < count; ++i)
            {
                var j = Next(total, i);
                if (i != j)
                {
                    var temp = indexes[i];
                    indexes[i] = indexes[j];
                    indexes[j] = temp;
                }

                results.Add(datas[indexes[i]]);
            }

            return results;
        }
    }
}
