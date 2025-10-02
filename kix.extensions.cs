using System;
using System.Collections.Generic;
using System.Linq;

namespace kix
  {
  public static class Extensions
    {

    public static bool In<T>(this T element, params T[] set) => set.Contains(element);
    public static bool In<T>(this T element, IEnumerable<T> set) => set.Contains(element);
    public static bool In(this string element, StringComparer comparer, params string[] set) => set.Contains(element,comparer);
    public static bool In(this string element, StringComparer comparer, IEnumerable<string> set) => set.Contains(element,comparer);
    public static bool InLoosely(this string element, params string[] set) => set.Contains(element,StringComparer.OrdinalIgnoreCase);
    public static bool InLoosely(this string element, IEnumerable<string> set) => set.Contains(element,StringComparer.OrdinalIgnoreCase);
    public static bool NotIn<T>(this T element, params T[] set) => !element.In(set);
    public static bool NotIn<T>(this T element, IEnumerable<T> set) => !element.In(set);
    public static bool NotIn(this string element, StringComparer comparer, params string[] set) => !element.In(comparer,set);
    public static bool NotIn(this string element, StringComparer comparer, IEnumerable<string> set) => !element.In(comparer,set);
    public static bool NotInLoosely(this string element, params string[] set) => !element.InLoosely(set);
    public static bool NotInLoosely(this string element, IEnumerable<string> set) => !element.InLoosely(set);

    }
  }
