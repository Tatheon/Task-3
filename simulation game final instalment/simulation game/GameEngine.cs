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
                    Unit sample = building.DoBuildingFunction();

                    if (sample != null)
                    {
                        Array.Resize( ref world.players, world.players.Length+1);
                        world.players[world.players.Length-1] = sample; 
                    }
                }
            }
            world.UpdateWorld();

            foreach (Unit unit in world.players)
            {
                if (unit.IsDead) { continue; }

                unit.NearestBuilding(world.buildings);
                unit.NearestEnemy(world.players);

                if(unit.ClosestUnit == null)
                {
                    gameOver = true;
                    winningFaction = unit.Team;
                    world.UpdateWorld();
                    return;
                }

                if (unit is Wizzard)
                {
                    WizardLogic((Wizzard)unit);
                }
                else
                {
                    BasicUnitLogic(unit);
                }

                world.UpdateWorld();
                
            }
            rounds++;
        }

        public void BasicUnitLogic(Unit unit)
        {
            double healthPercent = unit.Health / unit.MaxHealth * 100;
            if (healthPercent <= 25)
            {
                unit.RandomMove(world.MapWidth, world.MapHeight);
            }
            else if (unit.ClosestBuilding != null & unit.WithinRange(unit.ClosestBuilding))
            {
                unit.Attack(unit.ClosestBuilding);
            }
            else if (unit.WithinRange())
            {
                unit.Attack(unit.ClosestUnit);
            }
            else
            {
                unit.Move();
            }
        }

        public void WizardLogic(Wizzard wizzard)
        {
            bool action = false;
            double healthPercent = wizzard.Health / wizzard.MaxHealth * 100;
            if (healthPercent <= 50)
            {
                wizzard.RandomMove(world.MapWidth, world.MapHeight);
                action = true;
                
            }
            else if (!action)
            {
              foreach (Unit unit in world.players )
              {
                if (wizzard.WithinRange(unit) & unit.Team != "C")
                {
                    wizzard.Attack(unit);
                    action = true;
                }
              }
            }

            if (!action)
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
