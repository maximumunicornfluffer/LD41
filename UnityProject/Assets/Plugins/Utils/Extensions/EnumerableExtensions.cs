using System.Collections.Generic;
using System.Text;


namespace Plugins.Utils.Extensions
{
  public static class EnumerableExtensions
  {
    public delegate string ToS<A>(A elem);
    
    public static string MkString<A>(this IEnumerable<A> enumerable, ToS<A> toS ,string separator)
    {
      var sb = new StringBuilder();
      foreach (A elem in enumerable)
      {
        sb.Append(toS(elem)).Append(separator);
      }

      if (sb.Length > 0)
        sb.Remove(sb.Length - 1, 1);

      return sb.ToString();
    }
  }
}