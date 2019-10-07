﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulation_game
{
    class FactoryBuilding : Building
    {
        private Unit unitType;
        private int productionSpeed;
        private int spawnPoint;// either +1 or -1 for either above or below the factory 


        public FactoryBuilding(int x, int y, string team, int spawnValue, int ProductionSpeed = 10, string symbol ="F", int health = 20) : base(x, y, health, team, symbol )
        {
            spawnPoint = spawnValue;
            this.productionSpeed = ProductionSpeed;
        }

        public override void Kill()
        {
            symbol = "X";

        }

        public override string ToString()
        {
            if (Health <= 0)
            {
                return "Building: Destroyed";
            }
            else
            {
                return "Building: Factory \n Health: " + health + "\n Team: " + team;
            }
            
        }

        public int GetFactorySpeed()
        {
            return productionSpeed;
        }

        public Unit MakeUnit()
        {
            return unitType = new Ranged(xPos, yPos+spawnPoint, team);
        }


        public override Unit DoBuildingFunction()
        {
            return MakeUnit();//makes and returns a unit to be added into the map
        }

        public override string SaveData()
        {
            return $"F,{XPos},{YPos},{Team},{spawnPoint},{ProductionSpeed},{Health}";//returns with all stats of the building
        }



        //     properties
        public int ProductionSpeed
        {
            get { return productionSpeed; }
        }

        public override int XPos
        {
            get { return base.xPos; }
            set { base.xPos = value; }
        }

        public override int YPos
        {
            get { return base.yPos; }
            set { base.yPos = value; }
        }

        public int Health
        {
            get { return base.health; }
        }

        public override string Team
        {
            get { return base.team; }
        }

        public override string Symbol
        {
            get { return base.symbol; }
        }

        public override int ResourcePoolRemaining// useless to this class
        {
            get { return 0; }
        }

        

        public override int Speed
        {
            get { return productionSpeed; }
        }
    }
}
