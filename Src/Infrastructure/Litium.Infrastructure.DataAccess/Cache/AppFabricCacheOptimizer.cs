using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Litium.Infrastructure.DataAccess.Cache
{
	public class AppFabricCacheOptimizer
	{
		public static byte[] Compress(object value)
		{
			byte[] serialized = Serialize(value);
			byte[] compressed;
			using (var outStream = new MemoryStream())
			{
				using (var deflateStream = new DeflateStream(outStream, CompressionMode.Compress, true))
				{
					deflateStream.Write(serialized, 0, serialized.Length);
				}
				compressed = outStream.ToArray();
			}
			return compressed;
		}

		public static object Decompress(byte[] bytes)
		{
			byte[] decompressed;
			using (var inStream = new MemoryStream(bytes))
			{
				using (var outStream = new MemoryStream())
				{
					using (var deflateStream = new DeflateStream(inStream, CompressionMode.Decompress))
					{
						deflateStream.CopyTo(outStream);
					}
					decompressed = outStream.ToArray();
				}
			}
			return Deserialize(decompressed);
		}

		private static byte[] Serialize(object value)
		{
			using (var stream = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, value);
				return stream.ToArray();
			}
		}

		private static object Deserialize(byte[] bytes)
		{
			using (var stream = new MemoryStream())
			{
				stream.Write(bytes, 0, bytes.Length);
				stream.Seek(0, SeekOrigin.Begin);
				IFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(stream);
			}
		}
	}
}
