using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{

    [Header("Standard Timer Variables")]
    public int timeBeforeFirstOrder = 5;
    public float timeBeforeNextOrder = 10.0f;
    public float minimumTimeBetweenOrders = 5f;

    private float maxTimeBeforeNextOrder;
    public float timeBeforeOrderTimer = 0;
    public static bool firstOrder = true;

    [Header("Random Timer Decrease Values")]
    public float maxDecreaseValue = 3f;
    public float minDecreaseValue = 0f;

    [Header("Order Information")]
    public int sausageMaxOrderTime = 30;
    public int chickenMaxOrderTime = 40;
    public int steakMaxOrderTime = 35;
    private FoodOrderInfo[] allFoodOrdersPossible = new FoodOrderInfo[3];
    public FoodOrderUI[] foodOrderUIText = new FoodOrderUI[4];

    public List<CurrentFoodOrders> currentFoodOrders = new List<CurrentFoodOrders>();

    private float[] orderNumberTimers = new float[4];

    private bool canAcceptOrders = true;

    public float satisfactionOrderMissedDecrease = 1;

    public Sprite[] foodIcon = new Sprite[3];

    public GameObject player;

    public static bool firstOrderTutDone = false;

    private GameManager _gameManager;

    public static bool timeRanOut = false;

    public bool secondOrder = false;

    // Start is called before the first frame update
    void Start()
    {
        maxTimeBeforeNextOrder = timeBeforeFirstOrder;

        allFoodOrdersPossible[0] = new FoodOrderInfo("Sausage", Random.Range(sausageMaxOrderTime * 0.8f, sausageMaxOrderTime), foodIcon[0]);
        allFoodOrdersPossible[1] = new FoodOrderInfo("Chicken", Random.Range(chickenMaxOrderTime * 0.8f, chickenMaxOrderTime), foodIcon[1]);
        allFoodOrdersPossible[2] = new FoodOrderInfo("Steak", Random.Range(steakMaxOrderTime * 0.8f, steakMaxOrderTime), foodIcon[2]);

        foodOrderUIText[0].nameText.text = allFoodOrdersPossible[0].name.ToString();
        foodOrderUIText[1].nameText.text = allFoodOrdersPossible[1].name.ToString();

        ////Debug.LogError(allFoodOrdersPossible[0].name);
        ////Debug.LogError(allFoodOrdersPossible[1].name);
        ////Debug.LogError(allFoodOrdersPossible[2].name);

        currentFoodOrders.Clear();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        firstOrder = true;
        secondOrder = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.GameIsPaused)
        {
            if (GuestManager.currentGuestsAcceptingOrders.Count != 0)
            {
                OrderTimer();
            }


            /*

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentFoodOrders.RemoveAt(0);
                foodOrderUIText[0].orderUI.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShiftUI(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentFoodOrders.RemoveAt(2);
                foodOrderUIText[2].orderUI.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentFoodOrders.RemoveAt(3);
                foodOrderUIText[3].orderUI.SetActive(false);
            }

            */

            if (currentFoodOrders.Count == 1)
            {
                if (GameManager.tutorialDone)
                {
                    OrderOneTimer();
                }
            }
            else if (currentFoodOrders.Count == 2)
            {
                OrderOneTimer();
                OrderTwoTimer();
            }
            else if (currentFoodOrders.Count == 3)
            {
                OrderOneTimer();
                OrderTwoTimer();
                OrderThreeTimer();
            }
            else if (currentFoodOrders.Count == 4)
            {
                OrderOneTimer();
                OrderTwoTimer();
                OrderThreeTimer();
                OrderFourTimer();
            }
        }
    }

    private void OrderTimer()
    {

        if (currentFoodOrders.Count < 4)
        {
            if (timeBeforeOrderTimer < maxTimeBeforeNextOrder)
            {
                timeBeforeOrderTimer += Time.deltaTime;
            }
            else
            {

                if (currentFoodOrders.Count < 4)
                {

                    if (GuestManager.currentGuestsAcceptingOrders.Count != 0)
                    {
                        int orderTypeNum = Random.Range(0, allFoodOrdersPossible.Length);
                        int guestToGetOrder = Random.Range(0, GuestManager.currentGuestsAcceptingOrders.Count);

                        firstOrderTutDone = true;

                        foreach (GameObject guestFood in GuestManager.currentGuestsAcceptingOrders)
                        {
                            //Debug.Log("Num" + guestFood.GetComponent<GuestController>().locationNum);
                        }

                        currentFoodOrders.Add(new CurrentFoodOrders(allFoodOrdersPossible[orderTypeNum].name, allFoodOrdersPossible[orderTypeNum].orderTime, GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().locationNum, allFoodOrdersPossible[orderTypeNum].orderImage));
                        player.GetComponent<AudioSource>().Play();

                        CurrentFoodOrders order = null;

                        for (int i = 0; i < currentFoodOrders.Count; i++)
                        {
                            if (currentFoodOrders[i].guestOrderLocation == GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().locationNum)
                            {
                                order = currentFoodOrders[i];
                                break;
                            }
                        }

                        ////Debug.Log(currentFoodOrders[0].guestOrderLocation);
                        ////Debug.Log(GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().locationNum);

                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().currentOrder = order;

                        ////Debug.Log(GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().currentOrder.name);

                        if (firstOrderTutDone)
                        {
                            _gameManager.tutorialText.SetActive(false);
                            _gameManager.exclamationMarkUI.SetActive(false);
                            _gameManager.orderText.SetActive(true);
                        }

                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().awaitingOrder = true;
                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().canAcceptOrder = false;
                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().maxOrderTimer = GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().currentOrder.orderTime;
                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().setMaxOrderTimer = GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().maxOrderTimer;
                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().currentOrderSet = true;

                        GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().orderImageUI.sprite = GuestManager.currentGuestsAcceptingOrders[guestToGetOrder].GetComponent<GuestController>().currentOrder.guestOrderImage;

                        ////Debug.Log(allFoodOrdersPossible.Length);

                        if (GuestManager.currentGuestsAcceptingOrders.Count != 0)
                        {
                            canAcceptOrders = true;
                        }
                        else
                        {
                            canAcceptOrders = false;
                        }


                        GuestManager.currentGuestsAcceptingOrders.RemoveAt(guestToGetOrder);
                    }

                    ReloadUI();

                    //New Food Order

                    timeBeforeOrderTimer = 0;

                    //firstOrderTutDone = true;

                    if (firstOrder)
                    {
                        maxTimeBeforeNextOrder = timeBeforeFirstOrder * 2.7f;
                        //firstOrder = false;

                        firstOrder = false;

                        secondOrder = true;

                        //Debug.Log("YEAHHH");
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

                        if (secondOrder)
                        {
                            maxTimeBeforeNextOrder = timeBeforeNextOrder;
                            secondOrder = false;
                        }
                        else
                        {
                            timeBeforeNextOrder -= Random.Range(minDecreaseValue, maxDecreaseValue);
                        }

                        //Debug.Log("TIME: " + timeBeforeNextOrder);

                        if (timeBeforeNextOrder > minimumTimeBetweenOrders)
                        {
                        }
                        else
                        {
                            timeBeforeNextOrder = minimumTimeBetweenOrders;
                        }

                        maxTimeBeforeNextOrder = Random.Range(timeBeforeNextOrder * 0.8f, timeBeforeNextOrder);
                    }
                }
                else if (currentFoodOrders.Count > 4)
                {
                    timeBeforeOrderTimer = 0;
                }
            }
        }
        
    }

    private void ReloadUI()
    {

        for (int i = 0; i < foodOrderUIText.Length; i++)
        {
            if (currentFoodOrders.Count <= i)
            {
                foodOrderUIText[i].orderUI.SetActive(false);
            }
            else if (currentFoodOrders.Count >= i)
            {
                if (!foodOrderUIText[i].orderUI.activeSelf)
                {
                    foodOrderUIText[i].orderUI.SetActive(true);
                    foodOrderUIText[i].nameText.text = currentFoodOrders[i].name.ToString();
                    foodOrderUIText[i].orderImage.sprite = currentFoodOrders[i].guestOrderImage;

                    orderNumberTimers[i] = currentFoodOrders[i].orderTime;

                    ////Debug.Log(currentFoodOrders[i].orderTime);
                }
            }
        }
        
    }

    public void ShiftUI(int shiftFrom)
    {

        for (int i = shiftFrom; i < currentFoodOrders.Count; i++)
        {

            if (i + 1 < currentFoodOrders.Count)
            {
                orderNumberTimers[i] = orderNumberTimers[i + 1];

                foodOrderUIText[i].nameText.text = currentFoodOrders[i + 1].name.ToString();
                foodOrderUIText[i].orderImage.sprite = currentFoodOrders[i + 1].guestOrderImage;

                ////Debug.Log("IT HAS RUN");
            }
            
        }

        foodOrderUIText[currentFoodOrders.Count - 1].orderUI.SetActive(false);

        currentFoodOrders.RemoveAt(shiftFrom);

    }

    private void OrderOneTimer()
    {
        if (orderNumberTimers[0] > 0)
        {
            foodOrderUIText[0].orderTimeText.text = orderNumberTimers[0].ToString("f0");
            orderNumberTimers[0] -= Time.deltaTime;
        }
        else
        {
            orderNumberTimers[0] = 0;
            foodOrderUIText[0].orderTimeText.text = orderNumberTimers[0].ToString("f0");

            ShiftUI(0);

            GuestManager.overallSatisfaction -= satisfactionOrderMissedDecrease;

            timeRanOut = true;

            ////Debug.Log(GuestManager.overallSatisfaction);
        }
    }

    private void OrderTwoTimer()
    {
        if (orderNumberTimers[1] > 0)
        {
            foodOrderUIText[1].orderTimeText.text = orderNumberTimers[1].ToString("f0");
            orderNumberTimers[1] -= Time.deltaTime;
        }
        else
        {
            orderNumberTimers[1] = 0;
            foodOrderUIText[1].orderTimeText.text = orderNumberTimers[1].ToString("f0");

            ShiftUI(1);

            GuestManager.overallSatisfaction -= satisfactionOrderMissedDecrease;

            timeRanOut = true;

            ////Debug.Log(GuestManager.overallSatisfaction);
        }
    }

    private void OrderThreeTimer()
    {
        if (orderNumberTimers[2] > 0)
        {
            foodOrderUIText[2].orderTimeText.text = orderNumberTimers[2].ToString("f0");
            orderNumberTimers[2] -= Time.deltaTime;
        }
        else
        {
            orderNumberTimers[2] = 0;
            foodOrderUIText[2].orderTimeText.text = orderNumberTimers[2].ToString("f0");

            ShiftUI(2);

            GuestManager.overallSatisfaction -= satisfactionOrderMissedDecrease;

            timeRanOut = true;

            ////Debug.Log(GuestManager.overallSatisfaction);
        }
    }

    private void OrderFourTimer()
    {
        if (orderNumberTimers[3] > 0)
        {
            foodOrderUIText[3].orderTimeText.text = orderNumberTimers[3].ToString("f0");
            orderNumberTimers[3] -= Time.deltaTime;
        }
        else
        {
            orderNumberTimers[3] = 0;
            foodOrderUIText[3].orderTimeText.text = orderNumberTimers[0].ToString("f0");

            ShiftUI(3);

            GuestManager.overallSatisfaction -= satisfactionOrderMissedDecrease;

            timeRanOut = true;

            ////Debug.Log(GuestManager.overallSatisfaction);
        }
    }

}
