namespace TaskScheduler
{
    using System;

    internal class ScheduleInfo
    {
        private DateTime _scheduledFirstRun;
        private DateTime? _lastScheduledTime;
        private TimeSpan _interval;

        public bool IsAlive
        {
            get
            {
                return _scheduledFirstRun >= DateTime.Now || _interval > TimeSpan.Zero;
            }
        }

        public DateTime ScheduledTime
        {
            get
            {
                if (!_lastScheduledTime.HasValue)
                    return _scheduledFirstRun;

                // if first run happened and interval is not positive don't run anymore
                if (_lastScheduledTime.Value < DateTime.Now && _interval <= TimeSpan.Zero)
                    return DateTime.MaxValue;
                
                return _lastScheduledTime.Value.Add(_interval);
            }
        }

        public ScheduleInfo(DateTime scheduledFirstRun, TimeSpan interval)
        {
            _scheduledFirstRun = scheduledFirstRun;
            _interval = interval;
        }

        public void OnTaskQueued(DateTime queueTime)
        {
            _lastScheduledTime = queueTime;
        }

        public void Discard()
        {
            _interval = TimeSpan.FromMilliseconds(-1);


            if (_scheduledFirstRun > DateTime.Now)
                _scheduledFirstRun = DateTime.MinValue;
        }

        //public void Adjust(DateTime lastRun, TimeSpan interval)
        //{
        //    _lastScheduledTime = lastRun;
        //    _interval = interval;
        //}        
    }
}