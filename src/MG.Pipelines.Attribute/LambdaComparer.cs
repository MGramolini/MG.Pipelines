namespace MG.Pipelines.Attribute
{
	using System;
	using System.Collections.Generic;

	public class LambdaComparer<T> : IEqualityComparer<T>
	{
		/// <summary>
		/// The inner comparer
		/// </summary>
		private readonly Func<T, T, bool> innerComparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="LambdaComparer{T}"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		/// <exception cref="System.ArgumentNullException">comparer</exception>
		public LambdaComparer(Func<T, T, bool> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			this.innerComparer = comparer;
		}

		#region IEqualityComparer<T> Members

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
		/// <returns>
		/// true if the specified objects are equal; otherwise, false.
		/// </returns>
		public bool Equals(T x, T y)
		{
			return this.innerComparer(x, y);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public int GetHashCode(T obj)
		{
			return 0;
		}

		#endregion
	}
}