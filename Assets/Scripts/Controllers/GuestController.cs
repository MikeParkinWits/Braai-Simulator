using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GuestController : MonoBehaviour
{

    public int locationNum;
    public bool awaitingOrder = false;
    public bool canAcceptOrder = false;

    public float cooldownTimerAmount = 5f;

    public float minOrderTimer = 0;
    public float maxOrderTimer;

    public float minCooldownTimer = 0;
    public float maxCooldownTimer;

    public CurrentFoodOrders currentOrder;

    public GameObject orderPrefab;
    public GameObject worldCanvas;

    public GameObject orderPrefabOne;
    public GameObject worldCanvasOne;
    private GameObject spawnedUIOne;

    public GameObject UISpawnPoint;

    private GameObject spawnedUI;
    public Text orderNameUI;
    private Text orderTimeUI;
    public Image orderImageUI;
    private Image orderTimerUI;
    private GameObject orderTimerUIGameObject;
    public Gradient circleTimerGradient;
    public float setMaxOrderTimer;

    private GameObject lookAtTarget;

    public GameObject targetPosForNav;

    private NavMeshAgent navMeshAgent;

    private bool finishedMoving = false;

    private float satisfactionLevel;

    public float satisfactionDecreaseBurntFood = 1;

    public float satisfactionDecreaseOverTimeMax = 45;
    public float satisfactionDecreaseOverTimeTimer = 0;

    public float satisfactionDecreaseAmount = 0.4f;

    public bool currentOrderSet = false;

    public OrderManager orderManager;

    private GameObject satisfactionUI;
    public GameObject satisfactionBubble;
    private Image satisfactionIcon;
    public Sprite[] satisfactionImage = new Sprite[2];
    public float disappearTimer = DisappearTimerMax;
    public bool UIDisappear;
    float moveSpeed = 1f;
    private Color UIColor;
    float disappearSpeed = 1f;
    private const float DisappearTimerMax = 2f;
    float scaleAmount = 1f;

    public GameObject plateHoldPos;
    public GameObject plateHoldObject;
    public bool guestIsHoldingPlate = false;    

    private float tutorialTimer = 0;

    public Rigidbody rb;

    [SerializeField]
    public Animator animator;
    public bool canEat = true;


    public Transform rotationTarget;
    public float RotationSpeed;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    public GameObject materialObject;

    public Material[] playerMaterials = new Material[3];

    private float angryTimer = 4;

    private float moveTimer;

    public GuestManager _guestManager;

    // Start is called before the first frame update
    void Start()
    {
        maxCooldownTimer = cooldownTimerAmount;

        SkinnedMeshRenderer renderer = materialObject.GetComponent<SkinnedMeshRenderer>();

        Material[] mats = renderer.materials;

        mats[1] = playerMaterials[Random.Range(0,3)];

        renderer.materials = mats;

        //materialObject.GetComponent<SkinnedMeshRenderer>().materials[1] = playerMaterials[1];

        //Material[] mats = materialObject.GetComponent<Renderer>().materials;

        //worldCanvas = GameObject.Find("World Canvas");

        _guestManager = GameObject.Find("GameManager").GetComponent<GuestManager>();

        orderManager = GameObject.Find("GameManager").GetComponent<OrderManager>();

        worldCanvas = Instantiate(worldCanvasOne);

        worldCanvas.transform.parent = this.transform;

        spawnedUI = Instantiate(orderPrefab, UISpawnPoint.transform.position, Quaternion.identity, worldCanvas.transform);

        lookAtTarget = GameObject.FindGameObjectWithTag("Player");

        orderNameUI = spawnedUI.transform.Find("Order Names").GetComponent<Text>();
        orderTimeUI = spawnedUI.transform.Find("Order Time").GetComponent<Text>();
        orderImageUI = spawnedUI.transform.Find("Food Icon").GetComponent<Image>();
        orderTimerUI = spawnedUI.transform.Find("Circle Timer").GetComponent<Image>();
        spawnedUI.SetActive(false);

        orderTimerUIGameObject = orderTimerUI.gameObject;


        navMeshAgent = GetComponent<NavMeshAgent>();

        satisfactionLevel = Random.Range(30.0f, 70.0f);


        satisfactionUI = Instantiate(satisfactionBubble, UISpawnPoint.transform.position, Quaternion.identity, worldCanvas.transform);
        satisfactionUI.SetActive(false);

        //rb = GetComponent<Rigidbody>();

        moveTimer = Random.Range(30.0f, 60.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.GameIsPaused)
        {




            //Debug.Log("Vel: " + navMeshAgent.velocity.magnitude);


            animator.SetFloat("Velocity", navMeshAgent.velocity.magnitude);

            if (!canAcceptOrder)
            {
                OrderTimer();

                for (int i = 0; i < orderManager.currentFoodOrders.Count; i++)
                {
                    if (orderManager.currentFoodOrders[i].guestOrderLocation == locationNum)
                    {
                        currentOrder = orderManager.currentFoodOrders[i];
                        break;
                    }
                }

            }
            else
            {
                spawnedUI.SetActive(false);

                satisfactionDecreaseTimer();
            }

            if (GameManager.tutorialDone)
            {
                orderTimerUIGameObject.SetActive(true);
            }
            else
            {
                orderTimerUIGameObject.SetActive(false);
            }

            Vector3 targetPos = new Vector3(lookAtTarget.transform.position.x, spawnedUI.transform.position.y, lookAtTarget.transform.position.z);

            spawnedUI.transform.LookAt(targetPos);

            spawnedUI.transform.position = UISpawnPoint.transform.position;

            satisfactionUI.transform.LookAt(targetPos);

            satisfactionUI.transform.position = UISpawnPoint.transform.position;

            navMeshAgent.destination = targetPosForNav.transform.position;

            if (Vector3.Distance(transform.position, targetPosForNav.transform.position) < 0.82f)
            {

                if (GuestManager.currentGuests.Count > 1)
                {

                    float distanceToClosestEnemy = Mathf.Infinity;
                    GameObject closestEnemy = null;
                    //Edit Enemy in the FindObjectsOfType to a component on the object you
                    //want to find nearest 
                    GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Guest");

                    foreach (GameObject currentEnemy in allEnemies)
                    {
                        float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
                        if (distanceToEnemy < distanceToClosestEnemy)
                        {
                            if (currentEnemy != this.gameObject)
                            {
                                distanceToClosestEnemy = distanceToEnemy;
                                closestEnemy = currentEnemy;
                            }

                        }
                    }

                    if (angryTimer < 4)
                    {
                        angryTimer += Time.deltaTime;
                        rotationTarget = GameObject.Find("Player").GetComponent<Transform>();
                    }
                    else
                    {
                        rotationTarget = closestEnemy.transform;
                    }
                }
                else
                {
                    rotationTarget = GameObject.Find("Player").GetComponent<Transform>();
                }

                GameObject rotationGameobject = rotationTarget.gameObject;
                //rotationTarget.transform.position = new Vector3(this.transform.position.x, rotationTarget.transform.position.y, rotationTarget.transform.position.z);

                _direction = (rotationTarget.position - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                if (GuestManager.currentGuests.Count == 1 && !guestIsHoldingPlate)
                {
                    this.transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 1 * Time.deltaTime);
                }
                else if (GuestManager.currentGuests.Count > 1)
                {
                    this.transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 1 * Time.deltaTime);
                }

                navMeshAgent.updateRotation = false;

                if (!finishedMoving)
                {
                    finishedMoving = true;

                    GuestManager.currentGuestsAcceptingOrders.Add(gameObject);
                    GetComponent<AudioManager>().Play("Hello");

                    

                    if (GuestManager.currentGuests.Count > 2)
                    {
                        GameObject.Find("GuestAudioSource").GetComponent<AudioSource>().volume += 0.1f;
                                                
                    }

                                                          

                    awaitingOrder = false;
                    canAcceptOrder = true;
                }

                
                /*

                if (canAcceptOrder && finishedMoving && GuestManager.currentGuests.Count == 6)
                {
                    if (moveTimer > 0)
                    {
                        moveTimer -= Time.deltaTime;
                    }
                    else
                    {

                        int randomNum = Random.Range(0, GuestManager.guestPositionsAvailable.Count);
                        targetPosForNav = _guestManager.guestSpawnLocations[GuestManager.guestPositionsAvailable[randomNum]];

                        GuestManager.guestPositionsAvailable.RemoveAt(randomNum);

                        GuestManager.guestPositionsAvailable.Add(this.GetComponentInParent<GameObject>());

                        rotationTarget = targetPosForNav.GetComponent<Transform>();
                        finishedMoving = false;
                    }
                }

                */
            }
                        

            if (UIDisappear == true)
            {
                disappearTimer -= Time.deltaTime;

                if (disappearTimer <= 0)
                {
                    satisfactionUI.SetActive(false);
                    UIDisappear = false;
                    disappearTimer = DisappearTimerMax;
                }
            }
        }
              
        
    }

    private void OrderTimer()
    {
        if (GameManager.tutorialDone)
        {
            if (maxOrderTimer > 0)
            {
                spawnedUI.SetActive(true);
                maxOrderTimer -= Time.deltaTime;
            }
            else
            {
                spawnedUI.SetActive(false);

                CoolDownTimer();

                currentOrder = null;

                if (OrderManager.timeRanOut)
                {
                    OrderManager.timeRanOut = false;
                    animator.SetTrigger("Angry");
                    angryTimer = 0;
                    SatisfactionDown();
                }
            }

            float valueNormalized = Mathf.InverseLerp(0, setMaxOrderTimer, maxOrderTimer);
            ////Debug.Log("Value Normalized" + valueNormalized);
            //orderTimeUI.text = maxOrderTimer.ToString("f0");
            orderTimerUI.fillAmount = valueNormalized;
            orderTimerUI.color = circleTimerGradient.Evaluate(valueNormalized);
        }
        else
        {
            spawnedUI.SetActive(true);

            if (guestIsHoldingPlate != false)
            {

                //animator.SetTrigger("Eating");
                //animator.ResetTrigger("Eating");

                if (tutorialTimer < 2)
                {
                    tutorialTimer += Time.deltaTime;
                }
                else
                {
                    plateHoldObject.GetComponent<PlateController>().foodPlacedOntop = false;
                    Destroy(plateHoldObject.GetComponent<PlateController>().foodPlacedObject);

                    plateHoldObject.GetComponent<PlateController>().foodPlacedObject = null;

                    guestIsHoldingPlate = false;

                    FindClosest();

                    tutorialTimer = 0;
                }
            }
        }

    }

    private void CoolDownTimer()
    {
        if (minCooldownTimer < maxCooldownTimer)
        {
            minCooldownTimer += Time.deltaTime;
            awaitingOrder = false;

            if (guestIsHoldingPlate && canEat)
            {
                animator.SetTrigger("Eating");
                //animator.ResetTrigger("Eating");

                canEat = false;
            }
            else
            {
                //animator.ResetTrigger("Eating");
            }
        }
        else
        {
            canAcceptOrder = true;
            minCooldownTimer = 0;
            minOrderTimer = 0;
            maxOrderTimer = 0;
            setMaxOrderTimer = 0;
            //satisfactionDecreaseOverTimeTimer = 0;

            currentOrder = null;

            currentOrderSet = false;

            canEat = true;

            GuestManager.currentGuestsAcceptingOrders.Add(gameObject);

            if (guestIsHoldingPlate != false)
            {
                plateHoldObject.GetComponent<PlateController>().foodPlacedOntop = false;
                Destroy(plateHoldObject.GetComponent<PlateController>().foodPlacedObject);

                plateHoldObject.GetComponent<PlateController>().foodPlacedObject = null;

                guestIsHoldingPlate = false;

                FindClosest();
            }


        }
    }

    private void satisfactionDecreaseTimer()
    {
        if (satisfactionDecreaseOverTimeTimer < satisfactionDecreaseOverTimeMax)
        {
            satisfactionDecreaseOverTimeTimer += Time.deltaTime;
        }
        else
        {
            if (GuestManager.currentGuests.Count >= 4 && GuestManager.overallSatisfaction < 70)
            {
                GuestManager.overallSatisfaction -= satisfactionDecreaseAmount/2;
            }
            else
            {
                GuestManager.overallSatisfaction -= satisfactionDecreaseAmount;                
            }

            satisfactionDecreaseOverTimeTimer = 0;
        }
    }

    public void SatisfactionUp()
    {
        satisfactionUI.SetActive(true);
        satisfactionIcon = satisfactionUI.transform.Find("Icon").GetComponent<Image>();
        satisfactionIcon.sprite = satisfactionImage[0];        
        UIDisappear = true;
    }

    public void SatisfactionDown()
    {
        satisfactionUI.SetActive(true);
        satisfactionIcon = satisfactionUI.transform.Find("Icon").GetComponent<Image>();
        satisfactionIcon.sprite = satisfactionImage[1];        
        UIDisappear = true;
    }

    private void FindClosest()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        PlaceableController closestEnemy = null;
        //Edit Enemy in the FindObjectsOfType to a component on the object you
        //want to find nearest 
        PlaceableController[] allEnemies = GameObject.FindObjectsOfType<PlaceableController>();

        foreach (PlaceableController currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                if (currentEnemy.itemPlaced != true)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }

            }
        }

        //Debug.Log("CLOSEST OBJECT POS:" + closestEnemy.transform.position);

        this.GetComponent<AudioManager>().Play("Place");

        plateHoldObject.gameObject.transform.position = new Vector3(closestEnemy.GetComponent<PlaceableController>().placePos.transform.position.x, closestEnemy.GetComponent<PlaceableController>().placePos.transform.position.y - 0.047f, closestEnemy.GetComponent<PlaceableController>().placePos.transform.position.z);
        plateHoldObject.gameObject.transform.parent = closestEnemy.GetComponent<PlaceableController>().placePos.transform;
        closestEnemy.GetComponent<PlaceableController>().itemPlacedBool = true;
        closestEnemy.GetComponent<PlaceableController>().itemPlaced = plateHoldObject;

        plateHoldObject = null;
    }
        
}
