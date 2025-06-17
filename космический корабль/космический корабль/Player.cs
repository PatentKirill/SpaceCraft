using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль
{
    class Player : Space_craft
    {
        int kol = 10;
        Stopwatch timer_machine_gun;
        double recharge_time_machine_gun;
        int kol_use_flamethrower = 10;
        Stopwatch timer_flamethrower = new Stopwatch();
        double recharge_time_flamethrower = 5;

        public Player(int kol_details, char sign, ConsoleColor color, int HP) : base(kol_details, sign, color, HP)
        {
            timer_flamethrower.Start();
            timer_machine_gun = new Stopwatch();
            recharge_time_machine_gun = 0.56;
            timer_machine_gun.Start();
            form = new (int x, int y)[]
            {
                (-1, 1), (0, 1), (1, 1)
            };
        }

        public void machine_gun(Pole pole)
        {
            if (timer_machine_gun.Elapsed.TotalSeconds > recharge_time_machine_gun)
            {
                Bullets bullets = new Bullets
                    (
                    '|', new Point(details[0].location.X, details[0].location.Y - 1), ConsoleColor.Blue, 0.5, pole.ChunkHeight, (LimitedInt.Zero, LimitedInt.NegativeOne), pole, false
                    );
                
                timer_machine_gun.Restart();
            }
        }
        public void flamethrower(Pole pole)
        {
            if(kol_use_flamethrower > 0 && timer_flamethrower.Elapsed.TotalSeconds > recharge_time_flamethrower)
            {
                new Bullets
                    (
                    '|', new Point(details[0].location.X, details[0].location.Y - 1), ConsoleColor.DarkYellow, 0, 0, (LimitedInt.Zero, LimitedInt.Zero), pole, false
                    );

                for (int i = 2; i <= 3; i++)
                {
                    new Bullets
                       (
                       '|', new Point(details[0].location.X, details[0].location.Y - i), ConsoleColor.DarkYellow, 0, 0, (LimitedInt.Zero, LimitedInt.NegativeOne), pole, false
                       );
                    new Bullets
                       (
                       '|', new Point(details[0].location.X - 1, details[0].location.Y - i), ConsoleColor.DarkYellow, 0, 0, (LimitedInt.Zero, LimitedInt.NegativeOne), pole, false
                       );
                    new Bullets
                           (
                           '|', new Point(details[0].location.X + 1, details[0].location.Y - i), ConsoleColor.DarkYellow, 0, 0, (LimitedInt.Zero, LimitedInt.NegativeOne), pole, false
                           );
                }
                kol_use_flamethrower--;
            }
            else
            {
                if(kol_use_flamethrower <= 0)
                {
                    kol_use_flamethrower = kol;
                    timer_flamethrower.Restart();
                }
            }
            
        }
        public override Space_craft Clone()
        {
            return new Player(details.Length, sign, details[0].color_sign, HP);
        }

    }
}
