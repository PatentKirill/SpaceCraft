using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace космический_корабль
{
    class Boss: Enemy_space_craft
    {
        public Boss(int kol_details, char sign, ConsoleColor color, int HP) : base(kol_details, sign, color, HP) 
        {}
        public virtual void appearance(Pole pole)
        {}
    }
    
}
