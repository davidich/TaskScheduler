namespace TaskScheduler
{
    using System;

    class DailyScheduleInfo : ScheduleInfo
    {
        public DailyScheduleInfo(TimeSpan timeOfDay)
            : base(GetFirstRun(timeOfDay), TimeSpan.FromDays(1))
        {
        }

        static DateTime GetFirstRun(TimeSpan timeOfDay)
        {
            var time = DateTime.Today.Add(timeOfDay);

            return time >= DateTime.Now
                ? time
                : time.AddDays(1);
        }

        //public void Adjust
    }
}