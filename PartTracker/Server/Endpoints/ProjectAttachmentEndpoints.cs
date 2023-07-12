using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class ProjectAttachmentEndpoints
{
    public static void MapProjectAttachmentEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ProjectAttachment").WithTags(nameof(ProjectAttachment));

        group.MapGet("/search", async Task<PaginatedResult<ProjectAttachment>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.ProjectAttachments.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetProjectAttachmentPaginated")
            .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.ProjectAttachments.ToListAsync();
        })
        .WithName("GetAllProjectAttachments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ProjectAttachment>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.ProjectAttachments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ProjectAttachment model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProjectAttachmentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ProjectAttachment projectAttachment, PartTrackerContext db) =>
        {
            var affected = await db.ProjectAttachments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ProjectId, projectAttachment.ProjectId)
                  .SetProperty(m => m.AttachmentId, projectAttachment.AttachmentId)
                  .SetProperty(m => m.Id, projectAttachment.Id)
                  .SetProperty(m => m.Created, projectAttachment.Created)
                  .SetProperty(m => m.Updated, projectAttachment.Updated)
                  .SetProperty(m => m.Deleted, projectAttachment.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProjectAttachment")
        .WithOpenApi();

        group.MapPost("/", async (ProjectAttachment projectAttachment, PartTrackerContext db) =>
        {
            db.ProjectAttachments.Add(projectAttachment);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ProjectAttachment/{projectAttachment.Id}",projectAttachment);
        })
        .WithName("CreateProjectAttachment")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.ProjectAttachments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProjectAttachment")
        .WithOpenApi();
    }
}
