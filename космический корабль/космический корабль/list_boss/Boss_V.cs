using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using космический_корабль.list_enemy_space_craft;

namespace космический_корабль.list_boss
{
    class Boss_V: Boss
    {
        private int _hp;
        ConsoleColor orig_color;
        Stopwatch timer_stun = new Stopwatch();
        double time_stun = 3;
        protected Stopwatch timer_attack_2 = new Stopwatch();
        protected double recharge_time_attack_2 = 10;
        public Boss_V(int kol_details, char sign, ConsoleColor color, int HP) : base(kol_details, sign, color, HP)
        {
            orig_color = color;
            form = new (int x, int y)[0];
            attackMethods += attack_1;
            attackMethods += attack_2;
            recharge_time_attack_1 = 6;
            timer_attack_2.Start();
            _hp = HP;
        }
        public override int HP 
        {
            set 
            {
                if(value == HP - 1)
                {
                    details[0].color_sign = orig_color;
                    timer_stun.Restart();
                }
                base.HP = value;  // Сперва стандартная обработка
                
                
            }
        }
                
        void attack_1(Pole pole)
        {
            if (stun_check())
                return;
            if (timer_attack_1.Elapsed.TotalSeconds > recharge_time_attack_1)
            {
                teleportation(pole);
                for (int x = (details[0].location.X - 2); x < (details[0].location.X + 2); x++)
                {
                    new Bullets
                            (
                                '|', new Point(x, details[0].location.Y + 1), ConsoleColor.Green, 0.5, pole.ChunkHeight,
                                (LimitedInt.Zero, LimitedInt.One), pole
                            );
                }
                timer_attack_1.Restart();
                
            }
        }
        void attack_2(Pole pole)
        {
            if (stun_check())
                return;
            if (timer_attack_2.Elapsed.TotalSeconds > recharge_time_attack_2 && pole.mas_space_crafts.Count < 3)
            {
               
                teleportation(pole);
                for (int i = details[0].location.X - 1, y = 1; i <= details[0].location.X + 1; i += 2, y++)
                {
                    if(y < pole.raz_i && i < pole.raz_g && i >=0)
                    {
                        if (pole.pole[details[0].location.Y - y, i] == null)
                        {
                            V v = new V(1, '#', ConsoleColor.White, 2, 0.8, pole.ChunkHeight);
                            v.details[0].location = new Point(i, details[0].location.Y - y);
                            v.add_detail_to_pole(pole.pole);
                            pole.mas_space_crafts.Add(v);
                        }
                    }
                    
                    
                }
                timer_attack_2.Restart();
                
            }
        }
        void teleportation(Pole pole)
        {
            Random rand = new Random();
            while(true)
            {
                int x = rand.Next(pole.raz_g);
                int y = rand.Next(pole.startY, pole.endY);
                if (pole.pole[y, x] == null && pole.player?.details[0].location.Y > y)
                {
                    pole.pole[y, x] = details[0];
                    pole.pole[details[0].location.Y, details[0].location.X] = null;
                    details[0].location = new Point(x, y);
                    break;
                }
            }
        }
        bool stun_check()
        {
            
            if((timer_stun.Elapsed.TotalSeconds < time_stun && timer_stun.Elapsed.TotalSeconds != 0) )
            {
                return true;
            }
            if (orig_color == details[0].color_sign)
                details[0].color_sign = Console.BackgroundColor;
            return false;
        }
        public override Space_craft Clone()
        {
            return new Boss_V(details.Length, sign, details[0].color_sign, HP);
        }
        public override void appearance(Pole pole)
        {
            details[0].color_sign = Console.BackgroundColor ;
            pole.Print();
        }
    }
}
