using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Document").WithTags(nameof(Document));

        group.MapGet("/search", async Task<PaginatedResult<Document>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.Documents.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
        .WithName("GetDocumentPaginated")
        .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.Documents.ToListAsync();
        })
        .WithName("GetAllDocuments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Document>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.Documents.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Document model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetDocumentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Document document, PartTrackerContext db) =>
        {
            var affected = await db.Documents
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Filename, document.Filename)
                  .SetProperty(m => m.ServerDirPath, document.ServerDirPath)
                  .SetProperty(m => m.SizeBytes, document.SizeBytes)
                  .SetProperty(m => m.Uploaded, document.Uploaded)
                  .SetProperty(m => m.Id, document.Id)
                  .SetProperty(m => m.Created, document.Created)
                  .SetProperty(m => m.Updated, document.Updated)
                  .SetProperty(m => m.Deleted, document.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateDocument")
        .WithOpenApi();

        group.MapPost("/", async (Document document, PartTrackerContext db) =>
        {
            db.Documents.Add(document);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Document/{document.Id}", document);
        })
        .WithName("CreateDocument")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.Documents
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteDocument")
        .WithOpenApi();
    }
}
