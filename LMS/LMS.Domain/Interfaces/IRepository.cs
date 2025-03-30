namespace LMS.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<ICollection<TEntity>> GettAllAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetByCriteriaAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteHardlyAsync(Guid id);
        Task DeleteAsync(Guid id);

        Task SaveChangesAsync();
    }
}
