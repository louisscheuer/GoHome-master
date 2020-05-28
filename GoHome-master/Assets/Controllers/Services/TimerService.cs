using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Controllers.Services
{
    // Timer Service. Creates easy API for creating and using timers.
    public class TimerService
    {
        private float _timer;
        private float _timerLength;
        private bool _timerStarted;

        public void SetTimer(float timerLength)
        {
            _timerLength = timerLength;
        }

        public void StartTimer()
        {
            if (!_timerStarted)
            {
                _timerStarted = true;
            }
        }

        public void ResetAndStartTimer()
        {
            ResetTimer();

            if (!_timerStarted)
            {
                _timerStarted = true;
            }
        }

        public void IncrementTimer(float deltaTime)
        {
            if (_timerStarted)
            {
                _timer += deltaTime;
            }
        }

        public bool CheckTimer()
        {
            if (_timerStarted)
            {
                return _timer > _timerLength;
            }

            return false;
        }

        public void ResetTimer()
        {
            _timerStarted = false;
            _timer = 0;
        }
    }
}
