using Newtonsoft.Json;

namespace Delfi.Glo.Common.Services
{
    // add common services here in this folder
    public static class UtilityService
    {
        public static T? Read<T>(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public static async Task<T?> ReadAsync<T>(string filePath)
        {
            string text = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<T>(text);
        }
        public static bool Write<T>(List<T> list, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
            return true;
        }
        public static async Task<bool> WriteAsync<T>(List<T> list, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(list, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, jsonData);
            return true;
        }
        public static IEnumerable<TEntity> SortAndPagination<TEntity>(string SortColumn,string SortDirection,int PageNumber,int PageSize, IQueryable<TEntity> JsonData) where TEntity : class
        {
            var jsonResult = JsonData;
            if (SortColumn != null && SortColumn != "" && SortDirection != null && SortDirection != "")
            {
                if (SortDirection == "asc")
                {
                    jsonResult = jsonResult.AsQueryable().OrderBy(SortColumn);
                }
                else
                {
                    jsonResult = jsonResult.AsQueryable().OrderByDescending(SortColumn);
                }
            }

            var finalResults = jsonResult.Skip((PageNumber - 1) * PageSize)
                                       .Take(PageSize)
                                       .ToList();
            return finalResults;
        }
    }

}
