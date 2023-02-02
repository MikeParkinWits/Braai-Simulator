using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour
{

    [Header("Global")]
    public bool onGrill = false;
    public int grillPos;
    public Material sausageBurnMat;
    public bool sausageBurnt = false;
    public GameObject burningParticleEffect;
    public bool burnLoaded = false;
    public GameObject smokeSpawnPoint;

    public GameObject burnParticleInst;

    [Header("Left Side Cooking")]
    public Material sausageHalfCookedLeftMat;
    public float maxLeftSideCookTimer = 10;
    public float maxLeftSideBurnTimer = 10;
    public float leftSideCooktimer;
    public float leftSideBurnTimer;
    public bool cookingLeftSide = true;
    public bool leftSideCooked = false;

    [Header("Right Side Cooking")]
    public Material sausageHalfCookedRightMat;
    public float maxRightSideCookTimer = 10;
    public float maxRightSideBurnTimer = 10;
    public float rightSideCooktimer;
    public float rightSideBurnTimer;
    public bool cookingRightSide = false;
    public bool rightSideCooked = false;

    [Header("Fully Cooked")]
    public Material sausageFullyCookedMat;
    public float maxFinalBurnTimer = 10;
    public float finalBurnTimer;
   

    private OrderManager _orderManager;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {

        _orderManager = GameObject.Find("GameManager").GetComponent<OrderManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)){
            this.GetComponent<MeshRenderer>().material = sausageHalfCookedLeftMat;
        }

        if (onGrill && !sausageBurnt && !GameManager.GameIsPaused)
        {            
            if (cookingLeftSide)
            {
                LeftSideTimers();
            }
            else if (cookingRightSide)
            {
                RightSideTimers();
            }
        }


        foreach(CurrentFoodOrders currentOrder in _orderManager.currentFoodOrders)
        {
            if (currentOrder.name == this.name && leftSideCooked && rightSideCooked && !sausageBurnt)
            {
                //Debug.Log("Name: " + this.name);
                _gameManager.foodCookedTut = true;
            }
        }

    }

    private void LeftSideTimers()
    {
        if (!leftSideCooked)
        {
            if (leftSideCooktimer <= maxLeftSideCookTimer)
            {
                leftSideCooktimer += Time.deltaTime;
            }
            else
            {
                leftSideCooked = true;
                gameObject.GetComponent<AudioManager>().Play("Cooked");

                if (leftSideCooked && !rightSideCooked)
                {
                    this.GetComponent<MeshRenderer>().material = sausageHalfCookedLeftMat;
                }
                else if (leftSideCooked && rightSideCooked)
                {
                    this.GetComponent<MeshRenderer>().material = sausageFullyCookedMat;
                }
            }
        }
        else if (leftSideCooked && !rightSideCooked)
        {
            if (leftSideBurnTimer <= maxLeftSideBurnTimer)
            {
                leftSideBurnTimer += Time.deltaTime;

                if ((leftSideBurnTimer > (maxLeftSideBurnTimer / 2)) && !burnLoaded)
                {
                    burnParticleInst = Instantiate(burningParticleEffect, new Vector3(smokeSpawnPoint.transform.position.x, smokeSpawnPoint.transform.position.y, smokeSpawnPoint.transform.position.z), Quaternion.Euler(-90, 0, 0));
                    burnLoaded = true;
                    gameObject.GetComponent<AudioManager>().Play("Burning");
                }

            }
            else
            {
                sausageBurnt = true;
                this.GetComponent<MeshRenderer>().material = sausageBurnMat;
            }
        }
        else if (rightSideCooked && leftSideCooked)
        {            
            FullyCookedBurnTimer();            
        }

    }

    private void RightSideTimers()
    {
        if (!rightSideCooked)
        {
            if (rightSideCooktimer <= maxRightSideCookTimer)
            {
                rightSideCooktimer += Time.deltaTime;
            }
            else
            {
                rightSideCooked = true;
                gameObject.GetComponent<AudioManager>().Play("Cooked");

                if (!leftSideCooked && rightSideCooked)
                {
                    this.GetComponent<MeshRenderer>().material = sausageHalfCookedRightMat;
                }
                else if (leftSideCooked && rightSideCooked)
                {
                    this.GetComponent<MeshRenderer>().material = sausageFullyCookedMat;
                }
            }
        }
        else if (rightSideCooked && !leftSideCooked)
        {
            if (rightSideBurnTimer <= maxRightSideBurnTimer)
            {
                rightSideBurnTimer += Time.deltaTime;

                if ((rightSideBurnTimer > (maxRightSideBurnTimer / 2)) && !burnLoaded)
                {
                    burnParticleInst = Instantiate(burningParticleEffect, new Vector3(smokeSpawnPoint.transform.position.x, smokeSpawnPoint.transform.position.y, smokeSpawnPoint.transform.position.z), Quaternion.Euler(-90, 0, 0));
                    burnLoaded = true;
                    gameObject.GetComponent<AudioManager>().Play("Burning");
                }
            }
            else
            {
                sausageBurnt = true;
                this.GetComponent<MeshRenderer>().material = sausageBurnMat;
            }
        }
        else if (rightSideCooked && leftSideCooked)
        {            
            FullyCookedBurnTimer();            
        }

    }

    private void FullyCookedBurnTimer()
    {

        if (finalBurnTimer <= maxFinalBurnTimer)
        {
            finalBurnTimer += Time.deltaTime;

            if ((finalBurnTimer > (maxFinalBurnTimer / 2)) && !burnLoaded)
            {
                burnParticleInst = Instantiate(burningParticleEffect, new Vector3(smokeSpawnPoint.transform.position.x, smokeSpawnPoint.transform.position.y, smokeSpawnPoint.transform.position.z), Quaternion.Euler(-90,0,0));
                burnLoaded = true;
                gameObject.GetComponent<AudioManager>().Play("Burning");
            }
        }
        else
        {
            sausageBurnt = true;
            this.GetComponent<MeshRenderer>().material = sausageBurnMat;
        }

    }
      


}
