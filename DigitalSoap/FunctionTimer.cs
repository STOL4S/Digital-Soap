using System;

namespace DigitalSoap
{
    public class FunctionTimer
    {
        private DateTime _Start;

        public TimeSpan Time { get { return _Time; } }
        private TimeSpan _Time;

        public FunctionTimer()
        {
            this._Start = DateTime.Now;
            this._Time = TimeSpan.Zero;
        }

        public void Start()
        {
            this._Start = DateTime.Now;
        }

        public void Stop()
        {
            this._Time = DateTime.Now - _Start;
        }

        public override string ToString()
        {
            return _Time != TimeSpan.Zero ? _Time.Hours.ToString("00") + "h:" 
                + _Time.Minutes.ToString("00") + "m:" + _Time.Seconds.ToString("00") 
                + "s:" + _Time.Milliseconds.ToString("0000") + "ms" : ThrowNullTimeException();
        }

        public string ToSeconds()
        {
            return _Time != TimeSpan.Zero ? _Time.TotalSeconds.ToString("N3") + "" : ThrowNullTimeException();
        }
        
        public string ToMinutes()
        {
            return _Time != TimeSpan.Zero ? _Time.TotalMinutes.ToString("#.##") + "m" : ThrowNullTimeException();
        }

        public string GetResultString()
        {
            return _Time != TimeSpan.Zero ? "Elapsed Time: " + _Time.Minutes.ToString("00") + "m : " + _Time.Seconds.ToString("00") + "s : " + _Time.Milliseconds.ToString("000") + "ms" : ThrowNullTimeException();
        }

        public void PrintResult()
        {
            Console.WriteLine(GetResultString());
        }

        private string ThrowNullTimeException()
        {
            throw new Exception("Stopwatch was not stopped prior to requesting ToString(). "
                + "TimeSpan is null, please call Stop() before calling ToString().");
        }
    }
}
