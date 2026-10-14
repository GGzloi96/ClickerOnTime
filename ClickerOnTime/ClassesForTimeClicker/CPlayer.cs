using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CPlayer
    {
        private bool canClick;
        private double timeBeforeClick;
        private CCountdownTimer countdownTimer;

        // Конструктор без контроллера
        public CPlayer(double timeBeforeClick)
        {
            this.timeBeforeClick = timeBeforeClick;
            this.countdownTimer = new CCountdownTimer(timeBeforeClick);
            this.canClick = true;
        }

        // Публичные свойства — чтобы MainWindow мог их читать
        public bool CanClick => canClick;
        public double TimeBeforeClick => timeBeforeClick;

        /// <summary>
        /// Пытается «потратить» клик.
        /// Возвращает true, если клик разрешён (кулдаун прошёл),
        /// и false, если ещё идёт перезарядка.
        /// Сам клик по объектам выполняет MainWindow через controller.mouseClick(...).
        /// </summary>
        public bool tryClick()
        {
            if (!canClick) return false;

            canClick = false;
            countdownTimer.reset();
            return true;
        }

        public void update(double delta)
        {
            if (!canClick)
            {
                countdownTimer.update(delta);
                if (countdownTimer.isFinished())
                    countdownEnded();
            }
        }

        public void countdownEnded()
        {
            canClick = true;
        }

        public void increaseSpeed(double speedModifier)
        {
            // уменьшаем время перезарядки — например, на 10%
            timeBeforeClick *= (1 - speedModifier);

            // защита от нуля и отрицательных значений
            if (timeBeforeClick < 0.05)
                timeBeforeClick = 0.05;

            countdownTimer = new CCountdownTimer(timeBeforeClick);
        }
    }
}
