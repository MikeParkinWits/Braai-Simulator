using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FoodOrderUI
{
    public GameObject orderUI;
    public Text nameText;
    public Text orderTimeText;
    public Image orderImage;

    public FoodOrderUI(GameObject orderUI, Text nameText, Text orderTimeText, Image orderImage)
    {
        this.orderUI = orderUI;
        this.nameText = nameText;
        this.orderTimeText = orderTimeText;
        this.orderImage = orderImage;
    }
}
