using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class PartEndpoints
{
    public static void MapPartEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Part").WithTags(nameof(Part));

        group.MapGet("/search", async Task<PaginatedResult<Part>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.Parts.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetPartPaginated")
            .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.Parts.ToListAsync();
        })
        .WithName("GetAllParts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Part>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.Parts.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Part model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPartById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Part part, PartTrackerContext db) =>
        {
            var affected = await db.Parts
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Title, part.Title)
                  .SetProperty(m => m.Name, part.Name)
                  .SetProperty(m => m.SerialNumber, part.SerialNumber)
                  .SetProperty(m => m.ModelNumber, part.ModelNumber)
                  .SetProperty(m => m.Quantity, part.Quantity)
                  .SetProperty(m => m.Notes, part.Notes)
                  .SetProperty(m => m.Id, part.Id)
                  .SetProperty(m => m.Created, part.Created)
                  .SetProperty(m => m.Updated, part.Updated)
                  .SetProperty(m => m.Deleted, part.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePart")
        .WithOpenApi();

        group.MapPost("/", async (Part part, PartTrackerContext db) =>
        {
            db.Parts.Add(part);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Part/{part.Id}", part);
        })
        .WithName("CreatePart")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.Parts
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePart")
        .WithOpenApi();
    }
}
