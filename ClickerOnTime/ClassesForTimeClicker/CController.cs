using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CController
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
        private double cooldown;
        private double maxCooldown;
        private double baseSpawnRate = 2.0;
        private double spawnInterval => 1.0 / spawnRate;

        public List<CObject> GetObjects() => objects;
        public double GetPoints() => points;
        public event Action<double, double> ObjectDied;

        public double GetMaxCooldown() => maxCooldown;
        public double GetMaxLifetime() => maxLifetime;
        public double GetSpawnInterval() => spawnInterval;

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
            cooldown = 0.35; //изменяется
            maxCooldown = 0.35; //для вывода на окно - статистика
        }

        public void AddPoints(double amount) => points += amount;

        public void ReduceCooldown(double amount)
        {
            maxCooldown -= amount;
            if (maxCooldown < 0.1) maxCooldown = 0.1;
        }

        public void IncreaseSpawnRate(double amount) => spawnRate += amount;

        public void IncreaseAllLifetimes(double amount)
        {
            foreach (var obj in objects)
            {
                obj.AddLifetime(amount);
            }
        }

        public void SpawnObject()
        {
            // Генерируем случайный размер
            double sizeValue = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);

            // Генерируем случайную позицию (учитывая размер, чтобы объект не выходил за границы)
            double x = rng.NextDouble() * Math.Max(0, sceneSize.Width - sizeValue);
            double y = rng.NextDouble() * Math.Max(0, sceneSize.Height - sizeValue);
            Point position = new Point(x, y);

            // Генерируем случайное время жизни
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);

            CObject newObj;
            double chance = rng.NextDouble();

            if (chance < 0.10)
            {
                newObj = new BonusPointsSphere(position, sizeValue, lifetime); 
            }
            else if (chance < 0.20)
            {
                newObj = new ReduceCooldownSphere(position, sizeValue, lifetime); 
            }
            else if (chance < 0.25) // 5% шанс
            {
                newObj = new IncreaseLifetimeSphere(position, sizeValue, lifetime); 
            }
            else if (chance < 0.30) // 5% шанс
            {
                newObj = new ReduceSpawnRateSphere(position, sizeValue, lifetime); 
            }
            else
            {
                newObj = new NormalSphere(position, sizeValue, lifetime); 
            }

            objects.Add(newObj);
        }

        public void DestroyObject(CObject obj)
        {
            if (objects.Contains(obj))
            {
                objects.Remove(obj);
            }
        }

        public void Update(double delta)
        {
            if (cooldown < delta) cooldown = 0;
            else cooldown -= delta;

            //Обновляем таймер спавна
            time += delta;
            double spawnInterval = 1.0 / spawnRate; // Интервал между спавнами

            if (time >= spawnInterval)
            {
                SpawnObject();
                time -= spawnInterval;
            }

            //Обновляем все объекты и удаляем "мертвые"
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                CObject obj = objects[i];

                if (!obj.updateLifetime(delta))
                {
                    ObjectDied?.Invoke(obj.Position.X, obj.Position.Y);
                    DestroyObject(obj);
                }
            }
        }

        public void MouseClick(Point mousePosition)
        {
            if (cooldown > 0) return;
            cooldown = maxCooldown;

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                CObject obj = objects[i];

                if (obj.isMouseOnObject(mousePosition))
                {
                    obj.OnClick(this);
                    DestroyObject(obj);
                    break;
                }
            }
        }
    }
}
