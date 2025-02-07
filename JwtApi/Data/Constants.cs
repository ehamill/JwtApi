
namespace JwtApi.Data
{
    public class Constants
    {
        #region Walls

        public const int TrapFoodReq = 50;
        public const int TrapWoodReq = 500;
        public const int TrapStoneReq = 100;
        public const int TrapIronReq = 50;
        public const int TrapTimeReq = 1 * 60;

        public const int AbatisFoodReq = 100;
        public const int AbatisWoodReq = 1200;
        public const int AbatisStoneReq = 0;
        public const int AbatisIronReq = 150;
        public const int AbatisTimeReq = 2 * 60;

        public const int ATFoodReq = 200;
        public const int ATWoodReq = 2000;
        public const int ATStoneReq = 1500;
        public const int ATIronReq = 500;
        public const int ATTimeReq = 30 * 60;

        public const int RollLogFoodReq = 300;
        public const int RollLogWoodReq = 6000;
        public const int RollLogStoneReq = 0;
        public const int RollLogIronReq = 0;
        public const int RollLogTimeReq = 6 * 60;

        public const int TrebFoodReq = 600;
        public const int TrebWoodReq = 10;
        public const int TrebStoneReq = 8000;
        public const int TrebIronReq = 0;
        public const int TrebTimeReq = 10 * 60;

        #endregion

        #region Farms
        public const string FarmPrereq  = " ";
        public const int FarmFoodReq = 50;// FoodReq = FarmFoodReq * 2^(nextLevel -1) 
        public const int FarmStoneReq = 200;
        public const int FarmWoodReq = 300;
        public const int FarmIronReq = 150;
        public const int FarmTimeReq = 30; // seconds TimeReq * 2^(nextLevel -1)
        public const int FarmPopulationReq = 10; // int level = 4; int farmPop = farmPopReq * level*(level + 1) / 2;
        public const int FarmFoodRate = 100;//  farmPopReq* level*(level + 1) / 2; Triangular number Seq //Tri sequence
        public const int FarmFoodCapacity = 10000; // Tri seq 10000, 30000, 60000, etc
                                                   //quest level 1	Beginner Guidelines (1)	food 100 wood 500 stone 500 iron 300
                                                   //quest level Level 5 food 1,000 wood 5,000 stone 3,000 iron 2,500
        public const int QuarryFoodReq = 180;//FarmFoodReq * 2^(nextLevel -1) 
        public const int QuarryStoneReq = 150;
        public const int QuarryWoodReq = 500;
        public const int QuarryIronReq = 400;
        public const int QuarryTimeReq = 60; //secs 2^
        public const int QuarryPopulationReq = 20; //tri seq
        public const int QuarryStoneRate = 100; //tri seq
        public const int QuarryStoneCap = 10000; //tri seq.
                                    //Quest lvl1 1	Beginner's Guidelines (1)	food 300 Wood 750 stone	300	iron 500

        public const int SawFoodReq = 100; //2^
        public const int SawWoodReq = 100;
        public const int SawStoneReq = 250;
        public const int SawIronReq = 300;
        public const int SawTimeReq = 45; //secs  2^
        public const int SawPopReq = 10; //tri seq
        public const int SawWoodRate = 100; //tri seq
        public const int SawMaxCap = 10000; //tri seq
                               //Quest Sawmill Lv.2 Award: Food 1,000 Wood 1,000 Stone 1,000 Iron 1,000


        public const int IronMineFoodReq = 210;
        public const int IronMineWoodReq = 600;
        public const int IronMineStoneReq = 500;
        public const int IronMineIronReq = 200;
        public const int IronMineTimeReq = 90;
        public const int IronMinePopReq = 25;
        public const int IronMineRate = 100;
        public const int IronMineCap = 10000;
        //Quest: Level 1	Beginner Guidelines (1)	500	800	750	500
        #endregion

        #region Troops

