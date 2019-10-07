using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulation_game
{
    class Wizzard:Unit
    {
        public Wizzard(int x, int y, string team, int health = 20, int speed = 1, int attack = 10, int range = 2, string symbol = " @  ") : base(x, y, health, speed, attack, range, team, symbol)
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

        

        public bool WithinRange(Unit unit)
        {
            double distance = 0;
            int Xdistance;
            int Ydistance;

            Xdistance = unit.Xvalue - Xvalue;
            Ydistance = unit.Yvalue - Yvalue;
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

        public void Move(Unit unit)
        {
            int xDifference;
            int yDifference;

            for (int i = 0; i < speed; i++)
            {

                 xDifference = unit.Xvalue - Xvalue;
                 yDifference = unit.Yvalue - Yvalue;

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

    }    
}
