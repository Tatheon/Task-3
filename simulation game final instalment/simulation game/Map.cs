using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace simulation_game
{
    class Map
    {
        public string[,] map;

        public Unit[] players;
        public Building[] buildings;

        
        public Random r = new Random();      //r.Next(1,20);   random number within the array's bounds
        string[] teams = { "A", "B", "C" };

        private int mapWidth;
        private int mapHeight;


        const string FILENAME = "MapData.txt";
        const string FILENAMEMAP = "MettaData.txt";
        const char DELIMITER = ',';

        enum UnitSTAT  { Symbol = 0, X = 1, Y = 2, Team = 3, Health = 4, Speed =5, MaxHealth = 6}
        enum FactorySTAT { Symbol = 0, X = 1, Y = 2, Team = 3, Spawnpoint = 4, ProductionSpeed = 5, Health = 6}
        enum ResourceSTAT { Symbol = 0, X = 1, Y = 2, Team = 3, Health = 4, ResourcePerRound = 5, ResourcePool = 6}

        public Map()
        {
            
        }

        public void setWorld(int numUnits, int numBuildings, string[] mapSize)//multi use method that creates and sets up the map
        {
            mapWidth = (int.Parse)(mapSize[0]);
            mapHeight = (int.Parse)(mapSize[1]);

            map = new string[mapWidth, mapHeight];

            players = new Unit[numUnits];
            buildings = new Building[numBuildings];
            WorldBuild();
        }

        

        public void WorldBuild()
        {
            
            for (int i = 0; i < players.Length; i++)//creates the player types and randomizes their positions
            {
                int unitType = r.Next(3);
                if ( unitType == 0)
                {
                    players[i] = new Ranged(r.Next(1, mapWidth), r.Next(0, mapHeight), teams[r.Next(2)]);   // make a Ranged Boi

                }
                else if(unitType == 1)
                {
                    players[i] = new Melee(r.Next(1, mapWidth), r.Next(0, mapHeight), teams[r.Next(2)]);   // make a melee Boi
                }
                else
                {
                    players[i] = new Wizzard(r.Next(1, mapWidth), r.Next(0, mapHeight), teams[2]);  // make a wizard Boi
                }
            }

            for (int i = 0; i < buildings.Length; i++)//creates the building types and randomizes their positions
            {
                if (r.Next(2) == 0)
                {
                    int yValue = r.Next(1, mapHeight);
                    int SpawnVal;
                    if (yValue == 0)
                    {
                        SpawnVal = -1;
                    }
                    else
                    {
                        SpawnVal = 1;
                    }

                    buildings[i] = new FactoryBuilding(r.Next(1, mapWidth), yValue, teams[r.Next(2)], SpawnVal);//make a factory

                }
                else
                {
                    buildings[i] = new ResourceBuilding(r.Next(1, mapWidth), r.Next(0, mapHeight), teams[r.Next(2)]);//make a recource building
                }
            }

            UpdateWorld();
        }

        public void UpdateWorld()//update the map of any changes
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    map[x, y] = "  '  ";
                }
            }
            foreach(Building structure in buildings)
            {
                map[structure.XPos, structure.YPos] = structure.Team + structure.Symbol;
            }

            foreach (Unit playerType in players)
            {
                map[playerType.Xvalue, playerType.Yvalue] =  playerType.Team + playerType.Symbol; //as each unit moves the entire array will need to be displayed so this method will surfise to update the positions of the units on map after they are altered by game engine or something
            }
        }

        public int LoadWorld()
        {
            FileStream infile = new FileStream(FILENAME, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader reader = new StreamReader(infile);
            string line = reader.ReadLine();
            string[] details;

            buildings = new Building[0];           
            players = new Unit[0];

            while (line!=null)
            {

                details = line.Split(DELIMITER);

                switch ( details[ (int)UnitSTAT.Symbol] )
                {
                    case "M":
                        Array.Resize(ref players, players.Length+1);
                        players[players.Length - 1] = new Melee((int.Parse)(details[(int)UnitSTAT.X]), (int.Parse)(details[(int)UnitSTAT.Y]), details[(int)UnitSTAT.Team], (int.Parse)(details[(int)UnitSTAT.Health]), (int.Parse)(details[(int)UnitSTAT.Speed]) );
                        players[players.Length - 1].MaxHealth = (int.Parse)(details[(int)UnitSTAT.MaxHealth]);
                        break;

                    case "R":
                        Array.Resize(ref players, players.Length + 1);
                        players[players.Length - 1] = new Ranged((int.Parse)(details[(int)UnitSTAT.X]), (int.Parse)(details[(int)UnitSTAT.Y]), details[(int)UnitSTAT.Team], (int.Parse)(details[(int)UnitSTAT.Health]), (int.Parse)(details[(int)UnitSTAT.Speed]));
                        players[players.Length - 1].MaxHealth = (int.Parse)(details[(int)UnitSTAT.MaxHealth]);
                        break;

                    case "W":
                        Array.Resize(ref players, players.Length + 1);
                        players[players.Length - 1] = new Wizzard((int.Parse)(details[(int)UnitSTAT.X]), (int.Parse)(details[(int)UnitSTAT.Y]), details[(int)UnitSTAT.Team], (int.Parse)(details[(int)UnitSTAT.Health]), (int.Parse)(details[(int)UnitSTAT.Speed]));
                        players[players.Length - 1].MaxHealth = (int.Parse)(details[(int)UnitSTAT.MaxHealth]);
                        break;

                    case "F":
                        Array.Resize(ref buildings, buildings.Length + 1);
                        buildings[buildings.Length - 1] = new FactoryBuilding((int.Parse)(details[(int)FactorySTAT.X]), (int.Parse)(details[(int)FactorySTAT.Y]), details[(int)FactorySTAT.Team], (int.Parse)(details[(int)FactorySTAT.Spawnpoint]), (int.Parse)(details[(int)FactorySTAT.ProductionSpeed]), details[(int)FactorySTAT.Symbol], (int.Parse)(details[(int)FactorySTAT.Health]));
                        break;

                    case "RE":
                        Array.Resize(ref buildings, buildings.Length + 1);
                        buildings[buildings.Length - 1] = new ResourceBuilding((int.Parse)(details[(int)ResourceSTAT.X]), (int.Parse)(details[(int)ResourceSTAT.Y]), details[(int)ResourceSTAT.Team], (int.Parse)(details[(int)ResourceSTAT.Health]), "R", (int.Parse)(details[(int)ResourceSTAT.ResourcePerRound]), (int.Parse)(details[(int)ResourceSTAT.ResourcePool]));
                        break;
                }
                line = reader.ReadLine();

            }

            int round = LoadMapInfo();
            UpdateWorld();

            

            reader.Close();
            infile.Close();

            return round;
        }

        public int LoadMapInfo()
        {
            FileStream infile = new FileStream(FILENAMEMAP, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader reader = new StreamReader(infile);

            string[] dimentions = reader.ReadLine().Split(DELIMITER);
            mapWidth = (int.Parse)(dimentions[0]);
            mapHeight = (int.Parse)(dimentions[1]);
            map = new string[mapWidth, mapHeight];

            int round = (int.Parse)(reader.ReadLine());

            reader.Close();
            infile.Close();

            return round;
        }

        public void SaveWorld(int round)
        {
            
            FileStream outfile = new FileStream(FILENAME, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outfile);
            
            foreach(Building building in buildings)
            {
                writer.WriteLine(building.SaveData());
            }

            foreach (Unit unit in players)
            {
                writer.WriteLine(unit.SaveData());
            }

            saveMapInfo(round);

            writer.Close();
            outfile.Close();
        }


        public void saveMapInfo(int round)
        {
            FileStream outfile = new FileStream(FILENAMEMAP, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outfile);

            writer.WriteLine(mapWidth + "," + mapHeight);
            writer.WriteLine(round);

            writer.Close();
            outfile.Close();
        }

        public int MapHeight
        {
            get { return mapHeight; }
        }

        public int MapWidth
        {
            get { return mapWidth; }
        }
    }
}
