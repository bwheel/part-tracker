using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class PartCategoryEndpoints
{
    public static void MapPartCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PartCategory").WithTags(nameof(PartCategory));

        group.MapGet("/search", async Task<PaginatedResult<PartCategory>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.PartCategories.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetPartCategoryPaginated")
            .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.PartCategories.ToListAsync();
        })
        .WithName("GetAllPartCategories")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<PartCategory>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.PartCategories.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is PartCategory model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPartCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, PartCategory partCategory, PartTrackerContext db) =>
        {
            var affected = await db.PartCategories
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.PartId, partCategory.PartId)
                  .SetProperty(m => m.CategoryId, partCategory.CategoryId)
                  .SetProperty(m => m.Id, partCategory.Id)
                  .SetProperty(m => m.Created, partCategory.Created)
                  .SetProperty(m => m.Updated, partCategory.Updated)
                  .SetProperty(m => m.Deleted, partCategory.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePartCategory")
        .WithOpenApi();

        group.MapPost("/", async (PartCategory partCategory, PartTrackerContext db) =>
        {
            db.PartCategories.Add(partCategory);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/PartCategory/{partCategory.Id}", partCategory);
        })
        .WithName("CreatePartCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.PartCategories
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePartCategory")
        .WithOpenApi();
    }
}
