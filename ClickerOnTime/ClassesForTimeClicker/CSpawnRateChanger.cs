using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CSpawnRateChanger : CCollectable
    {
        private double spawnRateModifier;

        public CSpawnRateChanger(Point position, double size, double lifetime, double spawnRateModifier)
            : base(position, size, lifetime)
        {
            this.spawnRateModifier = spawnRateModifier;
            sprite.Fill = Brushes.Orange;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (!isMouseOnObject(mousePosition))
                return false;

            controller.changeSpawnRate(spawnRateModifier);
            return true;
        }
    }
}
