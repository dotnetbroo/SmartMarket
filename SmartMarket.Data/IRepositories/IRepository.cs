using SmartMarket.Domin.Commons;

namespace SmartMarket.Data.IRepositories;

public interface IRepository<TEntity> where TEntity : Auditable
{
    Task<bool> DeleteAsync(long id);
    IQueryable<TEntity> SelectAll();
    Task<TEntity> SelectByIdAsync(long id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}
