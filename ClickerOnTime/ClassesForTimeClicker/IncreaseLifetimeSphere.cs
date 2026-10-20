using System.Windows;
using System.Windows.Media;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class IncreaseLifetimeSphere : CObject
    {
        public IncreaseLifetimeSphere(Point position, double size, double lifetime) : base(position, size, lifetime) { }

        protected override Brush GetColor() => Brushes.Green; 

        public override void OnClick(CController controller)
        {
            controller.AddPoints(PointsValue);
            controller.IncreaseAllLifetimes(0.5); 
        }
    }
}
