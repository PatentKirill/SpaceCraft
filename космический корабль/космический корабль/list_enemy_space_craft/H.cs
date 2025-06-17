using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль.list_enemy_space_craft
{
    class H: Enemy_space_craft
    {
        public H(int kol_details, char sign, ConsoleColor color, int HP) : base(kol_details, sign, color, HP)
        {
            form = new (int x, int y)[]
            {
                (2, 0), 
                (0, 1),(1, 1), (2, 1),
                (0, 2),(2, 2)
            };
            attackMethods += attack_1;
            recharge_time_attack_1 = 1.8;
        }
        void attack_1(Pole pole)
        {
            if (timer_attack_1.Elapsed.TotalSeconds > recharge_time_attack_1)
            {
                
                for(int i = 0; i < details.Length; i++)
                {
                  
                    if(i != 3)
                    {
                       
                        if (i == 0 || i == 2 || i == 5)
                        {
                            new Bullets
                            (
                                '/', new Point(details[i].location.X- 1, details[i].location.Y), ConsoleColor.Green, 0.5, pole.ChunkHeight, 
                                (LimitedInt.NegativeOne, LimitedInt.One), pole
                            );
                        }
                        else
                        {
                            new Bullets
                            (
                                '\\', new Point(details[i].location.X + 1, details[i].location.Y), ConsoleColor.Green, 0.5, pole.ChunkHeight,
                                (LimitedInt.One, LimitedInt.One), pole
                            );
                        }
                    }
                    else
                    {
                        new Bullets
                            (
                                '|', new Point(details[i].location.X, details[i].location.Y + 1), ConsoleColor.Green, 0.5, pole.ChunkHeight,
                                (LimitedInt.Zero, LimitedInt.One), pole
                            );
                    }
                }
                timer_attack_1.Restart();

            }
        }
        public override Space_craft Clone()
        {
            return new H(details.Length, sign, details[0].color_sign, HP);
        }
    }
}
