using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CController
    {
        // --- Поля ---
        private List<CCollectable> objects;
        private double spawnRate;
        private double time;
        private Random rng;
        private double maxLifetime;
        private double minLifetime;
        private double maxSpriteSize;
        private double minSpriteSize;
        private Size sceneSize;
        private double points;

        // --- Геттеры ---
        public List<CCollectable> GetObjects() => objects;
        public double GetPoints() => points;
        public double SpawnRate => spawnRate;
        public double MinLifetime => minLifetime;
        public double MaxLifetime => maxLifetime;

        // --- Событие для анимации смерти ---
        public event Action<double, double> ObjectDied;

        // --- Конструктор ---
        public CController(double spawnRate, int startTime, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CCollectable>();
            this.spawnRate = spawnRate;
            time = startTime;
            this.sceneSize = sceneSize;
            points = 0;
            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 20;
            maxSpriteSize = 50;
        }

        // =========================================================
        //  МЕТОДЫ ДЛЯ БОНУСОВ
        // =========================================================

        /// <summary>Добавить очки. Вызывает CPointGiver.</summary>
        public void pointsIncrease(double value)
        {
            points += value;
        }

        /// <summary>
        /// Изменить границы времени жизни для НОВЫХ объектов.
        /// modifier < 1 — уменьшить, modifier > 1 — увеличить.
        /// Вызывает CLifetimeModifier.
        /// </summary>
        public void changeLifetime(double modifier)
        {
            if (modifier <= 0) return;   // защита от некорректного множителя

            minLifetime *= modifier;
            maxLifetime *= modifier;

            if (minLifetime < 0.1) minLifetime = 0.1;
            if (maxLifetime < minLifetime) maxLifetime = minLifetime;
        }

        /// <summary>
        /// Изменить скорость спавна. modifier < 1 — спавн чаще, > 1 — реже.
        /// Вызывает CSpawnRateChanger.
        /// </summary>
        public void changeSpawnRate(double modifier)
        {
            if (modifier <= 0) return;

            spawnRate *= modifier;

            // ограничиваем разумными рамками
            if (spawnRate < 0.05) spawnRate = 0.05;
            if (spawnRate > 20) spawnRate = 20;
        }

        // =========================================================
        //  ОСНОВНЫЕ МЕТОДЫ
        // =========================================================

        /// <summary>Создать новый случайный объект на сцене.</summary>
        public void spawnObject()
        {
            double sizeValue = minSpriteSize + rng.NextDouble() * (maxSpriteSize - minSpriteSize);
            double x = rng.NextDouble() * Math.Max(0, sceneSize.Width - sizeValue);
            double y = rng.NextDouble() * Math.Max(0, sceneSize.Height - sizeValue);
            Point position = new Point(x, y);
            double lifetime = minLifetime + rng.NextDouble() * (maxLifetime - minLifetime);

            // Случайный тип бонуса
            int type = rng.Next(4);
            CCollectable newObj;
            switch (type)
            {
                case 0:
                    newObj = new CPointGiver(position, sizeValue, lifetime);
                    break;
                case 1:
                    newObj = new CClickSpeedUp(position, sizeValue, lifetime, 0.1);
                    break;
                case 2:
                    newObj = new CLifetimeModifier(position, sizeValue, lifetime, 0.9);
                    break;
                default:
                    newObj = new CSpawnRateChanger(position, sizeValue, lifetime, 0.9);
                    break;
            }

            objects.Add(newObj);
        }

        /// <summary>Удалить объект со сцены.</summary>
        public void destroyObject(CCollectable obj)
        {
            if (objects.Contains(obj))
                objects.Remove(obj);
        }

        /// <summary>
        /// Обновление состояния: спавн по таймеру + уменьшение lifetime.
        /// Вызывается из MainWindow.Timer_Tick.
        /// </summary>
        public void update(double delta)
        {
            // ШАГ 1. Спавн
            time += delta;
            double spawnInterval = 1.0 / spawnRate;
            if (time >= spawnInterval)
            {
                spawnObject();
                time -= spawnInterval;
            }

            // ШАГ 2. Обновление и удаление мёртвых
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                CCollectable obj = objects[i];

                if (!obj.updateLifetime(delta))
                {
                    ObjectDied?.Invoke(obj.Position.X, obj.Position.Y);
                    destroyObject(obj);
                }
            }
        }

        /// <summary>
        /// Обработка клика. Каждый объект сам решает, что делать.
        /// player нужен, потому что бонус CClickSpeedUp меняет состояние игрока.
        /// </summary>
        public void mouseClick(Point mousePosition, CPlayer player)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                // onClick вернёт true, если клик попал и объект нужно удалить
                if (objects[i].onClick(player, this, mousePosition))
                {
                    destroyObject(objects[i]);
                    break;   // клик обработан — выходим
                }
            }
        }
    }
}
