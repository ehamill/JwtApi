using JwtApi.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApi.Models
{
  
        public class CityDefender
        {
            public int CityDefenderId { get; set; }
            public DateTime ArrivalTime { get; set; }
            public int DefCityId { get; set; }
            public int DefTroopId { get; set; }
        }
        public class Battle {
            public int BattleId { get; set; }
            public int AtkCityId { get; set; }
            public int DefCityId { get; set; }
            public DateTime TimeSent { get; set; }
            public DateTime BattleTime { get; set; }    
            
            public int AtkSentTroopId { get; set; }
            public int AtkRemainTroopId { get; set; }
            public int DefSentTroopId { get; set; }
            public int DefRemainTroopId { get; set; }
            public bool Processed { get; set; }
            public bool AttackersWon { get; set; }
            public int MarchType { get; set; }
            public int MarchTimeInSecs { get; set; }
            [NotMapped]
            public List<CityDefender> CitiesDefenders { get; set; }  

        }

        public class Troop {
            public int TroopId { get; set; }    
            public int CityId { get; set; }
            public int HeroId { get; set; } = 0;
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
        }
        public class Defenses {
            public int DefenseId { get; set; }  
            public int CityId { get; set; }
            public DateTime DateTime { get; set; } = DateTime.UtcNow;
            public int Trap { get; set; } = 0;
            public int Abatis { get; set; } = 0;
            public int Archers_Tower { get; set; } = 0;
            public int Rolling_Log { get; set; } = 0;
            public int Defensive_Trebuchet { get; set; } = 0;

        }

        public class Army
        {
            public int Workers { get; set; } = 0;
            public int Scouts { get; set; } = 0;
            public int Warriors { get; set; } = 0;
            public int Pikemen { get; set; } = 0;
            public int Swordsmen { get; set; } = 0;
            public int Archer { get; set; } = 0;
            public int Cavalry { get; set; } = 0;
            public int Cataphract { get; set; } = 0;
            public int Transport { get; set; } = 0;
            public int Ballista { get; set; } = 0;
            public int Battering_Ram { get; set; } = 0;
            public int Catapult { get; set; } = 0;
            public int Trap { get; set; } = 0;
            public int Abatis { get; set; } = 0;
            public int Archers_Tower { get; set; } = 0;
            public int Rolling_Log { get; set; } = 0;
            public int Defensive_Trebuchet { get; set; } = 0;
        }

        public class WarrResults {
            public bool AttackersWon { get; set; } = false;
            public Army AttackingTroopsSent { get; set; }
            public Army AttackingTroopsRemain { get; set; }
            public Army DefenceTroopsSent { get; set; }
            public Army DefenceTroopsRemain { get; set; }
        }
        public class TroopProperties
        {
            public TroopType TroopType { get; set; }
            public string Requirement { get; set; }
            public bool RequirementMet { get; set; } = false;
            public string Description { get; set; }
            public int Qty { get; set; } = 0;
            public int BarracksLevel { get; set; } = 0;
            public int WallsLevel { get; set; } = 0;
            public int Food { get; set; } = 0;
            public int Stone { get; set; } = 0;
            public int Lumber { get; set; } = 0;
            public int Iron { get; set; } = 0;
            public int Time { get; set; } = 0;
            public int Population { get; set; } = 0;
            public int Attack { get; set; } = 0;
            public int Defense { get; set; } = 0;
            public int Speed { get; set; } = 0;
            public int Life { get; set; } = 0;
            public int Load { get; set; } = 0;
            public int Range { get; set; } = 0;
            public int FoodCity { get; set; } = 0;//per hour
            public bool SupplyTroop { get; set; } = false;
            public bool WallDefense { get; set; } = false;
            public string Image { get; set; } = "missing.jpg";
        }
        public class BattleTroop
        {
            public TroopType TroopType { get; set; } = TroopType.None;
            public int BeginningQty { get; set; } = 0;
            public int Quantity { get; set; } = 0;
            public double Attack { get; set; } = 0;
            public double Defense { get; set; } = 0;
            public double Life { get; set; } = 0;
            public int Speed { get; set; } = 0;
            public int Range { get; set; } = 0;
            public bool Fighting { get; set; } = false;
            public int Position { get; set; } = 0;
            public int Killed { get; set; } = 0;
            public bool RangedUnit { get; set; } = false;
            public double TotalAttack { get; set; } = 0;
            public int KillZone { get; set; } = 0;
            public bool SupplyTroop { get; set; } = false;
        }
        public class Troopjlkjl
        {
            public string TypeString { get; set; } = "";
            public TroopType TypeInt { get; set; }
            public string PreReq { get; set; } = "";
            public bool ReqMet { get; set; } = false;
            public string Description { get; set; } = "";
            public int Qty { get; set; } = 0;
            public int FoodCost { get; set; } = 0;
            public int StoneCost { get; set; } = 0;
            public int WoodCost { get; set; } = 0;
            public int IronCost { get; set; } = 0;
            public int TimeCost { get; set; } = 0;
            public bool ForWalls { get; set; } = false;
            public int Attack { get; set; } = 0;
            public int Defense { get; set; } = 0;
            public int Speed { get; set; } = 0;
            public int Load { get; set; } = 0;
            public int Life { get; set; } = 0;
            public int Range { get; set; } = 0;
            public string Image { get; set; } = "missing.jpg";
        }

        public class TroopPreReqCheck
        {
            public string BuildingType { get; set; }
            public bool ReqMet { get; set; }
        }
        public class TrainTroopsModel
        {
            public int CityId { get; set; }
            public int BuildingId { get; set; }
            public int TroopTypeInt { get; set; }
            public int Qty { get; set; }
        }
        public class TrainTroopsDoneModel
        {
            public int CityId { get; set; }
            public int QueueId { get; set; }
        }

    
}
