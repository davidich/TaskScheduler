namespace TaskScheduler
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    internal class SchedulerTask
    {
        private readonly ScheduleInfo _schedule;

        /// <summary>
        /// If set to true, then task is executed immediatelly upon service start
        /// </summary>
        private bool _needImmediateRun;

        private readonly Action _task;

        public bool HasFutureRuns
        {
            get
            {
                return _schedule.IsAlive || _needImmediateRun;
            }
        }

        public SchedulerTask(Action task, ScheduleInfo schedule, bool needImmediateRun = false)
        {
            _task = task;
            _schedule = schedule;
            _needImmediateRun = needImmediateRun;
        }

        public void AdjustSchedule(TimeSpan timeOfDay, TimeSpan interval)
        {
            var lastRun = DateTime.Today.Add(timeOfDay);
            _schedule.Adjust(lastRun, interval);
        }

        public void Discard()
        {
            _schedule.Discard();
        }

        public void OnSchedulerTick()
        {
            Debug.WriteLine("\tOnSchedulerTick");

            if (_needImmediateRun)
            {
                _needImmediateRun = false;
                ScheduleExecution(DateTime.Now);
            }
            else
            {
                var scheduledTime = _schedule.ScheduledTime;

                Debug.WriteLine("ScheduledTime: {0}", scheduledTime);
                Debug.WriteLine("Now: {0}", DateTime.Now);
                
                if (DateTime.Now > scheduledTime)
                {
                    ScheduleExecution(scheduledTime);
                }
            }
        }

        private void ScheduleExecution(DateTime scheduledTime)
        {
            ThreadPool.QueueUserWorkItem(state => _task());
            _schedule.OnTaskQueued(scheduledTime);
        }
    }
}