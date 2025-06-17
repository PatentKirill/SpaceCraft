using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль
{
    public class Object : ICloneable
    {
        public ConsoleColor color_sign { get; set; }
        public ConsoleColor color_background { get; set; }
        public char sign { get; set; }
        public Point location { get; set; }
        public Object(char sign, Point location, ConsoleColor color_sign)
        {
            this.sign = sign;
            this.location = location;
            this.color_sign = color_sign;
            this.color_background = ConsoleColor.Black;
        }
        public Object(char sign, Point location, ConsoleColor color_sign, ConsoleColor color_background)
        {
            this.sign = sign;
            this.location = location;
            this.color_sign = color_sign;
            this.color_background = color_background;
        }
        public bool prov_location(Point location)
        {
            return location == this.location;
        }
        public void Print()
        {
            Console.ForegroundColor = color_sign;
            Console.BackgroundColor = color_background;
            Console.Write(sign);
            Console.ResetColor();
        }

        public override string ToString()
        {
            return Convert.ToString(sign);
        }
        public object Clone()
        {
            return new Object(sign, location, color_sign);
        }
    }
}
