using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Framwork
{
    public static class AsyncIEnumerableExtensions
    {
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IEnumerable<TSource> source,
          Func<TSource, ValueTask<TResult>> predicate)
        {
            return source.ToAsyncEnumerable().SelectAwait(predicate);
        }
    }
}
