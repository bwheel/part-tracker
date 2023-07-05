using Microsoft.AspNetCore.Mvc;
using PartTracker.Server.Data;
using PartTracker.Shared;
using PartTracker.Shared.Models;
using System;

namespace PartTracker.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {

        private readonly ILogger<PartsController> m_logger;
        private readonly PartTrackerContext m_dbContext;

        public PartsController(
            ILogger<PartsController> logger,
            PartTrackerContext dbContext
            )
        {
            m_logger = logger;
            m_dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<PaginatedResult<Part>> Get(
            [FromQuery] string sortBy = nameof(Part.Title),
            [FromQuery] bool sortAscending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = m_dbContext.Parts.AsQueryable();

                data = sortData(data, sortBy, sortAscending);
                var result = paginateData(data, page, pageSize);
                return Ok(result);
            }
            catch(Exception ex)
            {
                m_logger.LogError(ex, "Error in fetching Parts");
                return NoContent();
            }
        }

        private PaginatedResult<Part> paginateData(IQueryable<Part> data, int page, int pageSize)
        {
            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedData = data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Part>
            {
                Items = pagedData,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        private IQueryable<Part> sortData(IQueryable<Part> data, string sortBy, bool sortAscending)
        {
            switch (sortBy)
            {
                case nameof(Part.Title):
                    return sortAscending ? data.OrderBy(p => p.Title) : data.OrderByDescending(p => p.Title);
                case nameof(Part.Name):
                    return sortAscending ? data.OrderBy(p => p.Name) : data.OrderByDescending(p => p.Name);
                case nameof(Part.Id):
                    return sortAscending ? data.OrderBy(p => p.Id) : data.OrderByDescending(p => p.Id);
                case nameof(Part.Created):
                    return sortAscending ? data.OrderBy(p => p.Created) : data.OrderByDescending(p => p.Created);
                case nameof(Part.Updated):
                    return sortAscending ? data.OrderBy(p => p.Updated) : data.OrderByDescending(p => p.Updated);
                case nameof(Part.Deleted):
                    return sortAscending ? data.OrderBy(p => p.Deleted) : data.OrderByDescending(p => p.Deleted);
                default:
                    throw new ArgumentException("Invalid sortBy parameter.");
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult> SaveAsync(Part part)
        {
            m_dbContext.Add(part);
            int count = await m_dbContext.SaveChangesAsync();
            if (count == 1)
            {
                return Ok(); // Created() TODO: when getById works.
            } else
            {
                return StatusCode(500);
            }
        }
    }
}