        public const string WorkerBuildReq = "Barracks Level 1";
        public const int WorkerFoodCost = 50;
        public const int WorkerWoodCost = 150;
        public const int WorkerIronCost = 10;
        public const int WorkerTimeCost = 50;//secs
        public const int WorkerLife = 100;
        public const int WorkerPopulation = 1;
        public const int WorkerAttk = 5;
        public const int WorkerDef = 10;
        public const int WorkerLoad = 200;
        public const int WorkerFoodCity = 2; //per hour
        public const int WorkerSpeed = 180;
        public const int WorkerRange = 10;
        //public const int WorkerQuest = { qtyWorkers:10, award:{ food:500, Wood:1500, iron:100 } };


        public const string WarrBuildReq = "Barracks Level 1";
        public const int WarrFoodCost = 80;
        public const int WarrWoodCost = 100;
        public const int WarrIronCost = 50;
        public const int WarrTimeCost = 25;//secs
        public const int WarrLife = 200;
        public const int WarrPopulation = 1;
        public const int WarrAttk = 50;
        public const int WarrDef = 50;
        public const int WarrLoad = 20;
        public const int WarrFoodCity = 3; //per hour
        public const int WarrSpeed = 200;
        public const int WarrRange = 20;
        //public const int WarrQuest = { qtywarrs:10, award:{ food:800, Wood:1000, iron:500 } };

        public const string ScoutBuildReq = "Barracks Level 2";
        public const int ScoutFoodCost = 120;
        public const int ScoutWoodCost = 200;
        public const int ScoutIronCost = 150;
        public const int ScoutTimeCost = 100;//secs
        public const int ScoutLife = 100;
        public const int ScoutPopulation = 1;
        public const int ScoutAttk = 20;
        public const int ScoutDef = 20;
        public const int ScoutLoad = 5;
        public const int ScoutFoodCity = 5; //per hour
        public const int ScoutSpeed = 3000;
        public const int ScoutRange = 20;
        //public const int ScoutQuest = { qtyScouts:10, award:{ food:1200, Wood:2000, iron:1500 } };

        public const string PikeBuildReq = "Barracks Level 2";
        public const string PikeBuildReq2 = "Military Tradition Level 1";
        public const int PikeFoodCost = 150;
        public const int PikeWoodCost = 500;
        public const int PikeIronCost = 100;
        public const int PikeTimeCost = 2 * 60 + 30;//secs
        public const int PikeLife = 300;
        public const int PikePopulation = 1;
        public const int PikeAttk = 150;
        public const int PikeDef = 150;
        public const int PikeLoad = 40;
        public const int PikeFoodCity = 6; //per hour
        public const int PikeSpeed = 300;
        public const int PikeRange = 50;
        // public const int PikeQuest = { qtyPikes:10, award:{ food:1500, Wood:5000, iron:1000 } };

        public const string SwordBuildReq = "Barracks Level 3";
        public const string SwordBuildReq2 = "Iron Working Level 1";
        public const int SwordFoodCost = 200;
        public const int SwordWoodCost = 150;
        public const int SwordIronCost = 400;
        public const int SwordTimeCost = 3 * 60 + 45;//secs
        public const int SwordLife = 350;
        public const int SwordPopulation = 1;
        public const int SwordAttk = 100;
        public const int SwordDef = 250;
        public const int SwordLoad = 30;
        public const int SwordFoodCity = 7; //per hour
        public const int SwordSpeed = 275;
        public const int SwordRange = 30;
        //public const int SwordQuest = { qtySwords:10, award:{ food:2000, Wood:1500, iron:4000 } };

        public const string ArchBuildReq = "Barracks Level 4";
        public const string ArchBuildReq2 = "Archery Level 1";
        public const int ArchFoodCost = 300;
        public const int ArchWoodCost = 350;
        public const int ArchIronCost = 300;
        public const int ArchTimeCost = 5 * 60 + 50;//secs
        public const int ArchLife = 250;
        public const int ArchPopulation = 2;
        public const int ArchAttk = 120;
        public const int ArchDef = 50;
        public const int ArchLoad = 25;
        public const int ArchFoodCity = 9; //per hour
        public const int ArchSpeed = 250;
        public const int ArchRange = 1200;
        //public const int ArchQuest = { qtyArchs:10, award:{ food:3000, Wood:3500, iron:3000 } };

