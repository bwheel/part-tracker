using Microsoft.EntityFrameworkCore;
using PartTracker.Shared.Models;

namespace PartTracker.Server.Helpers
{
    public static class TableExtensions
    {
        /// <summary>
        /// Paginate
        /// 
        /// This method paginates the data from the database
        /// </summary>
        /// <typeparam name="T">The type of data</typeparam>
        /// <param name="data">The collection of data from DB</param>
        /// <param name="sortBy">The name of the column to sort. ex: nameof(Part.name)</param>
        /// <param name="sortAscending">Direction of sort</param>
        /// <param name="page">The page number to return</param>
        /// <param name="pageSize">The number of records on the page</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">No data is provided</exception>
        /// <exception cref="ArgumentNullException">No sortBy column is provided</exception>
        /// /// <exception cref="MissingMemberException">The sortBy column does not map to class</exception>
        /// <exception cref="ArgumentOutOfRangeException">Page is not a positive number</exception>
        /// <exception cref="ArgumentOutOfRangeException">pageSize is not a positive number</exception>
        public static async Task<PaginatedResult<T>> Paginate<T>(IQueryable<T> data, string sortBy, bool sortAscending, string? searchText, int page = 1, int pageSize = 10) where T : notnull
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(sortBy)) throw new ArgumentNullException(nameof(sortBy));
            if (typeof(T).GetProperty(sortBy) == null) throw new MissingMemberException(nameof(sortBy));
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var getValsForObject = (object? o) => typeof(T).GetProperties().Select( p =>  p.GetValue(o)?.ToString() );
                data = data.Where(o => string.Join("", getValsForObject(o)).Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            if (sortAscending)
                data = data.OrderBy(p => EF.Property<T>(p, sortBy));
            else
                data = data.OrderByDescending(p => EF.Property<T>(p, sortBy));

            var totalItems = await data.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedData = await data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>
            {
                Items = pagedData,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}
