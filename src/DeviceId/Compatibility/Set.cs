#if NET35

namespace System.Collections.Generic
{
    class Set<T> : HashSet<T>, ISet<T>
    {
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.HashSet`1 class
        //     that is empty and uses the default equality comparer for the set type.
        public Set()
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.HashSet`1 class
        //     that is empty and uses the specified equality comparer for the set type.
        //
        // Parameters:
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer`1 implementation to use when
        //     comparing values in the set, or null to use the default System.Collections.Generic.EqualityComparer`1
        //     implementation for the set type.
        public Set(IEqualityComparer<T> comparer) : base(comparer)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.HashSet`1 class
        //     that uses the default equality comparer for the set type, contains elements copied
        //     from the specified collection, and has sufficient capacity to accommodate the
        //     number of elements copied.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements are copied to the new set.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     collection is null.
        public Set(IEnumerable<T> collection) : base(collection)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Collections.Generic.HashSet`1 class
        //     that uses the specified equality comparer for the set type, contains elements
        //     copied from the specified collection, and has sufficient capacity to accommodate
        //     the number of elements copied.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements are copied to the new set.
        //
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer`1 implementation to use when
        //     comparing values in the set, or null to use the default System.Collections.Generic.EqualityComparer`1
        //     implementation for the set type.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     collection is null.
        public Set(IEnumerable<T> collection, IEqualityComparer<T> comparer) :
            base(collection, comparer)
        {

        }
    }
}

#endif
