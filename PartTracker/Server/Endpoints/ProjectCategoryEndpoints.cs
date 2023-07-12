using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PartTracker.Server.Data;
using PartTracker.Shared.Models;
using PartTracker.Server.Helpers;

namespace PartTracker.Server.Endpoints;

public static class ProjectCategoryEndpoints
{
    public static void MapProjectCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ProjectCategory").WithTags(nameof(ProjectCategory));


        group.MapGet("/search", async Task<PaginatedResult<ProjectCategory>> (
            string sortBy,
            bool sortAscending,
            string? searchText,
            int page,
            int pageSize,
            PartTrackerContext db
            ) =>
        {
            var data = db.ProjectCategories.AsQueryable();
            return await TableExtensions.Paginate(data, sortBy, sortAscending, searchText, page, pageSize);
        })
            .WithName("GetProjectCategoryPaginated")
            .WithOpenApi();

        group.MapGet("/", async (PartTrackerContext db) =>
        {
            return await db.ProjectCategories.ToListAsync();
        })
        .WithName("GetAllProjectCategories")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ProjectCategory>, NotFound>> (int id, PartTrackerContext db) =>
        {
            return await db.ProjectCategories.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ProjectCategory model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProjectCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ProjectCategory projectCategory, PartTrackerContext db) =>
        {
            var affected = await db.ProjectCategories
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ProjectId, projectCategory.ProjectId)
                  .SetProperty(m => m.CategoryId, projectCategory.CategoryId)
                  .SetProperty(m => m.Id, projectCategory.Id)
                  .SetProperty(m => m.Created, projectCategory.Created)
                  .SetProperty(m => m.Updated, projectCategory.Updated)
                  .SetProperty(m => m.Deleted, projectCategory.Deleted)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProjectCategory")
        .WithOpenApi();

        group.MapPost("/", async (ProjectCategory projectCategory, PartTrackerContext db) =>
        {
            db.ProjectCategories.Add(projectCategory);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ProjectCategory/{projectCategory.Id}",projectCategory);
        })
        .WithName("CreateProjectCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, PartTrackerContext db) =>
        {
            var affected = await db.ProjectCategories
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProjectCategory")
        .WithOpenApi();
    }
}
