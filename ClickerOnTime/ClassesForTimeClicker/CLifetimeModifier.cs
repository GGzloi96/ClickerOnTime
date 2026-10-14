using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CLifetimeModifier : CCollectable
    {
        private double lifetimeModifier;

        public CLifetimeModifier(Point position, double size, double lifetime, double lifetimeModifier)
            : base(position, size, lifetime)
        {
            this.lifetimeModifier = lifetimeModifier;
            sprite.Fill = Brushes.Yellow;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (!isMouseOnObject(mousePosition))
                return false;

            controller.changeLifetime(lifetimeModifier);
            return true;
        }
    }
}
