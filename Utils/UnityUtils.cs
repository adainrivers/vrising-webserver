using System.Collections.Generic;
using Unity.Entities;

namespace VRising.WebServer.Utils
{
    public static class UnityUtils
    {
        public static List<T> ToList<T>(this BlobArray<T> blobArray) where T : new()
        {
            if (blobArray == null)
            {
                return null;
            }

            var result = new List<T>();
            for (var i = 0; i < blobArray.Length; i++)
            {
                result.Add(blobArray[i]);
            }

            return result;
        }
    }
}
