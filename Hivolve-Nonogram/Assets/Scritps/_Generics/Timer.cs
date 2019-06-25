using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text Text;
    public bool Began = false;
    public float BeginTime;
    private float time;
    private Action actionTimer;

    public void Update()
    {
        if(Began)
        {
            if(time < 0)
            {
                Began = false;
                TimeUp();
                this.Text.text = "0:00:0";
            }
            else
            {
                Decrement();
            }
        }
    }

    public void Begin(float time)
    {
        this.Text.gameObject.SetActive(true);
        this.Began = true;
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

    private void TimeUp()
    {
        GameManager.Instance.TimeUpWindow();
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
