using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MiniGame
{
    class Player
    {
        public UIElement Healthbar { get; set; }
        public UIElement UiElement { get; set; }
        public double Health { get; set; }
        public double Damage { get; set; }
    }
}
