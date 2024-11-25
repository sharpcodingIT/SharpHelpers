// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SharpCoding.SharpHelpers
{
    public static class StreamHelper
    {
        /// <summary>
        /// This method returns a string from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToString(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// This method returns a Base64 stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream ToBase64(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            var base64 = Convert.ToBase64String(bytes);
            return new MemoryStream(Encoding.UTF8.GetBytes(base64));
        }

        /// <summary>
        /// This method returns a byte array from a stream
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="stream">Input.</param>
        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// This method returns a MemoryStream from a stream
        /// </summary>
        /// <returns>The memory stream.</returns>
        /// <param name="stream">Stream.</param>
        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream;
            }
        }

        /// <summary>
        /// This method returns an object from a stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T ToObject<T>(this Stream stream) where T : class
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }

        /// <summary>
        /// Turns a string into a stream
        /// </summary>
        /// <param name="stringToConvert"></param>
        /// <returns></returns>
        public static Stream FromString(this string stringToConvert)
        {
            if (stringToConvert == null) throw new ArgumentNullException(nameof(stringToConvert));
            var bufferString = Encoding.UTF8.GetBytes(stringToConvert);
            var stream = new MemoryStream(stringToConvert.Length);
            stream.Write(bufferString, 0, bufferString.Length);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Turns a byte array into a stream
        /// </summary>
        /// <param name="arrayToConvert"></param>
        /// <returns></returns>
        public static Stream FromArray(this byte[] arrayToConvert)
        {
            return new MemoryStream(arrayToConvert, writable: false);
        }

        /// <summary>
        /// Writes a stream to file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ToFile(this Stream stream, string fileName)
        {
            return ToFile(stream, fileName, true);
        }

        /// <summary>
        /// Writes a stream to file
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="overrideExisting">If set to <c>true</c> override existing file.</param>
        /// <returns>True if successful</returns>
        public static bool ToFile(this Stream stream, string fileName, bool overrideExisting)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    if (overrideExisting)
                        File.Delete(fileName);
                    else
                        throw new IOException("File already exists");
                }

                stream.Position = 0;
                using (var fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads a stream from a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream FromFile(this string fileName)
        {
            byte[] fileBytes = File.ReadAllBytes(fileName);
            return FromArray(fileBytes);
        }
    }
}
