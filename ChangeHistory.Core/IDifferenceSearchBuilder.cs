using System;

namespace ChangeHistory.Core
{
    public interface IDifferenceSearchBuilder<TSource>
    {
        IDifferenceSearchBuilder<TSource> One<TProp>(Func<TSource, TProp> func);
        IDifferenceSearchBuilder<TSource> All<TProp>(Func<TSource, TProp> func) where TProp : class;
        IDifferenceSearchBuilder<TSource> GoDepth<TProp>(Func<TSource, TProp> func) where TProp : class;
        void Build();
    }
}