        public const string CavBuildReq = "Barracks Level 5";
        public const string CavBuildReq2 = "Horseback Riding Level 1";
        public const int CavFoodCost = 1000;
        public const int CavWoodCost = 600;
        public const int CavIronCost = 500;
        public const int CavTimeCost = 8 * 60 + 20;//secs
        public const int CavLife = 500;
        public const int CavPopulation = 3;
        public const int CavAttk = 250;
        public const int CavDef = 180;
        public const int CavLoad = 100;
        public const int CavFoodCity = 18; //per hour
        public const int CavSpeed = 1000;
        public const int CavRange = 100;
        //public const int CavQuest = { qtyCavs:10, award:{ food:10000, Wood:6000, iron:5000 } };

        public const string CatBuildReq = "Barracks Level 7";
        public const string CatBuildReq2 = "Iron Working Level 5";
        public const string CatBuildReq3 = "Horseback Riding Level 5";
        public const int CatFoodCost = 2000;
        public const int CatWoodCost = 500;
        public const int CatIronCost = 2500;
        public const int CatTimeCost = 25 * 60;//secs
        public const int CatLife = 1000;
        public const int CatPopulation = 6;
        public const int CatAttk = 350;
        public const int CatDef = 350;
        public const int CatLoad = 80;
        public const int CatFoodCity = 35; //per hour
        public const int CatSpeed = 750;
        public const int CatRange = 80;
        //public const int CatQuest = { qtyCats:10, award:{ food:8000, Wood:10000, iron:8000 } };

        public const string TransBuildReq = "Barracks Level 6";
        public const string TransBuildReq2 = "Logistics Level 1";
        public const string TransBuildReq3 = "Metal Casting Level 5";
        public const int TransFoodCost = 600;
        public const int TransWoodCost = 1500;
        public const int TransIronCost = 350;
        public const int TransTimeCost = 16 * 60 + 40;//secs
        public const int TransLife = 700;
        public const int TransPopulation = 4;
        public const int TransAttk = 10;
        public const int TransDef = 60;
        public const int TransLoad = 5000;
        public const int TransFoodCity = 10; //per hour
        public const int TransSpeed = 150;
        public const int TransRange = 10;
        //Logistics lvl 1= 10% inc= 5500, lvl10 = 100% = 10000

        public const string BallBuildReq = "Barracks Level 9";
        public const string BallBuildReq2 = "Archery Level 6";
        public const string BallBuildReq3 = "Metal Casting Level 5";
        public const int BallFoodCost = 2500;
        public const int BallWoodCost = 3000;
        public const int BallIronCost = 1800;
        public const int BallTimeCost = 50 * 60;//secs
        public const int BallLife = 320;
        public const int BallPopulation = 5;
        public const int BallAttk = 450;
        public const int BallDef = 160;
        public const int BallLoad = 35;
        public const int BallFoodCity = 50; //per hour
        public const int BallSpeed = 100;
        public const int BallRange = 1400;

        public const string RamBuildReq = "Barracks Level 9";
        public const string RamBuildReq2 = "Iron Working Level 8";
        public const string RamBuildReq3 = "Metal Casting Level 7";
        public const int RamFoodCost = 4000;
        public const int RamWoodCost = 6000;
        public const int RamIronCost = 1500;
        public const int RamTimeCost = 75 * 60;//secs
        public const int RamLife = 5000;
        public const int RamPopulation = 10;
        public const int RamAttk = 250;
        public const int RamDef = 160;
        public const int RamLoad = 45;
        public const int RamFoodCity = 50; //per hour
        public const int RamSpeed = 120;
        public const int RamRange = 600;

