using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class PartAttachmentEndpoints
{
    public static void MapPartAttachmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PartAttachment").WithTags(nameof(PartAttachment));

        group.MapGet("/search", async Task<PaginatedResult<PartAttachment>> (
           string sortBy,
           bool sortAscending,
           string? searchText,
           int page,
           int pageSize,
           PartTrackerContext db
           ) =>
        {
            var data = db.PartAttachments.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
           .WithName("GetParAttachmentPaginated")
           .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.PartAttachments.ToListAsync();
        })
        .WithName("GetAllPartAttachments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<PartAttachment>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.PartAttachments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is PartAttachment model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPartAttachmentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, PartAttachment partAttachment, PartTrackerContext db) =>
        {
            var affected = await db.PartAttachments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.PartId, partAttachment.PartId)
                  .SetProperty(m => m.AttachmentId, partAttachment.AttachmentId)
                  .SetProperty(m => m.Id, partAttachment.Id)
                  .SetProperty(m => m.Created, partAttachment.Created)
                  .SetProperty(m => m.Updated, partAttachment.Updated)
                  .SetProperty(m => m.Deleted, partAttachment.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePartAttachment")
        .WithOpenApi();

        group.MapPost("/", async (PartAttachment partAttachment, PartTrackerContext db) =>
        {
            db.PartAttachments.Add(partAttachment);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/PartAttachment/{partAttachment.Id}", partAttachment);
        })
        .WithName("CreatePartAttachment")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.PartAttachments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePartAttachment")
        .WithOpenApi();
    }
}
