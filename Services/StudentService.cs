using EasyCaching.Interfaces;
using EasyCaching.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace EasyCaching.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private const string CacheKey = "services";
        public StudentService(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }
        public async Task<List<Student>> GetStudents()
        {
            if(!_memoryCache.TryGetValue(CacheKey, out List<Student>? students))
            {
                //students = await GetValuesFromDbAsync();
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                    Size = 1024
                };

                _memoryCache.Set(CacheKey, students, cacheExpiryOptions);
            }
            return students ?? new List<Student>();
            
        }
        public async Task<List<Student>> GetStudentss()
        {
            var cachedArticles = await _distributedCache.GetStringAsync(CacheKey);
            if (cachedArticles != null) 
            { 
                return JsonConvert.DeserializeObject<List<Student>>(
                    cachedArticles) ?? new List<Student>();
                }

            var students = await GetValuesFromDbAsync();
            if(students.Any())
            {
                await SetArticles(students);
                return students;

            }
        }

        private async Task SetArticles(List<Student> students)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
            };
            await _distributedCache.SetStringAsync(CacheKey, JsonConvert.SerializeObject(students), cacheOptions);
        }

    }
}
