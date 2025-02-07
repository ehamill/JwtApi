using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApi.Models
{
    public class WarReport
    {
        public int WarReportId { get; set; }
        public int BattleId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string AtkPlayerName { get; set; }
        public string DefPlayerName { get; set; }
        public string AtkHeroName { get; set; }
        public string AtkHeroImg { get; set; }
        public string AtkCityName { get; set; }
        public string DefCityName { get; set; }
public string AtkCityCoords { get; set; }
        public string DefCityCoords { get; set; }
        public int AtkTroopSentId { get; set; }
        [NotMapped]
        public Troop AtkTroopsSent { get; set; }
        public int AtkTroopSurvivedId { get; set; }
        [NotMapped]
        public Troop AtkTroopsSurvived { get; set; }
        public string DefHeroName { get; set; }
        public string DefHeroImg { get; set; }
        
        
        
        public int DefTroopSentId { get; set; }
        [NotMapped]
        public Troop DefTroopsSent { get; set; }
        public int DefTroopSurvivedId { get; set; }
        [NotMapped]
        public Troop DefTroopsSurvived { get; set; }
        public int DefCityFood { get; set; }
        public int DefCityStone { get; set; }
        public int DefCityWood { get; set; }
        public int DefCityIron { get; set; }
        public int DefCityGold { get; set; }
        public int DefCityLoyalty { get; set; }
        

        public bool AttackersWon { get; set; } = false;

        public bool Read { get; set; } = false;
        public bool Opened { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public int ReturnTimeInSec { get; set; } = 0;


    }
}
