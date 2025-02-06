using JwtApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
       public class City { 
            public int cityID {  get; set; }
            public string cityName { get; set; }
        }    
        
        [HttpGet]  //api/City/
        public async Task<JsonResult> Get()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userID = int.Parse(UserId);

            var newCity = new City() { 
            cityID = userID,
            cityName   ="my city name",
            };   
            return new JsonResult(new { newCity });

           // return new JsonResult(new { WarReports, map = Map, userId = UserId, city = UserCity, userItems = UserItems, userResearch = userResearch });
        }
    }
}
