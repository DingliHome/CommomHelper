using System;
using System.Windows.Threading;

namespace ClassLibraryHelper.WPF.Validates
{
    public class GapDispatcherTimer
    {
        private DispatcherTimer gapValidateTimer;

        public GapDispatcherTimer()
        {

        }

        public void Invoke(double second, Action action)
        {
            if (gapValidateTimer == null && second > 0)
            {
                gapValidateTimer = new DispatcherTimer();
                gapValidateTimer.Interval = TimeSpan.FromSeconds(second);
                gapValidateTimer.Tick += delegate
                                             {
                                                 action();
                                                 gapValidateTimer.Stop();
                                             };
            }
            if (second > 0)
                gapValidateTimer.Start();
        }

        public static void LaterAction(TimeSpan time, Action action)
        {
            var timer = new DispatcherTimer();
            timer.Interval = time;
            timer.Tick += delegate
                              {
                                  action();
                                  timer.Stop();
                                  timer = null;
                              };
            timer.Start();
        }
    }
}