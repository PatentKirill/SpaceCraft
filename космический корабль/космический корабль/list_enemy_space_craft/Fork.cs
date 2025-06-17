using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль.list_enemy_space_craft
{
    class Fork: Enemy_space_craft
    {
        public Fork(int kol_details, char sign, ConsoleColor color, int HP) : base(kol_details, sign, color, HP)
        {
            form = new(int x, int y)[]
            {
                (1, 0), (2, 0), (3, 0), (4, 0),
                (1, 1), (3, 1)
            };
           attackMethods += attack_1;
           recharge_time_attack_1 = 2;
        }
        void attack_1(Pole pole)
        {
            if (timer_attack_1.Elapsed.TotalSeconds > recharge_time_attack_1)
            {
                for(int i = 5; i < details.Length; i++)
                {
                    new Bullets
                    (
                    '|', new Point(details[i].location.X, details[i].location.Y + 1), ConsoleColor.Green, 0.1, pole.ChunkHeight,
                    (LimitedInt.Zero, LimitedInt.One), pole
                    );
                    
                }
                timer_attack_1.Restart();
            }
        }
        public override Space_craft Clone()
        {
            return new Fork(details.Length, sign, details[0].color_sign, HP);
        }
        
    }
    
}
