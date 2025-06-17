using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль
{
    class Pole
    {
        public Object?[,] pole { get; set; }
        public Player? player { get; set; }
        public Boss? boss { get; set; }

        int viewportHeight = 10;
        public List<Bullets> mas_bullets { get; set; } = new List<Bullets>();
        public List<Space_craft> mas_space_crafts { get; set; } = new List<Space_craft>();
        public int raz_i { get; set; }
        public int raz_g { get; set; }
        public Pole()
        {
            string[] lines = File.ReadAllLines("lvl1.txt");
            raz_i = lines.Length;
            raz_g = lines[0].Length;
            (char sign, bool used)[,] char_pole = new (char, bool)[raz_i, raz_g];
            pole = new Object?[raz_i, raz_g];
            int nextChar;

            StreamReader file = new StreamReader("lvl1.txt");
            int i = 0, g = 0;
            while ((nextChar = file.Read()) != -1)
            {
                // Полу
                char c = (char)nextChar;
                if (c == '\r')
                {
                    i++;
                    g = 0;
                    file.Read();

                }

                else
                {
                    char_pole[i, g] = (c, false);
                    g++;
                }

            }
            for (i = 0; i < raz_i; i++)
            {
                for (g = 0; g < raz_g; g++)
                {
                    if (Dictionary.Space_craft.ContainsKey(char_pole[i, g].sign))
                    {
                        Space_craft? space_craft = Dictionary.Space_craft[char_pole[i, g].sign].prov_form(new Point(g, i), char_pole);
                        if (space_craft != null)
                        {
                            space_craft.check_raz_form();
                            if (space_craft is Player)
                            {
                                player = space_craft as Player;

                                player.add_detail_to_pole(pole);
                                mas_space_crafts.Add(player);

                            }
                            else if(space_craft is Boss)
                            {
                                boss = space_craft as Boss;

                                boss.add_detail_to_pole(pole);
                                mas_space_crafts.Add(boss);
                            }
                            else
                            {
                                space_craft.add_detail_to_pole(pole);
                                mas_space_crafts.Add(space_craft);
                            }
                        }
                    }
                    else
                    {
                        pole[i, g] = null;
                    }
                }
            }
            for (i = 0; i < raz_i; i++)
            {
                for (g = 0; g < raz_g; g++)
                {
                    if (char_pole[i, g].used == false && pole[i, g] != null)
                        pole[i, g] = null;
                }
            }
            file.Close();

            int currentChunkY = GetCurrentChunkY();
            startY = currentChunkY * ChunkHeight;
            endY = startY + ChunkHeight;




            int extraLine = 1;
            startY = Math.Max(0, startY - extraLine);
            endY = Math.Min(raz_i, endY);
        }

        //Print
        public int endY { get; set; }
        public int startY { get; set; }
        public int ChunkHeight { get; set; } = 20; 

        
        public int GetCurrentChunkY()
        {
            if (player == null)
                return -1;
            int playerY = player.details[1].location.Y;
            return playerY / ChunkHeight;
        }

        
        public void Print()
        {
            

            


            var output = new StringBuilder();
            ConsoleColor currentFg = Console.ForegroundColor;
            ConsoleColor currentBg = Console.BackgroundColor;

            // Добавляем строку с HP игрока в начало вывода
            

            for (int y = startY; y < endY; y++)
            {
                for (int x = 0; x < raz_g; x++)
                {
                    var obj = pole[y, x];

                    if (obj != null)
                    {
                        // Конвертируем ConsoleColor в ANSI-код правильно
                        string fgCode = GetAnsiColorCode(obj.color_sign, isBackground: false);
                        string bgCode = GetAnsiColorCode(obj.color_background, isBackground: true);

                        output.Append(fgCode);
                        output.Append(bgCode);
                        output.Append(obj.sign);
                    }
                    else
                    {
                        output.Append("\x1b[0m "); // Сброс цвета + пробел
                    }
                }
                output.AppendLine();
            }
            output.AppendLine($"HP: {player?.HP}");
            output.AppendLine($"StartY = {startY} | EndY = {endY}");
            output.AppendLine("\x1b[0m");
            // Вывод всего буфера
            Console.SetCursorPosition(0, 0);
            Console.Write(output.ToString());



        }
        private string GetAnsiColorCode(ConsoleColor color, bool isBackground)
        {
            int code = color switch
            {
                ConsoleColor.Black => 0,
                ConsoleColor.DarkBlue => 4,
                ConsoleColor.DarkGreen => 2,
                ConsoleColor.DarkCyan => 6,
                ConsoleColor.DarkRed => 1,
                ConsoleColor.DarkMagenta => 5,
                ConsoleColor.DarkYellow => 3,
                ConsoleColor.Gray => 7,
                ConsoleColor.DarkGray => 8,
                ConsoleColor.Blue => 12,
                ConsoleColor.Green => 10,
                ConsoleColor.Cyan => 14,
                ConsoleColor.Red => 9,
                ConsoleColor.Magenta => 13,
                ConsoleColor.Yellow => 11,
                ConsoleColor.White => 15,
                _ => 7 // По умолчанию
            };
            return isBackground
                ? $"\x1b[48;5;{code}m"
                : $"\x1b[38;5;{code}m";
        }

        public void player_move(LimitedInt Y, LimitedInt X)
        {

            if (player == null)
                return;
            int newY = player.details[1].location.Y + Convert.ToInt32(Y);

            if (newY <= endY - 1)
            {
                player.move(Y, X, this);
                int currentChunkY = GetCurrentChunkY();
                startY = currentChunkY * ChunkHeight;
                endY = startY + ChunkHeight;




                int extraLine = 1;
                startY = Math.Max(0, startY - extraLine);
                endY = Math.Min(raz_i, endY);
                prov_boss_fights();
            }
            
        }

        public void create_arena()
        {
            player.move(LimitedInt.NegativeOne, LimitedInt.Zero, this);
            if (player.details[0].location.X == 0)
                player.move(LimitedInt.Zero, LimitedInt.NegativeOne, this);
            if (player.details[3].location.X == ( raz_g - 1) )
                player.move(LimitedInt.Zero, LimitedInt.NegativeOne, this);
            
            
            // Рисуем верхнюю границу (слева направо с анимацией)
            for (int g = 0; g < raz_g; g++)
            {
               

                char symbol = '─';
                if (g == 0) symbol = '┌';
                else if (g == raz_g - 1) symbol = '┐';

                pole[startY, g] = new Object(symbol, new Point(g, startY), ConsoleColor.Red);
                Print();
                Thread.Sleep(50); 
            }

            
            for (int i = startY + 1; i < endY; i++)
            {
                

                pole[i, raz_g - 1] = new Object('│', new Point(raz_g - 1, i), ConsoleColor.Red);
                Print();
                Thread.Sleep(50);
            }

            
            for (int g = raz_g - 1; g >= 0; g--)
            {
               

                char symbol = '─';
                if (g == 0) symbol = '└';
                else if (g == raz_g - 1) symbol = '┘';

                pole[endY - 1, g] = new Object(symbol, new Point(g, endY - 1), ConsoleColor.Red);
                Print();
                Thread.Sleep(50);
            }

            // Рисуем левую границу (снизу вверх с анимацией)
            for (int i = endY - 2; i > startY; i--)
            {
                

                pole[i, 0] = new Object('│', new Point(0, i), ConsoleColor.Red);
                Print();
                Thread.Sleep(50);
            }
            for(int i = 0; i < mas_space_crafts.Count; i++)
            {
                if (mas_space_crafts[i].GetType() == typeof(Enemy_space_craft) )
                {
                    mas_space_crafts[i].death(this, i);
                }
            }
            boss.appearance(this);
        }
        bool one_create_arena = true;
        void prov_boss_fights()
        {
            if(GetCurrentChunkY() == boss.details[0].location.Y / ChunkHeight && one_create_arena)
            {
                create_arena();
                one_create_arena = false;
                close_mas_space_crafts();
            }
        }
        public void flight_bullets()
        {
            bool prov = false;
            for (int i = 0; i < mas_bullets?.Count; i++)
            {
                if (mas_bullets[i].flight(this, i))
                    prov = true;
            }
            if (prov && player != null)
                Print();
        }
        void close_mas_space_crafts()
        {
            for (int i = mas_space_crafts.Count - 1; i >= 0; i--)
            {
                if (mas_space_crafts[i] != boss && mas_space_crafts[i] != player)
                {
                    mas_space_crafts[i].death(this, i);
                }
            }
        }
    }
}
