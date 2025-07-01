namespace Person.Routes;

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Person.Data;
using Person.Models;

public static class PersonRoute
{
    public static void PersonRoutes(this WebApplication app)
    {
        var route = app.MapGroup("person");

        route.MapPost("",
            async (PersonRequest req, PersonContext context) =>
            {
                var person = new PersonModel
                {
                    Name = req.Name,
                };
                await context.People.AddAsync(person);
                await context.SaveChangesAsync();
            }
        );

        route.MapGet("",
            async (PersonContext context) =>
            {
                var people = await context.People.ToListAsync();
                return Results.Ok(people);
            });

        route.MapPut("{id:guid}",
        async (Guid id, PersonRequest req, PersonContext context) =>
        {
            var person = await context.People.FindAsync(id);

            if (person == null)
            {
                return Results.NotFound();
            }
            person.Name = req.Name;
            await context.SaveChangesAsync();

            return Results.Ok(person);
        }
        );

        route.MapDelete("{id:guid}",
         async (Guid id, PersonContext context) =>
         {
             var person = await context.People.FindAsync(id);
             if (person == null)
             {
                 return Results.NotFound();
             }
             context.People.Remove(person);
             await context.SaveChangesAsync();
             return Results.NoContent();
         }
        );
    }

}