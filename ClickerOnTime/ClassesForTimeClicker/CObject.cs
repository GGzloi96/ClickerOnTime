using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime
{
    public class CObject
    {
        // --- Поля (private, как требует инкапсуляция) ---
        private Point position;       // левый верхний угол объекта
        private Size size;            // размер (Width = Height)
        private double lifetime;      // сколько ещё проживёт (в секундах)
        private double pointsValue;   // сколько очков даёт за клик
        private Ellipse sprite;       // визуальная фигура

        // Публичные свойства только для чтения — чтобы контроллер мог читать
        public Point Position => position;
        public Size Size => size;
        public double Lifetime => lifetime;
        public double PointsValue => pointsValue;

        // --- Конструктор ---
        public CObject(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            // Создаём кружок
            sprite = new Ellipse();
            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;

            // Размеры
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;

            // Позиция через трансформацию (как в методичке)
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            // Чем меньше объект и меньше живёт — тем дороже
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        // --- Методы из UML ---

        /// <summary>Проверяет, попал ли курсор в объект.</summary>
        public bool isMouseOnObject(Point mousePosition)
        {
            // Область объекта — квадрат со стороной size.Width
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
    }

}
