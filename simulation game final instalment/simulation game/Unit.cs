﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulation_game
{
    abstract class Unit
    {
        double closestDistance = 1000;
        Unit closestUnit = null;
        Building closestBuilding = null;

        protected int x, y, maxHealth, health, speed, attack, attackRange;
        protected string team, symbol,name;
        protected bool isInAction, isDead;
        protected Random r = new Random();

        public void Move()
        {
            int xDifference;
            int yDifference;
           
            for (int i = 0; i < speed; i++)
            {
                if (closestBuilding != null)
                {
                    xDifference = closestBuilding.XPos - Xvalue;
                    yDifference = closestBuilding.YPos - Yvalue;
                }
                else
                {
                    xDifference = closestUnit.Xvalue - Xvalue;
                    yDifference = closestUnit.Yvalue - Yvalue;
                }
               

                if (yDifference == xDifference & yDifference <= 0 & xDifference <= 0)//diagonal movement
                {
                    Xvalue--;
                    Yvalue--;
                }
                else if (Math.Abs(yDifference) == Math.Abs(xDifference) & yDifference >= 0 & xDifference <= 0)
                {
                    Xvalue--;
                    Yvalue++;
                }
                else if (Math.Abs(yDifference) == Math.Abs(xDifference) & yDifference <= 0 & xDifference >= 0)
                {
                    Xvalue++;
                    Yvalue--;
                }
                else if (Math.Abs(yDifference) == Math.Abs(xDifference) & yDifference >= 0 & xDifference >= 0)
                {
                    Xvalue++;
                    Yvalue++;
                }
                else if (Math.Abs(xDifference) < Math.Abs(yDifference) & yDifference < 0)//up dowm left right
                    {
                        Yvalue--;
                    }
                    else if (Math.Abs(xDifference) < Math.Abs(yDifference) & yDifference > 0)
                    {
                        Yvalue++;
                    }
                    else if (Math.Abs(xDifference) > Math.Abs(yDifference) & xDifference > 0)
                    {
                        Xvalue++;
                    }
                    else if (Math.Abs(xDifference) > Math.Abs(yDifference) & xDifference < 0)
                    {
                        Xvalue--;
                    }
                    else
                    {
                        //have an else do nothing incase if something weared were to occure so nothing will crash
                    } 
            }
        }

        

        public void RandomMove(int mapWidth, int mapHeight)
        {
            int tryX;
            int tryY;
            tryX = Xvalue;
            tryY = Yvalue;
            bool movable = false;


            while (movable == false)
            {
                switch (r.Next(8))
                {
                    case 0://up
                        tryY--;
                        break;
                    //////////////////////////////////////////////////////////////////////////////////                                                                                                                                                                                                          property of christopher kessler
                    case 1://down
                        tryY++;
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////
                    case 2://left
                        tryX--;
                        break;
                    ///////////////////////////////////////////////////////////////////////////////
                    case 3://right
                        tryX++;
                        break;
                    /////////////////////////////////////////////////////////////////////////////////
                    case 4://up left
                        tryX--;
                        tryY--;
                        break;
                    //////////////////////////////////////////////////////////////////////////////////
                    case 5://up right
                        tryX++;
                        tryY--;
                        break;
                    ///////////////////////////////////////////////////////////////////////////////////
                    case 6://down left
                        tryX--;
                        tryY++;
                        break;
                    //////////////////////////////////////////////////////////////////////////////////////
                    case 7://down right
                        tryX++;
                        tryY++;
                        break;
                }

                if (CanMove(tryX, tryY, mapWidth, mapHeight))
                {
                    movable = true;
                }
            }
            Xvalue = tryX;
            Yvalue = tryY;
        }

        
       

        public bool CanMove(int x, int y, int mapWidth, int mapHeight)
        {
            bool clear = true;
            if (x < 0 | x >= mapWidth | y < 0 | y >= mapHeight)
            {
                clear = false;
            }
            return clear;
        }

        public bool WithinRange()
        {
            double distance = 0;
            int Xdistance;
            int Ydistance;

            Xdistance = closestUnit.Xvalue - Xvalue;
            Ydistance = closestUnit.Yvalue - Yvalue;
            distance = Math.Sqrt(Xdistance * Xdistance + Ydistance * Ydistance);

            if (distance <= attackRange)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool WithinRange(Building building)
        {
            double distance = 0;
            int Xdistance;
            int Ydistance;
            if (building == null)
            {
                return false;   // if there are no buildings left this will be null, hence this way nothing will crash.
            }
            Xdistance = building.XPos - Xvalue;
            Ydistance = building.YPos - Yvalue;
            distance = Math.Sqrt(Xdistance * Xdistance + Ydistance * Ydistance);

            if (distance <= attackRange)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void NearestEnemy(Unit[] units)
        {
            closestDistance = int.MaxValue;
            closestUnit = null;

            foreach (Unit unitFocus in units)
            {
                double distance = 0;
                int Xdistance;
                int Ydistance;
                if (unitFocus != this & unitFocus.Team != Team & unitFocus.IsDead == false)
                {
                    Xdistance = unitFocus.Xvalue - Xvalue;
                    Ydistance = unitFocus.Yvalue - Yvalue;
                    distance = Math.Sqrt(Xdistance * Xdistance + Ydistance * Ydistance);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestUnit = unitFocus;
                    }
                }
            }
        }

        public void NearestBuilding(Building[] buildings)
        {
            closestDistance = int.MaxValue;
            closestUnit = null;

            foreach (Building BuildingFocus in buildings)
            {
                double distance = 0;
                int Xdistance;
                int Ydistance;
                if (BuildingFocus.Team != Team & BuildingFocus.Health > 0)
                {
                    Xdistance = BuildingFocus.XPos - Xvalue;
                    Ydistance = BuildingFocus.YPos - Yvalue;
                    distance = Math.Sqrt(Xdistance * Xdistance + Ydistance * Ydistance);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestBuilding = BuildingFocus;
                    }
                }
            }
        }

        public void Kill()
        {
            Symbol = "X";
            IsDead = true;
        }

        public override string ToString()
        {
            return "ye";
        }

        public Unit(int x, int y, int health, int speed, int attack, int range, string team, string symbol)
        {
            this.x = x;
            this.y = y;
            this.health = health;
            this.maxHealth = health;
            this.speed = speed;
            this.attack = attack;
            this.attackRange = range;
            this.team = team;
            this.symbol = symbol;
            isDead = false;
        }

        public void Attack(Unit otherUnit)
        {
            otherUnit.Health -= attack;
            isInAction = false;

            if (otherUnit.Health <= 0)
            {
                otherUnit.Kill();
            }
        }

        public void Attack(Building otherBuilding)
        {
            otherBuilding.Health -= attack;
            isInAction = false;

            if (otherBuilding.Health <= 0)
            {
                otherBuilding.Kill();
            }
        }

        public abstract string SaveData();

        //    acess statements

        public int Xvalue
        {
            get { return x; }
            set { x = value; }
        }

        public Unit ClosestUnit
        {
            get { return closestUnit; }
        }

        public Building ClosestBuilding
        {
            get { return closestBuilding; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int Yvalue
        {
            get { return y; }
            set { y = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public string Team
        {
            get { return team; }
        }

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }
        

    
    }
}
