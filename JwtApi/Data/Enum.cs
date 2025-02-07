namespace JwtApi.Data
{
    public enum MarchType
    {
        Attack = 1,
        Scout = 2,
        Reinforce = 3,
    }
    public enum Terrain
    {
        None = 0,
        Npc = 1,
        Flat = 2,
        Grassland = 3,
        Swamp = 4,
        Lake = 5,
        Hill = 6,
        Desert = 7,
        Forest = 8,
        UserCity =9,
        Field = 10,
    }
    public enum BuildingType
    {
        Empty = 0,
        Academy = 1,
        Barrack = 2,
        Beacon_Tower = 3,
        Cottage = 4,
        Embassy = 5,
        Feasting_Hall = 6,
        Forge = 7,
        Inn = 8,
        Marketplace = 9,
        Rally_Spot = 10,
        Relief_Station = 11,
        Stable = 12,
        Town_Hall = 13,
        Warehouse = 14,
        Workshop = 15,
        Farm = 16,
        Iron_Mine = 17,
        Sawmill = 18,
        Quarry = 20,
        Walls = 21,
        Not_Found = 50,
    }


    //public enum CityBuildingType
    //{
    //    Empty = 0,
    //    Academy = 1,
    //    Barrack = 2,
    //    Beacon_Tower=3,
    //    Cottage = 4,
    //    Embassy = 5,
    //    Feasting_Hall = 6,
    //    Forge = 7,
    //    Inn = 8,
    //    Marketplace = 9,
    //    Rally_Spot = 10,
    //    Relief_Station = 11,
    //    Stable = 12,
    //    Town_Hall = 13,
    //    Warehouse=14,
    //    Workshop = 15,
    //    Farm = 16,
    //    Iron_Mill = 17,
    //    Sawmill = 18,
    //    Iron_Mine = 19,
    //    Quarry = 20,
    //    Not_Found = 21,
    //}
    public enum FarmBuildingType
    {
        Empty = 0,
        Farm = 1,
        Iron_Mill = 2,
        Sawmill = 3,
        Iron_Mine = 4,
        Quarry = 5,
    }
    public enum TroopType
    {
        None = 0,
        Worker = 1,
        Warrior = 2,
        Pikeman = 3,
        Swordsman = 4,
        Archer = 5,
        Battering_Ram = 6,
        Scout = 7,
        Cavalry = 8,
        Cataphract = 9,
        Transporter = 10,
        Ballista = 11,
        Catapult = 12,
        Trap = 13,
        Abatis = 14,
        Archers_Tower = 15,
        Rolling_Log = 16,
        Defensive_Trebuchet = 17
    }
    public enum SpeedUp { 
        Builder1 = 1,
        Builder2 = 2,
        Research = 3,
    }
    public enum ResearchType
    {
        None = 0,
        Agriculture = 1,
        Lumbering = 2,
        Masonry = 3,
        Mining = 4,
        Metal_Casting = 5,
        Informatics = 6,
        Military_Science = 7,
        Military_Tradition = 8,
        Iron_Working = 9,
        Logistics = 10,
        Compass = 11,
        Horseback_Riding = 12,
        Archery = 13,
        Stockpile = 14,
        Medicine = 15,
        Construction = 16,
        Engineering = 17,
        Machinery = 18,
        Privateering = 19,
    }
    
    //    Agriculture • Lumbering • Masonry • Mining • Metal Casting • Informatics • Military Science • Military Tradition • Iron Working
    //Logistics • Compass • Horseback Riding • Archery • Stockpile • Medicine • Construction • Engineering • 
    //        Machinery • Privateering

    //string AggPrereq = "Academy lvl1, farm Level = level";//lvl3 needs a farm lvl3
    //public const int AggWoodReq = 500; //lvl3 500*2*2
    //public const int AggGoldReq = 1000;
    //public const int AggTimeReq = 6 * 60 + 40;
    ////each lvl = 10% inc in food , lvl5 = 50%

    //string WoodingPrereq = "Academy lvl1, wood Level = level";//lvl3 needs a farm lvl3
    //public const int WoodingWoodReq = 500; //lvl3 500*2*2
    //public const int WoodingIronReq = 100; //lvl3 500*2*2
    //public const int WoodingGoldReq = 1200;
    //public const int WoodingTimeReq = 8 * 60 + 20;
    ////each lvl = 10% inc in food , lvl5 = 50%

    //string MasonryPrereq = "Academy lvl2, wood Level = level";//lvl3 needs a farm lvl3
    //public const int MasonryStoneReq = 500; //lvl3 500*2*2
    //public const int MasonryIronReq = 200; //lvl3 500*2*2
    //public const int MasonryGoldReq = 1500;
    //public const int MasonryTimeReq = 10 * 60;
    ////each lvl = 10% inc in food , lvl5 = 50%

    //string MiningPrereq = "Academy lvl2, Masonry Level 1";//lvl3 needs a ironmine lvl3
    //public const int MiningIronReq = 800; //lvl3 500*2*2
    //public const int MiningGoldReq = 2000;
    //public const int MiningTimeReq = 11 * 60 + 40;
    ////each lvl = 10% inc in food , lvl5 = 50%


    //string MetalCastingPrereq = "Academy lvl3, Mining Level 2";
    //public const int MetalCastingWoodReq = 500;
    //public const int MetalCastingIronReq = 500; //lvl3 500*2*2
    //public const int MetalCastingGoldReq = 5000;
    //public const int MetalCastingTimeReq = 15 * 60;


}
