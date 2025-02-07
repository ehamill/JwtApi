using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApi.Models
{
    public class UserItems
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? NickName { get; set; }
        public int Cents { get; set; } = 0;
        public int FiveMinuteSpeedups { get; set; } = 0;
        public int FifteenMinuteSpeedups { get; set; } = 0;
        public int OneHourSpeedups { get; set; } = 0;
        public int RedGems { get; set; } = 0;
        public int BlueGems { get; set; } = 0;
        public int FarmBonuses { get; set; } = 0;


    }
}
