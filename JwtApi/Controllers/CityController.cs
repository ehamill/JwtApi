using Azure.Core;
using JwtApi.Data;
using JwtApi.Entities;
using JwtApi.Models;
using JwtApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static JwtApi.Controllers.AuthController;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JwtApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CityController(IGameService gameService, UserDbContext db) : ControllerBase
    {
        //private readonly UserDbContext db;  tightly coupled..change later

        //public CityController(UserDbContext _db)
        //{
        //    db = _db;
        //}

        [HttpGet]  //City/
        public async Task<JsonResult> Get()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            //UserId = "c51e59c1-08e0-4c38-b2c7-e81454f8b04f";

            var UserCity = new City(); // await gameService.GetCity(UserId);
            var UserItems = new UserItems();
            var userResearch = new UserResearch();
            var WarReports = new List<WarReport>();
            var Map = new List<City>();

            try
            {
                Map = await db.Cities.ToListAsync();
                UserCity = await db.Cities
                    .Include(c => c.Buildings)
                    .Include(c => c.Heros).Include(c => c.TroopQueues)
                    .Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateCity(UserId, Map);

                //delete later
                if (UserCity.Buildings.FirstOrDefault(c => c.Location == 34) == null) {
                    // locations 34 to 74 are farms
                    for (int i = 34; i <= 73; i++)  // 1-2
                    {
                        Building NewBuilding = new Building()
                        {
                            CityId = UserCity.CityId,
                            Location = i,
                            BuildingType = BuildingType.Empty,
                            Level = 0,
                            Image = "emptyCitySlot.jpg"
                        };
                        UserCity.Buildings.Add(NewBuilding);
                        db.Buildings.Add(NewBuilding);
                    }
                    db.SaveChanges();
                }
                

                //WarReports = await GetWarReports();
                ////battles = await db.Battles.Where(c => c.AtkCityId == UserCity.CityId || c.DefCityId == UserCity.CityId).ToListAsync();
                ////UserCity.FutureCityData = await db.FutureCityData.Where(c => c.CityId == UserCity.CityId).OrderByDescending(c => c.DateTime).ToListAsync();
                //UserItems = await db.UserItems.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateUserItems(UserId);
                userResearch = await db.UserResearch.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateUserResearch(UserId);
                //await ReplenishHeros(UserCity);//Replensh Free heros

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, [GET]: " + ex.Message);
                Console.WriteLine(ex.Message);
            }

            //await UpdateResources(UserCity);

            if (UserCity.Builder1Busy)
            {
                await CheckBuilder1(UserCity);
            }
            
            if (UserCity.ResearchBusy)
            {
                await CheckResearcher(UserCity, userResearch);
            }

            UserCity.ListOfBuildingsCost = GetNewBuildingsCost(UserCity, userResearch);
            UserCity.Troops = await db.Troops.Where(c => c.CityId == UserCity.CityId).ToListAsync();
            //delete later.
            if (UserCity.Troops.FirstOrDefault(c => c.MarchType == (int)MarchType.Home) == null) { 
                var newTroop = new Troop() { 
                    CityId = UserCity.CityId,
                    MarchType = (int)MarchType.Home,
                    DateTime = DateTime.UtcNow,
                };
                db.Troops.Add(newTroop);
                db.SaveChanges();
                UserCity.Troops.Add(newTroop);
            }
            //UserCity.ResearchCost = GetResearchCosts(UserCity, userResearch);
            //await CheckTroopQueues(UserCity);
            UserCity.TroopProperties = GetDefaultTroops(UserCity, userResearch);

            //If done, add troops to city... delete queue?? Status..training-complete-cancelled
            //if not...

            //GetUpgradeBuildings..only need one for each, can calculate costs off of it
            //Sleep doesn't work..

            // return new JsonResult(new { WarReports, map = Map, userId = UserId, city = UserCity, userItems = UserItems, userResearch = userResearch });

            return new JsonResult( new { city = UserCity, message="testing" });  //return Ok(retObj);

           // return new JsonResult(new { WarReports, map = Map, userId = UserId, city = UserCity, userItems = UserItems, userResearch = userResearch });
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<loginVM>> Login(UserDto request)
        [HttpPost("UpdateCity")]
        public async Task<JsonResult> UpdateCity(UpdateCityModel update)
        {
            //change UpdateCityModel to all fields
            //var update = new updateCityModel() { 

            //};
            string message = CheckForUpdateErrors(update); //not necessary..checks update model for errors

            if (message != "ok")
            {
                return new JsonResult(new { message = message });
            }

            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            //int userID = int.Parse(UserId);
            //UserId = "c51e59c1-08e0-4c38-b2c7-e81454f8b04f";
            //string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var u = await _userManager.FindByIdAsync(UserId);

            var UserCity = new City();

            try
            {
                var userCities = await db.Cities.Where(c => c.UserId == UserId).ToListAsync(); //ANd server id == update.serverID
                UserCity = userCities.Where(c => c.CityId == update.CityId).FirstOrDefault();
                //if UserCity IS null  post error message. should never happen..
                UserCity.Buildings = await db.Buildings.Where(c => c.CityId == UserCity.CityId).ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, UpdateCity: getting user's city " + ex.Message);
                Console.WriteLine(ex.Message);
            }



            UserResearch UserResearch = await db.UserResearch.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? new UserResearch();

            //Check if builders busy ..
            await CheckBuilder1(UserCity);
            if (UserCity.Builder1Busy)
            {
                message = "Can only build one building at a time.";
                return new JsonResult(new { message });
            }
            string TestingResult = CheckIfBuildingPreReqMet(UserCity, update);
            if (TestingResult != "ok")
            {
                message = TestingResult;
                return new JsonResult(new { message = message });
            }

            BuildingCost BuildingCost = GetUpgradeCostOfBuilding(update, UserCity, UserResearch);

            //Check if user has enough resources ....send error message if somehow 

            await StartConstruction(UserCity, update, BuildingCost);

            //await ReloadCity(UserCity.CityId, UserId);

            try
            {
                // UserCity.CityData = await db.CityData.Where(c => c.CityId == UserCity.CityId).OrderByDescending(c => c.DateTime).ToListAsync();
                UserCity.TroopQueues = await db.TroopQueues.Where(c => c.CityId == UserCity.CityId && c.Complete == false).ToListAsync();
                UserCity.Heros = await db.Heros.Where(c => c.CityId == UserCity.CityId).ToListAsync();
                //UserCity.Heros = await ReplenishHeros(UserCity);
                await ReplenishHeros(UserCity);
                UserCity.ListOfBuildingsCost = GetNewBuildingsCost(UserCity, UserResearch);
                UserCity.TroopProperties = GetDefaultTroops(UserCity, UserResearch);
                UserCity.ResearchCost = GetResearchCosts(UserCity, UserResearch);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, UpdateCity: " + ex.Message);
                Console.WriteLine(ex.Message);
            }

            //await UpdateResources(UserCity);


            return new JsonResult(new { message = message, city = UserCity });
        }
        [HttpPost("SpeedUpUsed")]
        public async Task<JsonResult> SpeedUpUsed(SpeedUpModel model)// buildingID, speedupType, UsedOn(builder1, reseach)
        {
            var message = "ok";
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var Building = await db.Buildings.FirstOrDefaultAsync(c => c.BuildingId == model.BuildingId);
            City UserCity = await db.Cities.Where(c => c.CityId == Building.CityId).FirstOrDefaultAsync() ?? new City();
            UserCity.Buildings = await db.Buildings.Where(c => c.CityId == UserCity.CityId).ToListAsync();
            UserResearch UserResearch = await db.UserResearch.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? new UserResearch();
            UserItems UserItems = await db.UserItems.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? new UserItems();

            if (model.UsedOn == (int)BuilderType.Builder1)
            {
                //var beginnerUserResearch = new UserResearch() { Construction = 0};
                //var buildingCosts = GetNewBuildingsCost(UserCity, beginnerUserResearch);
                
                // rally spot to lv1 free -3min
                // cottage/quarty/ironmine to lvl2 free
                //farm/saw to lvl3 free,
                if (model.SpeedUpTypeId == (int)SpeedUpTypes.Free) {
                    if (Building.BuildingType == BuildingType.Rally_Spot) {
                        if (Building.Level < 1) {
                            UserCity.Construction1Ends = UserCity.Construction1Ends.AddMinutes(-3);
                            UserCity.Builder1Time = UserCity.Builder1Time - (3 * 60);
                        }
                    }
                    else if (Building.BuildingType == BuildingType.Cottage || Building.BuildingType == BuildingType.Quarry
                        || Building.BuildingType == BuildingType.Sawmill)
                    {
                        if (Building.Level <= 1)
                        {
                            UserCity.Construction1Ends = UserCity.Construction1Ends.AddMinutes(-3);
                            UserCity.Builder1Time = UserCity.Builder1Time - (3 * 60);
                        }
                    }
                    else if (Building.BuildingType == BuildingType.Farm || Building.BuildingType == BuildingType.Sawmill)
                    {
                        if (Building.Level <= 2)
                        {
                            UserCity.Construction1Ends = UserCity.Construction1Ends.AddMinutes(-3);
                            UserCity.Builder1Time = UserCity.Builder1Time - (3 * 60);
                        }
                    }
                }
                    
                if (model.SpeedUpTypeId == (int)SpeedUpTypes.Beginner_Guidelines)
                {
                    UserCity.Construction1Ends = UserCity.Construction1Ends.AddMinutes(-15);
                    UserCity.Builder1Time = UserCity.Builder1Time - (15 * 60);
                    UserItems.FiveMinuteSpeedups--;
                }
                await db.SaveChangesAsync();
                await CheckBuilder1(UserCity);
            }
            else if (model.UsedOn == (int)BuilderType.Research)
            {
                if (model.SpeedUpTypeId == (int)SpeedUpTypes.Beginner_Guidelines)
                {
                    UserCity.ResearchEnds = UserCity.ResearchEnds.AddMinutes(-15);
                    UserCity.ResearchTime = UserCity.ResearchTime - (15 * 60);
                    UserItems.FiveMinuteSpeedups--;
                }
                await db.SaveChangesAsync();
                await CheckResearcher(UserCity, UserResearch);
            }

            //await db.SaveChangesAsync();

            //UserCity = await ReloadCity(UserCity.CityId, UserId);

            return new JsonResult(new { message = message, city = UserCity, userResearch = UserResearch, userItems = UserItems });
        }


        private async Task<City> ReloadCity(int CityId, string UserId)
        {

            var UserCity = new City();
            try
            {
                UserCity = await db.Cities
                    .Include(c => c.Buildings)
                    .Include(c => c.Heros)
                    .Include(c => c.TroopQueues)
                    .Where(c => c.CityId == CityId).FirstOrDefaultAsync() ?? new City();

                //Check if builders busy ..
                //await CheckBuilder1(UserCity);
                if (UserCity.Builder1Busy)
                {
                    await CheckBuilder1(UserCity);
                }

                UserResearch UserResearch = await db.UserResearch.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? new UserResearch();
                if (UserCity.ResearchBusy)
                {
                    await CheckResearcher(UserCity, UserResearch);
                }
                await CheckTroopQueues(UserCity);
                await ReplenishHeros(UserCity);
                UserCity.ListOfBuildingsCost = GetNewBuildingsCost(UserCity, UserResearch);
                UserCity.TroopProperties = GetDefaultTroops(UserCity, UserResearch);
                UserCity.ResearchCost = GetResearchCosts(UserCity, UserResearch);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, ReloadCity: " + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return UserCity;
        }

        private List<TroopProperties> GetDefaultTroops(City city, UserResearch research)
        {
            var troops = new List<TroopProperties>();

            var Worker = new TroopProperties()
            {
                TroopType = TroopType.Worker,
                Requirement = "Barracks Level 1",
                BarracksLevel = 1,
                RequirementMet = true,
                Description = "Designed for logistics transportation, they barely have any fighting ability.",
                Qty = city.Workers,
                Food = 50,
                Lumber = 150,
                Iron = 10,
                Time = 50, //BaseTime*(0.9^Const)*(0.995^Poli)
                Life = 100,
                Population = 1,
                Attack = 5,
                Defense = 10,
                Load = 200,
                FoodCity = 2, // per hour
                Speed = 180,
                Range = 10,
                Image = "worker.jpg",

            };
            troops.Add(Worker);
            var Warrior = new TroopProperties()
            {
                TroopType = TroopType.Warrior,
                Qty = city.Warriors,
                BarracksLevel = 1,
                Requirement = "Barracks Level 1.",
                RequirementMet = true,
                Description = "The earliest military forces were simply the citizens of the city, armed with whatever implements they could find to use as weapons. Even though these militias made up of warriors were inexpensive, they were no match for an organized army. Warriors were used as a stopgap measure while waiting for better trained and equipped units to be trained or to defend a city that had been cut off from military support. In a crisis these warriors are better then no defence at all.",
                Food = 80,
                Lumber = 100,
                Iron = 50,
                Time = 25,
                Life = 200,
                Population = 1,
                Attack = 50,
                Defense = 50,
                Load = 20,
                FoodCity = 3,
                Speed = 200,
                Range = 20,
                Image = "warrior.jpg",
            };
            troops.Add(Warrior);
            var Scout = new TroopProperties()
            {
                TroopType = TroopType.Scout,
                Qty = city.Scouts,
                BarracksLevel = 2,
                Requirement = "Barracks Level 2",
                RequirementMet = true,
                Description = "Specially trained Scouts, spy for enemies' resources and intelligence.",
                Food = 120,
                Lumber = 200,
                Iron = 150,
                Time = 100,
                Life = 100,
                Population = 1,
                Attack = 20,
                Defense = 20,
                Load = 5,
                FoodCity = 5,
                Speed = 3000,
                Range = 20,
                Image = "scout.jpg",
            };
            troops.Add(Scout);

            var Pike = new TroopProperties()
            {
                TroopType = TroopType.Pikeman,
                Qty = city.Pikemen,
                BarracksLevel = 2,
                Requirement = "Barracks Level 1, Military Tradition Level 1",
                RequirementMet = (research.MilitaryTradition >= 1) ? true : false,
                Description = "Equipped with long pikes, they are effective against horsemen.",
                Food = 150,
                Lumber = 500,
                Iron = 100,
                Time = 150,
                Life = 300,
                Population = 1,
                Attack = 150,
                Defense = 150,
                Load = 40,
                FoodCity = 6,
                Speed = 300,
                Range = 50,
                Image = "pikeman.jpg",
            };
            troops.Add(Pike);
            var Sword = new TroopProperties()
            {
                TroopType = TroopType.Swordsman,
                Qty = city.Swordsmen,
                BarracksLevel = 3,
                RequirementMet = (research.IronWorking >= 1) ? true : false,
                Requirement = "Barracks Level 3, Iron Working Level 1",
                Description = "The strongest melee unit, they are effective against archers.",
                Food = 200,
                Lumber = 150,
                Iron = 400,
                Time = 225,
                Life = 350,
                Population = 1,
                Attack = 100,
                Defense = 250,
                Load = 30,
                FoodCity = 7,
                Speed = 275,
                Range = 30,
                Image = "swordsman.jpg",
            };
            troops.Add(Sword);
            var Arch = new TroopProperties()
            {
                TroopType = TroopType.Archer,
                Qty = city.Archer,
                Requirement = "Barracks Level 4, Archery Level 1",
                RequirementMet = (research.Archery >= 1) ? true : false,
                Description = "Archers are good at performing assaults from long distances. They are effective against nearly all types of infantry.",
                Food = 300,
                Lumber = 350,
                Iron = 300,
                Time = 350,
                Life = 250,
                Population = 2,
                Attack = 120,
                Defense = 50,
                Load = 25,
                FoodCity = 9,
                Speed = 250,
                Range = 1200,
                Image = "archer.jpg",
            };
            troops.Add(Arch);
            var Cav = new TroopProperties()
            {
                TroopType = TroopType.Cavalry,
                Qty = city.Cavalry,
                RequirementMet = (research.HorsebackRiding >= 1) ? true : false,
                Requirement = "Barracks Level 5, Horseback Riding Level 1",
                Description = "Light-armored units that are fast and flexible, they are ideal for performing raids and plunders. On the down side they are expensive and slow to recruit.",
                Food = 1000,
                Lumber = 600,
                Iron = 500,
                Time = 500,
                Life = 500,
                Population = 3,
                Attack = 250,
                Defense = 180,
                Load = 100,
                FoodCity = 18,
                Speed = 1000,
                Range = 100,
                Image = "cavalry.jpg",
            };
            troops.Add(Cav);

            var Cata = new TroopProperties()
            {
                TroopType = TroopType.Cataphract,
                Qty = city.Cataphract,
                Requirement = "Barracks Level 7, Horseback Riding Level 5, Iron Working Level 5",
                RequirementMet = (research.HorsebackRiding >= 5 && research.IronWorking >= 5) ? true : false,
                Description = "Heavily armored cavalry that are both excellent in attack and defense, but are costly.",
                Food = 2000,
                Lumber = 500,
                Iron = 2500,
                Time = 1500,
                Life = 1000,
                Population = 6,
                Attack = 350,
                Defense = 350,
                Load = 80,
                FoodCity = 35,
                Speed = 750,
                Range = 80,
                Image = "cataphract.jpg",
            };
            troops.Add(Cata);
            var Trans = new TroopProperties()
            {
                TroopType = TroopType.Transporter,
                Qty = city.Transporters,
                Requirement = "Barracks Level 6, Logistics Level 1, Metal Casting Level 5",
                RequirementMet = (research.Logistics >= 1 && research.MetalCasting >= 5) ? true : false,
                Description = "Horse carriage enables you to carry great amounts of resources.",
                Food = 600,
                Lumber = 1500,
                Iron = 350,
                Time = 1000,
                Life = 700,
                Population = 4,
                Attack = 10,
                Defense = 60,
                Load = 5000,
                FoodCity = 10,
                Speed = 150,
                Range = 10,
                Image = "transporter.jpg",
                //Load increases w/ logistics lvl0 0% 5k, lvl10 = 100% or 10k
            };
            troops.Add(Trans);
            var Ball = new TroopProperties()
            {
                TroopType = TroopType.Ballista,
                Qty = city.Ballista,
                Requirement = "Barracks Level 9, Archery Level 6, Metal Casting Level 5",
                RequirementMet = (research.Archery >= 6 && research.MetalCasting >= 5) ? true : false,
                Description = "Ballistae have a long shooting range and are effective against other siege weapons. They are very effective against fortifications as they out-range Archer Towers. Currently, they can capture Cities on their own without any supporting troops. Balanced against this is the amount of resources required to build them and the very slow rate of training.",
                Food = 2500,
                Lumber = 3000,
                Iron = 1800,
                Time = 3000,
                Life = 320,
                Population = 5,
                Attack = 450,
                Defense = 160,
                Load = 35,
                FoodCity = 50,
                Speed = 100,
                Range = 1400,
                Image = "ballista.jpg",
            };
            troops.Add(Ball);
            var Ram = new TroopProperties()
            {
                TroopType = TroopType.Battering_Ram,
                Qty = city.Battering_Ram,
                Requirement = "Barracks Level 9, Iron Working Level 8, Metal Casting Level 7",
                RequirementMet = (research.IronWorking >= 8 && research.MetalCasting >= 7) ? true : false,
                Description = "Battering Ram is a heavy support weapon, which used for overcoming opponent's fortification.",
                Food = 4000,
                Lumber = 6000,
                Iron = 1500,
                Time = 4500,
                Life = 5000,
                Population = 10,
                Attack = 250,
                Defense = 160,
                Load = 45,
                FoodCity = 50,
                Speed = 120,
                Range = 600,
                Image = "batteringRam.jpg",
            };
            troops.Add(Ram);
            var Pult = new TroopProperties()
            {
                TroopType = TroopType.Catapult,
                Qty = city.Catapult,
                Requirement = "Barracks Level 10, Archery Level 10, Metal Casting Level 10",
                RequirementMet = (research.Archery >= 10 && research.MetalCasting >= 10) ? true : false,
                Description = "Catapults throw huge rocks from a long distance; they are most effective in destroying enemy fortifications.",
                Food = 5000,
                Lumber = 8000,
                Iron = 5000,
                Time = 6000,
                Life = 480,
                Population = 8,
                Attack = 600,
                Defense = 200,
                Load = 75,
                FoodCity = 250,
                Speed = 80,
                Range = 1500,
                Image = "catapult.jpg",
            };
            troops.Add(Pult);
            var Trap = new TroopProperties()
            {
                TroopType = TroopType.Trap,
                Qty = city.Traps,
                WallsLevel = 1,
                Requirement = "Walls Level 1.",
                RequirementMet = true,
                Description = "Traps are a type of Fortified unit used to kill attacking troops. Like all defenses, it is only effective in huge numbers. The trap is a good defense if your Beacon_Tower gives you an early warning. They are quick and cheap. But the down side to traps are that they are not very effective. ",
                Food = 50,
                Lumber = 500,
                Stone = 100,
                Iron = 50,
                Time = 60,
                Range = 5000,
                WallDefense = true,
                Image = "trap.jpg",
                //# fortified spaces	1
                //wall spaces: lvl1 1000, 3k, 6k, 10k, 15k 21k, 28k, 36,, 45, 55
                //The trap is designed to kill an even number of all infantry,
                //so 100 warriors, 100 swordsmen, and 100 archers were sent to attack,
                //and the wall had 100 traps, then the attackers would lose roughly 33 warriors,
                //swordsmen, and archers.
                //It has also been found that if you send 100 warriors at 100 traps, 
                //then 100 warriors will die.
                //However if you send 200 warriors then only about 90 warriors will die, 300 and less will die. (This is a rough estimate and the actual numbers could greatly vary.)
            };
            troops.Add(Trap);
            var ab = new TroopProperties()
            {
                TroopType = TroopType.Abatis,
                Qty = city.Abatis,
                WallsLevel = 2,
                Requirement = "Walls Level 2",
                RequirementMet = true,
                Description = "An Abatis is a type of fortified unit consisting of wooden barricades with spear points. It is effective only against mounted troops, such as cavalry and cataphract. It has no effect on other units.",
                Food = 100,
                Lumber = 1200,
                Iron = 150,
                Time = 120,
                Range = 5000,
                WallDefense = true,
                Image = "abatis.jpg",
                //spaces = 2, # kills all cav (This is a rough estimate and the actual numbers could greatly vary.)
            };
            troops.Add(ab);
            var RollingLog = new TroopProperties()
            {
                TroopType = TroopType.Rolling_Log,
                Qty = city.Rolling_Log,
                WallsLevel = 5,
                RequirementMet = (research.MetalCasting >= 5) ? true : false,
                Requirement = "Walls Level 5, Metal Casting Level 5",
                Description = "A rollinglog is a type of fortified unit consisting of a large rolling log. These logs weigh a lot, roll downhill quickly, and will crush attacking units to death. They are a one-shot weapon.",
                Food = 300,
                Lumber = 6000,
                Time = 360, //BaseTime*(0.9^Const)*(0.995^Poli)
                Attack = 500,
                Defense = 500,
                Range = 1300,
                Life = 1500,
                WallDefense = true,
                Image = "rollingLog.jpg",
                //    one time use, machinery can help to respawn  
                //# fortified spaces	4
            };
            troops.Add(RollingLog);
            var at = new TroopProperties()
            {
                TroopType = TroopType.Archers_Tower,
                Qty = city.Archers_Tower,

                Requirement = "Walls Level 3, Archery Level 3",
                WallsLevel = 3,
                RequirementMet = (research.Archery >= 3) ? true : false,
                Description = "An archer's tower is a type of fortified unit consisting of a tower built atop the city walls. It provides a vantage point for archers to shoot enemies. The height (level)of the wall determines the shooting range and thus the effectiveness. The actual number of archer units in the city has no effect on the performance of archer's towers.",
                Food = 200,
                Lumber = 2000,
                Stone = 1500,
                Iron = 500,
                Time = 180, //BaseTime*(0.9^Const)*(0.995^Poli)
                Attack = 300,
                Defense = 360,
                Life = 2000,
                Range = 1300,
                WallDefense = true,
                Image = "archerTower.jpg",
                //Archer Tower Range = (( 1 + (WallLvl+Archery)*0.05))*1300
                //# fortified spaces	3
            };
            troops.Add(at);
            var dt = new TroopProperties()
            {
                TroopType = TroopType.Defensive_Trebuchet, //one shot wonder, kills highest Attck seige weapon: cata, balls, battram
                Qty = city.Defensive_Trebuchet,
                Requirement = "Walls Level 7, Metal Casting Level 6",
                WallsLevel = 7,
                RequirementMet = (research.MetalCasting >= 6) ? true : false,
                Description = "A defensive trebuchet, formerly known as a Rock Fall, is a type of fortified unit consisting of a mechanical rock sling. Due to the long time and effort it takes to reset and reload these units, they can only fire once in an attack and must be rebuilt to be used again.",
                Food = 600,
                Stone = 8000,
                Time = 600, //BaseTime*(0.9^Const)*(0.995^Poli)
                Attack = 800,
                Defense = 800,
                Life = 1000,
                Range = 5200,
                WallDefense = true,
                Image = "dt.jpg",
                //Has to be max range of battlefield to get to all seige weapons
                //    one time use, machinery can help to respawn  
                //# fortified spaces	5
            };
            troops.Add(dt);

            return troops;
        }
        private List<Research> GetResearchCosts(City city, UserResearch research)
        {

            var researchList = new List<Research>();

            var farms = city.Buildings.Where(c => c.BuildingType == BuildingType.Farm).ToList();

            int highestBuilding = GetHighestBuilding(city, BuildingType.Farm) + 1;
            int levelNeeded = (research.Agriculture + 1 < highestBuilding) ? research.Agriculture + 1 : highestBuilding;
            var agg = new Research()
            {
                ResearchType = ResearchType.Agriculture,
                Requires = "Academy Level 1, Farm Level " + levelNeeded.ToString(),//Cannot go above farm level
                MinAcademyLevel = 1,
                BuildingTypeRequired = BuildingType.Farm, // Farm /lumber/iron/quary
                Food = 500,
                Stone = 0,
                Lumber = 0,
                Iron = 0,
                Gold = 1000,
                Time = 6 * 60 + 40,
                ProductionIncreasePercent = 10,//10* lvl === lvl1 = 10%, lvl2 20% ...lvl10 is 100%
                RequirementsMet = false,
                Description = "Agriculture is the science of cultivating soil, planting and harvesting crops. ",
            };
            researchList.Add(agg);

            highestBuilding = GetHighestBuilding(city, BuildingType.Sawmill) + 1;
            levelNeeded = (research.Lumbering + 1 < highestBuilding) ? research.Lumbering + 1 : highestBuilding;
            var lumbering = new Research()
            {
                ResearchType = ResearchType.Lumbering,
                Requires = "Academy Level 1, Sawmill Level " + levelNeeded.ToString(),//Cannot go above farm level
                MinAcademyLevel = 1,
                BuildingTypeRequired = BuildingType.Sawmill,
                //MinFarmLevel = 0, //FarmTypeRequired = Farm/lumber/iron
                //Food = 0,
                //Stone = 0,
                Lumber = 500,
                Iron = 100,
                Gold = 1200,
                Time = 8 * 60 + 20,
                ProductionIncreasePercent = 10,//10* lvl === lvl1 = 10%, lvl2 20% ...lvl10 is 100%,
                RequirementsMet = false,
                Description = "Lumbering concerns the logistics of moving wood from the stump to somewhere outside the forest, usually a sawmill or a lumber yard. ",
            };
            researchList.Add(lumbering);

            highestBuilding = GetHighestBuilding(city, BuildingType.Quarry) + 1;
            levelNeeded = (research.Masonry + 1 < highestBuilding) ? research.Masonry + 1 : highestBuilding;
            var masonry = new Research()
            {
                ResearchType = ResearchType.Masonry,
                Requires = "Academy Level 2, Quarry Level " + levelNeeded.ToString(),//Cannot go above farm level
                MinAcademyLevel = 2,
                BuildingTypeRequired = BuildingType.Quarry,
                //MinFarmLevel = 0, //FarmTypeRequired = Farm/lumber/iron
                //Food = 0,
                Stone = 500,
                //Lumber = 500,
                Iron = 200,
                Gold = 1500,
                Time = 10 * 60,
                ProductionIncreasePercent = 10,//10* lvl === lvl1 = 10%, lvl2 20% ...lvl10 is 100%,
                RequirementsMet = false,
                Description = "Masonry concerns the logistics of quarrying stone. ",
            };
            researchList.Add(masonry);

            highestBuilding = GetHighestBuilding(city, BuildingType.Iron_Mine) + 1;
            levelNeeded = (research.Mining + 1 < highestBuilding) ? research.Mining + 1 : highestBuilding;
            var mining = new Research()
            {
                ResearchType = ResearchType.Mining,
                Requires = "Academy Level 2, Masonry Level 1, Iron Mine Level " + levelNeeded.ToString(),//Cannot go above farm level
                MinAcademyLevel = 2,
                BuildingTypeRequired = BuildingType.Iron_Mine,
                //MinFarmLevel = 0, //FarmTypeRequired = Farm/lumber/iron
                //Food = 0,
                //Stone = 500,
                //Lumber = 500,
                Iron = 800,
                Gold = 2000,
                Time = 11 * 60 + 40,
                ProductionIncreasePercent = 10,//10* lvl === lvl1 = 10%, lvl2 20% ...lvl10 is 100%,
                RequirementsMet = false,
                Description = "Mining is the science of extracting and molding iron. ",
            };
            researchList.Add(mining);

            var mc = new Research()
            {
                ResearchType = ResearchType.Metal_Casting,
                Requires = "Academy Level 3, Mining Level 2",//Cannot go above farm level
                MinAcademyLevel = 3,
                BuildingTypeRequired = BuildingType.Empty,
                //MinFarmLevel = 2, //FarmTypeRequired = Farm/lumber/iron
                //Food = 0,
                //Stone = 500,
                Lumber = 500,
                Iron = 500,
                Gold = 5000,
                Time = 15 * 60,
                ProductionIncreasePercent = 10,//10* lvl === lvl1 = 10%, lvl2 20% ...lvl10 is 100%,
                RequirementsMet = false,
                Description = "Metal Casting is the science of the speed of mechanics. ",
            };
            researchList.Add(mc);

            var info = new Research()
            {
                ResearchType = ResearchType.Informatics,
                Requires = "Academy Level 3, Mining Level 2",//Cannot go above farm level
                MinAcademyLevel = 3,
                BuildingTypeRequired = BuildingType.Empty,
                //MinFarmLevel = 2, //FarmTypeRequired = Farm/lumber/iron
                //Food = 300,
                //Stone = 500,
                //Lumber = 500,
                //Iron = 500,
                Gold = 2000,
                Time = 5 * 60,
                ProductionIncreasePercent = 10,//10* lvl === lvl1 = 10%, lvl2 20% ...lvl10 is 100%,
                RequirementsMet = false,
                Description = "Each level of Informatics increases the amount of information you can gain about your enemies. ",
            };
            researchList.Add(info);

            highestBuilding = GetHighestBuilding(city, BuildingType.Forge) + 1;
            levelNeeded = (research.MilitaryScience + 1 < highestBuilding) ? research.MilitaryScience + 1 : highestBuilding;
            var ms = new Research()
            {
                ResearchType = ResearchType.Military_Science,
                Requires = "Academy Level 1, Forge Level " + levelNeeded.ToString(),//Cannot go above forge level
                MinAcademyLevel = 1,
                BuildingTypeRequired = BuildingType.Forge,
                //MinFarmLevel = 2, //FarmTypeRequired = Farm/lumber/iron
                Food = 600,
                Stone = 100,
                Lumber = 100,
                Iron = 150,
                Gold = 2500,
                Time = 15 * 60,
                ProductionIncreasePercent = 10,//Each level 10,19,27,34,41,47,52,57,61,65 lvl10 65%
                RequirementsMet = false,
                Description = "Military Science enhances army training speed. ",
            };
            researchList.Add(ms);

            var mt = new Research()
            {
                ResearchType = ResearchType.Military_Tradition,
                Requires = "Academy Level 2, Military Science Level 1 ",
                MinAcademyLevel = 2,
                BuildingTypeRequired = BuildingType.Empty,
                Food = 800,
                Stone = 0,
                Lumber = 120,
                Iron = 200,
                Gold = 3000,
                Time = 20 * 60,
                ProductionIncreasePercent = 5,//Each level of Military Tradition enhances army attack by 5%. lvl10 50%
                RequirementsMet = false,
                Description = "Military Tradition enhances army attack methods. ",
            };
            researchList.Add(mt);

            var iw = new Research()
            {
                ResearchType = ResearchType.Iron_Working,
                Requires = "Academy Level 3",
                MinAcademyLevel = 3,
                BuildingTypeRequired = BuildingType.Empty,
                Food = 700,
                Stone = 0,
                Lumber = 120,
                Iron = 300,
                Gold = 3500,
                Time = 25 * 60,
                ProductionIncreasePercent = 5,//Each level of Military Tradition enhances army attack by 5%. lvl10 50%
                RequirementsMet = false,
                Description = "Iron Working science of working iron into various weapons and Armour. ",
            };
            researchList.Add(iw);

            var logistics = new Research()
            {
                ResearchType = ResearchType.Logistics,
                Requires = "Academy Level 4 ",
                MinAcademyLevel = 4,
                BuildingTypeRequired = BuildingType.Empty,
                Food = 500,
                Stone = 200,
                //Lumber = 120,
                //Iron = 300,
                Gold = 3000,
                Time = 26 * 60 + 40,
                ProductionIncreasePercent = 10,//Each level enhances  by 10%. lvl10 100%
                RequirementsMet = false,
                Description = " Logistics involve the integration of information, transportation, inventory, warehousing, material-handling, and packaging.",
            };
            researchList.Add(logistics);

            var compass = new Research()
            {
                ResearchType = ResearchType.Compass,
                Requires = "Academy Level 4, Military Science Level 3",
                MinAcademyLevel = 4,
                BuildingTypeRequired = BuildingType.Empty,
                Food = 600,
                //Stone = 200,
                //Lumber = 120,
                //Iron = 300,
                Gold = 3000,
                Time = 30 * 60,
                ProductionIncreasePercent = 10,//Each level enhances  by 10%. lvl10 100%
                RequirementsMet = false,
                Description = "The science of troop speed.",
            };
            researchList.Add(compass);

            highestBuilding = GetHighestBuilding(city, BuildingType.Stable) + 1;
            levelNeeded = (research.HorsebackRiding + 1 < highestBuilding) ? research.HorsebackRiding + 1 : highestBuilding;
            var hbr = new Research()
            {
                ResearchType = ResearchType.Horseback_Riding,
                Requires = "Academy Level 5, Military Science Level 5, Stable Level " + levelNeeded.ToString(),//cannot go above stbl lvl
                MinAcademyLevel = 5,
                BuildingTypeRequired = BuildingType.Stable,
                Food = 1000,
                //Stone = 200,
                //Lumber = 120,
                //Iron = 300,
                Gold = 6000,
                Time = 33 * 60 + 20,
                ProductionIncreasePercent = 5,//Each level enhances  by 5%. lvl10 50%
                RequirementsMet = false,
                Description = "The science of Horseback speed.",
            };
            researchList.Add(hbr);

            var arch = new Research()
            {
                ResearchType = ResearchType.Archery,
                Requires = "Academy Level 4, Military Science Level 4",
                MinAcademyLevel = 4,
                //Food = 1000,
                Lumber = 800,
                Stone = 500,
                Iron = 600,
                Gold = 5000,
                Time = 40 * 60,
                ProductionIncreasePercent = 5,//Each level enhances  by 5%. lvl10 50%
                RequirementsMet = false,
                Description = "Increase army archery skill.",
            };
            researchList.Add(arch);

            highestBuilding = GetHighestBuilding(city, BuildingType.Warehouse) + 1;
            levelNeeded = (research.Stockpile + 1 < highestBuilding) ? research.Stockpile + 1 : highestBuilding;
            var stock = new Research()
            {
                ResearchType = ResearchType.Stockpile,
                Requires = "Academy Level 6, Lumbering Level 3, Warehouse Level " + levelNeeded.ToString(),//cannot go above warehouse lvl 
                MinAcademyLevel = 6,
                BuildingTypeRequired = BuildingType.Warehouse,
                //Food = 1200,
                Lumber = 1200,
                Stone = 1000,
                Iron = 800,
                Gold = 2000,
                Time = 15 * 60,
                ProductionIncreasePercent = 10,//Each level enhances  by 5%. lvl10 100%
                RequirementsMet = false,
                Description = "Increase your warehouse capacity.",
            };
            researchList.Add(stock);

            var medicine = new Research()
            {
                ResearchType = ResearchType.Medicine,
                Requires = "Academy Level 6, Logistics Level 3",//cannot go above warehouse lvl 
                MinAcademyLevel = 6,
                BuildingTypeRequired = BuildingType.Empty,
                Food = 1500,
                //Lumber = 1200,
                //Stone = 1000,
                //Iron = 800,
                Gold = 3600,
                Time = 30 * 60,
                ProductionIncreasePercent = 5,//Each level enhances  by 5%. lvl10 50%
                RequirementsMet = false,
                Description = "Better armor for yuor troops.",
            };
            researchList.Add(medicine);

            var construction = new Research()
            {
                ResearchType = ResearchType.Construction,
                Requires = "Academy Level 5, Lumbering Level 5, Metal Casting Level 2",
                MinAcademyLevel = 5,
                BuildingTypeRequired = BuildingType.Empty,
                //Food = 1500,
                Lumber = 2000,
                Stone = 2000,
                Iron = 2000,
                Gold = 5000,
                Time = 25 * 60 + 40,
                ProductionIncreasePercent = 10,//Each level enhances  by 10%. lvl10 100%
                RequirementsMet = false,
                Description = "Train your construction workers.",
            };
            researchList.Add(construction);
            //            The Following Formula can be used to determine the effect of your construction level

            //[Base Building Time]*(0.9) ^[Construction Level]

            //Example: 60 * (0.9) ^ 10 = 21.0 seconds

            //1   10 % 10.0 % 54.0 seconds
            //2   20 % 19.0 % 48.6 seconds
            //3   30 % 27.1 % 43.7 seconds
            //4   40 % 34.4 % 39.4 seconds
            //5   50 % 41.0 % 35.4 seconds
            //6   60 % 46.9 % 31.8 seconds
            //7   70 % 52.2 % 28.7 seconds
            //8   80 % 57.0 % 25.8 seconds
            //9   90 % 61.3 % 23.2 seconds
            //10  100 % 65.1 % 21.0 seconds


            var engineering = new Research()
            {
                ResearchType = ResearchType.Engineering,
                Requires = "Academy Level 8, Construction Level 3",
                MinAcademyLevel = 8,
                BuildingTypeRequired = BuildingType.Empty,
                //Food = 1500,
                Lumber = 2500,
                Stone = 3000,
                Iron = 1500,
                Gold = 6000,
                Time = 35 * 60,
                ProductionIncreasePercent = 10,//Each level enhances  by 10%. lvl10 100%
                RequirementsMet = false,
                Description = "Improves the endurance of Walls and fortified units.",
            };
            researchList.Add(engineering);


            var machinery = new Research()
            {
                ResearchType = ResearchType.Machinery,
                Requires = "Academy Level 8, Construction Level 3",
                MinAcademyLevel = 8,
                BuildingTypeRequired = BuildingType.Empty,
                //Food = 1500,
                Lumber = 600,
                Stone = 700,
                Iron = 800,
                Gold = 5500,
                Time = 40 * 60,
                ProductionIncreasePercent = 2,//+1 every level up to 2x, 3x, 4x, ...., 11x
                RequirementsMet = false,
                Description = "Increases repairable rate of fortified units.",
            };
            researchList.Add(machinery);

            // If an attack would reduce the remaining fortified units of that type to zero, then the base rate is reduced to 0 %, and no units of that type will regenerate at all.
            //Unit Base Repair Rate
            //Trap    5 %
            //Abatis  5 %
            //Archer's Tower	?*
            //Rollinglog  7 %
            //Defensive Trebuchet 8 %
            //*Towers always incur 100 % loss in a losing battle


            var privateering = new Research()
            {
                ResearchType = ResearchType.Privateering,
                Requires = "Academy Level 10, Informatics Level 5, Military Science Level 8",
                MinAcademyLevel = 10,
                BuildingTypeRequired = BuildingType.Empty,
                Food = 5000,
                Iron = 2000,
                Gold = 10000,
                Time = 60 * 60,
                ProductionIncreasePercent = 3,//3% lvl1 , to 30% lvl10
                RequirementsMet = false,
                Description = "Increases security of your warehouses.",
            };
            researchList.Add(privateering);

            return researchList;
        }
        private int GetHighestBuilding(City city, BuildingType b)
        {
            int highest = 0;
            foreach (var building in city.Buildings.Where(c => c.BuildingType == b))
            {
                if (building.Level > highest)
                    highest = building.Level;
            }
            return highest;
        }
        private async Task ReplenishHeros(City userCity)
        {
            if (userCity.Heros == null)
                userCity.Heros = new List<Hero>();

            var cityHeros = userCity.Heros;

            bool updateDb = false;
            foreach (var hero in cityHeros)
            {
                DateTime disposeHeroDateTime = DateTime.UtcNow.AddMinutes(-5420);
                if (!hero.IsHired && hero.Created < disposeHeroDateTime)
                {
                    updateDb = true;
                    db.Heros.Remove(hero);
                }
            }
            try
            {
                if (updateDb)
                {
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var m = e.InnerException.Message;
                System.Diagnostics.Debug.WriteLine("error ", m);
            }

            var Inn = userCity.Buildings.Where(c => c.BuildingType == BuildingType.Inn).FirstOrDefault();
            int InnLevel = (Inn == null) ? 0 : Inn.Level;

            if (cityHeros.Where(c => c.IsHired == false).Count() < 5)
            {
                int Qty = 5 - cityHeros.Where(c => c.IsHired == false).Count();
                List<Hero> newHeros = await CreateHeros(Qty, InnLevel, userCity.CityId);
                foreach (var hero in newHeros)
                {
                    cityHeros.Add(hero);
                }
            }
            //System.Diagnostics.Debug.WriteLine("hero" + i + ": Pol:" + PolPoints + " Attck: " + AttkPoints + " intel: " + IntelPoints);

            //GetHeroPercentages(NewHeros);

            //return cityHeros;

        }
        //private async Task<List<Hero>> CreateHeros(int Qty, int CityId)
        //{
        //    List<Hero> NewHeros = new List<Hero>();
        //    //System.Diagnostics.Debug.WriteLine("hero" + i + ": Pol:" + PolPoints + " Attck: " + AttkPoints + " intel: " + IntelPoints);
        //    var HeroNames = new List<string>() {
        //        "Zion", "Davis", "April", "Fritz", "Aarav", "Gates", "Valentino", "Shannon", "Kaya", "Cook", "Jadiel", "Humphrey", "Bria", "Brennan", "Maya", "Leblanc", "Corbin", "Hood", "Yaretzi", "Townsend", "Keira", "Warner", "Broderick", "Landry", "Malakai", "Grant", "Ryan", "Small", "Hayden", "Cole", "Katrina", "Conner", "Caitlyn", "Wells", "Edith", "Barker", "Ivy", "Marquez", "Alexander", "Harvey", "Brynn", "Mcdaniel", "Jarrett", "Olson", "Alayna", "Colon", "Regan", "Fox", "Julio", "Walker", "Sierra", "Elliott", "Janet", "Shelton", "Tess", "Willis"
        //    };

        //    Random random = new Random();
        //    for (int i = 0; i < Qty; i++)
        //    {
        //        double rand = random.NextDouble();
        //        int PolPoints = (rand < 0.3) ? random.Next(3, 70) : random.Next(3, 50);
        //        int IntelPoints = (rand < 0.2) ? random.Next(3, 70) : random.Next(3, 50);
        //        int AttkPoints = (rand > 0.9) ? random.Next(3, 70) : random.Next(3, 50);
        //        Hero NewHero = new Hero();
        //        NewHero.CityId = CityId;
        //        NewHero.Politics = PolPoints;
        //        NewHero.Intelligence = IntelPoints;
        //        NewHero.Attack = AttkPoints;
        //        NewHero.Level = random.Next(1, 10); ////Adjust Hero Level by Inn level
        //        NewHero.Name = HeroNames[random.Next(0, HeroNames.Count())];

        //        NewHeros.Add(NewHero);
        //        //db.Heros.Add(NewHero);
        //    }
        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        var m = e.InnerException.Message;
        //        System.Diagnostics.Debug.WriteLine("error ", m);
        //    }

        //    return NewHeros;

        //}
        private async Task<List<Hero>> CreateHeros(int Qty, int InnLevel, int CityId)
        {    //Queen King lvl1 base stats are at 85
            //base = pol+attk+def-lvl anywhere from 96 to 150 
            //holy water...decreases largest stat by level..thus if highest def, can move those to attck
            //lvl1-2 1-9 -- lvl3-4 lvl10-20 --lvl5-7 20-30 lvl5-7 30-40 lvl10 30-50
            //lvl10  30-50 highest attk/pol/int 120 70+20
            var heros = new List<Hero>();
            var HeroNames = new List<string>() {
                "Zion", "Davis", "April", "Fritz", "Aarav", "Gates", "Valentino", "Shannon", "Kaya", "Cook", "Jadiel", "Humphrey", "Bria", "Brennan", "Maya", "Leblanc", "Corbin", "Hood", "Yaretzi", "Townsend", "Keira", "Warner", "Broderick", "Landry", "Malakai", "Grant", "Ryan", "Small", "Hayden", "Cole", "Katrina", "Conner", "Caitlyn", "Wells", "Edith", "Barker", "Ivy", "Marquez", "Alexander", "Harvey", "Brynn", "Mcdaniel", "Jarrett", "Olson", "Alayna", "Colon", "Regan", "Fox", "Julio", "Walker", "Sierra", "Elliott", "Janet", "Shelton", "Tess", "Willis"
            };

            Random random = new Random();
            for (int i = 0; i < Qty; i++)
            {
                int basePoints = random.Next(96, 150);
                double rand = random.NextDouble(); //tested 50% < .5, 50%> .5, and 10% > .9 

                int heroLevel = random.Next(1, 10);
                int PolPoints = (rand < 0.3) ? random.Next(60, 71) : random.Next(6, 50); //random.Next(6, 71); 
                //int BaseLeft = basePoints - 6 - PolPoints;
                //int IntelMax = (BaseLeft > 71) ? 71 : BaseLeft;
                int IntelPoints = (rand > 0.5 && rand < 0.7) ? random.Next(55, 71) : random.Next(6, 50); //random.Next(6, IntelMax);
                //BaseLeft = basePoints - PolPoints - IntelPoints;
                //int AttckMax = (BaseLeft > 71) ? 71 : BaseLeft;
                int AttkPoints = (rand > 0.9) ? random.Next(50, 71) : random.Next(6, 50);// random.Next(6, AttckMax); 
                if (InnLevel <= 2)
                {
                    heroLevel = random.Next(1, 10);
                }
                else
                if (InnLevel == 3 || InnLevel == 4)
                {
                    heroLevel = random.Next(10, 20);
                }
                else if (InnLevel >= 5 && InnLevel <= 7)
                {
                    heroLevel = random.Next(20, 30);
                }
                else if (InnLevel >= 8 && InnLevel <= 9)
                {
                    heroLevel = random.Next(30, 40);
                }
                else if (InnLevel == 10)
                {
                    heroLevel = random.Next(30, 50);
                }
                if (InnLevel > 3)
                {
                    PolPoints = (rand < 0.3) ? random.Next(60 + heroLevel, 71 + heroLevel) : random.Next(6 + heroLevel, 50 + heroLevel);
                    IntelPoints = (rand > 0.5 && rand < 0.7) ? random.Next(60 + heroLevel, 71 + heroLevel) : random.Next(6 + heroLevel, 50 + heroLevel);                                                            //int AttckMax = (BaseLeft > 71) ? 71 : BaseLeft;
                    AttkPoints = (rand > 0.9) ? random.Next(60 + heroLevel, 71 + heroLevel) : random.Next(6 + heroLevel, 50 + heroLevel);
                }

                Hero NewHero = new Hero()
                {
                    CityId = CityId,
                    Politics = PolPoints,
                    Intelligence = IntelPoints,
                    Attack = AttkPoints,
                    Level = heroLevel, ////Adjust Hero Level by Inn level
                    Name = HeroNames[random.Next(0, HeroNames.Count())],
                };
                if (AttkPoints > 50 && InnLevel <= 2)
                    NewHero.Level = 1;
                if ((PolPoints > 50 || IntelPoints > 50) && InnLevel <= 2)
                {
                    NewHero.Level = 1;
                }

                await db.Heros.AddAsync(NewHero);
                heros.Add(NewHero);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var m = e.Message; // e.InnerException.Message;
                System.Diagnostics.Debug.WriteLine("error  at create hero " + m);
            }

            return heros;
        }
        private async Task StartConstruction(City UserCity, UpdateCityModel update, BuildingCost BuildingCost)
        {
            BuildingType buildingType = (BuildingType)update.BuildingTypeInt; //GetBuildingType(update.buildingType);

            var b = UserCity.Buildings.Where(c => c.BuildingId == update.BuildingId).FirstOrDefault();

            bool upgrading = (update.Level - b.Level < 0) ? false : true;

            if (upgrading)
            {
                UserCity.Food = UserCity.Food - BuildingCost.Food;
                UserCity.Stone = UserCity.Stone - BuildingCost.Stone;
                UserCity.Wood = UserCity.Wood - BuildingCost.Wood;
                UserCity.Iron = UserCity.Iron - BuildingCost.Iron;
            }
            else
            {
                UserCity.Food = UserCity.Food + BuildingCost.Food;
                UserCity.Stone = UserCity.Stone + BuildingCost.Stone;
                UserCity.Wood = UserCity.Wood + BuildingCost.Wood;
                UserCity.Iron = UserCity.Iron + BuildingCost.Iron;
            }

            UserCity.Construction1Started = DateTime.UtcNow;
            UserCity.Construction1Ends = DateTime.UtcNow.AddSeconds(BuildingCost.Time);
            UserCity.Construction1BuildingId = update.BuildingId;
            UserCity.Construction1BuildingLevel = update.Level;

            UserCity.Builder1Busy = true;
            UserCity.Builder1Time = BuildingCost.Time;

            b.Image = upgrading + buildingType.ToString() + "lvl" + update.Level;
            b.BuildingType = buildingType;
            b.Description = GetBuildingDescription(buildingType);

            UserCity.BuildingWhat = buildingType.ToString();

            await db.SaveChangesAsync();
        }
        private string GetBuildingDescription(BuildingType buildingType)
        {
            switch (buildingType)
            {
                case BuildingType.Academy:
                    return "Academy allows you to research skills.";
                case BuildingType.Barrack:
                    return "Barracks are where you train your troops.";
                case BuildingType.Cottage:
                    return "Cottages increase your cities population allowing you to make more resoures and train more " +
                         "troops";
                case BuildingType.Feasting_Hall:
                    return "Manage your Heros.";
                case BuildingType.Inn:
                    return "The Inn is where you hire heros. The higher the level, the more hero's to choose from"
                        + " and the higher level of hero.";
                case BuildingType.Rally_Spot:
                    return "Place to heal troops. Test fighting. Limits amount of troops you can send at one time.";
                case BuildingType.Town_Hall:
                    return "Manage your city.";
                case BuildingType.Walls:
                    return "Manage your city's defenses.";

                default:
                    return "Empty";
            }

        }
        private BuildingCost GetUpgradeCostOfBuilding(UpdateCityModel update, City userCity, UserResearch userResearch)
        {
            var building = userCity.Buildings.Where(c => c.BuildingId == update.BuildingId).FirstOrDefault();
            //if downgrading, time is current level.
            var level = update.Level;
            if (building.Level > update.Level)
            {
                level++;
            }
            BuildingCost bc = new BuildingCost();

            //BuildingType BuildingType = GetBuildingType(update.buildingType);
            //int buildingTypeInt = update.buildingTypeInt;
            switch ((BuildingType)update.BuildingTypeInt)
            {
                case BuildingType.Academy:
                    bc.TypeString = BuildingType.Academy.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Academy;
                    bc.Food = Constants.AcademyFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.AcademyStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.AcademyWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.AcademyIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.AcademyTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;

                case BuildingType.Barrack:
                    bc.TypeString = BuildingType.Barrack.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Barrack;
                    bc.Food = Constants.BarrFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.BarrStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.BarrWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.BarrIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.BarrTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Beacon_Tower:
                    bc.TypeString = BuildingType.Beacon_Tower.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Beacon_Tower;
                    bc.Food = Constants.BeaconFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.BeaconStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.BeaconWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.BeaconIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.BeaconTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Cottage:
                    bc.TypeString = BuildingType.Cottage.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Cottage;
                    bc.Food = Constants.CottFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.CottStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.CottWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.CottIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.CottTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Feasting_Hall:
                    bc.TypeString = BuildingType.Feasting_Hall.ToString().Replace("_", " ");
                    bc.BuildingTypeInt = (int)BuildingType.Feasting_Hall;
                    bc.Food = Constants.FeastFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.FeastStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.FeastWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.FeastIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.FeastTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Forge:
                    bc.TypeString = BuildingType.Forge.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Forge;
                    bc.Food = Constants.ForgeFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.ForgeStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.ForgeWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.ForgeIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.ForgeTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Inn:
                    bc.TypeString = BuildingType.Inn.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Inn;
                    bc.Food = Constants.InnFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.InnStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.InnWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.InnIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.InnTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Rally_Spot:
                    bc.TypeString = BuildingType.Rally_Spot.ToString().Replace("_", " ");
                    bc.BuildingTypeInt = (int)BuildingType.Rally_Spot;
                    bc.Food = Constants.RallyFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.RallyStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.RallyWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.RallyIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.RallyTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Town_Hall:
                    bc.TypeString = BuildingType.Town_Hall.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Town_Hall;
                    bc.Food = Constants.ThFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.ThStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.ThWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.ThIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.ThTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Walls:
                    bc.TypeString = BuildingType.Walls.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Walls;
                    bc.Food = Constants.ThFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.ThStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.ThWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.ThIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.ThTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Farm:
                    bc.TypeString = BuildingType.Farm.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Farm;
                    bc.Food = Constants.FarmFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.FarmStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.FarmWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.FarmIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.FarmTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
                case BuildingType.Quarry:
                    bc.TypeString = BuildingType.Quarry.ToString();
                    bc.BuildingTypeInt = (int)BuildingType.Quarry;
                    bc.Food = Constants.QuarryFoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Stone = Constants.QuarryStoneReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Wood = Constants.QuarryWoodReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Iron = Constants.QuarryIronReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    bc.Time = Constants.QuarryTimeReq * Convert.ToInt32(Math.Pow(2, level - 1));
                    break;
            }
            return bc;

        }

        private async Task CheckBuilder1(City userCity)
        {
            //Need to keep track of building..need new table w/ cityID ..else all 
            DateTime TimeNow = DateTime.UtcNow;
            DateTime ConstructionEnds = userCity.Construction1Ends;

            if (userCity.Builder1Busy == false)
            {
                return;
            }
            //Can be busy in number of ways:
            // 1. Click build a building, and Construction has not ended yet. recalculate time.
            // 2. Building was done a long time ago. TimeNow > ConstructionEnds
            if (TimeNow >= ConstructionEnds && userCity.Construction1BuildingId != -1)
            {
                userCity.Builder1Busy = false;

                var b = userCity.Buildings.Where(c => c.BuildingId == userCity.Construction1BuildingId).FirstOrDefault();
                if (b.Level - userCity.Construction1BuildingLevel > 0)
                {
                    //downgrading.. add res
                }

                b.Level = userCity.Construction1BuildingLevel;
                //Get buildingTypeID  get rid of this
                BuildingType BuildingType = GetBuildingType(userCity.BuildingWhat);
                if (b.Level == 0 && b.BuildingType != BuildingType.Walls)
                {
                    BuildingType = BuildingType.Empty;
                    b.BuildingType = BuildingType.Empty;
                }
                b.Image = BuildingType.ToString() + "lvl" + b.Level + ".jpg";
                userCity.Construction1BuildingId = -1; //Building Complete.
                userCity.Construction1BuildingLevel = -1;
                userCity.Builder1Time = -1;
            }
            else
            {
                userCity.Builder1TimeLeft = Convert.ToInt32(Math.Floor((ConstructionEnds - TimeNow).TotalSeconds));
                //Get total building time..for the timer
                //if(userCity.Builder1Time == null)
                var b = userCity.Buildings.Where(c => c.BuildingId == userCity.Construction1BuildingId).FirstOrDefault();
                UpdateCityModel update = new UpdateCityModel() {
                    Level = userCity.Construction1BuildingLevel,
                    BuildingId = userCity.Construction1BuildingId,
                    CityId = userCity.CityId,
                    BuildingTypeInt = (int)b.BuildingType,
                };
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var userResearch = await db.UserResearch.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateUserResearch(UserId);
                BuildingCost bc =  GetUpgradeCostOfBuilding(update, userCity, userResearch);
                userCity.Builder1Time = bc.Time;
            }
            await db.SaveChangesAsync();
        }
        private BuildingType GetBuildingType(string name)
        {
            if (name.ToLower().Contains("academy"))
            {
                return BuildingType.Academy;
            }
            else if (name.ToLower().Contains("forge"))
            {
                return BuildingType.Forge;
            }
            else if (name.ToLower().Contains("feast"))
            {
                return BuildingType.Feasting_Hall;
            }
            else if (name.ToLower().Contains("beacon"))
            {
                return BuildingType.Beacon_Tower;
            }
            else if (name.ToLower().Contains("barrack"))
            {
                return BuildingType.Barrack;
            }
            else if (name.ToLower().Contains("cottage"))
            {
                return BuildingType.Cottage;
            }
            else if (name.ToLower().Contains("empty"))
            {
                return BuildingType.Empty;
            }
            else if (name.ToLower().Contains("inn"))
            {
                return BuildingType.Inn;
            }
            else if (name.ToLower().Contains("rally"))
            {
                return BuildingType.Rally_Spot;
            }
            else if (name.ToLower().Contains("town"))
            {
                return BuildingType.Town_Hall;
            }
            else if (name.ToLower().Contains("farm"))
            {
                return BuildingType.Farm;
            }
            else if (name.ToLower().Contains("sawmill"))
            {
                return BuildingType.Sawmill;
            }
            else if (name.ToLower().Contains("quar"))
            {
                return BuildingType.Quarry;
            }
            else if (name.ToLower().Contains("iron"))
            {
                return BuildingType.Iron_Mine;
            }
            else
            {
                return BuildingType.Not_Found;
            }

        }

        private string CheckForUpdateErrors(UpdateCityModel update)
        {
            string result = "ok";
            if (update.CityId == 0)
            {
                result += "City not found. update.cityId == " + update.CityId.ToString();
            }
            if (update.BuildingId == 0)
            {
                result += "No buildingId found. update.buildingId == " + update.BuildingId.ToString();
            }
            if (update.BuildingTypeInt == 0)
            {
                result += "No building found. update.buildingTypeInt == " + update.BuildingTypeInt.ToString();
            }

            return result;
        }


        private List<BuildingCost> GetNewBuildingsCost(City userCity, UserResearch userResearch)
        {
            List<BuildingCost> lbc = new List<BuildingCost>();
            //[Base Building Time] *(0.9) ^[Construction Level]
            int Time = Convert.ToInt32(Math.Ceiling(60 * Math.Pow(0.9, userResearch.Construction)));

            string TestingResult = "ok";
            bool requirementsMet = true;

            UpdateCityModel update = new UpdateCityModel();

            //update.BuildingTypeString = "Academy";
            update.BuildingTypeInt = (int)BuildingType.Academy;
            TestingResult = CheckIfBuildingPreReqMet(userCity, update);
            requirementsMet = true;
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost Academy = new BuildingCost()
            {
                TypeString = BuildingType.Academy.ToString().Replace("_", " "),
                BuildingTypeInt = (int)BuildingType.Academy,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.AcademyFoodReq,
                Stone = Constants.AcademyStoneReq,
                Wood = Constants.AcademyWoodReq,
                Iron = Constants.AcademyIronReq,
                Time = Constants.AcademyTimeReq,
                Description = "Learn new skills and do your research."
            };
            lbc.Add(Academy);
            //}

            //update.BuildingTypeString = "Barrack";
            update.BuildingTypeInt = (int)BuildingType.Barrack;
            TestingResult = CheckIfBuildingPreReqMet(userCity, update);
            requirementsMet = true;
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost barr = new BuildingCost()
            {
                TypeString = BuildingType.Barrack.ToString(),
                BuildingTypeInt = (int)BuildingType.Barrack,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.BarrFoodReq,
                Stone = Constants.BarrStoneReq,
                Wood = Constants.BarrWoodReq,
                Iron = Constants.BarrIronReq,
                Time = Constants.BarrTimeReq,
                Description = "Place where you train your troops",
            };
            lbc.Add(barr);

            //update.BuildingTypeString = "Cottage";
            update.BuildingTypeInt = (int)BuildingType.Cottage;
            requirementsMet = true;
            TestingResult = "ok"; // CheckIfBuildingPreReqMet(userCity, update);
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost cott = new BuildingCost()
            {
                TypeString = BuildingType.Cottage.ToString(),
                BuildingTypeInt = (int)BuildingType.Cottage,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.CottFoodReq,
                Stone = Constants.CottStoneReq,
                Wood = Constants.CottWoodReq,
                Iron = Constants.CottIronReq,
                Time = Constants.CottTimeReq,
                Description = "Increases your population",
            };
            lbc.Add(cott);

            BuildingCost townHall = new BuildingCost()
            {
                TypeString = BuildingType.Town_Hall.ToString(),
                BuildingTypeInt = (int)BuildingType.Town_Hall,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.ThFoodReq,
                Stone = Constants.ThStoneReq, //CottStoneReq,
                Wood = Constants.ThWoodReq,
                Iron = Constants.ThIronReq,
                Time = Constants.ThTimeReq,
                Description = "Adds more farm spots.",
            };
            lbc.Add(townHall);

            BuildingCost walls = new BuildingCost()
            {
                TypeString = BuildingType.Walls.ToString(),
                BuildingTypeInt = (int)BuildingType.Walls,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.WallsFoodReq,
                Stone = Constants.WallsStoneReq,
                Wood = Constants.WallsWoodReq,
                Iron = Constants.WallsIronReq,
                Time = Constants.WallsTimeReq,
                Description = "Fortify your walls.",
            };
            lbc.Add(walls);

            BuildingCost beacon = new BuildingCost()
            {
                TypeString = BuildingType.Beacon_Tower.ToString().Replace("_", " "),
                BuildingTypeInt = (int)BuildingType.Beacon_Tower,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.BeaconFoodReq,
                Stone = Constants.BeaconStoneReq,
                Wood = Constants.BeaconWoodReq,
                Iron = Constants.BeaconIronReq,
                Time = Constants.BeaconTimeReq,
                Description = "Get advanced warning of an attack.",
            };
            lbc.Add(beacon);


            BuildingCost embassy = new BuildingCost()
            {
                TypeString = BuildingType.Embassy.ToString(),
                BuildingTypeInt = (int)BuildingType.Embassy,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.EmbassyFoodReq,
                Stone = Constants.EmbassyStoneReq,
                Wood = Constants.EmbassyWoodReq,
                Iron = Constants.EmbassyIronReq,
                Time = Constants.EmbassyTimeReq,
                Description = "Join an alliance.",
            };
            lbc.Add(embassy);

            //update.BuildingTypeString = "Feasting";
            update.BuildingTypeInt = (int)BuildingType.Feasting_Hall;
            TestingResult = CheckIfBuildingPreReqMet(userCity, update);
            requirementsMet = true;
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost Feast = new BuildingCost()
            {
                TypeString = BuildingType.Feasting_Hall.ToString().Replace("_", " "),
                BuildingTypeInt = (int)BuildingType.Feasting_Hall,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.FeastFoodReq,
                Stone = Constants.FeastStoneReq,
                Wood = Constants.FeastWoodReq,
                Iron = Constants.FeastIronReq,
                Time = Constants.FeastTimeReq,
                Description = "Where your hero's live and are managed."
            };
            lbc.Add(Feast);
            //}
            //BuildingCount = userCity.Buildings.Where(c => c.BuildingType == BuildingType.Forge).Count();
            //if (BuildingCount == 0)
            //{

           // update.BuildingTypeString = "Forge";
            update.BuildingTypeInt = (int)BuildingType.Forge;
            TestingResult = CheckIfBuildingPreReqMet(userCity, update);
            requirementsMet = true;
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost Forge = new BuildingCost()
            {
                TypeString = BuildingType.Forge.ToString().Replace("_", " "),
                BuildingTypeInt = (int)BuildingType.Forge,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.ForgeFoodReq,
                Stone = Constants.ForgeStoneReq,
                Wood = Constants.ForgeWoodReq,
                Iron = Constants.ForgeIronReq,
                Time = Constants.ForgeTimeReq,
                Description = "Improve your iron working skills.",
            };
            lbc.Add(Forge);
            //}

            //BuildingCount = userCity.Buildings.Where(c => c.BuildingType == BuildingType.Inn).Count();
            //if (BuildingCount == 0)
            //{
           // update.BuildingTypeString = "Inn";
            update.BuildingTypeInt = (int)BuildingType.Inn;
            TestingResult = CheckIfBuildingPreReqMet(userCity, update);
            requirementsMet = true;
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost inn = new BuildingCost()
            {
                TypeString = BuildingType.Inn.ToString(),
                BuildingTypeInt = (int)BuildingType.Inn,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.InnFoodReq,
                Stone = Constants.InnStoneReq,
                Wood = Constants.InnWoodReq,
                Iron = Constants.InnIronReq,
                Time = Constants.InnTimeReq,
                Description = "Recruit new heros.",
            };
            lbc.Add(inn);

            BuildingCost market = new BuildingCost()
            {
                TypeString = BuildingType.Marketplace.ToString(),
                BuildingTypeInt = (int)BuildingType.Marketplace,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.MarketFoodReq,
                Stone = Constants.MarketStoneReq,
                Wood = Constants.MarketWoodReq,
                Iron = Constants.MarketIronReq,
                Time = Constants.MarketTimeReq,
                Description = "Buy and Sell resources.",
            };
            lbc.Add(market);

            //update.BuildingTypeString = "Rally";
            update.BuildingTypeInt = (int)BuildingType.Rally_Spot;
            TestingResult = CheckIfBuildingPreReqMet(userCity, update);
            requirementsMet = true;
            if (TestingResult != "ok")
            {
                requirementsMet = false;
            }
            BuildingCost rally = new BuildingCost()
            {
                TypeString = BuildingType.Rally_Spot.ToString().Replace("_", " "),
                BuildingTypeInt = (int)BuildingType.Rally_Spot,
                PreReq = TestingResult,
                ReqMet = requirementsMet,
                Food = Constants.RallyFoodReq,
                Stone = Constants.RallyStoneReq,
                Wood = Constants.RallyWoodReq,
                Iron = Constants.RallyIronReq,
                Time = Constants.RallyTimeReq,
                Description = "Heal troops, test troops, and increases amount of troops you can send.",
            };
            lbc.Add(rally);

            BuildingCost relief = new BuildingCost()
            {
                TypeString = BuildingType.Relief_Station.ToString(),
                BuildingTypeInt = (int)BuildingType.Relief_Station,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.ReliefFoodReq,
                Stone = Constants.ReliefStoneReq,
                Wood = Constants.ReliefWoodReq,
                Iron = Constants.ReliefIronReq,
                Time = Constants.ReliefTimeReq,
                Description = "A place for those deliverymen to rest, stable and fodder their horses.",

            };
            lbc.Add(relief);
            BuildingCost stable = new BuildingCost()
            {
                TypeString = BuildingType.Stable.ToString(),
                BuildingTypeInt = (int)BuildingType.Stable,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.StableFoodReq,
                Stone = Constants.StableStoneReq,
                Wood = Constants.StableWoodReq,
                Iron = Constants.StableIronReq,
                Time = Constants.StableTimeReq,
                Description = "A stable is any building where horses are kept.",
            };
            lbc.Add(stable);
            BuildingCost warehouse = new BuildingCost()
            {
                TypeString = BuildingType.Warehouse.ToString(),
                BuildingTypeInt = (int)BuildingType.Warehouse,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.WareFoodReq,
                Stone = Constants.WareStoneReq,
                Wood = Constants.WareWoodReq,
                Iron = Constants.WareIronReq,
                Time = Constants.WareTimeReq,
                Description = "Protect your resources from invaders.",
            };
            lbc.Add(warehouse);
            BuildingCost workshop = new BuildingCost()
            {
                TypeString = BuildingType.Workshop.ToString(),
                BuildingTypeInt = (int)BuildingType.Workshop,
                PreReq = "ok",
                ReqMet = true,
                Food = Constants.WorkshopFoodReq,
                Stone = Constants.WorkshopStoneReq,
                Wood = Constants.WorkshopWoodReq,
                Iron = Constants.WorkshopIronReq,
                Time = Constants.WorkshopTimeReq,
                Description = "A place where skilled workers gather to manufacture goods.",
            };
            lbc.Add(workshop);


            BuildingCost farm = new BuildingCost()
            {
                TypeString = BuildingType.Farm.ToString(),
                BuildingTypeInt = (int)BuildingType.Farm,
                PreReq = "",
                ReqMet = true,
                Food = Constants.FarmFoodReq,
                Stone = Constants.FarmStoneReq,
                Wood = Constants.FarmWoodReq,
                Iron = Constants.FarmIronReq,
                Time = Constants.FarmTimeReq,
                Description = "Increases food production.",
                Farm = true,
            };
            lbc.Add(farm);

            BuildingCost quarry = new BuildingCost()
            {
                TypeString = BuildingType.Quarry.ToString(),
                BuildingTypeInt = (int)BuildingType.Quarry,
                PreReq = "",
                ReqMet = true,
                Food = Constants.QuarryFoodReq,
                Stone = Constants.QuarryStoneReq,
                Wood = Constants.QuarryWoodReq,
                Iron = Constants.QuarryIronReq,
                Time = Constants.QuarryTimeReq,
                Description = "Increases stone production.",
                Farm = true,
            };
            lbc.Add(quarry);

            BuildingCost sawmill = new BuildingCost()
            {
                TypeString = BuildingType.Sawmill.ToString(),
                BuildingTypeInt = (int)BuildingType.Sawmill,
                PreReq = "",
                ReqMet = true,
                Food = Constants.SawFoodReq,
                Stone = Constants.SawStoneReq,
                Wood = Constants.SawWoodReq,
                Iron = Constants.SawIronReq,
                Time = Constants.SawTimeReq,
                Description = "Increases wood production.",
                Farm = true,
            };
            lbc.Add(sawmill);

            BuildingCost ironMine = new BuildingCost()
            {
                TypeString = BuildingType.Iron_Mine.ToString(),
                BuildingTypeInt = (int)BuildingType.Iron_Mine,
                PreReq = "",
                ReqMet = true,
                Food = Constants.IronMineFoodReq,
                Stone = Constants.IronMineStoneReq,
                Wood = Constants.IronMineWoodReq,
                Iron = Constants.IronMineIronReq,
                Time = Constants.IronMineTimeReq,
                Description = "Increases iron production.",
                Farm = true,
            };
            lbc.Add(ironMine);
            return lbc;

        }
        private string CheckIfBuildingPreReqMet(City city, UpdateCityModel update)
        {
            string res = "ok";
            //var updateBuilding = city.Buildings.Where(c => c.BuildingId == update.buildingId).FirstOrDefault();
            BuildingType buildingType = (BuildingType)update.BuildingTypeInt;


            var th = city.Buildings.Where(c => c.BuildingType == BuildingType.Town_Hall).FirstOrDefault();

            if (buildingType == BuildingType.Academy)
            {
                if (th.Level < 2)
                {
                    res = "Requires Town Hall level 2";
                }
            }
            else if (buildingType == BuildingType.Barrack)
            {
                var RallySpot = city.Buildings.Where(c => c.BuildingType == BuildingType.Rally_Spot).FirstOrDefault();
                if (RallySpot == null)
                {
                    res = "Must build a RallySpot.";
                }
            }
            else if (buildingType == BuildingType.Cottage)
            {
                if (update.Level - 1 > th.Level)
                {
                    res = "Need to upgrade the Town Hall to level " + th.Level + 1 + ".";
                }
            }
            else if (buildingType == BuildingType.Inn)
            {
                var cottageLvl2 = city.Buildings.Where(c => c.BuildingType == BuildingType.Cottage && c.Level >= 2).FirstOrDefault();
                if (cottageLvl2 == null)
                {
                    res = "Must build a Cottage to level 2.";
                }
            }
            else if (buildingType == BuildingType.Town_Hall)
            {
                //Req quary lvl2 and forge lvl1
                int wallsLevel = city.Buildings.Where(c => c.BuildingType == BuildingType.Walls).Select(c => c.Level).FirstOrDefault();
                if (th.Level - wallsLevel >= 2)
                {
                    res = "Must upgrade walls first.";
                }
            }
            else if (buildingType == BuildingType.Walls)
            {
                //Req quary lvl2 and forge lvl1
                //int highestLvlQuarry = 0; 
                //var quarries = city.Buildings.Where(c => c.BuildingType == BuildingType.Quarry).ToList();
                //if (quarries.Count() > 0) {
                //    highestLvlQuarry = quarries.Max(c => c.Level);
                //}
                //int forgeCount = city.Buildings.Where(c => c.BuildingType == BuildingType.Forge).Count();
                //if (highestLvlQuarry < 2 || forgeCount == 0)
                //{
                //    res = "";
                //}
                //if (highestLvlQuarry < 2)
                //{
                //    res += "Requires Quarry level 2. ";
                //} 
                //if (forgeCount == 0)
                //{
                //    res += "Requires Forge level 1.";
                //}
            }
            return res;
        }

        private List<BuildingCost> GetCostOfTroops(City userCity, UserResearch userResearch)
        {
            var cost = new List<BuildingCost>();

            var worker = new BuildingCost()
            {
                TypeString = TroopType.Worker.ToString(),
                TroopType = (int)TroopType.Worker,
                PreReq = Constants.WorkerBuildReq,
                ReqMet = false,
                Food = Constants.WorkerFoodCost,
                Stone = 0,
                Wood = Constants.WorkerWoodCost,
                Iron = Constants.WorkerIronCost,
                Time = Constants.WorkerTimeCost,
            };
            cost.Add(worker);

            var warr = new BuildingCost()
            {
                TypeString = TroopType.Warrior.ToString(),
                TroopType = (int)TroopType.Warrior,
                PreReq = Constants.WarrBuildReq,
                ReqMet = false,
                Food = Constants.WarrFoodCost,
                Stone = 0,
                Wood = Constants.WarrWoodCost,
                Iron = Constants.WarrIronCost,
                Time = Constants.WarrTimeCost,
            };
            cost.Add(warr);

            var scout = new BuildingCost()
            {
                TypeString = TroopType.Scout.ToString(),
                TroopType = (int)TroopType.Scout,
                PreReq = Constants.ScoutBuildReq,
                ReqMet = false,
                Food = Constants.ScoutFoodCost,
                Stone = 0,
                Wood = Constants.ScoutWoodCost,
                Iron = Constants.ScoutIronCost,
                Time = Constants.ScoutTimeCost,
            };
            cost.Add(scout);

            var pike = new BuildingCost()
            {
                TypeString = TroopType.Pikeman.ToString(),
                TroopType = (int)TroopType.Pikeman,
                PreReq = Constants.PikeBuildReq,
                ReqMet = false,
                Food = Constants.PikeFoodCost,
                Stone = 0,
                Wood = Constants.PikeWoodCost,
                Iron = Constants.PikeIronCost,
                Time = Constants.PikeTimeCost,
            };
            cost.Add(pike);

            var arch = new BuildingCost()
            {
                TypeString = TroopType.Archer.ToString(),
                TroopType = (int)TroopType.Archer,
                PreReq = Constants.ArchBuildReq,
                ReqMet = false,
                Food = Constants.ArchFoodCost,
                Stone = 0,
                Wood = Constants.ArchWoodCost,
                Iron = Constants.ArchIronCost,
                Time = Constants.ArchTimeCost,
            };
            cost.Add(arch);

            var cav = new BuildingCost()
            {
                TypeString = TroopType.Cavalry.ToString(),
                TroopType = (int)TroopType.Cavalry,
                PreReq = Constants.CavBuildReq,
                ReqMet = false,
                Food = Constants.CavFoodCost,
                Stone = 0,
                Wood = Constants.CavWoodCost,
                Iron = Constants.CavIronCost,
                Time = Constants.CavTimeCost,
            };
            cost.Add(cav);

            var ball = new BuildingCost()
            {
                TypeString = TroopType.Ballista.ToString(),
                TroopType = (int)TroopType.Ballista,
                PreReq = Constants.BallBuildReq,
                ReqMet = false,
                Food = Constants.BallFoodCost,
                Stone = 0,
                Wood = Constants.BallWoodCost,
                Iron = Constants.BallIronCost,
                Time = Constants.BallTimeCost,
            };
            cost.Add(ball);

            var cata = new BuildingCost()
            {
                TypeString = TroopType.Catapult.ToString(),
                TroopType = (int)TroopType.Catapult,
                PreReq = Constants.CataBuildReq,
                ReqMet = false,
                Food = Constants.CataFoodCost,
                Stone = Constants.CataStoneCost,
                Wood = Constants.CataWoodCost,
                Iron = Constants.CataIronCost,
                Time = Constants.CataTimeCost,
            };
            cost.Add(cata);

            var Trap = new BuildingCost()
            {
                TypeString = TroopType.Trap.ToString(),
                TroopType = (int)TroopType.Trap,
                PreReq = "Requires Walls level 1.",
                ReqMet = false,
                Food = Constants.TrapFoodReq,
                Stone = Constants.TrapStoneReq,
                Wood = Constants.TrapWoodReq,
                Iron = Constants.TrapIronReq,
                Time = Constants.TrapTimeReq,
            };
            cost.Add(Trap);


            var Abatis = new BuildingCost()
            {
                TypeString = TroopType.Abatis.ToString().Replace("_", " "),
                TroopType = (int)TroopType.Abatis,
                PreReq = "Requires Walls level 2.",
                ReqMet = false,
                Food = Constants.AbatisFoodReq,
                Stone = Constants.AbatisStoneReq,
                Wood = Constants.AbatisWoodReq,
                Iron = Constants.AbatisIronReq,
                Time = Constants.AbatisTimeReq,
            };
            cost.Add(Abatis);

            var AT = new BuildingCost()
            {
                TypeString = TroopType.Archers_Tower.ToString().Replace("_", " "),
                TroopType = (int)TroopType.Archers_Tower,
                PreReq = "Requires Walls level 3.",
                ReqMet = false,
                Food = Constants.ATFoodReq,
                Stone = Constants.ATStoneReq,
                Wood = Constants.ATWoodReq,
                Iron = Constants.ATIronReq,
                Time = Constants.ATTimeReq,
            };
            cost.Add(AT);

            var rl = new BuildingCost()
            {
                TypeString = TroopType.Rolling_Log.ToString().Replace("_", " "),
                TroopType = (int)TroopType.Rolling_Log,
                PreReq = "Requires Walls level 5.",
                ReqMet = false,
                Food = Constants.RollLogFoodReq,
                Stone = Constants.RollLogStoneReq,
                Wood = Constants.RollLogWoodReq,
                Iron = Constants.RollLogIronReq,
                Time = Constants.RollLogTimeReq,
            };
            cost.Add(rl);

            var treb = new BuildingCost()
            {
                TypeString = TroopType.Defensive_Trebuchet.ToString(),
                TroopType = (int)TroopType.Defensive_Trebuchet,
                PreReq = "Requires Walls level 7.",
                ReqMet = false,
                Food = Constants.TrebFoodReq,
                Stone = Constants.TrebStoneReq,
                Wood = Constants.TrebWoodReq,
                Iron = Constants.TrebIronReq,
                Time = Constants.TrebTimeReq,
            };
            cost.Add(treb);



            return cost;
        }

        private async Task<UserResearch> CreateUserResearch(string UserID)
        {
            UserResearch newUR = new UserResearch()
            {
                UserId = UserID,
            };
            await db.UserResearch.AddAsync(newUR);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, CreateUserResearch: " + ex.Message);
                Console.WriteLine(ex.Message);
            }

            await db.SaveChangesAsync();

            return newUR;
        }

        private async Task<UserItems> CreateUserItems(string UserID)
        {

            UserItems NewUserItems = new UserItems()
            {
                UserId = UserID
            };
            await db.UserItems.AddAsync(NewUserItems);
            await db.SaveChangesAsync();

            return NewUserItems;
        }

        private async Task<City> CreateCity(string UserID, List<City> Map)
        {
            //Get flats in state chosen.
            var flats = Map.Where(c => c.UserId == "npc" && c.Terrain == Terrain.Flat).ToList();
            var flatCount = flats.Count();
            var rnd = new Random();
            int randCity = rnd.Next(0, flatCount); //between 0 and flatCount-1
            var newUserCity = flats[randCity];

            newUserCity.UserId = UserID;
            newUserCity.ServerId = 1;
            newUserCity.Image = "cityLvl1.jpg";
            newUserCity.Terrain = Terrain.UserCity;
            newUserCity.Level = 1;
            //newUserCity.CityData = new CityData>()
            //{
            //    Food = 5000,
            //     Stone = 5000,
            //      Wood = 5000,
            //     Iron = 5000,
            //    Gold = 5000,

            //};
            newUserCity.Food = 5000;
            newUserCity.Stone = 5000;
            newUserCity.Wood = 5000;
            newUserCity.Iron = 5000;
            newUserCity.Gold = 5000;
            //newUserCity.Warriors = 0;
            newUserCity.Troops = new List<Troop>() { new Troop {CityId = newUserCity.CityId, MarchType = (int)MarchType.Home } };
            //newUserCity.Defenses = new List<Defenses>() { new Defenses { CityId = newUserCity.CityId } };
            newUserCity.Buildings = new List<Building>();

            //Remove npc hero
            var flatHero = db.Heros.FirstOrDefault(c => c.CityId == newUserCity.CityId);
            if(flatHero != null)
                db.Heros.Remove(flatHero);

            //Create buildings Buildings 1-33 are in the city, buildings 34-73 are for the town(farms) ..40 farms 16 +3 for every th level
            for (int i = 0; i <= 73; i++)
            {
                Building NewBuilding = new Building()
                {
                    CityId = newUserCity.CityId,
                    Location = i,
                    BuildingType = BuildingType.Empty,
                    Level = 0,
                    Image = "emptyCitySlot.jpg"
                };
                newUserCity.Buildings.Add(NewBuilding);
                db.Buildings.Add(NewBuilding);
            }

            var building = newUserCity.Buildings.Where(c => c.Location == 0).FirstOrDefault()!;
            building.BuildingType = BuildingType.Walls;
            building.Level = 0;
            building.Image = "WallsLvl0";
            building.Description = "Walls Protect the city and offer longer range for AT's.";

            building = newUserCity.Buildings.Where(c => c.Location == 1).FirstOrDefault()!;
            building.BuildingType = BuildingType.Town_Hall;
            building.Level = 1;
            building.Image = "TownHallLvl1";
            building.Description = "Manage your city.";
            

            //await db.SaveChangesAsync();

            var userItems = db.UserItems.FirstOrDefault(c => c.UserId == UserID);
            if (userItems == null)
            {
                UserItems NewUserItems = new UserItems()
                {
                    UserId = UserID
                };
                await db.UserItems.AddAsync(NewUserItems);
            }

            var userResaerch = db.UserResearch.FirstOrDefault(c => c.UserId == UserID);
            if (userResaerch == null)
            {
                UserResearch newUR = new UserResearch()
                {
                    UserId = UserID,
                };
                await db.UserResearch.AddAsync(newUR);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, CreateCity: " + ex.Message);
                Console.WriteLine(ex.Message);
            }

            return newUserCity;
        }


        private async Task CheckResearcher(City userCity, UserResearch ur)
        {
            DateTime TimeNow = DateTime.UtcNow;
            DateTime ResearchEnds = userCity.ResearchEnds;

            if (userCity.ResearchBusy == false)
            {
                return;
            }
            //Can be busy in number of ways:
            // 1. Click build a building, and Construction has not ended yet. recalculate time.
            // 2. Building was done a long time ago. TimeNow > ConstructionEnds
            if (TimeNow >= ResearchEnds && userCity.ResearchingLevel != -1)
            {
                userCity.ResearchBusy = false;

                switch (userCity.ResearchTypeId)
                {
                    case 1:
                        ur.Agriculture = userCity.ResearchingLevel;
                        break;
                    case 2:
                        ur.Lumbering = userCity.ResearchingLevel;
                        break;
                    case 3:
                        ur.Masonry = userCity.ResearchingLevel;
                        break;
                    case 4:
                        ur.Mining = userCity.ResearchingLevel;
                        break;
                    case 5:
                        ur.MetalCasting = userCity.ResearchingLevel;
                        break;
                    case 6:
                        ur.Informatics = userCity.ResearchingLevel;
                        break;
                    case 7:
                        ur.MilitaryScience = userCity.ResearchingLevel;
                        break;
                    case 8:
                        ur.MilitaryTradition = userCity.ResearchingLevel;
                        break;
                    case 9:
                        ur.IronWorking = userCity.ResearchingLevel;
                        break;
                    case 10:
                        ur.Logistics = userCity.ResearchingLevel;
                        break;
                    case 11:
                        ur.Compass = userCity.ResearchingLevel;
                        break;
                    case 12:
                        ur.HorsebackRiding = userCity.ResearchingLevel;
                        break;
                    case 13:
                        ur.Archery = userCity.ResearchingLevel;
                        break;
                    case 14:
                        ur.Stockpile = userCity.ResearchingLevel;
                        break;
                    case 15:
                        ur.Medicine = userCity.ResearchingLevel;
                        break;
                    case 16:
                        ur.Construction = userCity.ResearchingLevel;
                        break;
                    case 17:
                        ur.Engineering = userCity.ResearchingLevel;
                        break;
                    case 18:
                        ur.Machinery = userCity.ResearchingLevel;
                        break;
                    case 19:
                        ur.Privateering = userCity.ResearchingLevel;
                        break;
                }

                userCity.ResearchingLevel = -1;
                userCity.ResearchTime = -1;
            }
            else
            {
                userCity.ResearchTime = Convert.ToInt32(Math.Floor((ResearchEnds - TimeNow).TotalSeconds));
            }
            await db.SaveChangesAsync();
        }

        private async Task CheckTroopQueues(City city)
        {
            if (city.TroopQueues == null)
            {
                city.TroopQueues = new List<TroopQueue>();
            }
            List<int> QueuesToDelete = new List<int>();
            foreach (var queue in city.TroopQueues)
            {
                if (queue.Complete == false && queue.Ends < DateTime.UtcNow)
                {
                    await AddTroopsToCity(queue, city);
                    QueuesToDelete.Add(queue.TroopQueueId);
                }
                else if (queue.Complete == false)
                {
                    queue.TimeLeft = Convert.ToInt32(Math.Floor((queue.Ends - DateTime.UtcNow).TotalSeconds));
                    await db.SaveChangesAsync();
                }
            }
            await DeleteTroopQueues(QueuesToDelete, city.TroopQueues);
        }

        private async Task AddTroopsToCity(TroopQueue queue, City city)
        {

            switch (queue.TroopTypeInt)
            {
                case TroopType.Worker:
                    city.Workers = city.Workers + queue.Qty;
                    break;
                case TroopType.Warrior:
                    city.Warriors = city.Warriors + queue.Qty;
                    break;
                case TroopType.Scout:
                    city.Scouts = city.Scouts + queue.Qty;
                    break;
                case TroopType.Pikeman:
                    city.Pikemen = city.Pikemen + queue.Qty;
                    break;
                case (TroopType.Swordsman):
                    city.Swordsmen += queue.Qty;
                    break;
                case TroopType.Transporter:
                    city.Transporters = city.Transporters + queue.Qty;
                    break;
                case TroopType.Archer:
                    city.Archer = city.Archer + queue.Qty;
                    break;
                case TroopType.Cavalry:
                    city.Cavalry = city.Cavalry + queue.Qty;
                    break;
                case TroopType.Cataphract:
                    city.Cataphract = city.Cataphract + queue.Qty;
                    break;
                case TroopType.Ballista:
                    city.Ballista = city.Ballista + queue.Qty;
                    break;
                case TroopType.Catapult:
                    city.Catapult = city.Catapult + queue.Qty;
                    break;

                case TroopType.Trap:
                    city.Traps = city.Traps + queue.Qty;
                    break;
                case TroopType.Abatis:
                    city.Abatis = city.Abatis + queue.Qty;
                    break;
                case TroopType.Archers_Tower:
                    city.Archers_Tower = city.Archers_Tower + queue.Qty;
                    break;
                case TroopType.Rolling_Log:
                    city.Rolling_Log = city.Rolling_Log + queue.Qty;
                    break;
                case TroopType.Defensive_Trebuchet:
                    city.Defensive_Trebuchet = city.Defensive_Trebuchet + queue.Qty;
                    break;
                default:
                    //log error..
                    break;
            }

            queue.Complete = true;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController AddTroopsToCity, []: " + ex.Message + ex.InnerException.Message);
                Console.WriteLine(ex.Message);
            }
        }
        private async Task DeleteTroopQueues(List<int> queueIds, List<TroopQueue> troopQueues)
        {
            try
            {
                foreach (var id in queueIds)
                {
                    //var q = await db.TroopQueues.Where(c => c.TroopQueueId == id).FirstOrDefaultAsync();
                    var q = troopQueues.Where(c => c.TroopQueueId == id).FirstOrDefault();
                    db.TroopQueues.Remove(q);
                    troopQueues.Remove(q);
                }
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController DeleteTroopQueues, []: " + ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        //private async Task<TroopQueue> TroopQueueAdd(TrainTroopsModel update, List<TroopQueue> troopQueues, City city, UserResearch userResearch)
        //{
        //    var costOfTroops = GetCostOfTroops(city, userResearch);
        //    BuildingCost singleTroopCost = costOfTroops.Where(c => c.TroopType ==  update.TroopTypeInt).FirstOrDefault();

        //    bool walls = (update.TroopTypeInt >= 13) ? true : false;

        //    var troopQueue = new TroopQueue()
        //    {
        //        Starts = DateTime.UtcNow,
        //        Ends = DateTime.UtcNow.AddSeconds(singleTroopCost.Time * update.Qty),
        //        Qty = update.Qty,
        //        BuildingId = update.BuildingId,
        //        CityId = city.CityId,
        //        TroopTypeInt = (TroopType)update.TroopTypeInt,
        //        TroopTypeString = ((TroopType)update.TroopTypeInt).ToString(),
        //        TimeLeft = singleTroopCost.Time * update.Qty,
        //        Complete = false,
        //        Walls = walls,
        //    };
        //    await db.TroopQueues.AddAsync(troopQueue);
        //    await db.SaveChangesAsync();

        //    troopQueues.Add(troopQueue);

        //    return troopQueue;

        //}

        //private async Task RemoveResourcesForTroops(City city, UserResearch userResearch, TroopType type, int qty)
        //{
        //    var costOfTroops = GetCostOfTroops(city, userResearch);
        //    BuildingCost singleTroopCost = costOfTroops.Where(c => c.TroopType == (int)type).FirstOrDefault();

        //    city.Food = city.Food - singleTroopCost.Food * qty;
        //    city.Stone = city.Stone - singleTroopCost.Stone * qty;
        //    city.Wood = city.Wood - singleTroopCost.Wood * qty;
        //    city.Iron = city.Iron - singleTroopCost.Iron * qty;

        //    await db.SaveChangesAsync();
        //}

    }
}
