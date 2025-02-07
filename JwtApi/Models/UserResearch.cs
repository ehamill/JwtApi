using JwtApi.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApi.Models
{
    public class UserResearch
    {
        public int UserResearchId { get; set; }
        public string UserId { get; set; }
        public int Agriculture { get; set; } = 0;
        public int Lumbering { get; set; } = 0;
        public int Masonry { get; set; } = 0;
        public int Mining { get; set; } = 0;
        public int MetalCasting { get; set; } = 0;
        public int Informatics { get; set; } = 0;
        public int MilitaryScience { get; set; } = 0;
        public int MilitaryTradition { get; set; } = 0;
        public int IronWorking { get; set; } = 0;
        public int Logistics { get; set; } = 0;
        public int Compass { get; set; } = 0;
        public int HorsebackRiding { get; set; } = 0;
        public int Archery { get; set; } = 0;
        public int Stockpile { get; set; } = 0;
        public int Medicine { get; set; } = 0;
        public int Construction { get; set; } = 0;
        public int Engineering { get; set; } = 0;
        public int Machinery { get; set; } = 0;
        public int Privateering { get; set; } = 0;

        //[ForeignKey("UserId")]
        //public virtual City City { get; set; }
    }

    public class Research
    {
        public ResearchType ResearchType { get; set; }
        public string Requires { get; set; } = "";
        public int MinAcademyLevel { get; set; } = 0;
        public BuildingType BuildingTypeRequired { get; set; }
        public int Food { get; set; } = 0;
        public int Stone { get; set; } = 0;
        public int Lumber { get; set; } = 0;
        public int Iron { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Time { get; set; } = 0;
        public int ProductionIncreasePercent { get; set; } = 0;
        public bool RequirementsMet { get; set; } = false;
        public string Description { get; set; } = "";
        //public int Level { get; set; } = 0;
        
        //public string Image { get; set; } = "missing.jpg";
    }

}
