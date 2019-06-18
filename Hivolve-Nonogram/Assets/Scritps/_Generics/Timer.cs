using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : Singleton<Timer>
{
    public float BeginTime;
    public Text Text;
    private float time;
    private Action actionTimer; 

    public void Update() => actionTimer?.Invoke();

    public void Begin(float time)
    {
        this.Text.gameObject.SetActive(true);
        this.BeginTime = time;
        this.time = time;
        this.actionTimer += Decrement;
    }
    public void AddTime(int increment)
    {
        this.time += increment;
    }
    public void Restart()
    {
        this.time = BeginTime;
    }
    public void Stop()
    {
        this.Text.gameObject.SetActive(false);
        this.actionTimer -= Decrement;
    }

    private void Decrement()
    {
        this.time -= Time.deltaTime;
        this.Text.text = string.Format(
                                "{0:#0} : {1:00} : {2:00}",
                                Mathf.Floor(time / 60),
                                Mathf.Floor(time) % 60,
                                Mathf.Floor((time * 100) % 100)
                                );
    }
}
