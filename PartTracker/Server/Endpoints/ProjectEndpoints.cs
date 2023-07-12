using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Project").WithTags(nameof(Project));

        group.MapGet("/search", async Task<PaginatedResult<Project>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.Projects.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetProjectPaginated")
            .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.Projects.ToListAsync();
        })
        .WithName("GetAllProjects")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Project>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.Projects.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Project model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProjectById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Project project, PartTrackerContext db) =>
        {
            var affected = await db.Projects
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Name, project.Name)
                  .SetProperty(m => m.Description, project.Description)
                  .SetProperty(m => m.Id, project.Id)
                  .SetProperty(m => m.Created, project.Created)
                  .SetProperty(m => m.Updated, project.Updated)
                  .SetProperty(m => m.Deleted, project.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProject")
        .WithOpenApi();

        group.MapPost("/", async (Project project, PartTrackerContext db) =>
        {
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Project/{project.Id}",project);
        })
        .WithName("CreateProject")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.Projects
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProject")
        .WithOpenApi();
    }
}
