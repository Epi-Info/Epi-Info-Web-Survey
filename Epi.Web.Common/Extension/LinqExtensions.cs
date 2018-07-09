using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.Extension
    {
    public static class LinqExtensions
        {
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
            {
            var stack = new Stack<T>(items);
            while (stack.Any())
                {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
                }
            }

         
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
                {
                return listToClone.Select(item => (T)item.Clone()).ToList();
                }
            
        }
    }
