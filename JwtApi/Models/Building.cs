using JwtApi.Data;

namespace JwtApi.Models
{
    public class Building
    {
        public int BuildingId { get; set; }
        public int CityId { get; set; }
        public int Location { get; set; } // 0-25 ..2 is always TH
        public BuildingType BuildingType {get; set;}
        public int Level { get; set; }
        public string Image { get; set; } = "EmptySlot.jpg";
        public string Description { get; set; } = "Empty";

    }
}
