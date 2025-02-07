
using JwtApi.Data;
using JwtApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApi.Models
{
    public class AttackTimer
    {
        public bool Returning { get; set; }
        public int ReturnTime { get; set; }
        
        // public int ServerId { get; set; } = 0;

    }
    public class Email {
        public int EmailId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
       // public int ServerId { get; set; } = 0;

    }
    //AttackMission(id, attkCityId,defCityId, SentTime, work, war, etc, arriveTime, returnTime)
    public class AttackMission
    {
        public int MissionId { get; set; }
        public int AttackCityId { get; set; }
        public int DefenseCityId { get; set; }
        public DateTime Sent { get; set; } = DateTime.UtcNow;
        public DateTime ArriveTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public int AtkWorkers { get; set; } = 0;
        public int AtkWarriors { get; set; } = 0;
        public int AtkPikemen { get; set; } = 0;
        public int AtkSwordsman { get; set; } = 0;
        public int AtkArcher { get; set; } = 0;
        public int AtkBatteringRam { get; set; } = 0;
        public int AtkScouts { get; set; } = 0;
        public int AtkCavalry { get; set; } = 0;
        public int AtkCataphract { get; set; } = 0;
        public int AtkTransporters { get; set; } = 0;
        public int AtkBallista { get; set; } = 0;
        public int AtkCatapult { get; set; } = 0;
        public int DefWorkers { get; set; } = 0;
        public int DefWarriors { get; set; } = 0;
        public int DefPikemen { get; set; } = 0;
        public int DefSwordsman { get; set; } = 0;
        public int DefArcher { get; set; } = 0;
        public int DefBatteringRam { get; set; } = 0;
        public int DefScouts { get; set; } = 0;
        public int DefCavalry { get; set; } = 0;
        public int DefCataphract { get; set; } = 0;
        public int DefTransporters { get; set; } = 0;
        public int DefBallista { get; set; } = 0;
        public int DefCatapult { get; set; } = 0;
        public int DefTraps { get; set; } = 0;
        public int DefAbatis { get; set; } = 0;
        public int DefArchers_Tower { get; set; } = 0;
        public int DefRolling_Log { get; set; } = 0;
        public int Defensive_Trebuchet { get; set; } = 0;

    }
    //public class City
    //{
    //    public int CityId { get; set; }
    //    public int CoordX { get; set; } = 0;
    //    public int CoordY { get; set; } = 0;
    //    public int Level { get; set; } = 0;
    //    public string UserId { get; set; }
    //    public int ServerId { get; set; }
    //    public string Image { get; set; }
    //    public int FoodRate { get; set; } = 100;
    //    public int StoneRate { get; set; } = 100;
        
    //    public int WoodRate { get; set; } = 100;
        
    //    public int IronRate { get; set; } = 100;
        
    //    public int GoldRate { get; set; } = 0;
    //    public DateTime ResourcesLastUpdated { get; set; } = DateTime.UtcNow;
        
    //    public DateTime Construction1Started { get; set; } = DateTime.UtcNow;
    //    public DateTime Construction1Ends { get; set; } = DateTime.UtcNow;
    //    public int Construction1BuildingId { get; set; } = 0;
    //    public int Construction1BuildingLevel { get; set; } = 0;
    //    public bool Builder1Busy { get; set; } = false;
    //    public int Builder1Time { get; set; } = 0;
    //    public string? BuildingWhat { get; set; }

    //    public int Food { get; set; } = 5000;
    //    public int Stone { get; set; } = 5000;
    //    public int Wood { get; set; } = 5000;
    //    public int Iron { get; set; } = 5000;
    //    public int Gold { get; set; } = 5000;
    //    public int Workers { get; set; } = 0;
    //    public int Warriors { get; set; } = 0;
    //    public int Pikemen { get; set; } = 0;
    //    public int Swordsmen { get; set; } = 0;
    //    public int Archer { get; set; } = 0;
    //    public int Battering_Ram { get; set; } = 0;
    //    public int Scouts { get; set; } = 0;
    //    public int Cavalry { get; set; } = 0;
    //    public int Cataphract { get; set; } = 0;
    //    public int Transporters { get; set; } = 0;
    //    public int Ballista { get; set; } = 0;
    //    public int Catapult { get; set; } = 0;
    //    public int Traps { get; set; } = 0;
    //    public int Abatis { get; set; } = 0;
    //    public int Archers_Tower { get; set; } = 0;
    //    public int Rolling_Log { get; set; } = 0;
    //    public int Defensive_Trebuchet { get; set; } = 0;



    //    public bool ResearchBusy { get; set; } = false;
    //    public int ResearchTime { get; set; } = 0;
    //    public string? ResearchingWhat { get; set; }
    //    public DateTime ResearchStarted { get; set; } = DateTime.UtcNow;
    //    public DateTime ResearchEnds { get; set; } = DateTime.UtcNow;
    //    public int ResearchingLevel { get; set; } = 0;
    //    public int ResearchTypeId { get; set; } = 0;
    //    public Terrain Terrain { get; set; } = Terrain.None;
    //   // [NotMapped]
    //    public List<Building> Buildings { get; set; }
    //    //[NotMapped]
    //    public List<TroopQueue> TroopQueues { get; set; }
    //    [NotMapped]
    //    public List<TroopProperties> TroopProperties { get; set; }
    //    [NotMapped]
    //    public List<FutureCityData> FutureCityData { get; set; }
    //    //[NotMapped]
    //    //public List<Troops> Troops { get; set; }
    //    //[NotMapped]
    //    //public List<Defenses> Defenses { get; set; }
    //    [NotMapped]
    //    public List<BuildingCost> ListOfBuildingsCost { get; set; }
    //    [NotMapped]
    //    public List<Research> ResearchCost { get; set; }
    //    //[NotMapped]
    //    public List<Hero> Heros { get; set; }
    //    [NotMapped]
    //    public List<Battle> Battles { get; set; }
    //}

    public class FutureCityData
    {
        public int FutureCityDataID { get; set; }
        public int CityId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int Workers { get; set; } = 0;
        public int Warriors { get; set; } = 0;
        public int Scouts { get; set; } = 0;
        public int Pikemen { get; set; } = 0;
        public int Swordsmen { get; set; } = 0;
        public int Archer { get; set; } = 0;
        public int Cavalry { get; set; } = 0;
        public int Cataphract { get; set; } = 0;
        public int Transporters { get; set; } = 0;
        public int Ballista { get; set; } = 0;
        public int Battering_Ram { get; set; } = 0;
        public int Catapult { get; set; } = 0;
        public int Trap { get; set; } = 0;
        public int Abatis { get; set; } = 0;
        public int Archers_Tower { get; set; } = 0;
        public int Rolling_Log { get; set; } = 0;
        public int Defensive_Trebuchet { get; set; } = 0;
        public int Food { get; set; } = 0;
        public int Stone { get; set; } = 0;
        public int Wood { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Loyalty { get; set; } = 100;
    }

    public class CityData
    {
        public int CityDataID { get; set; }
        public int CityId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int Workers { get; set; } = 0;
        public int Warriors { get; set; } = 0;
        public int Scouts { get; set; } = 0;
        public int Pikemen { get; set; } = 0;
        public int Swordsmen { get; set; } = 0;
        public int Archer { get; set; } = 0;
        public int Cavalry { get; set; } = 0;
        public int Cataphract { get; set; } = 0;
        public int Transporters { get; set; } = 0;
        public int Ballista { get; set; } = 0;
        public int Battering_Ram { get; set; } = 0;
        public int Catapult { get; set; } = 0;
        public int Trap { get; set; } = 0;
        public int Abatis { get; set; } = 0;
        public int Archers_Tower { get; set; } = 0;
        public int Rolling_Log { get; set; } = 0;
        public int Defensive_Trebuchet { get; set; } = 0;
        public int Food { get; set; } = 0;
        public int Stone { get; set; } = 0;
        public int Wood { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Loyalty { get; set; } = 100;
    }

}
