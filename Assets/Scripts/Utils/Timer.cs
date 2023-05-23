using UnityEngine;

public class Timer
{
    public class TimeFormat {
        private readonly int _minutes = 0;
        private readonly int _seconds = 0;

        public TimeFormat(float currentTime) {
            _minutes = Mathf.FloorToInt(currentTime / 60);
            _seconds = Mathf.FloorToInt(currentTime % 60);
        }

        public int minutes => _minutes;
        public int seconds => _seconds;
        public string String => $"{_minutes:00}:{_seconds:00}";
    }

    private bool timerOn = false;
    private float t = 0;
    private int start = 0;

    public Timer(int startSeconds = 0) {
        start = startSeconds;
        t = Time.time;
    }

    public void Start() {
        timerOn = true;
    }

    public float currentSeconds => Time.time - t;
    public TimeFormat currentTime => new TimeFormat(Time.time - t);
}
