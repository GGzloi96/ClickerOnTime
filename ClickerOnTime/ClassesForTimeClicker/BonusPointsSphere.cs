using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class BonusPointsSphere : CObject
    {
        public BonusPointsSphere(Point position, double size, double lifetime) : base(position, size, lifetime) { }

        protected override Brush GetColor() => Brushes.Yellow;

        public override void OnClick(CController controller)
        {
            controller.AddPoints(PointsValue * 3);
        }
    }
}