using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuestManager : MonoBehaviour
{

    public List<GameObject> guestSpawnLocations = new List<GameObject>();
    public GameObject guestSpawnLocationsParent;
    public static List<int> guestPositionsAvailable = new List<int>();
    public static int maxGuestsAllowed;

    public static List<GameObject> currentGuests = new List<GameObject>();
    public static List<GameObject> currentGuestsAcceptingOrders = new List<GameObject>();

    public List<GameObject> guestOriginPoints = new List<GameObject>();
    public GameObject guestOriginPointsParent;

    public static float overallSatisfaction = 70;

    [Header("Standard Timer Variables")]
    public int timeBeforeFirstGuest = 5;
    public float timeBeforeNextGuest = 10.0f;
    public float minTimeBetweenGuestsArriving = 5f;

    public float maxTimeBeforeNextGuest;
    public float timeBeforeGuestTimer = 0;
    public static bool firstGuest = true;

    [Header("Random Timer Decrease Values")]
    public float maxDecreaseValue = 3f;
    public float minDecreaseValue = 0f;

    [Header("Public Instantiations")]
    public GameObject guestPrefab;
    public Text satisfactionUI;
    public Slider satisfactionMeter;
    public GameObject audioSource;

    private const float maxAngle = -90;
    private const float minAngle = 207.5f;
    public Transform needleTransform;
    private float maxSatisfaction = 100;

    private bool soundGoodPlaying;
    private bool soundBadPlaying;
    // Start is called before the first frame update
    void Start()
    {
        ResetVariables();
        Setup();        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.tutorialDone)
        {
            GuestTimer();
        }

        
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            timeBeforeGuestTimer = maxTimeBeforeNextGuest;

            GuestTimer();
        }

        */

        if (overallSatisfaction > 100)
        {
            overallSatisfaction = 100;
        }

        satisfactionUI.text = overallSatisfaction.ToString("f0") + "%";
        satisfactionMeter.value = overallSatisfaction;

        needleTransform.eulerAngles = new Vector3(0, 0, GetRotation());

        if (currentGuests.Count >= 2)
        {
            if (overallSatisfaction < 60)
            {
                if (soundBadPlaying == true)
                {
                    soundBadPlaying = false;
                    soundGoodPlaying = true;
                    GameObject.Find("GuestAudioSource").GetComponent<AudioManager>().StopPlaying("Normal");
                    GameObject.Find("GuestAudioSource").GetComponent<AudioManager>().Play("Angry");
                    //Debug.Log("Sound Bad");
                }
                
            }
            else if (overallSatisfaction > 60)
            {
                if (soundGoodPlaying == true)
                {
                    soundBadPlaying = true;
                    soundGoodPlaying = false;
                    GameObject.Find("GuestAudioSource").GetComponent<AudioManager>().StopPlaying("Angry");
                    GameObject.Find("GuestAudioSource").GetComponent<AudioManager>().Play("Normal");
                    //Debug.Log("Sound Good");
                }
                
            }
        }
        

    }

    private void ResetVariables()
    {
        guestPositionsAvailable.Clear();
        maxGuestsAllowed = 0;
        currentGuests.Clear();
        currentGuestsAcceptingOrders.Clear();
        overallSatisfaction = 70;
    }

    private void Setup()
    {
        maxTimeBeforeNextGuest = timeBeforeFirstGuest;

        guestSpawnLocationsParent = GameObject.FindGameObjectWithTag("GuestLocations");

        foreach (Transform guestLocations in guestSpawnLocationsParent.transform)
        {
            guestSpawnLocations.Add(guestLocations.gameObject);
        }

        guestOriginPointsParent = GameObject.FindGameObjectWithTag("GuestOrigins");

        foreach (Transform guestOrigins in guestOriginPointsParent.transform)
        {
            guestOriginPoints.Add(guestOrigins.gameObject);
        }

        for (int i = 0; i < guestSpawnLocations.Count; i++)
        {
            guestPositionsAvailable.Add(i);
        }

        maxGuestsAllowed = guestPositionsAvailable.Count;

        soundGoodPlaying = false;
        soundBadPlaying = true;
    }

    public void GuestTimer()
    {
        if (timeBeforeGuestTimer < maxTimeBeforeNextGuest)
        {
            timeBeforeGuestTimer += Time.deltaTime;
        }
        else
        {
            
            if (guestPositionsAvailable.Count != 0)
            {
                int randomNum = Random.Range(0, guestPositionsAvailable.Count);

                GameObject newGuest = Instantiate(guestPrefab, guestOriginPoints[Random.Range(0, guestOriginPoints.Count - 1)].transform.position, Quaternion.identity, guestSpawnLocations[guestPositionsAvailable[randomNum]].transform);
                currentGuests.Add(newGuest);

                //currentGuestsAcceptingOrders.Add(newGuest);

                newGuest.GetComponent<GuestController>().targetPosForNav = guestSpawnLocations[guestPositionsAvailable[randomNum]];

                newGuest.GetComponent<GuestController>().locationNum = guestPositionsAvailable[randomNum];
                guestPositionsAvailable.RemoveAt(randomNum);
            }

            
            //currentFoodOrders.Add(allFoodOrdersPossible[Random.Range(0, allFoodOrdersPossible.Length)]);

            //ReloadUI();

            //New Food Order

            timeBeforeGuestTimer = 0;

            if (firstGuest)
            {
                maxTimeBeforeNextGuest = timeBeforeNextGuest;
                //firstGuest = false;
            }
            else
            {

                float incrementAmount = Random.Range(0f, 1f);

                if (incrementAmount < 0.5f)
                {
                    if (minDecreaseValue < (minDecreaseValue * 2.5f) || minDecreaseValue == 0)
                    {
                        minDecreaseValue += incrementAmount;
                    }

                    if (maxDecreaseValue < (maxDecreaseValue * 2.5f))
                    {
                        maxDecreaseValue += incrementAmount;
                    }
                }

                timeBeforeNextGuest -= Random.Range(minDecreaseValue, maxDecreaseValue);

                if (timeBeforeNextGuest > minTimeBetweenGuestsArriving)
                {
                }
                else
                {
                    timeBeforeNextGuest = minTimeBetweenGuestsArriving;
                }

                maxTimeBeforeNextGuest = Random.Range(timeBeforeNextGuest * 0.8f, timeBeforeNextGuest);
            }
            
        }
    }

    private float GetRotation()
    {
        float totalAngleSize = minAngle - maxAngle;

        float Normalized = overallSatisfaction / maxSatisfaction;

        return minAngle - Normalized * totalAngleSize;
    }
}