        public const string CataBuildReq = "Barracks Level 10";
        public const string CataBuildReq2 = "Archery Level 10";
        public const string CataBuildReq3 = "Metal Casting Level 10";
        public const int CataFoodCost = 5000;
        public const int CataStoneCost = 8000;
        public const int CataWoodCost = 5000;
        public const int CataIronCost = 1200;
        public const int CataTimeCost = 100 * 60;//secs
        public const int CataLife = 480;
        public const int CataPopulation = 8;
        public const int CataAttk = 600;
        public const int CataDef = 200;
        public const int CataLoad = 75;
        public const int CataFoodCity = 250; //per hour
        public const int CataSpeed = 80;
        public const int CataRange = 1500;
        //int CataQuest = { qtyCatas:10, award:{ food:500, Wood:1500, iron:100 } };
        #endregion

        #region City


        //string AcademyPrereq = "Town Hall Level 2";
        public const int AcademyFoodReq = 120; //reward prim
        public const int AcademyStoneReq = 1500;
        public const int AcademyWoodReq = 2500;
        public const int AcademyIronReq = 200;
        public const int AcademyTimeReq = 8 * 60;
        //lvl2 = lvl1*(2^currentlevel), etc. lvl 2 = 120 *2^2 =120*4
        //academyUnlocks
        //lvl1 = Agg, Wooding,MS
        //lvl2 = Masonry, mining,MT
        //lvl3=Metal Cast, info, Iron work
        //lvl4 Logistics, Compass,arch
        //lvl5 HBR, public const intruction
        //lvl6 stockpile, medicine
        //lvl7 ...nothing
        //lvl8 engineering
        //lvl9 machinery
        //lvl10 privatering.
        //GET quest reward for every table updrage.
        //lvl1 = prim guidline food 150,Wood 2500, stone 1500,iron 200 all * 2^currentlvl

        //string BarrPrereq = "Rally Spot Level 1";
        public const int BarrFoodReq = 250;
        public const int BarrStoneReq = 1500;
        public const int BarrWoodReq = 1200;
        public const int BarrIronReq = 500;
        public const int BarrTimeReq = 5 * 60;
        //Unlocks= lvl1 = worker, warr; lvl2=scout,pike, lvl3 Sword
        //lvl4arch, lvl5 cab, lvl6 trans, lvl7 cata
        //lvl8 nothingk, lvl9 balls, battRam, lvl10 cata
        //Quest = award for every lvl upgrade

        ///string BeaconPrereq = "Barracks Level 1";
        public const int BeaconFoodReq = 150;
        public const int BeaconStoneReq = 3000;
        public const int BeaconWoodReq = 1000;
        public const int BeaconIronReq = 500;
        public const int BeaconTimeReq = 7 * 60 + 30;
        //Unlocks= lvl1 = Pre inform invasion; lvl2= analysis of enemy purpose, lvl3 Show arrival time of enemy
        //lvl4 see enemy lord status lvl5 depart location of enemy lvl6 gives arms branch of enemy lvl7 approx numbers of enemy
        //lvl8 exact # of enemy lvl9 enemy lvl hero, lvl10 gives enemy MT
        //Quest = award for every lvl upgrade

        //Cotts
        //req after lvl2, requires (cottLvl-1) > Th.level
        public const int CottFoodReq = 100;//lvl2 = lvl1*(2^currentlevel)...lvl10 = 100 *2^9 ..100*12512
        public const int CottStoneReq = 100;
        public const int CottWoodReq = 500;
        public const int CottIronReq = 50;
        public const int CottTimeReq = 75;
        //public const int[] CottPopulationSupported =  [0, 100, 300, 600, 1000, 1500, 2100, 2800, 3600, 4500, 5500];
        //quest lvl1 = beg guid, amulet food:200 stone:200 wood:1000,iron:200, pop:30
        //quest lvl2=food:500 stone:500 wood:2000,iron:500,pop:50

