using System;
using System.Collections.Generic;

namespace kix
  {
  static public partial class k
    {

    //
    // subtype
    //
    #pragma warning disable CA1066 // Type {0} should implement IEquatable<T> because it overrides Equals
    #pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    #pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct subtype<TComparable> where TComparable : IComparable
    #pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    #pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    #pragma warning restore CA1066 // Type {0} should implement IEquatable<T> because it overrides Equals
      {

      public TComparable FIRST { get => first; }
      public TComparable LAST { get => last; }
      public static readonly Exception CONSTRAINT_ERROR = new Exception("kix.k.subtype<TComparable>.CONSTRAINT_ERROR");
      public subtype // CONSTRUCTOR
        (
        TComparable the_first,
        TComparable the_last
        )
        {
        first = the_first;
        current = the_first;
        last = the_last;
        }
      public subtype<TComparable> SetValTo(TComparable newVal) // to allow chaining an initial non-first/non-last val assignment to a new() operation
        {
        val = newVal;
        return this;
        }
      public TComparable val
        {
        get
          {
          return current;
          }
        set
          {
          if ((value.CompareTo(first) < 0) || (value.CompareTo(last) > 0))
            {
            throw CONSTRAINT_ERROR;
            }
          unchecked
            {
            current = value;
            }
          }
        }
      public override bool Equals(object obj)
        {
        return obj is subtype<TComparable> subtype && EqualityComparer<TComparable>.Default.Equals(val, subtype.val);
        }
      public static bool operator ==(subtype<TComparable> left, subtype<TComparable> right)
        {
        return left.Equals(right);
        }
      public static bool operator !=(subtype<TComparable> left, subtype<TComparable> right)
        {
        return !(left == right);
        }

      private readonly TComparable first;
      private TComparable current;
      private readonly TComparable last;

      }

    }
  }
