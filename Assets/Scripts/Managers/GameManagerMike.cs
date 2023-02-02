using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerMike : MonoBehaviour
{

    public float maxSecondsBetweenHours = 90f;

    public int startHour;
    public int endHour;
    private float timer = 0;

    private int timeOfDay;
    public Text timeOfDayText;

    private bool gameWon = false;
    private bool gameLost = false;

    public GameObject winScreen;
    public GameObject loseScreen;

    // Start is called before the first frame update
    void Start()
    {
        timeOfDay = startHour;

        timeOfDayText.text = timeOfDay.ToString("f0") + "PM";

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameLost)
        {
            if (timeOfDay != endHour)
            {
                DayTimer();
            }
            else if (timeOfDay == endHour && !gameWon)
            {
                gameWon = true;
                GameManager.GameIsPaused = true;

                winScreen.SetActive(true);
            }

            if (GuestManager.overallSatisfaction < 40)
            {
                gameLost = true;
                GameManager.GameIsPaused = true;

                loseScreen.SetActive(true);
            }
        }
        else
        {

        }
    }

    private void DayTimer()
    {
        if (timer < maxSecondsBetweenHours)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timeOfDay++;
            timer = 0;

            timeOfDayText.text = timeOfDay.ToString("f0") + "PM";

        }
    }
}