        //warehouse
        public const int WareFoodReq = 100;//lvl2 = lvl1*(2^currentlevel)...
        public const int WareStoneReq = 1000;
        public const int WareWoodReq = 1500;
        public const int WareIronReq = 300;
        public const int WareTimeReq = 10 * 60;
        //public const int WareStorage = [0, 10000, 30000, 60000, 100000, 150000, 210000, 280000, 360000, 450000, 550000];
        //public const int wareQuest = //lv1 food:500 wood 2000, stone 1500,iron 500

        //string InnPrereq = "Cottage Level 2";
        public const int InnFoodReq = 300;//lvl2 = lvl1*(2^currentlevel)...
        public const int InnStoneReq = 1000;
        public const int InnWoodReq = 2000;
        public const int InnIronReq = 400;
        public const int InnTimeReq = 4 * 60;
        //QtyHeros = lvl of inn
        //quest lvl1 food:500,wood:2000,stone1000,iron500

        //shows heros, click to view or delete, view has rename,exp,upgrade,redist,reward
        // string FeastPrereq = "Inn Level 1";
        public const int FeastFoodReq = 400;//lvl2 = lvl1*(2^currentlevel)...
        public const int FeastStoneReq = 1200;
        public const int FeastWoodReq = 2500;
        public const int FeastIronReq = 700;
        public const int FeastTimeReq = 6 * 60;
        //heroqty=lvl
        //quest lvl1 gold1000,food1000,Wood3000, stone 1500, iron 1000

        //string EmbassyPrereq = "Town Hall Level 2";
        public const int EmbassyFoodReq = 200;
        public const int EmbassyStoneReq = 500;
        public const int EmbassyWoodReq = 2000;
        public const int EmbassyIronReq = 300;
        public const int EmbassyTimeReq = 12 * 60;
        //Garrisons allowed = lvl
        //Alliance limit = lvl*10
        //quest lvl1 food500, wood2000,stone1000,iron 500

        //string MarketPrereq = "";
        public const int MarketFoodReq = 1000;
        public const int MarketStoneReq = 1000;
        public const int MarketWoodReq = 1000;
        public const int MarketIronReq = 1000;
        public const int MarketTimeReq = 12 * 60 + 30;
        //transaction qty = lvl
        //quest lvl1 5cents, food1500,wood 1500,stone1500 iron 1500


        //string RallyPrereq = "";
        public const int RallyFoodReq = 100;
        public const int RallyStoneReq = 2000;
        public const int RallyWoodReq = 600;
        public const int RallyIronReq = 150;
        public const int RallyTimeReq = 2 * 60 + 30;
        //public const int RallyMaxTroopsSent = lvl*10000;
        //no quest
        //string ThPrereq = "Walls Level th-2";
        public const int ThFoodReq = 200; //lvl2 400, lvl3 800, lvl4 1600 == 100*2^lvl
        public const int ThStoneReq = 2500;//lvl2 5000 1250*2^lvl
        public const int ThWoodReq = 3000;//lvl2 6000 1500*2^lvl
        public const int ThIronReq = 100;//lvl2 200  50*2^lvl
        public const int ThTimeReq = 15 * 60;//lvl2 1800 15*60*2^(lvl-1)..1800, 3600, etc

        //        Level Prerequisite    Food Lumber  Stone Iron    Costs Time  Valleys
        // allowed Resource
        // Fields
        //2		400	6,000	5,000	200	30m 00s	2	16
        //3	Walls Lv.1	800	12,000	10,000	500	1h 00m 00s	3	19
        //4*	Walls Lv.2	1,600	24,000	20,000	800	2h 00m 00s	4	22
        //city image ..lv1 to 3..no walls shown. lvl4 shows little walls, lvl 7 shows big walls, 10 bigger



        //th quest rewards..
        //        Domain Expansion:Town Hall Upgrade
        //Level   Item Food    Lumber Stone   Iron
        //2	Primary Guidelines(1)  2,500	6,500	5,500	2,500
        //3		5,000	13,000	11,000	5,000
        //4		10,000	26,000	22,000	10,000
        //5		20,000	50,000	50,000	20,000
        //6		20,000	50,000	50,000	20,000
        //7		20,000	50,000	50,000	20,000
        //8		20,000	50,000	50,000	20,000
        //9		20,000	50,000	50,000	20,000
        //10	MichaelAngelo Script	20,000	50,000	50,000	20,000



