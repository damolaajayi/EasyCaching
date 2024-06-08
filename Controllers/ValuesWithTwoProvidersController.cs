using EasyCaching.API;
using EasyCaching.Core;
using EasyCaching.Dtos;
using EasyCaching.Interfaces;
using EasyCaching.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EasyCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesWithTwoProvidersController(IEasyCachingProviderFactory _factory, IValidator<Student> _student, IStudentService _studentService) : Controller
    {
        private ApiResponse _response = new();
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetValues()
        {
            var sqliteProvider = _factory.GetCachingProvider("SQLiteCache");
            var prizeDto = new PrizeDto();
            var stopwatch = Stopwatch.StartNew();
            //_student.ValidateAsync(St);
            //SQLite
            if(await sqliteProvider.ExistsAsync("prizes"))
            {
                var cachedPrizes = await sqliteProvider.GetAsync<List<WinePrize>>("prizes");
                prizeDto.Prizes = cachedPrizes.Value;
            }
            else
            {
                await Task.Delay(50);
                var prizes = Data.GetWinePrizes();
                prizeDto.Prizes = prizes;
                await sqliteProvider.SetAsync("prizes", prizes, TimeSpan.FromMinutes(1));

            }
            stopwatch.Stop();

            _response.Result = prizeDto;
            _response.Duration = stopwatch.ElapsedMilliseconds;
            _response.StatusCode = System.Net.HttpStatusCode.OK;

            return Ok(_response);
        }
        //[ApiController]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var articles = await _studentService.GetStudents();
            return Ok(articles);
            //return await _studentService.GetStudents();
            //var resp = await _student.ValidateAsync(student);
        }
    }
}
