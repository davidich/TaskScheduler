namespace Examples
{
    using System;

    class Program
    {
        private static void Main(string[] args)
        {
            var scheduler = new Scheduler(TimeSpan.FromSeconds(1));

            Console.WriteLine("{0}: App is started", DateTime.Now);

            //// Run once in 5 secs from now
            //scheduler.SingleRun(
            //    DateTime.Now.AddSeconds(5),
            //    () => Console.WriteLine("{0}: Hello from SingleRun", DateTime.Now));

            //// run every 3 secs (first run in 3 secs from now)
            //scheduler.RecurringRun(
            //    interval: TimeSpan.FromSeconds(3),
            //    task: () => Console.WriteLine("{0}: This 'every 3 secs' task with no First Run value", DateTime.Now));

            //// run every 3 secs (first run is immediate)
            //scheduler.RecurringRun(
            //    interval: TimeSpan.FromSeconds(3),
            //    task: () => Console.WriteLine("{0}: This 'every 3 secs' task with immediate First Run", DateTime.Now),
            //    immediateFirstRun: true);

            // run daily, first run at (Now + 15sec)
            scheduler.RecurringRun(
                firstRun: DateTime.Today.AddHours(8),
                interval: TimeSpan.FromDays(1),
                task: () => Console.Write("Emails are sent every day at 8:00 am"));

            // run daily at (Now+8:00 AM
            var sendEmailTask = scheduler.RecurringRun(
                firstRun: DateTime.Today.AddHours(8),
                interval: TimeSpan.FromDays(1),
                task: () => Console.Write("Emails are sent every day at 8:00 am"));

            //// Immediate first Run with no First Run specified
            //var cancelationDemoTaskCnt = 0;
            //var cancelationDemoTask = scheduler.RecurringRun(
            //    interval: TimeSpan.FromMinutes(5),
            //    task: () =>
            //    {
            //        cancelationDemoTaskCnt++;
            //        Console.WriteLine("{0}: CancelationDemoTask, run #{1}", DateTime.Now, cancelationDemoTaskCnt);
            //    });

            //// Watch for some changes every 5 mins
            //scheduler.RecurringRun(
            //    TimeSpan.FromMinutes(5),
            //    () =>
            //    {
            //        // adjust schedule for SendEmailTask
            //        TimeSpan timeOfDay;
            //        TimeSpan interval;
            //        GetScheduleFromDB(out timeOfDay, out interval);

            //        sendEmailTask.AdjustSchedule(timeOfDay, interval);

            //        // limit amount of runs for demoTask
            //        if (cancelationDemoTaskCnt >= 5)
            //        {
            //            cancelationDemoTask.Discard();
            //        }
            //    });

            Console.ReadLine();
        }

        private static void GetScheduleFromDB(out TimeSpan timeOfDay, out TimeSpan interval)
        {
            timeOfDay = TimeSpan.FromHours(15);
            interval = TimeSpan.FromMinutes(60);
        }
    }
}
