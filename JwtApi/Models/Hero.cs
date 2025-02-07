namespace JwtApi.Models
{
    public class Hero
    {
        public int HeroId { get; set; }
        public int CityId { get; set; }
        public int Loyalty { get; set; } = 70;
        public int Level { get; set; } = 1;
        public int Attack { get; set; } = 1;
        public int Intelligence { get; set; } = 1;
        public int Politics { get; set; } = 1;
        public bool IsHired { get; set; } = false;
        public bool IsMayor { get; set; } = false;
        public bool IsFighting { get; set; } = false;
        public string Name { get; set; } = "test";
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
