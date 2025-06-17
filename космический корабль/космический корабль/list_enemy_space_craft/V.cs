using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль.list_enemy_space_craft
{
    class V : Enemy_space_craft
    {
        bool movement_to_the_right = true;
        bool movement_to_the_left = false;
        int distance_bullets;
        public V(int kol_details, char sign, ConsoleColor color, int HP) : base(kol_details, sign, color, HP)
        {
            form = new (int x, int y)[0];
            attackMethods += attack_1;
            recharge_time_attack_1 = 0.5;
            distance_bullets = 10;
        }
        public V(int kol_details, char sign, ConsoleColor color, int HP, double recharge_time_attack_1, int distance_bullets) : base(kol_details, sign, color, HP)
        {
            form = new (int x, int y)[0];
            attackMethods += attack_1;
            this.recharge_time_attack_1 = recharge_time_attack_1;
            this.distance_bullets = distance_bullets;
        }
        void attack_1(Pole pole)
        {
            if (timer_attack_1.Elapsed.TotalSeconds > recharge_time_attack_1)
            {
                Bullets bullets = new Bullets
                    (
                    '|', new Point(details[0].location.X, details[0].location.Y + 1), ConsoleColor.Green, 1, distance_bullets, (LimitedInt.Zero, LimitedInt.One), pole
                    );
                timer_attack_1.Restart();
                if(!movement_to_the_left)
                    movement_to_the_right = move(LimitedInt.Zero, LimitedInt.One, pole);
                if(!movement_to_the_right)
                {
                    movement_to_the_left = move(LimitedInt.Zero, LimitedInt.NegativeOne, pole);
                }
            }
        }
        public override Space_craft Clone()
        {
            return new V(details.Length, sign, details[0].color_sign, HP);
        }
    }
}
