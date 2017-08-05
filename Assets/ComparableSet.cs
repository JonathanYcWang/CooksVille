using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HashSetEqualityComparer<T> : IEqualityComparer<HashSet<T>>
{
    public bool Equals(HashSet<T> lhs, HashSet<T> rhs)
    {
        if (lhs == rhs)
            return true;

        if (lhs == null || rhs == null || lhs.Count != rhs.Count)
            return false;

        foreach (var item in lhs)
            if (!rhs.Contains(item))
                return false;

        return true;
    }

    public int GetHashCode(HashSet<T> hashset)
    {
        if (hashset == null)
            return 0;

        IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
        int hash = 0;
        foreach (var item in hashset)
            hash ^= comparer.GetHashCode(item);

        return hash;
    }
}
