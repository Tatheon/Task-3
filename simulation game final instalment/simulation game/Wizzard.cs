using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulation_game
{
    class Wizzard:Unit
    {
        public Wizzard(int x, int y, string team, int health = 10, int speed = 2, int attack = 3, int range = 2, string symbol = " @  ") : base(x, y, health, speed, attack, range, team, symbol)
        {
            name = "Blizzard";
        }

        public override string SaveData()
        {
            return $"W,{Xvalue},{Yvalue},{Team},{Health},{speed},{MaxHealth}";
        }    

        public override string ToString()
        {
            if (Health <= 0)
            {
                return "Wizard: DEAD";
            }
            else
            {
                return "Wizard unit: " + name + " \n Health: " + Health + "\n Range: " + attackRange + "\n Speed: " + speed + "\n Team: " + team;
            }
        }

    }

    
}
