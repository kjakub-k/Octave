using System;
using System.Collections;

namespace KJakub.Octave.Game.Core
{
    enum ThermometerStatus
    {
        Disabled = 0,
        Enabled = 1
    }
    public class Thermometer
    {
        private ThermometerStatus thermometerStatus = ThermometerStatus.Disabled;
        private float percent = 0;
        private float startingValue;
        private float maxPercent = 100;
        private float decreaseBy;
        public int Weight { get; private set; } = 1;
        public event Action<float> OnThermometerChanged;
        public event Action<int> OnWeightChanged;
        public Thermometer(float decreaseBy, float startingPercentValue)
        {
            this.decreaseBy = decreaseBy;
            this.startingValue = startingPercentValue;
        }
        public void Stop()
        {
            thermometerStatus = ThermometerStatus.Disabled;
            percent = 0;
        }
        public void Add(float percent)
        {
            this.percent += percent;

            if (percent > maxPercent)
                percent = maxPercent;

            OnThermometerChanged?.Invoke(percent);
        }
        public IEnumerator Decrease()
        {
            thermometerStatus = ThermometerStatus.Enabled;
            percent = startingValue;

            while (thermometerStatus == ThermometerStatus.Enabled)
            {
                if (percent > 0)
                {
                    percent -= decreaseBy;

                    OnThermometerChanged?.Invoke(percent);
                }

                if (percent > 90 && Weight != 10)
                {
                    Weight = 10;
                    OnWeightChanged?.Invoke(Weight);
                }
                else if (percent > 70 && Weight != 5)
                {
                    Weight = 5;
                    OnWeightChanged?.Invoke(Weight);
                }
                else if (percent > 40 && Weight != 2)
                {
                    Weight = 2;
                    OnWeightChanged?.Invoke(Weight);
                }
                else if (percent < 40 && Weight != 0)
                {
                    Weight = 1;
                    OnWeightChanged?.Invoke(Weight);
                }
                
                if (percent < 0)
                    percent = 0;

                yield return null;
            }
        }
    }
}