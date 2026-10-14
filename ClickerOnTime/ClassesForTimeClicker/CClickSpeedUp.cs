using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CClickSpeedUp : CCollectable
    {
        private double speedModifier;

        public CClickSpeedUp(Point position, double size, double lifetime, double speedModifier)
            : base(position, size, lifetime)
        {
            this.speedModifier = speedModifier;
            sprite.Fill = Brushes.HotPink;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (!isMouseOnObject(mousePosition))
                return false;

            player.increaseSpeed(speedModifier);
            return true;
        }
    }
}
