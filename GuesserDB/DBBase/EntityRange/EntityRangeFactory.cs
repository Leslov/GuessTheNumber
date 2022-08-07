using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GuesserDB.DBBase.EntityRange
{
	internal class EntityRangeFactory
	{
		public static IEntityRange<T> CreateSome<T>()
		{
			return new SerializableEntityRange<T>();
		}

		private class SerializableEntityRange<T> : IEntityRange<T>
		{
			private readonly string path;
			public SerializableEntityRange()
			{
				path = Path.Combine($"{typeof(T).FullName}.json");
				Load();
			}

			private void Load()
			{
				if (File.Exists(path))
				{
					var toDeserialize = File.ReadAllText(path);
					if (!string.IsNullOrWhiteSpace(toDeserialize))
						values = JsonSerializer.Deserialize<T[]>(toDeserialize);
				}
			}

			private T[] values { get; set; }
			public void AddRange(IEnumerable<T> range) => values = values.Concat(range.ToArray()).ToArray();
			public void SetRange(T[] range) => values = range;

			public void Add(T entity) => values = values.Concat(new T[] { entity }).ToArray();

			public T[] ToArray() => values.ToArray();

			public void Clear()
			{
				values = new T[0];
			}

			public void Save()
			{
				string jsonToSave = JsonSerializer.Serialize(values);
				File.WriteAllText(path, jsonToSave);
			}
		}
	}
}
