using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class ReduceCooldownSphere : CObject
    {
        public ReduceCooldownSphere(Point position, double size, double lifetime) : base(position, size, lifetime) { }

        private double reduceCooldownAmount = 0.05;

        protected override Brush GetColor() => Brushes.Cyan;

        public override void OnClick(CController controller)
        {
            controller.AddPoints(PointsValue);
            controller.ReduceCooldown(reduceCooldownAmount);
        }
    }
}
