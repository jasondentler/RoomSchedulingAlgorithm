namespace AlgorithmRunner.Filters
{

    public interface IFilter<TEntity1, TEntity2>
    {

        bool IsValid(TEntity1 entity1, TEntity2 entity2);

    }

}
