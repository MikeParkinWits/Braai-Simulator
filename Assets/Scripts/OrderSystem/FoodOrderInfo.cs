using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FoodOrderInfo
{
    public string name;
    public float orderTime;
    public Sprite orderImage;

    public FoodOrderInfo(string name, float orderTime, Sprite orderImage)
    {
        this.name = name;
        this.orderTime = orderTime;
        this.orderImage = orderImage;
    }
}
