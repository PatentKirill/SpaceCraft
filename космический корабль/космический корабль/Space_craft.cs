using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль
{
    class Space_craft
    {
        public virtual int HP { get; set; }
        public Object[] details;
        protected (int x, int y)[] form; //относительныt координты. Первая точка - x = 0, y = 0(первая точка должная первой проверятся при чтение из файла и ее не надо записывать)
        public char sign { get; set; }

        

        public void check_raz_form()
        {
            
            if(form.Length != (details.Length - 1) ) 
                throw new Exception("Размер form != details.Length - 1. Исправте размер form или kol_details");
        }
        public Space_craft? prov_form(Point starting_point, (char sign, bool used)[,] pole)
        {


            foreach ((int x, int y) point in form)
            {
                int x = point.x + starting_point.X;


                int y = point.y + starting_point.Y;



                if (x >= 0 && x < pole.GetLength(1) && y >= 0 && y < pole.GetLength(0) && !(pole[y, x].used))
                {
                    if (pole[y, x].sign != sign)
                        return null;
                }
                else
                    return null;

            }
            //если корабль найден, используем детали
            Space_craft new_space_craft = this.Clone();
            int i = 0;
            new_space_craft.details[i].location = new Point(starting_point.X, starting_point.Y);
            i++;
            pole[starting_point.Y, starting_point.X].used = true;
            foreach ((int x, int y) point in form)
            {
                int x = point.x + starting_point.X;
                int y = point.y + starting_point.Y;
                pole[y, x].used = true;
                new_space_craft.details[i].location = new Point(x, y);
                i++;
            }
            return new_space_craft;
        }

        public Space_craft(int kol_details, char sign, ConsoleColor color, int HP)
        {
            details = new Object[kol_details];
            for (int i = 0; i < kol_details; i++)
            {
                details[i] = new Object(sign, new Point(-1, -1), color);
            }
            this.sign = sign;
            this.HP = HP;
            form = new (int x, int y)[kol_details - 1];
        }
        public Space_craft(int kol_details, char sign, ConsoleColor color, ConsoleColor color_background, int HP)
        {
            details = new Object[kol_details];
            for (int i = 0; i < kol_details; i++)
            {
                details[i] = new Object(sign, new Point(-1, -1), color, color_background);
            }
            this.sign = sign;
            this.HP = HP;
            form = new (int x, int y)[kol_details - 1];
        }
        public void add_detail_to_pole(Object?[,] pole)
        {
            foreach (Object detail in details)
            {
                pole[detail.location.Y, detail.location.X] = detail;
            }
        }
        public void location_changes(Point new_location, int index)
        {
            details[index].location = new_location;
        }
        public virtual Space_craft Clone()
        {
            return new Space_craft(details.Length, sign, details[0].color_sign, HP);
        }
        public bool prov_details(int x, int y)
        {
            foreach (Object detail in details)
            {
                if (detail.location.X == x && detail.location.Y == y)
                    return true;
            }
            return false;
        }
        public void death(Pole pole, int index_space_craft)
        {
            pole.mas_space_crafts.RemoveAt(index_space_craft);
            foreach (Object detail in details)
            {
                pole.pole[detail.location.Y, detail.location.X] = null;
            }
        }
        public bool move(LimitedInt movement_y, LimitedInt movement_x, Pole pole)
        {
            int kol_collisions = 0;
            foreach (Object detail in details)
            {

                int new_y = detail.location.Y + Convert.ToInt32(movement_y);
                int new_x = detail.location.X + Convert.ToInt32(movement_x);
                if (new_x >= 0 && new_x < pole.pole.GetLength(1) && new_y >= 0 && new_y < pole.pole.GetLength(0) &&
                   (pole.pole[new_y, new_x] == null || pole.pole[new_y, new_x].sign == detail.sign))
                {

                }
                else
                    return false;
            }

            foreach (Object detail in details)
            {
                int new_y = detail.location.Y + Convert.ToInt32(movement_y);
                int new_x = detail.location.X + Convert.ToInt32(movement_x);
                if(detail == pole.pole[detail.location.Y, detail.location.X])
                    pole.pole[detail.location.Y, detail.location.X] = null;
                detail.location = new Point(new_x, new_y);
                pole.pole[detail.location.Y, detail.location.X] = detail;
            }
            
                pole.Print();
            return true;
        }


    }
}
