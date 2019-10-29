﻿using System;
using System.Management;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events.AppEvents;

namespace ChartATask.Interactors.Windows.Events
{
    public class WindowsRunningAppWatcher : IRunningAppWatcher
    {
        private readonly ManagementEventWatcher _processStartEvent;
        private readonly ManagementEventWatcher _processStopEvent;

        public WindowsRunningAppWatcher()
        {
            _processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
            _processStartEvent.EventArrived += ProcessStarted;

            _processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            _processStopEvent.EventArrived += ProcessStopped;
        }

        public event EventHandler<AppRunEvent> OnEvent;

        public void Start()
        {
            _processStartEvent?.Start();
            _processStopEvent?.Start();
        }

        public void Stop()
        {
            _processStartEvent?.Stop();
            _processStopEvent?.Stop();
        }

        public void Dispose()
        {
            _processStartEvent?.Dispose();
            _processStopEvent?.Dispose();
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());
            OnEvent?.Invoke(this, new AppRunEvent(processName, true));
        }

        private void ProcessStopped(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());
            OnEvent?.Invoke(this, new AppRunEvent(processName, false));
        }

        private static string ProcessNameToString(string processName)
        {
            return processName.ToLower().Split('.')[0];
        }
    }
}