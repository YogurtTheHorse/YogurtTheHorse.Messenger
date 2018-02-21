using System.Collections.Generic;

namespace YogurtTheHorse.Messenger {
    public static class Extensions {
        public static List<T> ToList<T>(this T[] array) {
            return new List<T>(array);
        }

        public static List<List<T>> ToList<T>(this T[,] array) {
            var res = new List<List<T>>();

            for (int i = 0; i < array.GetLength(0); ++i) {
                res[i] = new List<T>();
                for (int j = 0; j < array.GetLength(1); ++j) {
                    res[i].Add(array[i, j]);
                }
            }

            return res;
        }
    }
}
