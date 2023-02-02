using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CurrentFoodOrders
{
    public string name;
    public float orderTime;
    public float guestOrderLocation;
    public Sprite guestOrderImage;

    public CurrentFoodOrders(string name, float orderTime, float guestOrderLocation, Sprite guestOrderImage)
    {
        this.name = name;
        this.orderTime = orderTime;
        this.guestOrderLocation = guestOrderLocation;
        this.guestOrderImage = guestOrderImage;
    }
}
