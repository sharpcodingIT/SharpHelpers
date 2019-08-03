﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpCoding.SharpHelpers
{
   
    public static class ByteExtension
    {  
        /// <summary>
        /// Given a byte array, this method returns the specified object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="istance"></param>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] istance) where T : class
        {
            return (T)istance?.ToObject();
        }

        /// <summary>
        /// Given a byte array, this method returns an object 
        /// </summary>
        /// <param name="istance"></param>
        /// <returns></returns>
        public static object ToObject(this byte[] istance)
        {
            if (istance == null) return null;

            using (var memoryStream = new MemoryStream(istance))
            {
                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Given an object, this method returns a byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object value)
        {
            if (value == null) return new byte[0];

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, value);
                return memoryStream.ToArray();
            }
        }
    }
}
