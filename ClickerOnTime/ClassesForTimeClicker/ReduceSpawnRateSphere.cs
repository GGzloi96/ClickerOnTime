using System.Windows;
using System.Windows.Media;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class ReduceSpawnRateSphere : CObject
    {
        public ReduceSpawnRateSphere(Point position, double size, double lifetime) : base(position, size, lifetime) { }

        protected override Brush GetColor() => Brushes.Orange; 

        public override void OnClick(CController controller)
        {
            controller.AddPoints(PointsValue);
            controller.IncreaseSpawnRate(0.1); 
        }
    }
}