        // string ForgePrereq = "Iron Mine Level 3";
        public const int ForgeFoodReq = 125;
        public const int ForgeStoneReq = 600;
        public const int ForgeWoodReq = 1000;
        public const int ForgeIronReq = 1200;
        public const int ForgeTimeReq = 3 * 60;
        //quest lvl1 f500,w1500, w2000, i 1500

        //string StablePrereq = "Farm Level 5";
        public const int StableFoodReq = 1200;
        public const int StableStoneReq = 800;
        public const int StableWoodReq = 2000;
        public const int StableIronReq = 1000;
        public const int StableTimeReq = 4 * 60 + 30;
        //each lvl = HBRlvl ..for barrs
        //quest lvl1 f1000,w5000, s3000, i 2500

        //string WorkshopPrereq = "Forge Level 2";
        public const int WorkshopFoodReq = 150;
        public const int WorkshopStoneReq = 500;
        public const int WorkshopWoodReq = 1500;
        public const int WorkshopIronReq = 1500;
        public const int WorkshopTimeReq = 9 * 60;
        //each lvl = hero star lvl max
        //quest lvl1 f1500,s5000, w5000, i 5000

        //string ReliefPrereq = "Stable Level 1 and HBR level = level";
        public const int ReliefFoodReq = 1500;
        public const int ReliefStoneReq = 4500;
        public const int ReliefWoodReq = 5000;
        public const int ReliefIronReq = 500;
        public const int ReliefTimeReq = 60 * 60;
        //lvl1-2 = 2x speed, lvl3 =3x, 5-7 4x, 8-9 = 5x, 10=6x
        //quest lvl1 f2000,s5000, w5000, i 1000

        ///walls...
       // string WallsPrereq = "Quarry lv2 and Workshop lvl1";
        public const int WallsFoodReq = 3000; // every level x2.. lvl2 = 6000, lvl3 12000
        public const int WallsStoneReq = 10000;// 3000* 2^(lvl-1)
        public const int WallsWoodReq = 1500;
        public const int WallsIronReq = 500;
        public const int WallsTimeReq = 30 * 60;// 30min
                                                //Durability 10000, 30000, 60000, 100000, etc
                                                //Fortified spaces 1000, 3000, 6000, 10000
                                                //Allows lvl1 Trap and th lvl3
                                                //lvl2 abatis and th lvl4
                                                //lvl3 AT and th lvl5
                                                //lvl5 Rolling log ...lvl7 Def Trebuchet.. lvlv 8 allow lvl10 th.

        //Rewards table
        //        Target Food    Lumber Stone   Iron
        //Level 1	3,000	2,000	10,000	2,000
        //Level 2	5,000	5,000	20,000	5,000
        //Level 3	10,000	10,000	50,000	10,000
        //Level 4	10,000	10,000	50,000	10,000
        //Level 5	10,000	10,000	50,000	10,000
        //Level 6	10,000	10,000	50,000	10,000
        //Level 7	10,000	10,000	50,000	10,000
        //Level 8	10,000	10,000	50,000	10,000
        //Level 9	10,000	10,000	50,000	10,000
        //Level 10	10,000	10,000	50,000	10,000

        //        Fortified Unit Type Vacant Space Per Unit Food    Lumber Stone   Iron Time per Unit
        //Trap	1 Space	50	500	100	50	1m
        //Abatis	2 Spaces	100	1,200	0	150	2m
        //Archer's Tower	3 Spaces	200	2,000	1,500	500	3m
        //Rolling log	4 Spaces	300	6,000	0	0	6m
        //Defensive Trebuchet	5 Spaces	600	0	8,000	0	10m



        #endregion

