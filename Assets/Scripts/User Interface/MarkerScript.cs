using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerScript : MonoBehaviour
{
    public Image markerImage;
    private Image markerTimer;
    public FoodItem food;
    private GameObject Target;

    private Color yellow = Color.yellow;
    private Color green = Color.green;
    private Color red = Color.red;
    private Color white = Color.white;
    private Color black = Color.black;
    private Color orange = new Color(1.0f, 0.64f, 0.0f);

    void OnEnable()
    {
        markerImage = this.transform.Find("Marker").GetComponent<Image>();
        markerTimer = this.transform.Find("Circle Timer").GetComponent<Image>();
        markerTimer.color = black;

        Target = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        
        if (food.cookingLeftSide == true && food.rightSideCooked == false)
        {
            LeftSideCookTimer();
        }

        if (food.cookingLeftSide == true && food.rightSideCooked == true)
        {
            markerTimer.color = yellow;
            LeftSideCookTimer();
        }

        if (food.leftSideCooked == true)
        {
            markerTimer.color = orange;
            LeftSideBurnTimer();
        }

        if (food.cookingRightSide == true && food.leftSideCooked == false)
        {
            markerTimer.color = black;
            RightSideCookTimer();
        }

        if (food.cookingRightSide == true && food.leftSideCooked == true)
        {
            markerTimer.color = yellow;
            RightSideCookTimer();
        }
        
        if (food.rightSideCooked == true && food.cookingLeftSide == false)
        {
            markerTimer.color = orange;
            RightSideBurnTimer();
        }        

        if (food.leftSideCooked == true && food.rightSideCooked == true)
        {
            markerTimer.color = green;
            FinalBurnTimer();
        }

        if (food.sausageBurnt == true)
        {
            markerTimer.color = red;
        }
    }

    private void LeftSideCookTimer()
    {
        float valueNormalized = Mathf.InverseLerp(0, food.maxLeftSideCookTimer, food.leftSideCooktimer);
        markerTimer.fillAmount = valueNormalized;        
    }

    private void LeftSideBurnTimer()
    {
        float valueNormalized = Mathf.InverseLerp(0, food.maxLeftSideBurnTimer, food.leftSideBurnTimer);
        markerTimer.fillAmount = valueNormalized;
    }

    private void RightSideCookTimer()
    {
        float valueNormalized = Mathf.InverseLerp(0, food.maxRightSideCookTimer, food.rightSideCooktimer);
        markerTimer.fillAmount = valueNormalized;
    }

    private void RightSideBurnTimer()
    {
        float valueNormalized = Mathf.InverseLerp(0, food.maxRightSideBurnTimer, food.rightSideBurnTimer);
        markerTimer.fillAmount = valueNormalized;
    }

    private void FinalBurnTimer()
    {
        float valueNormalized = Mathf.InverseLerp(0, food.maxFinalBurnTimer, food.finalBurnTimer);
        markerTimer.fillAmount = valueNormalized;
    }
}
