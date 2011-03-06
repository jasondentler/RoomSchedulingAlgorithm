using System.Linq;

namespace AlgorithmRunner.Filters
{
    public class FilterContainer<TEntity1, TEntity2> 
        : IFilter<TEntity1, TEntity2>
    {
        private readonly IFilter<TEntity1, TEntity2>[] _filters;

        public FilterContainer(params IFilter<TEntity1, TEntity2>[] filters)
        {
            _filters = filters;
        }

        public bool IsValid(TEntity1 entity1, TEntity2 entity2)
        {
            return _filters.All(f => f.IsValid(entity1, entity2));
        }
    }
}