        #region Research
        //string AggPrereq = "Academy lvl1, farm Level = level";//lvl3 needs a farm lvl3
        public const int AggWoodReq = 500; //lvl3 500*2*2
        public const int AggGoldReq = 1000;
        public const int AggTimeReq = 6 * 60 + 40;
        //each lvl = 10% inc in food , lvl5 = 50%

        // string WoodingPrereq = "Academy lvl1, wood Level = level";//lvl3 needs a farm lvl3
        public const int WoodingWoodReq = 500; //lvl3 500*2*2
        public const int WoodingIronReq = 100; //lvl3 500*2*2
        public const int WoodingGoldReq = 1200;
        public const int WoodingTimeReq = 8 * 60 + 20;
        //each lvl = 10% inc in food , lvl5 = 50%

        //string MasonryPrereq = "Academy lvl2, wood Level = level";//lvl3 needs a farm lvl3
        public const int MasonryStoneReq = 500; //lvl3 500*2*2
        public const int MasonryIronReq = 200; //lvl3 500*2*2
        public const int MasonryGoldReq = 1500;
        public const int MasonryTimeReq = 10 * 60;
        //each lvl = 10% inc in food , lvl5 = 50%

        //string MiningPrereq = "Academy lvl2, Masonry Level 1";//lvl3 needs a ironmine lvl3
        public const int MiningIronReq = 800; //lvl3 500*2*2
        public const int MiningGoldReq = 2000;
        public const int MiningTimeReq = 11 * 60 + 40;
        //each lvl = 10% inc in food , lvl5 = 50%


        //string MetalCastingPrereq = "Academy lvl3, Mining Level 2";
        public const int MetalCastingWoodReq = 500;
        public const int MetalCastingIronReq = 500; //lvl3 500*2*2
        public const int MetalCastingGoldReq = 5000;
        public const int MetalCastingTimeReq = 15 * 60;
        //each lvl = 10% inc in production of Mechs (balls, rams, etc)

        //        Each level of Military Tradition enhances army attack by 5%.
        //Requires:
        //Academy Level 2
        //Military Science Level 1
        //Quest: Scientific Research:
        //Military Tradition Lv.1 Award: Gold 6,000


        //Level Requirements    Enhance in
        //army attack
        //Food Lumber  Iron Gold    Research
        //Time
        //1	800	120	200	3,000	00h 20m 00s	5%
        //2	1,600	240	400	6,000	00h 40m 00s	10%
        //3	3,200	480	800	12,000	1h 20m 00s	15%


//        Each level of Medicine enhances army's life by 5%.
//Requires:
//Academy Level 6
//Logistics Level 3
//Quest: Scientific Research:
//Medicine Lv.1 Award: Gold 7,200

//            Level Requirements    Increase
//in life
//Food    Gold Research
//Time
//1	1,500	3,600	30m 00s	5%
//2	3,000	7,200	1h 00m 00s	10%
//3	6,000	14,400	2h 00m 00s	15%

//            ===

//            The Following Formula can be used to determine the effect of your construction level

//[Base Building Time]*(0.9)^[Construction Level]

//        Example: 60*(0.9)^10 = 21.0 seconds

//        The following table breaks down the compounding build times.

//        Level   Total %	Compounded %	Adjusted time for 1 minute
//        1	10%	10.0%	54.0 seconds
//        2	20%	19.0%	48.6 seconds
//        3	30%	27.1%	43.7 seconds
//        Each level of Construction enhances construction speed of buildings and fortified units by 10%.
//Requires:
//Academy Level 5
//Lumbering Level 5
//Metal Casting Level 2
//Quest: Scientific Research:
//Construction Lv.1 Award: Gold 10,000

//Level Requirements    Increased
//construction
//speed
//Lumber  Stone Iron    Gold Research
//Time
//1	2,000	2,000	2,000	5,000	25m 40s	10%
//2	4,000	4,000	4,000	10,000	51m 21s	20%
//3	8,000	8,000	8,000	20,000	2h 00m 00s	30%

        #endregion


    }
}
