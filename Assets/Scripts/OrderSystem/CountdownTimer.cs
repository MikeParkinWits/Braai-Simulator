using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public OrderManager orderManager;
    public OrderNumber orderNum; 
    public enum OrderNumber { ONE, TWO, THREE, FOUR}

    public Sprite sausage;
    public Sprite chicken;
    public Sprite steak;

    private float orderTime;
    public Image[] Timers = new Image[4];

    private void OnEnable()
    {
        
    }
}
