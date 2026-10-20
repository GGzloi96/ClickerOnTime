using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class NormalSphere : CObject
    {
        public NormalSphere(Point position, double size, double lifetime) : base(position, size, lifetime) { }

        protected override Brush GetColor() => Brushes.Violet;

        public override void OnClick(CController controller)
        {
            controller.AddPoints(PointsValue);
        }
    }
}
