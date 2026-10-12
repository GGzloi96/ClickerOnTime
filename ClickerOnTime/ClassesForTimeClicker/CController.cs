using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClickerOnTime.ClassesForTimeClicker
{
    internal class CController
    {
        private List<CObject> objects;   // Список активных объектов на сцене
        private double spawnRate;        // Частота появления объектов (объектов в секунду)
        private double time;             // Накопленное время с последнего спавна
        private Random rng;              // Генератор случайных чисел
        private double maxLifetime;      // Максимальное время жизни объекта
        private double minLifetime;      // Минимальное время жизни объекта
        private double maxSpriteSize;    // Максимальный размер спрайта
        private double minSpriteSize;    // Минимальный размер спрайта
        private Size sceneSize;          // Размер игровой сцены (Width, Height)
        private double points;

        public List<CObject> GetObjects() => objects;
        public double GetPoints() => points;
        public event Action<double, double> ObjectDied;

        public CController(double spawnRate, int startTime, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CObject>();
            this.spawnRate = spawnRate;
            time = startTime;
            this.sceneSize = sceneSize;
            points = 0;
            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 20;
            maxSpriteSize = 50;
        }

        public void spawnObject()
        {
            // Генерируем случайный размер
            double sizeValue = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);

            // Генерируем случайную позицию (учитывая размер, чтобы объект не выходил за границы)
            double x = rng.NextDouble() * Math.Max(0, sceneSize.Width - sizeValue);
            double y = rng.NextDouble() * Math.Max(0, sceneSize.Height - sizeValue);
            Point position = new Point(x, y);

            // Генерируем случайное время жизни
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);

            // Создаем объект
            CObject newObj = new CObject(position, sizeValue, lifetime);

            // Добавляем в список
            objects.Add(newObj);
        }

        public void destroyObject(CObject obj)
        {
            if (objects.Contains(obj))
            {
                objects.Remove(obj);
            }
        }

        public void update(double delta)
        {
            // ШАГ 1. Обновляем таймер спавна
            time += delta;
            double spawnInterval = 1.0 / spawnRate; // Интервал между спавнами

            if (time >= spawnInterval)
            {
                spawnObject();
                time -= spawnInterval;
            }

            // ШАГ 2. Обновляем все объекты и удаляем "мертвые"
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                CObject obj = objects[i];

                if (!obj.updateLifetime(delta))
                {
                    ObjectDied?.Invoke(obj.Position.X, obj.Position.Y);
                    destroyObject(obj);
                }
            }
        }

        public void mouseClick(Point mousePosition)
        {
            // Ищем объект, по которому кликнули (проверяем с конца, чтобы взять верхний)
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                CObject obj = objects[i];

                if (obj.isMouseOnObject(mousePosition))
                {
                    // Начисляем очки
                    points += obj.getPointsValue();

                    // Удаляем объект
                    destroyObject(obj);

                    // Прерываем цикл, так как клик обработан
                    break;
                }
            }


        }
    }
}
