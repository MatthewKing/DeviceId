#if NET35

namespace System.Collections.Generic
{
     
    /// <summary>
    /// Provides access to instances of the C# code generator and code compiler.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {


        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the set</param>
        /// <exception cref="T:System.ArgumentNullException"> other is null</exception>
        void ExceptWith(IEnumerable<T> other);

        /// <summary>
        /// Modifies the current set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other"> The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        void IntersectWith(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns> true if the current set is a proper subset of other; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        bool IsProperSubsetOf(IEnumerable<T> other);

        /// <summary>
        ///  Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>true if the current set is a proper superset of other; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        bool IsProperSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>true if the current set is a subset of other; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        bool IsSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>other is null.</returns>
        bool IsSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns> true if the current set and other share at least one common element; otherwise,false.</returns>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        bool Overlaps(IEnumerable<T> other);

        /// <summary>
        ///  Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>rue if the current set is equal to other; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        bool SetEquals(IEnumerable<T> other);

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other"> The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        void SymmetricExceptWith(IEnumerable<T> other);

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in the current set, in the specified collection, or in both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException:">other is null</exception>
        void UnionWith(IEnumerable<T> other);
    }
}
#endif
