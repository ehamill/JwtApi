using JwtApi.Data;

namespace JwtApi.Models
{
    public class BuildingCost
    {
        //{ type: "cottage", reqMet: false, preReq: "TH level 2", food: 100, stone: 100, wood: 500, iron: 50, time: 75 },
        public string TypeString {get; set;} = "";
        public bool ReqMet { get; set; } = true;
        public string PreReq { get; set; } = "";
        public int Food { get; set; } = 0;
        public int Stone { get; set; } = 0;
        public int Wood { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Time { get; set; } = 0;
        public string Description { get; set; } = "";
        public int? BuildingTypeInt { get; set; }
        public int? TroopType { get; set; }
        public bool Farm { get; set; } = false;

    }
}
