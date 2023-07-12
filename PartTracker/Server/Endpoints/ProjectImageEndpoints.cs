using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class ProjectImageEndpoints
{
    public static void MapProjectImageEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ProjectImage").WithTags(nameof(ProjectImage));


        group.MapGet("/search", async Task<PaginatedResult<ProjectImage>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.ProjectImages.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetProjectImagePaginated")
            .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.ProjectImages.ToListAsync();
        })
        .WithName("GetAllProjectImages")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ProjectImage>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.ProjectImages.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ProjectImage model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProjectImageById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ProjectImage projectImage, PartTrackerContext db) =>
        {
            var affected = await db.ProjectImages
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ProjectId, projectImage.ProjectId)
                  .SetProperty(m => m.ImageId, projectImage.ImageId)
                  .SetProperty(m => m.Id, projectImage.Id)
                  .SetProperty(m => m.Created, projectImage.Created)
                  .SetProperty(m => m.Updated, projectImage.Updated)
                  .SetProperty(m => m.Deleted, projectImage.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProjectImage")
        .WithOpenApi();

        group.MapPost("/", async (ProjectImage projectImage, PartTrackerContext db) =>
        {
            db.ProjectImages.Add(projectImage);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ProjectImage/{projectImage.Id}",projectImage);
        })
        .WithName("CreateProjectImage")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.ProjectImages
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProjectImage")
        .WithOpenApi();
    }
}
