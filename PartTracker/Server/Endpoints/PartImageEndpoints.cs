using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class PartImageEndpoints
{
    public static void MapPartImageEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PartImage").WithTags(nameof(PartImage));

        group.MapGet("/search", async Task<PaginatedResult<PartImage>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.PartImages.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetPartImagePaginated")
            .WithOpenApi();


        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.PartImages.ToListAsync();
        })
        .WithName("GetAllPartImages")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<PartImage>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.PartImages.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is PartImage model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPartImageById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, PartImage partImage, PartTrackerContext db) =>
        {
            var affected = await db.PartImages
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.PartId, partImage.PartId)
                  .SetProperty(m => m.ImageId, partImage.ImageId)
                  .SetProperty(m => m.Updated, DateTime.UtcNow)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePartImage")
        .WithOpenApi();

        group.MapPost("/", async (PartImage partImage, PartTrackerContext db) =>
        {
            db.PartImages.Add(partImage);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/PartImage/{partImage.Id}", partImage);
        })
        .WithName("CreatePartImage")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.PartImages
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Deleted, DateTime.UtcNow)
                    );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("SoftDeletePartImage")
        .WithOpenApi();

        group.MapPost("/{id}/delete", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.PartImages
               .Where(model => model.Id == id)
               .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
            .WithName("DeletePartImage")
            .WithOpenApi();
    }
}
