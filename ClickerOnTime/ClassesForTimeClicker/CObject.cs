using ClickerOnTime.ClassesForTimeClicker;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime
{
    public abstract class CObject
    {
        private Point position;       
        private Size size;            
        private double lifetime;      
        private double pointsValue;   
        private Ellipse sprite;       

        public Point Position => position;
        public Size Size => size;
        public double Lifetime => lifetime;
        public double PointsValue => pointsValue;

        public CObject(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            // Создаём кружок
            sprite = new Ellipse();
            sprite.Fill = GetColor();
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;

            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;

            // Позиция через трансформацию (как в методичке)
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            // Чем меньше объект и меньше живёт — тем дороже
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        protected abstract Brush GetColor();

        public abstract void OnClick(CController controller);

        public bool isMouseOnObject(Point mousePosition)
        {
            double dx = mousePosition.X - position.X;
            double dy = mousePosition.Y - position.Y;

            return dx >= 0 && dx <= size.Width &&
                   dy >= 0 && dy <= size.Height;
        }

        /// <summary>Возвращает спрайт для отрисовки.</summary>
        public Ellipse getSprite() => sprite;

        /// <summary>Возвращает стоимость в очках.</summary>
        public double getPointsValue() => pointsValue;

        /// <summary>Уменьшает время жизни. Возвращает true, если объект ещё жив.</summary>
        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime > 0;
        }

        public void AddLifetime(double amount)
        {
            lifetime += amount;
        }
    }
}
