using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public OrderScript[] orderScript = new OrderScript[3];
    public bool[] OrderComplete = new bool[3];
    public bool[] OrderFail = new bool[3];
        
    public Image[] Timers = new Image[3];
    public Image[] Orders = new Image[3];
    
    public Image[] Images = new Image[3];

    public Text textItem;

    public Sprite chicken;
    public Sprite sausage;

    

    private void Start()
    {
        
        
    }

    private void Update()
    {
        
        if (orderScript[0].Food[0] == 1)
        {
            Orders[0].sprite = sausage;
        }
        else if (orderScript[0].Food[0] == 2)
        {
            Orders[0].sprite = chicken;
        }

        if (orderScript[1].Food[1] == 1)
        {
            Orders[1].sprite = sausage;
        }
        else if (orderScript[1].Food[1] == 2)
        {
            Orders[1].sprite = chicken;
        }

        if (orderScript[2].Food[2] == 1)
        {
            Orders[2].sprite = sausage;
        }
        else if (orderScript[2].Food[2] == 2)
        {
            Orders[2].sprite = chicken;
        }

        if (OrderComplete[0] == true)
        {
            
            Images[0].GetComponent<Image>().color = Color.green;
            
        }
        else if (orderScript[0].Timers[0] <= 0 && orderScript[0].OrderActive[0] == true)
        {
            Timers[0].fillAmount = 0;
            Images[0].GetComponent<Image>().color = Color.red;
            OrderFail[0] = true;
        }
        else if (orderScript[0].Timers[0] > 0 && orderScript[0].OrderActive[0] == true)
        {
            orderScript[0].Timers[0] -= Time.deltaTime;
            Timers[0].fillAmount -= 1 / orderScript[0].Timers[0] * Time.deltaTime;
        }

        if (OrderComplete[1] == true)
        {
            
            Images[1].GetComponent<Image>().color = Color.green;            
        }
        else if (orderScript[1].Timers[1] <= 0 && orderScript[1].OrderActive[1] == true)
        {
            Timers[1].fillAmount = 0;
            Images[1].GetComponent<Image>().color = Color.red;
            OrderFail[1] = true;
        }
        else if (orderScript[1].Timers[1] > 0 && orderScript[1].OrderActive[1] == true)
        {
            orderScript[1].Timers[1] -= Time.deltaTime;
            Timers[1].fillAmount -= 1 / orderScript[1].Timers[1] * Time.deltaTime; 
        }

        if (OrderComplete[2] == true)
        {
            
            Images[2].GetComponent<Image>().color = Color.green;            
        }
        else if (orderScript[2].Timers[2] <= 0 && orderScript[2].OrderActive[2] == true)
        {
            Timers[2].fillAmount = 0;
            Images[2].GetComponent<Image>().color = Color.red;
            OrderFail[2] = true;
        }
        else if (orderScript[2].Timers[2] > 0 && orderScript[2].OrderActive[2] == true)
        {
            orderScript[2].Timers[2] -= Time.deltaTime;
            Timers[2].fillAmount -= 1 / orderScript[2].Timers[2] * Time.deltaTime;
        }

        if (OrderComplete[0] == true && OrderComplete[1] == true && OrderComplete[2] == true)
        {
            textItem.text = "Success!";
        }

        if (OrderFail[0] == true && OrderFail[1] == true && OrderFail[2] == true)
        {
            textItem.text = "Fail!";
        }


    }
        
}
