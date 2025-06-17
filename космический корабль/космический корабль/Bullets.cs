using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using космический_корабль.list_boss;

namespace космический_корабль
{
    class Bullets : Object
    {
        public bool enemy_space_craft { get; set; } = true ;
        public double speed { get; set; }
        public int distance { get; set; }//расстояние, которое пуля может пролететь

        public (LimitedInt X, LimitedInt Y) direction { get; set; }

        Stopwatch speed_timer = new Stopwatch();
        public Bullets(char sign, Point location, ConsoleColor color, double speed, int distance, (LimitedInt X, LimitedInt Y) direction, Pole pole, bool enemy_space_craft = true) : base(sign, location, color)
        {
            this.enemy_space_craft = enemy_space_craft;
            speed_timer.Start();
            this.speed = speed;
            this.distance = distance;
            this.direction = direction;
            int new_y = location.Y;
            int new_x = location.X;

            // Проверка выхода за границы
            
            if (new_y < 0 || new_y >= pole.raz_i ||
                new_x < 0 || new_x >= pole.raz_g)
            {
                return;
            }

            // Получаем ссылку на целевой объект один раз
            var target = pole.pole[new_y, new_x];

            if (target == null)
            {
                

                pole.mas_bullets.Add(this);
                pole.pole[new_y, new_x] = this;
                pole.Print();

            }

            else if (target is Bullets)
            {

                // Столкновение двух пуль
                pole.mas_bullets.Remove(target as Bullets);
                pole.pole[new_y, new_x] = null;
                pole.Print();
            }
            else if (Dictionary.Space_craft.TryGetValue(target.sign, out var expectedSpacecraft))
            {
                // Попадание в космический корабльw


                foreach (var craft in pole.mas_space_crafts)
                {
                    if (craft.GetType() == expectedSpacecraft.GetType() && craft.prov_details(new_x, new_y))
                    {
                        if((craft is Player) || !enemy_space_craft)
                        {
                            craft.HP--;
                            if (craft.HP <= 0)
                            {
                                
                                if (craft is Player)
                                    pole.player = null;
                                else if (craft is Boss_V)
                                    pole.boss = null;
                                craft.death(pole, pole.mas_space_crafts.IndexOf(craft));
                                break;
                            }
                        }
                        
                    }
                }
            }

        }
        public bool flight(Pole pole, int index_bullet)
        {
            if (distance > 0 && speed_timer.Elapsed.TotalSeconds > speed)
            {
                int new_y = location.Y + (int)direction.Y;
                int new_x = location.X + (int)direction.X;

                // Проверка выхода за границы
                if (new_y < 0 || new_y >= pole.raz_i ||
                new_x < 0 || new_x >= pole.raz_g || pole.GetCurrentChunkY() != (new_y / pole.ChunkHeight))
                {
                    RemoveBullet(); 

                    return true;
                }

                // Получаем ссылку на целевой объект один раз
                var target = pole.pole[new_y, new_x];

                if (target == null)
                {
                    // Перемещение пули
                    pole.pole[location.Y, location.X] = null;
                    pole.pole[new_y, new_x] = this;
                    location = new Point(new_x, new_y);
                }

                else if (target is Bullets)
                {
                    RemoveBullet();
                    // Столкновение двух пуль
                    pole.mas_bullets.Remove(target as Bullets);

                    pole.pole[new_y, new_x] = null;
                }
                else if (Dictionary.Space_craft.TryGetValue(target.sign, out var expectedSpacecraft))
                {
                    // Попадание в космический корабль
                    RemoveBullet();

                    foreach (var craft in pole.mas_space_crafts)
                    {
                        if (craft.GetType() == expectedSpacecraft.GetType() && craft.prov_details(new_x, new_y))
                        {
                            if ((craft is Player) || !enemy_space_craft)
                            {
                                craft.HP--;
                                if (craft.HP <= 0)
                                {
                                    if (craft is Player)
                                        pole.player = null;
                                    else if (craft is Boss_V)
                                        pole.boss = null;
                                    craft.death(pole, pole.mas_space_crafts.IndexOf(craft));
                                    break;
                                }
                            } 
                        }
                    }
                }
                else if (target is Object)
                {
                    RemoveBullet();




                }
                distance--;
                speed_timer.Restart();

                return true;
            }
            else if (distance <= 0)
            {
                RemoveBullet();

                return true;
            }

            return false;

            // Локальная функция для удаления пули
            void RemoveBullet()
            {
                pole.mas_bullets.RemoveAt(index_bullet);
                pole.pole[location.Y, location.X] = null;

            }

        }

    }
}
