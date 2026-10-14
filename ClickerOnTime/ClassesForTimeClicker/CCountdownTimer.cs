using System;
using System.Collections.Generic;
using System.Text;

namespace ClickerOnTime.ClassesForTimeClicker
{
    public class CCountdownTimer
    {
        private double targetTime;   // изначальная длительность
        private double currentTime;  // сколько осталось

        public CCountdownTimer(double targetTime)
        {
            this.targetTime = targetTime;
            this.currentTime = targetTime;
        }

        public double getTime() => currentTime;

        public void update(double delta)
        {
            currentTime -= delta;
            if (currentTime < 0) currentTime = 0;
        }

        public bool isFinished() => currentTime <= 0;

        public void reset() => currentTime = targetTime;
    }
}
