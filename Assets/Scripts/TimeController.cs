using UnityEngine;

public class TimeController : MonoBehaviour
{
    public int StartingHour = 9; // 9am
    public int StartingDay = 0;
    public int DayEndHour = 21; // 9pm

    public float SecondsPerMinute = 1f;

    private int m_currentDay = 0;
    private int m_currentHour = 0;
    private int m_currentMinute = 0;
    private float m_currentRealTime = 0;
        
    private bool m_dayEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHour = StartingHour;
        m_currentDay = StartingDay;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dayEnded)
            return;

        m_currentRealTime += Time.deltaTime;

        if (m_currentRealTime >= SecondsPerMinute)
        {
            m_currentRealTime = 0;

            m_currentMinute++;

            if (m_currentMinute >= 60)
            {
                m_currentMinute = 0;
                m_currentHour++;

                if (m_currentHour >= DayEndHour)
                {
                    m_dayEnded = true;
                }
            }

            UIController.instance.UpdateTime(m_currentMinute, m_currentHour, m_currentDay);
        }
    }

    public void StartNewDay()
    {
        m_currentHour = StartingHour;
        m_currentMinute = 0;
        m_currentRealTime = 0;
        m_currentDay++;
        m_dayEnded = false;
        UIController.instance.UpdateTime(m_currentMinute, m_currentHour, m_currentDay);
    }
}
