using System;
using System.Collections.Generic;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Repositories
{
	/// <summary>
	/// Repository that stores object data in a type-object key value repository
	/// </summary>
	public class DictionaryObjectRepository :
		IObjectRepository,
		IReadOnlyObjectRepository,
		ICloneableObjectRepository
	{
		/// <summary>
		/// Actual storage
		/// </summary>
		private readonly IRepository<Type, object> database;

		public DictionaryObjectRepository(IRepository<Type, object> database)
		{
			this.database = database;
		}

		/// <summary>
		/// Does the repository have the data by the given type?
		/// </summary>
		/// <typeparam name="TValue">Value data type</typeparam>
		/// <returns>Does it or not</returns>
		public bool Has<TValue>()
		{
			return database.Has(typeof(TValue));
		}

		/// <summary>
		/// Add the given data by the given type
		/// </summary>
		/// <param name="value">Value</param>
		/// <typeparam name="TValue">Value data type</typeparam>
		public void Add<TValue>(TValue value)
		{
			database.Add(typeof(TValue), value);
		}

		/// <summary>
		/// Update the data by the given type
		/// </summary>
		/// <param name="value">Value</param>
		/// <typeparam name="TValue">Value data type</typeparam>
		public void Update<TValue>(TValue value)
		{
			database.Update(typeof(TValue), value);
		}

		/// <summary>
		/// Set the data by the given type
		/// </summary>
		/// <param name="value">Value</param>
		/// <typeparam name="TValue">Value data type</typeparam>
		public void AddOrUpdate<TValue>(TValue value)
		{
			database.AddOrUpdate(typeof(TValue), value);
		}

		/// <summary>
		/// Retrieve the data by the given type
		/// </summary>
		/// <typeparam name="TValue">Value data type</typeparam>
		/// <returns>Value</returns>
		public TValue Get<TValue>()
		{
			return (TValue)database.Get(typeof(TValue));
		}

		/// <summary>
		/// Retrieve the data by the given type if it is present
		/// </summary>
		/// <param name="value">Value</param>
		/// <typeparam name="TValue">Value data type</typeparam>
		/// <returns>Was the data present</returns>
		public bool TryGet<TValue>(out TValue value)
		{
			value = default(TValue);
			
			bool result = database.TryGet(typeof(TValue), out object valueObject);

			if (result)
				value = (TValue)valueObject;

			return result;
		}

		/// <summary>
		/// Remove the data by the given type
		/// </summary>
		/// <typeparam name="TValue">Value data type</typeparam>
		public void Remove<TValue>()
		{
			database.Remove(typeof(TValue));
		}

		/// <summary>
		/// List the types present in the repository
		/// </summary>
		/// <value>Keys</value>
		public IEnumerable<Type> Keys
		{
			get => database.Keys;
		}

		public IObjectRepository Clone()
		{
			return RepositoriesFactory.CloneDictionaryObjectRepository(database);
		}
	}
}