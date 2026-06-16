using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext context, string seedFolderPath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if(!context.Plans.Any())
                {
                    var plan = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");
                    
                    if(plan.Any())
                    {
                        await context.AddRangeAsync(plan);
                        await context.SaveChangesAsync(ct);
                    }

                }
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message);
            }

        }

        public static List<T> LoadDataFromJsonFile<T>(string folderePath, string fileName)
        {
                var filePath = Path.Combine(folderePath, fileName);
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"seed data file not found : {filePath}");

                var data = File.ReadAllText(filePath);
                var option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<List<T>>(data, option) ?? [];
            return result;

            }


    }
}
