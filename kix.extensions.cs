using System;
using System.Collections.Generic;
using System.Linq;

namespace kix
  {
  static public class Extensions
    {

    static public bool ContainsAllOf(this string subject, params string[] set) => set.All(subject.Contains);
    static public bool ContainsAllOf<T>(this IEnumerable<T> subject, IEnumerable<T> set) => set.All(subject.Contains);
    static public bool ContainsAllOf<T>(this IEnumerable<T> subject, params T[] set) => set.All(subject.Contains);
    static public bool ContainsAnyOf(this string subject, params string[] set) => set.Any(subject.Contains);
    static public bool ContainsAnyOf<T>(this IEnumerable<T> subject, IEnumerable<T> set) => set.Any(subject.Contains);
    static public bool ContainsAnyOf<T>(this IEnumerable<T> subject, params T[] set) => set.Any(subject.Contains);
    static public bool ContainsNoneOf(this string subject, params string[] set) => !set.Any(subject.Contains);
    static public bool ContainsNoneOf<T>(this IEnumerable<T> subject, IEnumerable<T> set) => !set.Any(subject.Contains);
    static public bool ContainsNoneOf<T>(this IEnumerable<T> subject, params T[] set) => !set.Any(subject.Contains);
    static public bool In<T>(this T element, IEnumerable<T> set) => set.Contains(element);
    static public bool In<T>(this T element, params T[] set) => set.Contains(element);
    static public bool In(this string element, StringComparer comparer, params string[] set) => set.Contains(element,comparer);
    static public bool In(this string element, StringComparer comparer, IEnumerable<string> set) => set.Contains(element,comparer);
    static public bool InLoosely(this string element, params string[] set) => set.Contains(element,StringComparer.OrdinalIgnoreCase);
    static public bool InLoosely(this string element, IEnumerable<string> set) => set.Contains(element,StringComparer.OrdinalIgnoreCase);
    static public bool NotIn<T>(this T element, params T[] set) => !element.In(set);
    static public bool NotIn<T>(this T element, IEnumerable<T> set) => !element.In(set);
    static public bool NotIn(this string element, StringComparer comparer, params string[] set) => !element.In(comparer,set);
    static public bool NotIn(this string element, StringComparer comparer, IEnumerable<string> set) => !element.In(comparer,set);
    static public bool NotInLoosely(this string element, params string[] set) => !element.InLoosely(set);
    static public bool NotInLoosely(this string element, IEnumerable<string> set) => !element.InLoosely(set);

    }
  }
