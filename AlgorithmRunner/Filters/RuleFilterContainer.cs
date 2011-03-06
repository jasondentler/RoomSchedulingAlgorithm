using System.Linq;

namespace AlgorithmRunner.Filters
{
    public class RuleFilterContainer<TEntity1, TEntity2> 
        : IRuleFilter<TEntity1, TEntity2>
    {
        private readonly IRuleFilter<TEntity1, TEntity2>[] _ruleFilters;

        public RuleFilterContainer(params IRuleFilter<TEntity1, TEntity2>[] ruleFilters)
        {
            _ruleFilters = ruleFilters;
        }

        public bool IsValid(TEntity1 entity1, TEntity2 entity2)
        {
            return _ruleFilters.All(f => f.IsValid(entity1, entity2));
        }
    }
}
