using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;
using космический_корабль.list_boss;
using космический_корабль.list_enemy_space_craft;

namespace космический_корабль
{
    enum LimitedInt
    {
        NegativeOne = -1,
        Zero = 0,
        One = 1
    }
   
    
    
    
    
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Pole po = new Pole();
            po.Print();
            Stopwatch timer_move_w = new Stopwatch();
            timer_move_w.Start();
            while (true)
            {
                var stopwatch = Stopwatch.StartNew();
                for(int i = 0; i < po.mas_space_crafts.Count; i++)
                {
                    if (po.mas_space_crafts[i] is Enemy_space_craft)
                    {
                        Enemy_space_craft enemy_space_craft = po.mas_space_crafts[i] as Enemy_space_craft;
                        enemy_space_craft.assault(po);
                    }
                }
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    if (key == ConsoleKey.W)
                    {
                        po.player_move(LimitedInt.NegativeOne, LimitedInt.Zero);
                    }
                    else if(key == ConsoleKey.S)
                    {
                        po.player_move(LimitedInt.One, LimitedInt.Zero);
                        
                    }
                    else if (key == ConsoleKey.A)
                    {
                        po.player_move(LimitedInt.Zero, LimitedInt.NegativeOne);
                    }
                    else if (key == ConsoleKey.D)
                    {
                        po.player_move(LimitedInt.Zero, LimitedInt.One);
                    }
                    else if(key == ConsoleKey.Spacebar)
                    {
                        po.player.machine_gun(po);
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        po.player.flamethrower(po);
                    }
                }
                 
                    


                po.flight_bullets();
                
                if (po.player == null)
                {
                    Console.Clear();
                    Console.WriteLine("Вы проиграли");
                    Console.ReadLine();
                    break;
                }
                if (po.boss == null)
                {
                    Console.Clear();
                    Console.WriteLine("Вы победили");
                    Console.ReadLine();
                    break;
                }
                // Ограничение до ~60 FPS
                int frameTime = (int)stopwatch.ElapsedMilliseconds;
                int delay = Math.Max(0, 8 - frameTime); // 16ms ~ 60 FPS
                Thread.Sleep(delay);
            }
                
            
        }
    }
    
    class Dictionary
    {
        public static Dictionary<char, Object> Object = new Dictionary<char, Object>
        {

        };

       public  static Player player = new Player(4, '*', ConsoleColor.Red, 3);
        static V v = new V(1, '#', ConsoleColor.White, 2);
        static Boss_V boss_V = new Boss_V(1, '$', ConsoleColor.White, 13);
        static Fork fork = new Fork(7, '@', ConsoleColor.White, 10);
        static H h = new H(7, '=', ConsoleColor.White, 10);
        public static Dictionary<char, Space_craft> Space_craft = new Dictionary<char, Space_craft>
        {
            { player.sign, player },
            {v.sign, v },
            {boss_V.sign, boss_V },
            {fork.sign, fork },
            {h.sign, h }
        };
    }
}




