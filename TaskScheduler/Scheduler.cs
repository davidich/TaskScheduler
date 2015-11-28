namespace TaskScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    class Scheduler : IDisposable
    {
        private readonly Timer _timer;
        private readonly List<SchedulerTask> _tasks;

        public Scheduler(TimeSpan tickInterval)
        {
            _tasks = new List<SchedulerTask>();
            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, tickInterval);
        }

        public SchedulerTask SingleRun(DateTime executionTime, Action task)
        {
            if (executionTime < DateTime.Now)
                throw new ArgumentException("Execution time value can't be in the past", "executionTime");

            var schedule = new ScheduleInfo(executionTime, TimeSpan.Zero);
            var schedulerTask = new SchedulerTask(task, schedule);

            _tasks.Add(schedulerTask);

            return schedulerTask;
        }

        public SchedulerTask RecurringRun(TimeSpan interval, Action task)
        {
            return RecurringRun(TimeSpan.Zero, interval, task);
        }

        public SchedulerTask RecurringRun(TimeSpan firstRunDelay, TimeSpan interval, Action task)
        {
            DateTime firstRun = DateTime.Now.Add(firstRunDelay);
            var schedule = new ScheduleInfo(firstRun, interval);
            var schedulerTask = new SchedulerTask(task, schedule);

            _tasks.Add(schedulerTask);

            return schedulerTask;
        }
       
        public SchedulerTask DailyRun(TimeSpan timeOfDay, Action task, bool immediateFirstRun = false)
        {
            var firstRun = DateTime.Today.Add(timeOfDay);

            if (firstRun < DateTime.Now)
                firstRun = firstRun.AddDays(1);
            

            var schedule = new ScheduleInfo(firstRun, TimeSpan.FromDays(1));
            var schedulerTask = new SchedulerTask(task, schedule, immediateFirstRun);

            _tasks.Add(schedulerTask);

            return schedulerTask;
        }

        private void TimerCallback(object state)
        {
            Debug.WriteLine("");

            string format = new string('-', 10) + "Scheduler Tick Callback {0}" + new string('-', 10);
            Debug.WriteLine(format, new object[] { "START TaskCount=" + _tasks.Count });

            for (int i = _tasks.Count - 1; i >= 0; i--)
            {
                var task = _tasks[i];
                task.OnSchedulerTick();

                if (!task.HasFutureRuns)
                    _tasks.RemoveAt(i);

            }

            Debug.WriteLine(format, new object[] { "END TaskCount=" + _tasks.Count });
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }
    }
}