using JwtApi.Data;

namespace JwtApi.Models
{
    
    public class UpdateCityModel
    {
        public int CityId { get; set; }
        public int BuildingId { get; set; }
       // public string? BuildingTypeString { get; set; }
        public int BuildingTypeInt { get; set; } // have to use int?
        /// </summary>
        public int Level { get; set; }
        //public int Location { get; set; } = -1;
    }
    public class StartBattleModel {
        public int BattleId { get; set; }
    }
    public class AttackCityModel
    {
        public int CityId { get; set; } = 0;
        public int AttackCityId { get; set; } = 0;
        public int HeroId { get; set; } = 0;
        public string? March { get; set; }
        public int Workers { get; set; } = 0;
        public int Warrs { get; set; } = 0;
        public int Pikes { get; set; } = 0;
        public int Swords { get; set; } = 0;
        public int Arch { get; set; } = 0;
        public int Batt { get; set; } = 0;
        public int Scout { get; set; } = 0;
        public int Cavs { get; set; } = 0;
        public int Phract { get; set; } = 0;
        public int Trans { get; set; } = 0;
        public int Balls { get; set; } = 0;
        public int Cata { get; set; } = 0;
    }
    public class UpdateResearchModel
    {
        public int CityId { get; set; }
        public int ResearchId { get; set; }
    }
    public class Resources
    {
        public int Food { get; set; } = 0;
        public int Stone { get; set; } = 0;
        public int Wood { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Gold { get; set; } = 0;
    }
    public class HireHeroModel
    {
        public int CityId { get; set; }
        public int HeroId { get; set; }
    }
    public class Result
    {
        public bool Failed { get; set; }
        public string Message { get; set; }
    }
    public class SpeedUpModel
    {
        public int BuildingId { get; set; }
        //public SpeedUpTypes SpeedUpType { get; set; }
        public int SpeedUpTypeId { get; set; } //15min, 1 hour, etc
        //public SpeedUp UsedOn { get; set; }
        public int UsedOn { get; set; } ///builder1, builder2, research, trooptraining, etc
    }
    public class SendEmailModel
    {
        public string EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
    
}
