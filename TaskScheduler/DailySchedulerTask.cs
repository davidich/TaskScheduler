namespace TaskScheduler
{
    using System;

    class DailySchedulerTask : SchedulerTask
    {
        public DailySchedulerTask(TimeSpan timeOfDay, Action task)
            : base(task, schedule, needImmediateRun)
        {
        }

        static ScheduleInfo GetSchedule(TimeSpan timeOfDay)
        {
            var firstRun = GetFirstRun(timeOfDay);
            var interval = TimeSpan.FromDays(1);

            return new ScheduleInfo(firstRun, interval);
        }

        static DateTime GetFirstRun(TimeSpan timeOfDay)
        {
            var time = DateTime.Today.Add(timeOfDay);

            return time >= DateTime.Now
                ? time
                : time.AddDays(1);
        }
    }
}