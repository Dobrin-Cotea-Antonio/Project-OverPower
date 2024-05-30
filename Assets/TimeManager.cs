using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timerText;
    
    [HideInInspector] 
    public float currentTime;
    
    void Update()
    {
        currentTime += Time.deltaTime;
        timerText.text = ShowTimer(currentTime);
    }
    
    public string ShowTimer(float time)
    {
        int intTime = (int) time;
        int seconds = intTime % 60;
        int minutes = intTime / 60;
        string timeText = $"{minutes:00}:{seconds:00}";
        return timeText;
    }
}
