using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;

    public float maxSecondsBetweenHours = 90f;

    public int startHour;
    public int endHour;
    private float timer = 0;

    private int timeOfDay;
    public Text timeOfDayText;

    private bool gameWon = false;
    private bool gameLost = false;

    public GameObject winScreen;
    public GameObject okayScreen;
    public GameObject loseScreen;

    private AudioSource radio;

    public float backgroundMusicStartPitch;
    public float backgroundMusicEndPitch;

    public static bool tutorialDone = false;

    public GameObject onScreenUI;

    public GameObject orderText;
    public GameObject tutorialText;

    public static bool tutorialPlateCheckDone = false;

    private List<PlateController> platesList = new List<PlateController>();

    private GuestManager _guestManager;

    public GameObject[] tutorialTextItems = new GameObject[4];
    public GameObject[] tutorialSuccessTicks = new GameObject[4];

    public bool foodCookedTut = false;

    public bool orderDeliveredTut = false;

    public GameObject exclamationMarkUI;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
            TutorialCheck();
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (tutorialDone)
        {
            if (radio.pitch <= backgroundMusicEndPitch)
            {
                IncreaseBackgroundAudio();
            }

            EndChecker();
            onScreenUI.SetActive(true);
        }
        else
        {
            tutorialText.SetActive(true);
            orderText.SetActive(false);
            exclamationMarkUI.SetActive(true);
        }

        if (!OrderManager.firstOrder && !GuestManager.firstGuest)
        {
            tutorialDone = true;
        }

        //Debug.Log("GUESTER: " + tutorialDone + "ORDER: " + OrderManager.firstOrder + "GUEST: " + GuestManager.firstGuest);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    private void Setup()
    {
        Time.timeScale = 1;
        GameIsPaused = false;

        tutorialPlateCheckDone = false;
        OrderManager.firstOrderTutDone = false;
        foodCookedTut = false;
        orderDeliveredTut = false;

        timeOfDay = startHour;

        timeOfDayText.text = timeOfDay.ToString("f0") + "PM";

        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        radio = GameObject.Find("Radio").GetComponent<AudioSource>();

        onScreenUI.SetActive(false);

        foreach (PlateController plate in FindObjectsOfType(typeof(PlateController)) as PlateController[])
        {
            if (plate.name == "Plate")
            {
                platesList.Add(plate);
            }
        }

        _guestManager = gameObject.GetComponent<GuestManager>();

        ////Debug.Log(platesList.Count);
    }

    private void EndChecker()
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

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                if (GuestManager.overallSatisfaction > 75)
                {
                    winScreen.SetActive(true);
                }
                else
                {
                    okayScreen.SetActive(true);
                }
            }

            if (GuestManager.overallSatisfaction < 40)
            {
                gameLost = true;
                GameManager.GameIsPaused = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; 


                loseScreen.SetActive(true);
            }
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

    private void IncreaseBackgroundAudio()
    {
        int hoursInDay = endHour - startHour;
        float secondsInDay = hoursInDay * maxSecondsBetweenHours;

        radio.pitch += ((backgroundMusicEndPitch - backgroundMusicStartPitch) / secondsInDay) * Time.deltaTime; 
    }

    private void TutorialCheck()
    {

        if (!tutorialPlateCheckDone)
        {
            //PlateController plates = GameObject.find
            int count = 0;
            foreach (PlateController plate in platesList)
            {
                //Debug.Log(tutorialPlateCheckDone);

                if (plate.plateDoneTut)
                {
                    tutorialPlateCheckDone = true;
                }
                else
                {
                    tutorialPlateCheckDone = false;

                    break;
                }
            }

            if (tutorialPlateCheckDone)
            {
                //Debug.Log("WOAH");

                _guestManager.timeBeforeGuestTimer = _guestManager.maxTimeBeforeNextGuest;

                _guestManager.GuestTimer();

                //tutorialSuccessTicks[0].SetActive(true);
                //stutorialTextItems[0].SetActive(false);
            }

        }

        //Debug.Log("Tut: " + OrderManager.firstOrderTutDone);

        if (!tutorialPlateCheckDone)
        {
            tutorialSuccessTicks[0].SetActive(false);
            tutorialTextItems[0].SetActive(true);
        }
        else if (!OrderManager.firstOrderTutDone)
        {
            tutorialSuccessTicks[0].SetActive(true);
            tutorialTextItems[0].SetActive(false);
            tutorialTextItems[1].SetActive(true);
        }
        else if (!foodCookedTut)
        {
            tutorialSuccessTicks[0].SetActive(true);
            tutorialSuccessTicks[1].SetActive(true);
            tutorialTextItems[0].SetActive(false);
            tutorialTextItems[1].SetActive(false);
            tutorialTextItems[2].SetActive(true);
        }
        else if (!orderDeliveredTut)
        {
            tutorialSuccessTicks[0].SetActive(true);
            tutorialSuccessTicks[1].SetActive(true);
            tutorialSuccessTicks[2].SetActive(true);
            tutorialTextItems[0].SetActive(false);
            tutorialTextItems[1].SetActive(false);
            tutorialTextItems[2].SetActive(false);
            tutorialTextItems[3].SetActive(true);
        }
        else
        {
            tutorialSuccessTicks[0].SetActive(true);
            tutorialSuccessTicks[1].SetActive(true);
            tutorialSuccessTicks[2].SetActive(true);
            tutorialSuccessTicks[3].SetActive(true);
            tutorialTextItems[0].SetActive(false);
            tutorialTextItems[1].SetActive(false);
            tutorialTextItems[2].SetActive(false);
            tutorialTextItems[3].SetActive(false);
            tutorialTextItems[4].SetActive(true);

            tutorialDone = true;
        }
    }

}

