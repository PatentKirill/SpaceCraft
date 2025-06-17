using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль
{
    class Enemy_space_craft: Space_craft
    {
        protected delegate void Attacks(Pole pole);

        protected Attacks attackMethods;

        protected Stopwatch timer_attack_1;
        protected double recharge_time_attack_1;
        public Enemy_space_craft(int kol_details, char sign, ConsoleColor color, int HP): base(kol_details, sign, color, HP)
        {
            timer_attack_1 = new Stopwatch();
            recharge_time_attack_1 = 3;
            timer_attack_1.Start();
        }
        public void assault(Pole pole)
        {
            if (pole.GetCurrentChunkY() == (this.details[0].location.Y / pole.ChunkHeight) )
            {
                Random rand = new Random();
                if (attackMethods == null)
                    return;
                int num = rand.Next(attackMethods.GetInvocationList().GetLength(0));
                Delegate[] methods = attackMethods.GetInvocationList();

                Attacks selectedAttack = (Attacks)methods[num];
                selectedAttack.Invoke(pole);
            }
            
        }
    }
    
}
