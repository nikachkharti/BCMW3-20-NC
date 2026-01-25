using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Forum.API.Repository
{
    public class RepositoryBase<T, TContext> : IRepositoryBase<T, TContext> where T : class where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(TContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public void RemoveRange(IEnumerable<T> entitites) => _dbSet.RemoveRange(entitites);
        public void Update(T entity) => _dbSet.Update(entity);
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string includeProperties = null, bool tracking = true)
        {
            IQueryable<T> query = _dbSet.Where(filter);

            if (!tracking)
                query = query.AsNoTracking();

            query = ApplyIncludes(query, includeProperties);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<(List<T> Items, int TotalCount)> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            int? pageNumber = null,
            int? pageSize = null,
            string orderBy = null,
            bool ascending = true,
            string includeProperties = null,
            bool tracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
                query = ApplyIncludes(query, includeProperties);

            int totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(orderBy))
                query = ApplyOrdering(orderBy, ascending, query);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skip = (pageNumber.Value - 1) * pageSize.Value;
                query = query
                    .Skip(skip)
                    .Take(pageSize.Value);
            }

            var items = await query.ToListAsync();

            return (items, totalCount);
        }



        private static IQueryable<T> ApplyIncludes(IQueryable<T> query, string includeProperties)
        {
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            return query;
        }


        //TODO IMPLEMENT
        private IQueryable<T> ApplyOrdering(string orderBy, bool ascending, IQueryable<T> query)
        {
            throw new NotImplementedException();
        }


    }
}
