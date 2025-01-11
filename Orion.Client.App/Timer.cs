using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App
{
    public static class Timer
    {
        public static void Wait(float seconds, Action onComplete)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(seconds);
            timer.Tick += (a, b) => onComplete.Invoke();
            timer.Start();
        }
    }
}
