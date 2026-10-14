using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public abstract class CCollectable
    {
        protected Point position;
        protected Size size;
        protected double lifetime;
        protected Ellipse sprite;

        public Point Position => position;
        public Size Size => size;
        public double Lifetime => lifetime;

        public CCollectable(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            sprite = new Ellipse();
            sprite.Fill = Brushes.BlueViolet;      // цвет по умолчанию
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
        }

        public Ellipse getSprite() => sprite;

        public bool isMouseOnObject(Point mousePosition)
        {
            double dx = mousePosition.X - position.X;
            double dy = mousePosition.Y - position.Y;
            return dx >= 0 && dx <= size.Width &&
                   dy >= 0 && dy <= size.Height;
        }

        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime > 0;
        }

        public abstract bool onClick(CPlayer player, CController controller, Point mousePosition);

    }
}

