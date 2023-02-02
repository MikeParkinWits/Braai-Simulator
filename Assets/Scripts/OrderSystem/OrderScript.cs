using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour
{    
    public Order orderNumber;
    public OrderUI orderUI;    

    public GameObject[] Orders = new GameObject[3];
    public float TimeBetweenOrders;
    public bool[] OrderActive = new bool[3];

    public float[] Timers = new float[3];
    public int[] Food = new int[3];

    public enum Order { ONE, TWO, THREE }

    public float maxTime;
    public float currentTime;
    private void Start()
    {
        maxTime = 5;
        currentTime = maxTime;

        TimeBetweenOrders = Random.Range(6, 10);
                        
    }

    public void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0 && OrderActive[2] == false)
        {
            AssignOrder();
        }
        

    }
    private void OnCollisionEnter(Collision collider)
    {
        if (orderNumber == Order.ONE)
        {
            if (orderUI.OrderFail[0] == false)
            {
                if (Food[0] == 1 && Timers[0] > 0)
                {
                    if (collider.gameObject.name == "Sausage")
                    {
                        orderUI.OrderComplete[0] = true;
                        Destroy(collider.gameObject);
                    }
                    else if (collider.gameObject.name == "Chicken")
                    {

                    }
                }
                else if (Food[0] == 2 && Timers[0] > 0)
                {
                    if (collider.gameObject.name == "Chicken")
                    {
                        orderUI.OrderComplete[0] = true;
                        Destroy(collider.gameObject);
                    }
                    else if (collider.gameObject.name == "Sausage")
                    {

                    }
                }
            }
            else if (orderUI.OrderFail[0] == true)
            {

            }
            
            
        }
        else if (orderNumber == Order.TWO)
        {
            if (orderUI.OrderFail[1] == false)
            {
                if (Food[1] == 1 && Timers[0] > 0)
                {
                    if (collider.gameObject.name == "Sausage")
                    {
                        orderUI.OrderComplete[1] = true;
                        Destroy(collider.gameObject);
                    }
                    else if (collider.gameObject.name == "Chicken")
                    {

                    }
                }
                else if (Food[1] == 2 && Timers[0] > 0)
                {
                    if (collider.gameObject.name == "Chicken")
                    {
                        orderUI.OrderComplete[1] = true;
                        Destroy(collider.gameObject);
                    }
                    else if (collider.gameObject.name == "Sausage")
                    {

                    }
                }
            }
            else if (orderUI.OrderFail[1] == true)
            {

            }
            
        }
        else if (orderNumber == Order.THREE)
        {
            if (orderUI.OrderFail[2] == false)
            {
                if (Food[2] == 1 && Timers[0] > 0)
                {
                    if (collider.gameObject.name == "Sausage")
                    {
                        orderUI.OrderComplete[2] = true;
                        Destroy(collider.gameObject);
                    }
                    else if (collider.gameObject.name == "Chicken")
                    {

                    }
                }
                else if (Food[2] == 2 && Timers[0] > 0)
                {
                    if (collider.gameObject.name == "Chicken")
                    {
                        orderUI.OrderComplete[2] = true;
                        Destroy(collider.gameObject);
                    }
                    else if (collider.gameObject.name == "Sausage")
                    {

                    }
                }
            }
            else if (orderUI.OrderFail[2] == true)
            {

            }
            
        }
    }

    private void AssignOrder()
    {
        if (OrderActive[0] == true)
        {
            if (OrderActive[1] == true)
            {
                if (OrderActive[2] == true)
                {

                }
                else if (OrderActive[2] == false)
                {
                    OrderActive[2] = true;
                    Orders[2].SetActive(true);
                    Timers[2] = Random.Range(30, 40);
                    Food[2] = Random.Range(1, 3);
                    currentTime = 0;
                }
            }
            else if (OrderActive[1] == false)
            {
                OrderActive[1] = true;
                Orders[1].SetActive(true);
                Timers[1] = Random.Range(30, 40);
                Food[1] = Random.Range(1, 3);
                currentTime = TimeBetweenOrders - 1;
            }
        }
        else if (OrderActive[0] == false)
        {
            OrderActive[0] = true;
            Orders[0].SetActive(true);
            Timers[0] = Random.Range(30, 40);
            Food[0] = Random.Range(1, 3);
            currentTime = TimeBetweenOrders;
        }
        


        
    }
}
