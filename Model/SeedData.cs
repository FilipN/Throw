using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Throw.Model
{
    public class SeedData
    {

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            DataContext context = app.ApplicationServices
                .GetRequiredService<DataContext>();
            context.Database.Migrate();
            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project
                    {
                        Title = "dnevna krema",
                        Content = "nega koze"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
