using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Clippers.FlowGenerator
{
    /// <summary>
    /// These extensions allow you to shedule a task in the future, and reset the timer if the event occurs while "waiting".
    /// </summary>    
    public static class TimerExtension
    {
        private static readonly Dictionary<object, Timer> _elementDictionary = new Dictionary<object, Timer>();

        private static readonly Dictionary<object, List<Timer>> _elementDictionaryList =
            new Dictionary<object, List<Timer>>();
        private static int defaultDelay = 60000;


        public static void DelayedExecute(this object key, Action<object, ElapsedEventArgs, object> method)
        {
            key.DelayedExecute(defaultDelay, method);
        }

        public static void DelayedExecute(this object key, int milliseconds, Action<object, ElapsedEventArgs, object> method)
        {
            lock (_elementDictionary)
            {
                if (_elementDictionary.ContainsKey(key))
                {
                    _elementDictionary[key].Stop();
                    _elementDictionary[key] = CreateNewTimer(method, key, milliseconds);
                }
                else
                    _elementDictionary.Add(key, CreateNewTimer(method, key, milliseconds));
            }
        }

        public static void RemoveDelayedExecute(this object key)
        {
            lock (_elementDictionary)
            {
                Timer local0;
                if (_elementDictionary.TryGetValue(key, out local0))
                {
                    local0.Stop();
                    _elementDictionary.Remove(key);
                }
            }
            RemoveDelayedExecuteQueue(key);
        }

        private static void AddNewTimer(object key, int milliseconds, Action<object, ElapsedEventArgs, object> method)
        {
            var newTimer = CreateNewTimer(method, key, milliseconds);
            lock (_elementDictionaryList)
                _elementDictionaryList[key].Add(newTimer);
        }

        private static Timer CreateNewTimer(Action<object, ElapsedEventArgs, object> method, object input, int milliseconds)
        {
            var timer = new Timer
            {
                Interval = milliseconds
            };
            //timer.Elapsed += (sender, e) => MyElapsedMethod(sender, e, theString);
            timer.Elapsed += (d, e) =>
            {
                method(d, e, input);
                timer.Stop();
                lock (_elementDictionary)
                    _elementDictionary.Remove(input);
                lock (_elementDictionaryList)
                {
                    if (!_elementDictionaryList.ContainsKey(input))
                        return;
                    _elementDictionaryList[input].Remove(timer);
                    if (_elementDictionaryList[input].Count != 0)
                        return;
                    _elementDictionaryList.Remove(input);
                }
            };
            timer.Start();
            return timer;
        }

        private static void RemoveDelayedExecuteQueue(object key)
        {
            lock (_elementDictionaryList)
            {
                List<Timer> local0;
                if (!_elementDictionaryList.TryGetValue(key, out local0))
                    return;
                foreach (var item0 in local0)
                    item0.Stop();
                _elementDictionaryList.Remove(key);
            }
        }
    }
}
