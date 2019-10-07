using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace simulation_game
{
    class GameEngine
    {
        private int rounds = 1;//after all units have had their turn this is incremented
        public Map world = new Map();
        public bool gameOver = false;
        string winningFaction;

        public void Run()
        {
            foreach (Building building in world.buildings)
            {
                if (building.Health <= 0) { continue; }

                if (rounds % building.Speed == 0)  // find mod to see if the building can perform its function
                {
                    Unit sample = building.DoBuildingFunction();//if the returned unit in sample is null then the building was a recource building, 

                    if (sample != null)
                    {
                        Array.Resize( ref world.players, world.players.Length+1);//else add the new unit to the player array
                        world.players[world.players.Length-1] = sample; 
                    }
                }
            }
            world.UpdateWorld();

            foreach (Unit unit in world.players)
            {
                if (unit.IsDead) { continue; }//if the unit is dead, dont waist your bread.....

                unit.NearestBuilding(world.buildings);//find a building
                unit.NearestEnemy(world.players);//find a unit

                if(unit.ClosestUnit == null)
                {
                    gameOver = true;
                    winningFaction = unit.Team;
                    world.UpdateWorld();
                    return;
                }

                if (unit is Wizzard)
                {
                    WizardLogic((Wizzard)unit);//if the unit is a wizzard, the unit must do what the wizzard should do
                }
                else
                {
                    BasicUnitLogic(unit);//if the unit is just a unit, it, must do what a normal unit must do
                }

                world.UpdateWorld();
                
            }
            rounds++;
        }

        public void BasicUnitLogic(Unit unit)
        {
            double healthPercent = unit.Health / unit.MaxHealth * 100;
            if (healthPercent <= 25)//if the unit is low on health it should run away
            {
                unit.RandomMove(world.MapWidth, world.MapHeight);
            }
            else if (unit.ClosestBuilding != null & unit.WithinRange(unit.ClosestBuilding))//if there is any enemy buildings and they are in range attack
            {
                unit.Attack(unit.ClosestBuilding);
            }
            else if (unit.WithinRange())//if the unit did not attack an enemy building then see if it can attack a unit.
            {
                unit.Attack(unit.ClosestUnit);
            }
            else//if they cant reach their enemy then they must move closer to their enemy.
            {
                unit.Move();
            }
        }

        public void WizardLogic(Wizzard wizzard)
        {
            bool action = false;
            double healthPercent = wizzard.Health / wizzard.MaxHealth * 100;
            if (healthPercent <= 50)//if the wizard is a tad low on health then they must flee for their lives
            {
                wizzard.RandomMove(world.MapWidth, world.MapHeight);
                action = true;
                
            }
            else if (!action)//if the wizard didnt do anything already then they must see if they can attack anything in its AOE of DOOM
            {
              foreach (Unit unit in world.players )//if anyone of these poor suckers are within attack distance, the wizard must attck them
              {
                if (wizzard.WithinRange(unit) & unit.Team != "C")
                {
                    wizzard.Attack(unit);
                    action = true;
                }
              }
            }

            if (!action)//if the wizard still has not done anything then the wizard must move to a neerby unit
            {
                wizzard.Move(wizzard.ClosestUnit);
            }
        }

        public int Rounds
        {
            get { return rounds; }
            set { rounds = value; }
        }

        public string WinningTeam
        {
            get { return winningFaction; }
        }
    }
}
