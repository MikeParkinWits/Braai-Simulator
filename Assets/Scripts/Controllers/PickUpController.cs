using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{

    [Header("Instantiations")]
    public Camera mainCam;
    public GameObject holdPos;
    public GameObject foodLetGoPos;
    public GameObject player;

    public GameObject instantiateSausage;
    public GameObject instantiateChicken;
    public GameObject instantiateSteak;

    private OrderManager _orderManager;
    private GameManager _gameManager;
    public GameObject braai;

    [Header("Customizable Variables")]
    [SerializeField] float distanceToItem = 3f;

    public GameObject pickedUpItem;
    public GameObject pickedUpItemAsset;

    public bool holdingUtensil = false;
    public bool holdingTong = false;
    public bool holdingSpatula = false;
    public bool holdingPlate = false;


    public UtensilController utens;

    bool test = false;

    public CapsuleCollider sausageCapsuleCollider;
    public BoxCollider chickenBoxCollider;
    public BoxCollider steakBoxCollider;

    public Animator anim;

    public bool[] braaiPositionStatus = new bool[4];
    public GameObject[] braaiPositions = new GameObject[4];    

    public GameObject wrongText;
    public Text wrongTextUI;

    public bool showWrongText = false;

    private bool wasFoodOrdered = false;

    public float wrongTimer = 0;

    public LayerMask ignore;

    public float maxSatisfactionPerOrder = 5f;
    public float satsifactionDropBurntFood = 2.5f;

    public OrderManager orderManager;

    public GameObject worldCanvas;

    public bool actionDone = false;

    public GameObject[] Marker = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        _orderManager = GameObject.Find("GameManager").GetComponent<OrderManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        orderManager = GameObject.Find("GameManager").GetComponent<OrderManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            utens.pickedUpItem.transform.localEulerAngles = new Vector3(90, 0, 0);
        }

        CompletedFood();

        PlaceOnBraai();

        ThrowAwayFood();

        PickUpItem();

        DropItem();

        anim.SetFloat("Velocity", player.GetComponent<CharacterController>().velocity.magnitude);

        FlipFood();

        if (showWrongText)
        {
            if (wrongTimer < 5)
            {
                wrongTimer += Time.deltaTime;
            }
            else
            {
                showWrongText = false;
                wrongText.SetActive(false);
                wrongTimer = 0;
            }
        }

        if (braaiPositionStatus[0] == false && braaiPositionStatus[1] == false && braaiPositionStatus[2] == false && braaiPositionStatus[3] == false)
        {
            braai.GetComponent<AudioSource>().Stop();
        }

        actionDone = false;                
    }

    private void PickUpItem()
    {
        
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));


            if (Physics.Raycast(ray, out hit, distanceToItem, ignore))
            {

                ////Debug.LogError(hit.collider.gameObject);

            if (hit.collider.gameObject.tag == "Interactable")
            {


                if (hit.collider.gameObject.name == "Sausage Pile" && holdingUtensil && holdingTong)
                {


                    if (utens != null && !utens.holdingItem)
                    {
                        utens.pickedUpItem = Instantiate(instantiateSausage);
                        utens.pickedUpItem.name = "Sausage";
                        utens.pickedUpItem.layer = 6;

                        utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;

                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                        utens.pickedUpItem.transform.position = new Vector3(utens.holdPos.transform.position.x, utens.holdPos.transform.position.y + 0.05f, utens.holdPos.transform.position.z);


                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                        utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x, pickedUpItem.transform.eulerAngles.y, -90);

                        utens.holdingItem = true;

                        sausageCapsuleCollider.enabled = false;
                        chickenBoxCollider.enabled = false;
                    }




                }
                else if (hit.collider.gameObject.name == "Chicken Pile" && holdingUtensil && holdingTong)
                {


                    if (utens != null && !utens.holdingItem)
                    {
                        utens.pickedUpItem = Instantiate(instantiateChicken);
                        utens.pickedUpItem.transform.position = holdPos.transform.position;
                        utens.pickedUpItem.name = "Chicken";
                        utens.pickedUpItem.layer = 6;

                        utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;

                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                        utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                        utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x + 90, pickedUpItem.transform.eulerAngles.y, -90);

                        utens.holdingItem = true;

                        sausageCapsuleCollider.enabled = false;
                        chickenBoxCollider.enabled = false;
                    }




                }
                else if (hit.collider.gameObject.name == "Steak Pile" && holdingUtensil && holdingSpatula)
                {


                    if (utens != null && !utens.holdingItem)
                    {
                        utens.pickedUpItem = Instantiate(instantiateSteak);
                        utens.pickedUpItem.transform.position = holdPos.transform.position;
                        utens.pickedUpItem.name = "Steak";
                        utens.pickedUpItem.layer = 6;

                        //Debug.Log(utens.pickedUpItem.transform.localEulerAngles.z);
                        utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;

                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                        utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                        utens.pickedUpItem.transform.localEulerAngles = new Vector3(0, 90, 0);

                        utens.holdingItem = true;

                        steakBoxCollider.enabled = false;
                    }




                }



                else if (hit.collider.gameObject.name == "Tong")
                {
                        hit.collider.GetComponent<AudioManager>().Play("Pickup");

                        if (!holdingPlate && (pickedUpItem == null || holdingSpatula || pickedUpItem.name == "Clipboard"))
                        {
                            if (holdingUtensil && hit.collider.gameObject.name == "Tong")
                            {

                                pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                pickedUpItemAsset.layer = 0;
                                ////worldCanvas.layer = 0;

                                pickedUpItem.transform.parent = null;
                                pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                                if (pickedUpItem.name == "Tong")
                                {
                                    pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                }
                                else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                {
                                    pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                }


                                if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                {
                                    if (utens.holdingItem)
                                    {
                                        utens.pickedUpItem.layer = 0;
                                    }
                                }

                                pickedUpItem = null;

                                holdingUtensil = false;
                                holdingTong = false;
                                holdingSpatula = false;

                            }

                            pickedUpItem = hit.collider.gameObject;

                            pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, -90);

                            pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                            pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;

                            pickedUpItem.transform.position = holdPos.transform.position;

                            pickedUpItem.transform.parent = holdPos.transform;

                            holdingUtensil = true;
                            holdingTong = true;

                            pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                            pickedUpItemAsset.layer = 6;

                            utens = pickedUpItem.GetComponent<UtensilController>();

                            if (utens.pickedUpItem != null)
                            {
                                utens.pickedUpItem.layer = 6;
                            }

                            steakBoxCollider.enabled = false;
                            chickenBoxCollider.enabled = false;
                            sausageCapsuleCollider.enabled = false;
                        }

                    }
                else if (hit.collider.gameObject.name == "Spatula")
                {
                        hit.collider.GetComponent<AudioManager>().Play("Pickup");

                        if (!holdingPlate && (pickedUpItem == null || holdingTong || pickedUpItem.name == "Clipboard"))
                        {
                            if (holdingUtensil && hit.collider.gameObject.name == "Spatula")
                            {

                                pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                pickedUpItemAsset.layer = 0;
                                ////worldCanvas.layer = 0;


                                pickedUpItem.transform.parent = null;
                                pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                                if (pickedUpItem.name == "Tong")
                                {
                                    pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                }
                                else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                {
                                    pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                }

                                if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                {
                                    if (utens.holdingItem)
                                    {
                                        utens.pickedUpItem.layer = 0;
                                    }
                                }

                                pickedUpItem = null;


                                holdingUtensil = false;

                                holdingTong = false;
                                holdingSpatula = false;
                            }


                            pickedUpItem = hit.collider.gameObject;

                            pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                            pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                            pickedUpItem.GetComponent<BoxCollider>().enabled = false;

                            pickedUpItem.transform.position = holdPos.transform.position;

                            pickedUpItem.transform.parent = holdPos.transform;

                            holdingUtensil = true;
                            holdingSpatula = true;

                            pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                            pickedUpItemAsset.layer = 6;

                            utens = pickedUpItem.GetComponent<UtensilController>();

                            if (utens.pickedUpItem != null)
                            {
                                utens.pickedUpItem.layer = 6;
                            }

                            steakBoxCollider.enabled = false;
                            chickenBoxCollider.enabled = false;
                            sausageCapsuleCollider.enabled = false;
                        }

                    }

                    else if (hit.collider.gameObject.name == "Clipboard" && !holdingPlate)
                    {

                        if (holdingUtensil && hit.collider.gameObject.name == "Clipboard")
                        {

                            pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                            pickedUpItemAsset.layer = 0;
                            //worldCanvas.layer = 0;

                            pickedUpItem.transform.parent = null;
                            pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                            if (pickedUpItem.name == "Tong")
                            {
                                pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                            }
                            else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                            {
                                pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                            }

                            pickedUpItem = null;

                            holdingUtensil = false;

                            holdingTong = false;
                            holdingSpatula = false;
                        }

                        pickedUpItem = hit.collider.gameObject;

                        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                        pickedUpItem.GetComponent<BoxCollider>().enabled = false;

                        pickedUpItem.transform.parent = holdPos.transform;

                        pickedUpItem.transform.localPosition = new Vector3(holdPos.transform.localPosition.x - 0.67f, holdPos.transform.localPosition.y + 0.7430068f, holdPos.transform.localPosition.z - 0.7f);

                        pickedUpItem.transform.localEulerAngles = new Vector3(-3, 102, -58);

                        holdingUtensil = true;
                        //holdingSpatula = true;

                        pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                        pickedUpItemAsset.layer = 6;
                        //worldCanvas.layer = 6;

                        //utens = pickedUpItem.GetComponent<UtensilController>();

                        /*

                        if (utens.pickedUpItem != null)
                        {
                            utens.pickedUpItem.layer = 6;
                        }

                        */

                    }
                    else if (hit.collider.gameObject.name == "Placeable Location")
                    {
                        if (hit.collider.GetComponent<PlaceableController>().itemPlaced != null)
                        {

                            int count = 0;

                            if (holdingPlate)
                            {

                                PlateController tempPickedUpItem = pickedUpItem.GetComponent<PlateController>();

                                while (tempPickedUpItem.plateStackedOntop)
                                {
                                    if (tempPickedUpItem.plateStackedOntop)
                                    {
                                        if (!tempPickedUpItem.stackedPlateObject.GetComponent<PlateController>().plateStackedOntop)
                                        {
                                            //tempPickedUpItem.plateStackedOntop = false;
                                        }
                                        tempPickedUpItem = tempPickedUpItem.stackedPlateObject.GetComponent<PlateController>();

                                        count++;                                        
                                    }
                                }
                                
                                //Debug.Log(count);
                            }
                            


                            if (holdingUtensil && hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Plate" && !holdingPlate && !utens.holdingItem)
                            {

                                hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<AudioManager>().Play("Pickup");

                                if (pickedUpItem.name == "Spatula" && !utens.holdingItem)
                                {

                                    GameObject tempPos = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = pickedUpItem;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.GetComponent<PlaceableController>().placePos.transform;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.GetComponent<PlaceableController>().placePos.transform.position;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(0, -45, 0);

                                    if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong")
                                    {
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Clipboard")
                                    {
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                    pickedUpItem = null;
                                    pickedUpItemAsset = null;


                                    //pickedUpItemAsset.layer = 0;
                                    ////worldCanvas.layer = 0;


                                    //pickedUpItem.transform.parent = null;
                                    //pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;



                                    pickedUpItem = null;


                                    holdingUtensil = false;

                                    holdingTong = false;
                                    holdingSpatula = false;

                                    pickedUpItem = tempPos.gameObject;
                                    pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                    {
                                        if (utens.holdingItem)
                                        {
                                            utens.pickedUpItem.layer = 0;
                                        }
                                    }
                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    pickedUpItemAsset.layer = 6;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 6;

                                        if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = false;
                                        }
                                        else
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = false;
                                        }
                                    }

                                    steakBoxCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;

                                    holdingPlate = true;

                                    actionDone = true;
                                }
                                else if (pickedUpItem.name == "Tong" && !utens.holdingItem)
                                {

                                    GameObject tempPos = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = pickedUpItem;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.GetComponent<PlaceableController>().placePos.transform;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.GetComponent<PlaceableController>().placePos.transform.position;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(0, -45, 0);

                                    if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong")
                                    {
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Clipboard")
                                    {
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                    pickedUpItem = null;
                                    pickedUpItemAsset = null;


                                    //pickedUpItemAsset.layer = 0;
                                    ////worldCanvas.layer = 0;


                                    //pickedUpItem.transform.parent = null;
                                    //pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;



                                    pickedUpItem = null;


                                    holdingUtensil = false;

                                    holdingTong = false;
                                    holdingSpatula = false;

                                    pickedUpItem = tempPos.gameObject;
                                    pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                    {
                                        if (utens.holdingItem)
                                        {
                                            utens.pickedUpItem.layer = 0;
                                        }
                                    }
                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    pickedUpItemAsset.layer = 6;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 6;

                                        if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = false;
                                        }
                                        else
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = false;
                                        }
                                    }

                                    steakBoxCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;

                                    holdingPlate = true;

                                    actionDone = true;
                                }
                                else
                                {

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    pickedUpItemAsset.layer = 0;
                                    //worldCanvas.layer = 0;

                                    pickedUpItem.transform.parent = null;
                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard" || pickedUpItem.name == "Plate")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                    {
                                        if (utens.holdingItem)
                                        {
                                            utens.pickedUpItem.layer = 0;
                                        }
                                    }

                                    pickedUpItem = null;

                                    holdingUtensil = false;

                                    holdingTong = false;
                                    holdingSpatula = false;

                                    pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced;

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                                    pickedUpItem.GetComponent<BoxCollider>().enabled = false;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                    //holdingUtensil = true;
                                    //holdingSpatula = true;
                                    holdingPlate = true;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    pickedUpItemAsset.layer = 6;
                                    //worldCanvas.layer = 6;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 6;

                                        if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = false;
                                        }
                                        else
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = false;
                                        }
                                    }

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                    hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;


                                    actionDone = true;
                                }
                            }
                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Plate" && holdingPlate && !pickedUpItem.GetComponent<PlateController>().foodPlacedOntop && !hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedOntop)
                            {
                                PlateController plateController = pickedUpItem.GetComponent<PlateController>();
                                
                                bool plateCheckDone = false;

                                PlateController testGameobject = plateController.GetComponent<PlateController>();

                                pickedUpItem.GetComponent<AudioManager>().Play("Stack");


                                while (!plateCheckDone)
                                {

                                    if (testGameobject.plateStackedOntop)
                                    {
                                        testGameobject = testGameobject.stackedPlateObject.GetComponent<PlateController>();
                                    }
                                    else
                                    {
                                        plateCheckDone = true;

                                        testGameobject.stackedPlateObject = hit.collider.GetComponent<PlaceableController>().itemPlaced;
                                        testGameobject.plateStackedOntop = true;

                                        testGameobject.stackedPlateObject.GetComponent<Rigidbody>().isKinematic = true;
                                        testGameobject.stackedPlateObject.GetComponent<BoxCollider>().enabled = false;

                                        testGameobject.stackedPlateObject.transform.parent = testGameobject.spawnPos.transform;

                                        testGameobject.stackedPlateObject.transform.position = testGameobject.spawnPos.transform.position;

                                        testGameobject.stackedPlateObject.transform.eulerAngles = new Vector3(testGameobject.spawnPos.transform.eulerAngles.x, testGameobject.spawnPos.transform.eulerAngles.y, 0);

                                        testGameobject.stackedPlateObject.transform.Find("Asset").gameObject.layer = 6;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;
                                    }
                                }

                                plateCheckDone = false;
                                actionDone = true;
                            }
                            else if (!holdingUtensil && hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Plate" && !holdingPlate)
                            {

                                ////Debug.Log("Hello");

                                pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced;
                                pickedUpItem.GetComponent<AudioManager>().Play("Pickup");

                                //Debug.Log(pickedUpItem);

                                pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                                pickedUpItem.GetComponent<BoxCollider>().enabled = false;


                                while (pickedUpItem.GetComponent<PlateController>().plateStackedOntop)
                                {
                                    if (pickedUpItem.GetComponent<PlateController>().plateStackedOntop)
                                    {
                                        if (!pickedUpItem.GetComponent<PlateController>().stackedPlateObject.GetComponent<PlateController>().plateStackedOntop)
                                        {
                                            pickedUpItem.GetComponent<PlateController>().plateStackedOntop = false;
                                        }
                                        pickedUpItem = pickedUpItem.GetComponent<PlateController>().stackedPlateObject;
                                    }
                                }

                                if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                {
                                    pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 6;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = false;
                                    }
                                    else
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = true;
                                    }
                                }

                                pickedUpItem.transform.parent = holdPos.transform;

                                pickedUpItem.transform.position = holdPos.transform.position;

                                pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                //holdingUtensil = true;
                                //holdingSpatula = true;
                                holdingPlate = true;

                                pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                pickedUpItemAsset.layer = 6;
                                //worldCanvas.layer = 6;

                                hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                actionDone = true;

                                //utens = pickedUpItem.GetComponent<UtensilController>();

                                /*

                                if (utens.pickedUpItem != null)
                                {
                                    utens.pickedUpItem.layer = 6;
                                }

                                */
                            }
                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula" && !holdingPlate)
                            {

                                //Debug.Log("Hello");

                                if (!holdingPlate && (pickedUpItem == null || holdingTong || pickedUpItem.name == "Clipboard"))
                                {
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<AudioManager>().Play("Pickup");

                                    if (holdingUtensil && hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula")
                                    {
                                        
                                        GameObject tempPos = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = pickedUpItem;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.GetComponent<PlaceableController>().placePos.transform;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.GetComponent<PlaceableController>().placePos.transform.position;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(0, -45, 0);

                                        if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong")
                                        {
                                            hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Clipboard")
                                        {
                                            hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                        pickedUpItem = null;
                                        pickedUpItemAsset = null;


                                        //pickedUpItemAsset.layer = 0;
                                        ////worldCanvas.layer = 0;


                                        //pickedUpItem.transform.parent = null;
                                        //pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;



                                        pickedUpItem = null;


                                        holdingUtensil = false;

                                        holdingTong = false;
                                        holdingSpatula = false;

                                        pickedUpItem = tempPos.gameObject;
                                        pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                        if (pickedUpItem.name == "Tong")
                                        {
                                            pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                        {
                                            pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                        {
                                            if (utens.holdingItem)
                                            {
                                                utens.pickedUpItem.layer = 0;
                                            }
                                        }
                                        pickedUpItem.transform.position = holdPos.transform.position;

                                        pickedUpItem.transform.parent = holdPos.transform;

                                        holdingUtensil = true;
                                        holdingSpatula = true;

                                        pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                        pickedUpItemAsset.layer = 6;

                                        utens = pickedUpItem.GetComponent<UtensilController>();

                                        if (utens.pickedUpItem != null)
                                        {
                                            utens.pickedUpItem.layer = 6;
                                        }

                                        steakBoxCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;

                                        holdingUtensil = true;
                                    }
                                    else if (!holdingUtensil)
                                    {

                                        pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced;

                                        pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = false;

                                        pickedUpItem.transform.position = holdPos.transform.position;

                                        pickedUpItem.transform.parent = holdPos.transform;

                                        holdingUtensil = true;
                                        holdingSpatula = true;

                                        pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                        pickedUpItemAsset.layer = 6;

                                        utens = pickedUpItem.GetComponent<UtensilController>();

                                        if (utens.pickedUpItem != null)
                                        {
                                            utens.pickedUpItem.layer = 6;
                                        }

                                        steakBoxCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;

                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                        actionDone = true;
                                    }

                                }

                            }
                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong" && !holdingPlate)
                            {
                                hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<AudioManager>().Play("Pickup");
                                ////Debug.Log("Hello");

                                if (!holdingPlate && (pickedUpItem == null || holdingSpatula || pickedUpItem.name == "Clipboard"))
                                {
                                    if (holdingUtensil && hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong")
                                    {
                                        
                                        GameObject tempPos = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = pickedUpItem;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.GetComponent<PlaceableController>().placePos.transform;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.GetComponent<PlaceableController>().placePos.transform.position;

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(0, -45, 0);

                                        if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong")
                                        {
                                            hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Clipboard")
                                        {
                                            hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                        pickedUpItem = null;
                                        pickedUpItemAsset = null;


                                        //pickedUpItemAsset.layer = 0;
                                        ////worldCanvas.layer = 0;


                                        //pickedUpItem.transform.parent = null;
                                        //pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;



                                        pickedUpItem = null;


                                        holdingUtensil = false;

                                        holdingTong = false;
                                        holdingSpatula = false;

                                        pickedUpItem = tempPos.gameObject;
                                        pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, -90);

                                        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                        if (pickedUpItem.name == "Tong")
                                        {
                                            pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                        {
                                            pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                        {
                                            if (utens.holdingItem)
                                            {
                                                utens.pickedUpItem.layer = 0;
                                            }
                                        }
                                        pickedUpItem.transform.position = holdPos.transform.position;

                                        pickedUpItem.transform.parent = holdPos.transform;

                                        holdingUtensil = true;
                                        holdingTong = true;

                                        pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                        pickedUpItemAsset.layer = 6;

                                        utens = pickedUpItem.GetComponent<UtensilController>();

                                        if (utens.pickedUpItem != null)
                                        {
                                            utens.pickedUpItem.layer = 6;
                                        }

                                        steakBoxCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;

                                        holdingUtensil = true;
                                    }
                                    else if (!holdingUtensil)
                                    {

                                        pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced;

                                        pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, -90);

                                        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;

                                        pickedUpItem.transform.position = holdPos.transform.position;

                                        pickedUpItem.transform.parent = holdPos.transform;

                                        holdingUtensil = true;
                                        holdingTong = true;

                                        pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                        pickedUpItemAsset.layer = 6;

                                        utens = pickedUpItem.GetComponent<UtensilController>();

                                        if (utens.pickedUpItem != null)
                                        {
                                            utens.pickedUpItem.layer = 6;
                                        }

                                        steakBoxCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;

                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                        actionDone = true;
                                    }
                                }

                            }


                            else if (holdingUtensil && utens.holdingItem && hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Plate" && !hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedOntop)
                            {

                                PlateController foodOnPlate = hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>();

                                foodOnPlate.foodPlacedObject = utens.pickedUpItem;
                                foodOnPlate.foodPlacedOntop = true;

                                utens.pickedUpItem = null;
                                utens.holdingItem = false;

                                foodOnPlate.foodPlacedObject.transform.parent = hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform;
                                if (foodOnPlate.foodPlacedObject.name == "Steak")
                                {
                                    foodOnPlate.foodPlacedObject.transform.position = new Vector3(hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position.x, hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position.y - 0.03f, hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position.z);
                                }
                                else
                                {
                                    foodOnPlate.foodPlacedObject.transform.position = hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position;
                                }

                                foodOnPlate.foodPlacedObject.transform.eulerAngles = new Vector3(90, 0, 90);

                                foodOnPlate.foodPlacedObject.layer = 0;

                                if (foodOnPlate.foodPlacedObject.name == "Sausage")
                                {
                                    foodOnPlate.foodPlacedObject.GetComponent<CapsuleCollider>().enabled = true;
                                }
                                else
                                {
                                    foodOnPlate.foodPlacedObject.GetComponent<BoxCollider>().enabled = true;
                                }

                                ////Debug.Log("Hello");
                            }
                            else if (holdingPlate && count == 0 && !pickedUpItem.GetComponent<PlateController>().foodPlacedOntop && (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong"))
                            {
                                if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Spatula")
                                {




                                    GameObject tempPos = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = pickedUpItem;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.GetComponent<PlaceableController>().placePos.transform;
                                    
                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.y - 0.047f, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.z);

                                    pickedUpItem.GetComponent<AudioManager>().Play("Place");

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.y, 0);

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                    

                                    //hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                    pickedUpItem = null;
                                    pickedUpItemAsset = null;


                                    //pickedUpItemAsset.layer = 0;
                                    ////worldCanvas.layer = 0;


                                    //pickedUpItem.transform.parent = null;
                                    //pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;



                                    pickedUpItem = null;


                                    holdingUtensil = false;

                                    holdingTong = false;
                                    holdingSpatula = false;

                                    pickedUpItem = tempPos.gameObject;
                                    pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = false;
                                    }


                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    utens = pickedUpItem.GetComponent<UtensilController>();

                                    pickedUpItemAsset.layer = 6;

                                    steakBoxCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;

                                    holdingSpatula = true;
                                    holdingUtensil = true;

                                    holdingPlate = false;
                                    actionDone = true;
                                }
                                else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Tong")
                                {

                                    GameObject tempPos = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = pickedUpItem;

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.GetComponent<PlaceableController>().placePos.transform;
                                    
                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.y - 0.047f, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.z);

                                    pickedUpItem.GetComponent<AudioManager>().Play("Place");

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.y, 0);

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;


                                    hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                    pickedUpItem = null;
                                    pickedUpItemAsset = null;


                                    //pickedUpItemAsset.layer = 0;
                                    ////worldCanvas.layer = 0;


                                    //pickedUpItem.transform.parent = null;
                                    //pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;



                                    pickedUpItem = null;


                                    holdingUtensil = false;

                                    holdingTong = false;
                                    holdingSpatula = false;

                                    pickedUpItem = tempPos.gameObject;
                                    pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, -90);

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = false;
                                    }

                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    utens = pickedUpItem.GetComponent<UtensilController>();


                                    pickedUpItemAsset.layer = 6;

                                    steakBoxCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;

                                    holdingTong = true;
                                    holdingUtensil = true;

                                    holdingPlate = false;

                                    actionDone = true;
                                }
                                else
                                {

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    pickedUpItemAsset.layer = 0;
                                    //worldCanvas.layer = 0;

                                    pickedUpItem.transform.parent = null;
                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard" || pickedUpItem.name == "Plate")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    if (pickedUpItem.name != "Clipboard" && pickedUpItem.name != "Plate")
                                    {
                                        if (utens.holdingItem)
                                        {
                                            utens.pickedUpItem.layer = 0;
                                        }
                                    }

                                    pickedUpItem = null;

                                    holdingUtensil = false;

                                    holdingTong = false;
                                    holdingSpatula = false;

                                    pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced;

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                                    pickedUpItem.GetComponent<BoxCollider>().enabled = false;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);

                                    //holdingUtensil = true;
                                    //holdingSpatula = true;
                                    holdingPlate = true;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    pickedUpItemAsset.layer = 6;
                                    //worldCanvas.layer = 6;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 6;

                                        if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = false;
                                        }
                                        else
                                        {
                                            pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = false;
                                        }
                                    }

                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                    hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;


                                    actionDone = true;
                                }
                            }

                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Plate" && holdingPlate && count < 1)
                            {
                                //Debug.Log("Hello");
                                
                                pickedUpItem.transform.parent = null;
                                pickedUpItem.layer = 0;

                                pickedUpItem.transform.Find("Asset").gameObject.layer = 0;

                                if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                {
                                    pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 0;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = true;
                                    }
                                }

                                holdingPlate = false;

                                GameObject itemTempPos;

                                itemTempPos = pickedUpItem.gameObject;


                                itemTempPos.GetComponent<BoxCollider>().enabled = false;



                                pickedUpItem.GetComponent<BoxCollider>().enabled = true;

                                pickedUpItem.transform.position = foodLetGoPos.transform.position;
                                pickedUpItem = null;

                                



                                pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;
                                hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                
                                pickedUpItem.layer = 6;
                                pickedUpItem.transform.Find("Asset").gameObject.layer = 6;


                                if (pickedUpItem.GetComponent<PlateController>().foodPlacedOntop)
                                {
                                    pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 6;

                                    if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = false;
                                    }
                                    else
                                    {
                                        pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<BoxCollider>().enabled = false;
                                    }
                                }

                                pickedUpItem.layer = 6;
                                pickedUpItem.transform.Find("Asset").gameObject.layer = 6;

                                pickedUpItem.transform.parent = holdPos.transform;

                                pickedUpItem.transform.position = holdPos.transform.position;

                                pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0); pickedUpItem.GetComponent<BoxCollider>().enabled = false;


                                pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                holdingPlate = true;






                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = itemTempPos.gameObject;

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = true;

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(0, 0, 0);

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.layer = 0;
                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;

                                actionDone = true;

                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;


                            }
                            else if (holdingPlate && !pickedUpItem.GetComponent<PlateController>().foodPlacedOntop && count == 0 && (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Sausage" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Chicken" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Steak"))
                            {
                                //Debug.Log("FOOD");

                                bool plateCheckDone = false;
                                int countWhile = 0;

                                PlateController testGameobject = pickedUpItem.GetComponent<PlateController>();

                                while (!plateCheckDone)
                                {

                                    if (testGameobject.plateStackedOntop)
                                    {
                                        countWhile++;

                                        if (!testGameobject.stackedPlateObject.GetComponent<PlateController>().plateStackedOntop)
                                        {
                                            testGameobject.plateStackedOntop = false;
                                        }

                                        testGameobject = testGameobject.stackedPlateObject.GetComponent<PlateController>();
                                    }
                                    else
                                    {

                                        testGameobject.transform.position = Vector3.zero;

                                        plateCheckDone = true;

                                        actionDone = true;

                                        
                                        GameObject foodObject = hit.collider.GetComponent<PlaceableController>().itemPlaced;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                        plateCheckDone = true;

                                        testGameobject.stackedPlateObject = null;
                                        testGameobject.plateStackedOntop = false;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = testGameobject.gameObject;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = true;
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().mass = 100f;
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.y - 0.047f, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.z);

                                        pickedUpItem.GetComponent<AudioManager>().Play("Place");

                                        //Debug.Log("P: " + hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.localPosition);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.y, 0);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject = foodObject.gameObject;
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.GetComponent<Rigidbody>().isKinematic = true;

                                        if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.name == "Steak")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.transform.position = new Vector3(hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position.x, hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position.y - 0.03f, hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position.z);
                                        }
                                        else
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.transform.position = hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPos.transform.position;
                                        }

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.transform.eulerAngles = new Vector3(90, 0, 90);


                                        testGameobject.GetComponent<PlateController>().foodPlacedOntop = true;

                                        if (countWhile == 0)
                                        {
                                            holdingPlate = false;
                                            
                                            pickedUpItem = null;
                                            pickedUpItemAsset = null;
                                        }

                                        
                                    }
                                }

                                plateCheckDone = false;

                                //Debug.Log(countWhile);
                            }



                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Sausage" && holdingUtensil && holdingTong)
                            {
                                if (utens.holdingItem && (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Chicken" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Sausage"))
                                {
                                    if (!hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<FoodItem>().onGrill)
                                    {
                                        utens.holdingItem = false;

                                        utens.pickedUpItem.layer = 0;

                                        utens.pickedUpItem.transform.parent = null;
                                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                                        if (utens.pickedUpItem.name == "Chicken")
                                        {
                                            utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                        }
                                        else
                                        {
                                            utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                        }

                                        GameObject utensItemTempPos;

                                        utensItemTempPos = utens.pickedUpItem.gameObject;

                                        utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                                        utens.pickedUpItem = null;

                                        sausageCapsuleCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;







                                        utens.pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                        utens.pickedUpItem.name = "Sausage";
                                        utens.pickedUpItem.layer = 6;


                                        utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x, pickedUpItem.transform.eulerAngles.y, -90);
                                        utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;


                                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                        utens.pickedUpItem.transform.position = new Vector3(utens.holdPos.transform.position.x, utens.holdPos.transform.position.y + 0.05f, utens.holdPos.transform.position.z);

                                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                                        utens.holdingItem = true;

                                        sausageCapsuleCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;






                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = utensItemTempPos.gameObject;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                        if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Sausage")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Chicken")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }
                                        else if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Steak")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;
                                        steakBoxCollider.enabled = false;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(90, 135, 90);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.layer = 0;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;


                                    }


                                }

                                if (!utens.holdingItem)
                                {
                                    utens.pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                    hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                    utens.pickedUpItem.name = "Sausage";
                                    utens.pickedUpItem.layer = 6;


                                    if (utens.pickedUpItem.GetComponent<FoodItem>().onGrill)
                                    {
                                        braaiPositionStatus[utens.pickedUpItem.GetComponent<FoodItem>().grillPos] = false;
                                        //hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");
                                    }




                                    utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x, pickedUpItem.transform.eulerAngles.y, -90);
                                    utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;


                                    utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    utens.pickedUpItem.transform.position = new Vector3(utens.holdPos.transform.position.x, utens.holdPos.transform.position.y + 0.05f, utens.holdPos.transform.position.z);

                                    utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                                    utens.holdingItem = true;

                                    sausageCapsuleCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;

                                    utens.pickedUpItem.GetComponent<FoodItem>().burnLoaded = false;
                                    utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = false;
                                    utens.pickedUpItem.GetComponent<FoodItem>().cookingRightSide = false;
                                    Destroy(utens.pickedUpItem.GetComponent<FoodItem>().burnParticleInst);

                                    actionDone = true;
                                }




                            }
                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Chicken" && holdingUtensil && holdingTong)
                            {
                                if (utens.holdingItem && (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Chicken" || hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Sausage"))
                                {
                                    if (!hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<FoodItem>().onGrill)
                                    {
                                        utens.holdingItem = false;

                                        utens.pickedUpItem.layer = 0;

                                        utens.pickedUpItem.transform.parent = null;
                                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                                        if (utens.pickedUpItem.name == "Chicken")
                                        {
                                            utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                        }
                                        else
                                        {
                                            utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                        }

                                        GameObject utensItemTempPos;

                                        utensItemTempPos = utens.pickedUpItem.gameObject;

                                        utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                                        utens.pickedUpItem = null;

                                        sausageCapsuleCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;







                                        utens.pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                        utens.pickedUpItem.name = "Chicken";
                                        utens.pickedUpItem.layer = 6;


                                        utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x + 90, pickedUpItem.transform.eulerAngles.y, -90);
                                        utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;


                                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                        utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                                        utens.holdingItem = true;

                                        sausageCapsuleCollider.enabled = false;
                                        chickenBoxCollider.enabled = false;






                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = utensItemTempPos.gameObject;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                        if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Sausage")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Chicken")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }
                                        else if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Steak")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;
                                        steakBoxCollider.enabled = false;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(90, 135, 90);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.layer = 0;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;


                                    }


                                }

                                if (!utens.holdingItem)
                                {
                                    utens.pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                    hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                    utens.pickedUpItem.name = "Chicken";
                                    utens.pickedUpItem.layer = 6;

                                    if (utens.pickedUpItem.GetComponent<FoodItem>().onGrill)
                                    {
                                        braaiPositionStatus[utens.pickedUpItem.GetComponent<FoodItem>().grillPos] = false;
                                        //hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");
                                    }

                                    utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x + 90, pickedUpItem.transform.eulerAngles.y, -90);
                                    utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;


                                    utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                                    utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                                    utens.holdingItem = true;

                                    utens.pickedUpItem.GetComponent<FoodItem>().burnLoaded = false;
                                    utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = false;
                                    utens.pickedUpItem.GetComponent<FoodItem>().cookingRightSide = false;
                                    Destroy(utens.pickedUpItem.GetComponent<FoodItem>().burnParticleInst);

                                    sausageCapsuleCollider.enabled = false;
                                    chickenBoxCollider.enabled = true;

                                    actionDone = true;
                                }


                            }
                            else if (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Steak" && holdingUtensil && holdingSpatula)
                            {
                                if (utens.holdingItem && (hit.collider.GetComponent<PlaceableController>().itemPlaced.name == "Steak"))
                                {
                                    if (!hit.collider.GetComponent<PlaceableController>().itemPlaced.GetComponent<FoodItem>().onGrill)
                                    {
                                        utens.holdingItem = false;

                                        utens.pickedUpItem.layer = 0;

                                        utens.pickedUpItem.transform.parent = null;
                                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                                        utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;

                                        GameObject utensItemTempPos;

                                        utensItemTempPos = utens.pickedUpItem.gameObject;

                                        utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                                        utens.pickedUpItem = null;

                                        steakBoxCollider.enabled = false;







                                        utens.pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced.gameObject;
                                        hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                        hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                        utens.pickedUpItem.name = "Steak";
                                        utens.pickedUpItem.layer = 6;


                                        utens.pickedUpItem.transform.localEulerAngles = new Vector3(0, 90, 0);
                                        utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;


                                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                        utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                                        utens.holdingItem = true;

                                        steakBoxCollider.enabled = false;






                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = utensItemTempPos.gameObject;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                        if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Sausage")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                        else if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Chicken")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }
                                        else if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name == "Steak")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        }

                                        chickenBoxCollider.enabled = false;
                                        sausageCapsuleCollider.enabled = false;
                                        steakBoxCollider.enabled = false;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(90, 135, 90);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.layer = 0;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;


                                    }


                                }


                                if (!utens.holdingItem)
                                {
                                    utens.pickedUpItem = hit.collider.GetComponent<PlaceableController>().itemPlaced;
                                    hit.collider.GetComponent<PlaceableController>().itemPlaced = null;
                                    hit.collider.GetComponent<PlaceableController>().itemPlacedBool = false;

                                    utens.pickedUpItem.name = "Steak";
                                    utens.pickedUpItem.layer = 6;




                                    if (utens.pickedUpItem.GetComponent<FoodItem>().onGrill)
                                    {
                                        braaiPositionStatus[utens.pickedUpItem.GetComponent<FoodItem>().grillPos] = false;
                                        //hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");
                                    }

                                    utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;



                                    utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                                    utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                                    utens.pickedUpItem.transform.localEulerAngles = new Vector3(0, 90, 0);

                                    utens.holdingItem = true;

                                    utens.pickedUpItem.GetComponent<FoodItem>().burnLoaded = false;
                                    utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = false;
                                    utens.pickedUpItem.GetComponent<FoodItem>().cookingRightSide = false;
                                    Destroy(utens.pickedUpItem.GetComponent<FoodItem>().burnParticleInst);

                                    steakBoxCollider.enabled = false;

                                    actionDone = true;
                                }


                            }



                        }



                    }




                    else if (hit.collider.gameObject.name == "Sausage" && holdingUtensil && holdingTong)
                {
                    if (utens.holdingItem && (hit.collider.gameObject.name == "Chicken" || hit.collider.gameObject.name == "Sausage"))
                    {
                        if (!hit.collider.gameObject.GetComponent<FoodItem>().onGrill)
                        {
                            utens.holdingItem = false;

                            utens.pickedUpItem.layer = 0;

                            utens.pickedUpItem.transform.parent = null;
                            utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                                if (utens.pickedUpItem.name == "Chicken")
                                {
                                    utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                }
                                else
                                {
                                    utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                }
                                utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                            utens.pickedUpItem = null;

                            sausageCapsuleCollider.enabled = false;
                            chickenBoxCollider.enabled = false;
                        }


                    }

                    if (!utens.holdingItem)
                    {
                        utens.pickedUpItem = hit.collider.gameObject;
                        utens.pickedUpItem.name = "Sausage";
                        utens.pickedUpItem.layer = 6;


                            if (utens.pickedUpItem.GetComponent<FoodItem>().onGrill)
                            {
                                braaiPositionStatus[utens.pickedUpItem.GetComponent<FoodItem>().grillPos] = false;
                            }

                            hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");





                            utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x, pickedUpItem.transform.eulerAngles.y, -90);
                        utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;


                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                            utens.pickedUpItem.transform.position = new Vector3(utens.holdPos.transform.position.x, utens.holdPos.transform.position.y + 0.05f, utens.holdPos.transform.position.z);

                            utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                        utens.holdingItem = true;

                        sausageCapsuleCollider.enabled = true;
                        chickenBoxCollider.enabled = false;

                        utens.pickedUpItem.GetComponent<FoodItem>().burnLoaded = false;
                        utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = false;
                        utens.pickedUpItem.GetComponent<FoodItem>().cookingRightSide = false;
                        Destroy(utens.pickedUpItem.GetComponent<FoodItem>().burnParticleInst);
                    }




                }
                else if (hit.collider.gameObject.name == "Chicken" && holdingUtensil && holdingTong)
                {
                    if (utens.holdingItem && (hit.collider.gameObject.name == "Chicken" || hit.collider.gameObject.name == "Sausage"))
                    {

                        if (!hit.collider.gameObject.GetComponent<FoodItem>().onGrill)
                        {
                            utens.holdingItem = false;

                            utens.pickedUpItem.layer = 0;

                            utens.pickedUpItem.transform.parent = null;
                            utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                                if (utens.pickedUpItem.name == "Chicken")
                                {
                                    utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                }
                                else
                                {
                                    utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                }

                                utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                            utens.pickedUpItem = null;

                            sausageCapsuleCollider.enabled = false;
                            chickenBoxCollider.enabled = false;
                        }


                    }

                    if (!utens.holdingItem)
                    {
                        utens.pickedUpItem = hit.collider.gameObject;
                        utens.pickedUpItem.name = "Chicken";
                        utens.pickedUpItem.layer = 6;

                        if (utens.pickedUpItem.GetComponent<FoodItem>().onGrill)
                        {
                            braaiPositionStatus[utens.pickedUpItem.GetComponent<FoodItem>().grillPos] = false;
                            }
                            hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");

                            utens.pickedUpItem.transform.eulerAngles = new Vector3(pickedUpItem.transform.eulerAngles.x + 90, pickedUpItem.transform.eulerAngles.y, -90);
                        utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;


                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                        utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                        utens.holdingItem = true;

                        utens.pickedUpItem.GetComponent<FoodItem>().burnLoaded = false;
                        utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = false;
                        utens.pickedUpItem.GetComponent<FoodItem>().cookingRightSide = false;
                        Destroy(utens.pickedUpItem.GetComponent<FoodItem>().burnParticleInst);

                        sausageCapsuleCollider.enabled = false;
                        chickenBoxCollider.enabled = true;
                    }


                }
                else if (hit.collider.gameObject.name == "Steak" && holdingUtensil && holdingSpatula)
                {
                    if (utens.holdingItem && (hit.collider.gameObject.name == "Steak"))
                    {

                        if (!hit.collider.gameObject.GetComponent<FoodItem>().onGrill)
                        {
                            utens.holdingItem = false;

                            utens.pickedUpItem.layer = 0;

                            utens.pickedUpItem.transform.parent = null;
                            utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                            utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                            utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                            utens.pickedUpItem = null;

                            steakBoxCollider.enabled = false;
                        }


                    }

                    if (!utens.holdingItem)
                    {
                        utens.pickedUpItem = hit.collider.gameObject;
                        utens.pickedUpItem.name = "Steak";
                        utens.pickedUpItem.layer = 6;

                        


                            if (utens.pickedUpItem.GetComponent<FoodItem>().onGrill)
                            {
                                braaiPositionStatus[utens.pickedUpItem.GetComponent<FoodItem>().grillPos] = false;
                            }
                            hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");


                            utens.pickedUpItem.GetComponent<BoxCollider>().enabled = false;



                        utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                        utens.pickedUpItem.transform.position = utens.holdPos.transform.position;

                        utens.pickedUpItem.transform.parent = utens.holdPos.transform;

                        utens.pickedUpItem.transform.localEulerAngles = new Vector3(0, 90, 0);

                        utens.holdingItem = true;

                        utens.pickedUpItem.GetComponent<FoodItem>().burnLoaded = false;
                        utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = false;
                        utens.pickedUpItem.GetComponent<FoodItem>().cookingRightSide = false;
                        Destroy(utens.pickedUpItem.GetComponent<FoodItem>().burnParticleInst);

                        steakBoxCollider.enabled = false;
                    }


                }







            }
            else if (hit.collider.gameObject.name != "Interactable" && hit.collider.gameObject.tag != "Braai" && hit.collider.gameObject.tag != "CompleteFood")
            {
                    /*
                if (utens != null)
                    {
                        if (utens.holdingItem && holdingUtensil)
                        {
                        utens.holdingItem = false;

                        utens.pickedUpItem.transform.position = utens.pickedUpItem.transform.position;

                        utens.pickedUpItem.layer = 0;

                        if (utens.pickedUpItem.name == "Sausage" || utens.pickedUpItem.name == "Chicken")
                        {
                            utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                        }
                        else if (utens.pickedUpItem.name == "Steak")
                        {
                            utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                        }


                            

                        utens.pickedUpItem.transform.parent = null;
                            utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                        utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;

                        utens.pickedUpItem = null;


                        if (holdingTong)
                        {
                            sausageCapsuleCollider.enabled = false;
                            chickenBoxCollider.enabled = false;
                        }

                        if (holdingSpatula)
                        {
                            steakBoxCollider.enabled = false;
                        }

                    }
                }
                */
                    
            }
            }
            else if (Physics.Raycast(ray, out hit))
            {
                /*
                if (hit.collider.gameObject.tag != "Braai" && hit.collider.gameObject.tag != "CompleteFood")
                {
                    if (utens != null)
                    {
                        if (utens.holdingItem && holdingUtensil)
                        {
                            

                            utens.pickedUpItem.layer = 0;

                            utens.holdingItem = false;

                            utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                            if (utens.pickedUpItem.name == "Sausage")
                            {
                                utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                            }
                            else if (utens.pickedUpItem.name == "Steak" || utens.pickedUpItem.name == "Chicken")
                            {
                                utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                            }

                            utens.pickedUpItem.transform.parent = null;
                            utens.pickedUpItem.transform.position = foodLetGoPos.transform.position;
                            utens.pickedUpItem = null;

                            if (holdingTong)
                            {
                                sausageCapsuleCollider.enabled = false;
                                chickenBoxCollider.enabled = false;
                            }

                            if (holdingSpatula)
                            {
                                steakBoxCollider.enabled = false;
                            }

                        }
                    }
                }
                */


            }

        }
    }

    void DropItem()
    {
        if (holdingUtensil)
        {
            /*
            if (Input.GetMouseButtonDown(1))
            {
                
                if (pickedUpItem.name != "Clipboard" && !pickedUpItem.GetComponent<UtensilController>().holdingItem)
                {
                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;
                    
                    pickedUpItemAsset.layer = 0;
                    //worldCanvas.layer = 0;

                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                    if (pickedUpItem.name == "Tong")
                    {
                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;                        
                    }
                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard" || pickedUpItem.name == "Plate")
                    {
                        pickedUpItem.GetComponent<BoxCollider>().enabled = true;                        
                    }

                    if (pickedUpItem.name != "Clipboard")
                    {
                        if (utens.holdingItem)
                        {
                            utens.pickedUpItem.layer = 0;
                        }
                        if (utens.holdingItem)
                        {
                            if (utens.pickedUpItem.name == "Sausage")
                            {
                                sausageCapsuleCollider.enabled = true;
                            }
                            else if (utens.pickedUpItem.name == "Chicken")
                            {
                                chickenBoxCollider.enabled = true;
                            }
                            else if (utens.pickedUpItem.name == "Steak")
                            {
                                steakBoxCollider.enabled = true;
                            }
                        }

                    }

                    pickedUpItem.transform.parent = null;
                    pickedUpItem = null;

                    holdingUtensil = false;

                    holdingTong = false;
                    holdingSpatula = false;                    
                }

                if (pickedUpItem != null && pickedUpItem.name == "Clipboard")
                {
                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                    pickedUpItemAsset.layer = 0;
                    //worldCanvas.layer = 0;

                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;

                    if (pickedUpItem.name == "Tong")
                    {
                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                    }
                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard" || pickedUpItem.name == "Plate")
                    {
                        pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                    }

                    if (pickedUpItem.name != "Clipboard")
                    {
                        if (utens.holdingItem)
                        {
                            utens.pickedUpItem.layer = 0;
                        }
                        if (utens.holdingItem)
                        {
                            if (utens.pickedUpItem.name == "Sausage")
                            {
                                sausageCapsuleCollider.enabled = true;
                            }
                            else if (utens.pickedUpItem.name == "Chicken")
                            {
                                chickenBoxCollider.enabled = true;
                            }
                            else if (utens.pickedUpItem.name == "Steak")
                            {
                                steakBoxCollider.enabled = true;
                            }
                        }

                    }

                    pickedUpItem.transform.parent = null;
                    pickedUpItem = null;

                    holdingUtensil = false;

                    holdingTong = false;
                    holdingSpatula = false;
                }


            }
            */

        }

        if (!actionDone)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, distanceToItem, ignore))
                {
                    if (hit.collider.tag == "Interactable" && hit.collider.name == "Placeable Location" && !hit.collider.GetComponent<PlaceableController>().guestOnly)
                    {
                        if (holdingPlate)
                        {
                            bool plateCheckDone = true;

                            int count = 0;

                            //Debug.Log(hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool);

                            if (!hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool)
                            {

                                //Debug.Log("HERE");

                                plateCheckDone = false;

                                PlateController testGameobject = pickedUpItem.GetComponent<PlateController>();

                                while (!plateCheckDone)
                                {

                                    if (testGameobject.plateStackedOntop)
                                    {
                                        count++;

                                        if (!testGameobject.stackedPlateObject.GetComponent<PlateController>().plateStackedOntop)
                                        {
                                            testGameobject.plateStackedOntop = false;
                                        }

                                        testGameobject = testGameobject.stackedPlateObject.GetComponent<PlateController>();
                                    }
                                    else
                                    {
                                        plateCheckDone = true;

                                        testGameobject.stackedPlateObject = null;
                                        testGameobject.plateStackedOntop = false;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = testGameobject.gameObject;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.y - 0.047f, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position.z);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.y, 0);

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;

                                        if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedOntop)
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.layer = 0;

                                            if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                            {
                                                hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = true;
                                            }
                                        }

                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;
                                        pickedUpItem.GetComponent<AudioManager>().Play("Place");
                                        if (count == 0)
                                        {
                                            holdingPlate = false;
                                            
                                            pickedUpItem = null;
                                            pickedUpItemAsset = null;
                                        }

                                    }

                                    actionDone = true;
                                }

                                //plateCheckDone = false;

                                //Debug.Log(count);
                            }
                            
                            if (plateCheckDone && hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool && count == 0 && !actionDone && (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name != "Sausage" && hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name != "Steak" && hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.name != "Chicken"))
                            {

                                PlateController testGameobject = pickedUpItem.GetComponent<PlateController>();

                                plateCheckDone = false;

                                while (!plateCheckDone)
                                {

                                    if (testGameobject.plateStackedOntop)
                                    {
                                        count++;

                                        if (!testGameobject.stackedPlateObject.GetComponent<PlateController>().plateStackedOntop)
                                        {
                                            //testGameobject.plateStackedOntop = false;
                                        }

                                        testGameobject = testGameobject.stackedPlateObject.GetComponent<PlateController>();
                                    }
                                    else
                                    {
                                        plateCheckDone = true;
                                    }
                                }

                                //Debug.Log("C: " + count);

                                if (count == 0)
                                {
                                    //Debug.Log("PLACED");

                                    GameObject tempObj = hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced;
                                    GameObject firstObj = tempObj;


                                    testGameobject.stackedPlateObject = null;
                                    testGameobject.plateStackedOntop = false;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = testGameobject.gameObject;

                                    pickedUpItem = null;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = true;
                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.x, hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.eulerAngles.y, 0);

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.Find("Asset").gameObject.layer = 0;

                                    if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedOntop)
                                    {
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.layer = 0;

                                        if (hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.name == "Sausage")
                                        {
                                            hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<PlateController>().foodPlacedObject.GetComponent<CapsuleCollider>().enabled = true;
                                        }
                                    }

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;

                                    if (count == 0)
                                    {
                                        holdingPlate = false;

                                        pickedUpItem = null;
                                        pickedUpItemAsset = null;
                                    }












                                    pickedUpItem = tempObj;

                                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        pickedUpItem.GetComponent<CapsuleCollider>().enabled = false;
                                        pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, -90);
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                    {
                                        pickedUpItem.GetComponent<BoxCollider>().enabled = false;
                                        pickedUpItem.transform.eulerAngles = new Vector3(holdPos.transform.eulerAngles.x, holdPos.transform.eulerAngles.y, 0);
                                    }


                                    pickedUpItem.transform.position = holdPos.transform.position;

                                    pickedUpItem.transform.parent = holdPos.transform;

                                    pickedUpItemAsset = pickedUpItem.transform.Find("Asset").gameObject;

                                    utens = pickedUpItem.GetComponent<UtensilController>();

                                    pickedUpItemAsset.layer = 6;

                                    steakBoxCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        holdingTong = true;
                                    }
                                    else if (pickedUpItem.name == "Spatula" || pickedUpItem.name == "Clipboard")
                                    {
                                        holdingSpatula = true;
                                    }

                                    //holdingSpatula = true;
                                    holdingUtensil = true;

                                    holdingPlate = false;


                                    actionDone = true;




                                    //Debug.Log(count);
                                }

                                plateCheckDone = false;
                            }




                        }

                        if (utens != null)
                        {
                            if (utens.holdingItem && holdingUtensil)
                            {
                                if (!hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool)
                                {
                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = utens.pickedUpItem.gameObject;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                    if (utens.pickedUpItem.name == "Sausage")
                                    {
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                    }
                                    else if (utens.pickedUpItem.name == "Chicken")
                                    {
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                    }
                                    else if (utens.pickedUpItem.name == "Steak")
                                    {
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                    }

                                    utens.pickedUpItem = null;
                                    utens.holdingItem = false;

                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;
                                    steakBoxCollider.enabled = false;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(90, 135, 90);

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.layer = 0;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;
                                }

                            }
                            else if (!utens.holdingItem && holdingUtensil)
                            {
                                if (!hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool)
                                {
                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced = pickedUpItem.gameObject;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<Rigidbody>().isKinematic = false;

                                    if (pickedUpItem.name == "Tong")
                                    {
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<CapsuleCollider>().enabled = true;
                                        holdingTong = false;
                                    }
                                    else if (pickedUpItem.name == "Spatula")
                                    {
                                        hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.GetComponent<BoxCollider>().enabled = true;
                                        holdingSpatula = false;
                                    }

                                    pickedUpItem = null;
                                    holdingUtensil = false;

                                    pickedUpItemAsset.layer = 0;

                                    pickedUpItemAsset = null;

                                    chickenBoxCollider.enabled = false;
                                    sausageCapsuleCollider.enabled = false;
                                    steakBoxCollider.enabled = false;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.parent = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.eulerAngles = new Vector3(0, -45, 0);

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.transform.position = hit.collider.gameObject.GetComponent<PlaceableController>().placePos.transform.position;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlaced.layer = 0;

                                    hit.collider.gameObject.GetComponent<PlaceableController>().itemPlacedBool = true;
                                }

                            }

                        }

                    }
                }

            }
        }
    }

    private void PlaceOnBraai()
    {

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, distanceToItem))
            {

                if (hit.collider.gameObject.tag == "Braai")
                {


                    if (holdingUtensil)
                    {

                        ////Debug.Log("BRAAI");

                        utens = pickedUpItem.GetComponent<UtensilController>();

                        if (utens != null)
                        {

                            ////Debug.Log("BRAAI?");

                            if (utens.holdingItem)
                            {

                                for(int i = 0; i < 4; i++)
                                {
                                    ////Debug.Log(braaiPositionStatus[i]);
                                    if (!braaiPositionStatus[i])
                                    {
                                        utens.pickedUpItem.transform.position = new Vector3(braaiPositions[i].transform.position.x, braaiPositions[i].transform.position.y + 0.07f, braaiPositions[i].transform.position.z) ;

                                        braaiPositionStatus[i] = true;
                                        utens.pickedUpItem.GetComponent<FoodItem>().onGrill = true;

                                        utens.pickedUpItem.GetComponent<FoodItem>().grillPos = i;

                                        if (utens.pickedUpItem.GetComponent<FoodItem>().grillPos == 0)
                                        {
                                            Marker[0].GetComponentInChildren<MarkerScript>().food = utens.pickedUpItem.GetComponent<FoodItem>();
                                        }
                                        else if (utens.pickedUpItem.GetComponent<FoodItem>().grillPos == 1)
                                        {
                                            Marker[1].GetComponentInChildren<MarkerScript>().food = utens.pickedUpItem.GetComponent<FoodItem>();
                                        }
                                        else if (utens.pickedUpItem.GetComponent<FoodItem>().grillPos == 2)
                                        {
                                            Marker[2].GetComponentInChildren<MarkerScript>().food = utens.pickedUpItem.GetComponent<FoodItem>();
                                        }
                                        else if (utens.pickedUpItem.GetComponent<FoodItem>().grillPos == 3)
                                        {
                                            Marker[3].GetComponentInChildren<MarkerScript>().food = utens.pickedUpItem.GetComponent<FoodItem>();
                                        }

                                        break;
                                    }
                                }

                                utens.pickedUpItem.GetComponent<FoodItem>().cookingLeftSide = true;

                                utens.pickedUpItem.transform.parent = null;
                                utens.pickedUpItem.layer = 0;
                                utens.pickedUpItem.transform.rotation = Quaternion.Euler(-270, 0, -180);

                                if (utens.pickedUpItem.name == "Sausage")
                                {
                                    utens.pickedUpItem.GetComponent<CapsuleCollider>().enabled = true;
                                }
                                else if (utens.pickedUpItem.name == "Steak" || utens.pickedUpItem.name == "Chicken")
                                {
                                    utens.pickedUpItem.GetComponent<BoxCollider>().enabled = true;
                                }

                                utens.pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;

                                utens.pickedUpItem = null;

                                utens.holdingItem = false;

                                hit.collider.gameObject.GetComponent<AudioSource>().Play();

                                ////Debug.Log("BRAAI PLACED" + utens.holdingItem);

                                if (holdingTong)
                                {
                                    sausageCapsuleCollider.enabled = false;
                                    chickenBoxCollider.enabled = false;
                                }

                                if (holdingSpatula)
                                {
                                    steakBoxCollider.enabled = false;
                                }

                            }
                           
                        }




                    }






                }
            }

        }
    }

    private void FlipFood()
    {
        if (Input.GetKeyDown(KeyCode.F) && holdingUtensil)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, distanceToItem))
            {
                if (hit.collider.gameObject.tag == "Interactable" && (hit.collider.gameObject.name == "Sausage" || hit.collider.gameObject.name == "Chicken") && holdingTong)
                {


                    GameObject itemGameobject = hit.collider.gameObject;

                    ////Debug.Log(itemGameobject);

                    FoodItem foodItem = itemGameobject.GetComponent<FoodItem>();
                    hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");

                    if (foodItem.cookingLeftSide)
                    {
                        itemGameobject.transform.rotation = Quaternion.Euler(-90, 90, -90);

                        foodItem.burnLoaded = false;
                        Destroy(foodItem.burnParticleInst);

                        foodItem.cookingLeftSide = false;
                        foodItem.cookingRightSide = true;                        

                    }
                    else if (foodItem.cookingRightSide)
                    {
                        itemGameobject.transform.rotation = Quaternion.Euler(-270, 0, -180);
                        itemGameobject.transform.position = new Vector3(itemGameobject.transform.position.x, itemGameobject.transform.position.y, itemGameobject.transform.position.z);

                        foodItem.burnLoaded = false;
                        Destroy(foodItem.burnParticleInst);

                        foodItem.cookingLeftSide = true;
                        foodItem.cookingRightSide = false;
                    }
                }


                if (hit.collider.gameObject.tag == "Interactable" && (hit.collider.gameObject.name == "Steak") && holdingSpatula)
                {


                    GameObject itemGameobject = hit.collider.gameObject;

                    ////Debug.Log(itemGameobject);

                    FoodItem foodItem = itemGameobject.GetComponent<FoodItem>();

                    hit.collider.gameObject.GetComponent<AudioManager>().StopPlaying("Burning");


                    if (foodItem.cookingLeftSide)
                    {
                        itemGameobject.transform.rotation = Quaternion.Euler(-90, 90, -90);

                        foodItem.burnLoaded = false;
                        Destroy(foodItem.burnParticleInst);

                        foodItem.cookingLeftSide = false;
                        foodItem.cookingRightSide = true;


                    }
                    else if (foodItem.cookingRightSide)
                    {
                        itemGameobject.transform.rotation = Quaternion.Euler(-270, 0, -180);
                        itemGameobject.transform.position = new Vector3(itemGameobject.transform.position.x, itemGameobject.transform.position.y, itemGameobject.transform.position.z);

                        foodItem.burnLoaded = false;
                        Destroy(foodItem.burnParticleInst);

                        foodItem.cookingLeftSide = true;
                        foodItem.cookingRightSide = false;
                    }
                }

            }
        }
    }

    private void ThrowAwayFood()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, distanceToItem))
            {
                if (hit.collider.gameObject.tag == "Trashcan")
                {

                    if (holdingUtensil)
                    {
                        utens = pickedUpItem.GetComponent<UtensilController>();

                        if (utens.pickedUpItem != null)
                        {
                            utens.holdingItem = false;
                            Destroy(utens.pickedUpItem);
                            hit.collider.gameObject.GetComponent<AudioSource>().Play();

                            if (holdingTong)
                            {
                                sausageCapsuleCollider.enabled = false;
                                chickenBoxCollider.enabled = false;
                            }

                            if (holdingSpatula)
                            {
                                steakBoxCollider.enabled = false;
                            }
                        }
                    }

                    if (holdingPlate)
                    {
                        utens = pickedUpItem.GetComponent<UtensilController>();

                        PlateController foodOnPlate = pickedUpItem.GetComponent<PlateController>();

                        if (foodOnPlate.foodPlacedOntop)
                        {
                            foodOnPlate.foodPlacedOntop = false;
                            Destroy(foodOnPlate.foodPlacedObject);
                            hit.collider.gameObject.GetComponent<AudioSource>().Play();
                        }
                    }

                }
            }
        }
    }

    private void CompletedFood()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, distanceToItem))
            {
                if (hit.collider.gameObject.tag == "Guest")
                {

                    /*
                    if (holdingUtensil)
                    {

                        if (utens.pickedUpItem != null)
                        {

                            utens = pickedUpItem.GetComponent<UtensilController>();
                            FoodItem food = utens.pickedUpItem.GetComponent<FoodItem>();

                            if (hit.collider.GetComponent<GuestController>().currentOrder != null)
                            {

                                //Debug.Log(utens.pickedUpItem.name);
                                //Debug.Log(hit.collider.GetComponent<GuestController>().currentOrder.name);

                                if (utens.pickedUpItem.name == hit.collider.GetComponent<GuestController>().currentOrder.name)
                                {

                                    SatisfactionChecker(food, hit);

                                    ////Debug.Log("Hello");

                                    hit.collider.GetComponent<GuestController>().maxOrderTimer = 0;

                                    if (orderManager.currentFoodOrders.Count == 0)
                                    {
                                        wasFoodOrdered = true;
                                        showWrongText = true;
                                        wrongTimer = 0;
                                        wrongText.SetActive(true);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < orderManager.currentFoodOrders.Count; i++)
                                        {

                                            if (hit.collider.GetComponent<GuestController>().currentOrder.guestOrderLocation == orderManager.currentFoodOrders[i].guestOrderLocation)
                                            {
                                                ////Debug.Log("YAY" + i);

                                                _orderManager.ShiftUI(i);



                                                if (holdingTong)
                                                {
                                                    sausageCapsuleCollider.enabled = false;
                                                    chickenBoxCollider.enabled = false;
                                                }

                                                if (holdingSpatula)
                                                {
                                                    steakBoxCollider.enabled = false;
                                                }

                                                utens.holdingItem = false;
                                                Destroy(utens.pickedUpItem);

                                                wasFoodOrdered = true;

                                                break;
                                            }
                                            else
                                            {

                                                wasFoodOrdered = false;

                                            }

                                            ////Debug.Log("TRIED");


                                            if (i == orderManager.currentFoodOrders.Count - 1 && wasFoodOrdered == false)
                                            {
                                                wasFoodOrdered = true;
                                                showWrongText = true;
                                                wrongTimer = 0;
                                                wrongText.SetActive(true);
                                            }

                                        }

                                    }
                                }

                            }
                        }
                    }
                    */

                    if (holdingPlate)
                    {

                        if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject != null)
                        {

                            //utens = pickedUpItem.GetComponent<UtensilController>();
                            FoodItem food = pickedUpItem.GetComponent<PlateController>().foodPlacedObject.GetComponent<FoodItem>();

                            if (hit.collider.GetComponent<GuestController>().currentOrder != null)
                            {

                                //Debug.Log(pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name);
                                //Debug.Log(hit.collider.GetComponent<GuestController>().currentOrder.name);

                                if (pickedUpItem.GetComponent<PlateController>().foodPlacedObject.name == hit.collider.GetComponent<GuestController>().currentOrder.name)
                                {

                                    if (food.rightSideCooked && food.leftSideCooked && !food.sausageBurnt && !_gameManager.orderDeliveredTut)
                                    {
                                        _gameManager.orderDeliveredTut = true;
                                        GameManager.tutorialDone = true;
                                    }
                                    
                                    if (GameManager.tutorialDone)
                                    {
                                        SatisfactionChecker(food, hit);
                                    

                                    ////Debug.Log("Hello");

                                    hit.collider.GetComponent<GuestController>().maxOrderTimer = 0;

                                    if (orderManager.currentFoodOrders.Count == 0)
                                    {
                                        wasFoodOrdered = true;
                                        showWrongText = true;
                                        wrongTimer = 0;
                                        wrongText.SetActive(true);
                                    }
                                    else
                                    {
                                            for (int i = 0; i < orderManager.currentFoodOrders.Count; i++)
                                            {

                                                if (hit.collider.GetComponent<GuestController>().currentOrder.guestOrderLocation == orderManager.currentFoodOrders[i].guestOrderLocation)
                                                {
                                                    ////Debug.Log("YAY" + i);

                                                    _orderManager.ShiftUI(i);



                                                    if (holdingTong)
                                                    {
                                                        sausageCapsuleCollider.enabled = false;
                                                        chickenBoxCollider.enabled = false;
                                                    }

                                                    if (holdingSpatula)
                                                    {
                                                        steakBoxCollider.enabled = false;
                                                    }


                                                    //Destroy(pickedUpItem.GetComponent<PlateController>().foodPlacedObject);


                                                    pickedUpItem.transform.parent = hit.collider.GetComponent<GuestController>().plateHoldPos.transform;

                                                    //ADD FOR GREY BLOCK
                                                    //pickedUpItem.transform.localScale = new Vector3(1.294331f, 0.6897979f, 1.903416f);

                                                    pickedUpItem.transform.localScale = new Vector3(1f, 1f, 1f);




                                                    pickedUpItem.transform.position = hit.collider.GetComponent<GuestController>().plateHoldPos.transform.position;
                                                    pickedUpItem.transform.localEulerAngles = new Vector3(0, 0, 0);
                                                    pickedUpItem.transform.Find("Asset").gameObject.layer = 0;

                                                    pickedUpItem.GetComponent<PlateController>().foodPlacedObject.layer = 0;

                                                    hit.collider.GetComponent<GuestController>().plateHoldObject = pickedUpItem;
                                                    hit.collider.GetComponent<GuestController>().guestIsHoldingPlate = true;

                                                    holdingPlate = false;



                                                    pickedUpItem = null;

                                                    wasFoodOrdered = true;

                                                    break;
                                                }
                                                else
                                                {

                                                    wasFoodOrdered = false;

                                                }

                                                ////Debug.Log("TRIED");


                                                if (i == orderManager.currentFoodOrders.Count - 1 && wasFoodOrdered == false)
                                                {
                                                    wasFoodOrdered = true;
                                                    showWrongText = true;
                                                    wrongTimer = 0;
                                                    wrongText.SetActive(true);
                                                    hit.collider.GetComponent<AudioManager>().Play("Nope");
                                                    //Debug.Log("PLAY NO AUDIO!");
                                                }
                                            }

                                        }

                                    }
                                    else
                                    {
                                        hit.collider.GetComponent<AudioManager>().Play("Nope");
                                        //Debug.Log("PLAY NO AUDIO!");
                                    }
                                }
                                else
                                {
                                    hit.collider.GetComponent<AudioManager>().Play("Nope");
                                    //Debug.Log("PLAY NO AUDIO!");
                                }

                            }
                        }
                    }
                    else if (holdingUtensil && pickedUpItem.GetComponent<UtensilController>().holdingItem)
                    {
                        hit.collider.GetComponent<AudioManager>().Play("Nope");
                        //Debug.Log("PLAY NO AUDIO!");
                    }

                }
            }
        }
    }

    private void SatisfactionChecker(FoodItem foodItem, RaycastHit hit)
    {

        //FoodItem foodItem = utens.pickedUpItem.GetComponent<FoodItem>();
        GuestController guestController = hit.collider.GetComponent<GuestController>();

        if (foodItem.rightSideCooked && foodItem.leftSideCooked && !foodItem.sausageBurnt)
        {
            if (guestController.maxOrderTimer > guestController.currentOrder.orderTime/2)
            {
                GuestManager.overallSatisfaction += maxSatisfactionPerOrder;
                hit.collider.gameObject.GetComponent<AudioManager>().Play("Up");
                guestController.SatisfactionUp();
            }
            else if (guestController.maxOrderTimer < guestController.currentOrder.orderTime / 2)
            {
                float valueNormalized = Mathf.InverseLerp(0, (guestController.currentOrder.orderTime / 2), guestController.maxOrderTimer);

                GuestManager.overallSatisfaction += (maxSatisfactionPerOrder / 2) + ((maxSatisfactionPerOrder / 2) * valueNormalized);
                hit.collider.gameObject.GetComponent<AudioManager>().Play("Up");
                guestController.SatisfactionUp();
            }

            //OrderManager.firstOrder = false;
            //GuestManager.firstGuest = false;
        }
        else if ((foodItem.rightSideCooked || foodItem.leftSideCooked) && !(foodItem.rightSideCooked && foodItem.leftSideCooked) && !foodItem.sausageBurnt)
        {
            if (guestController.maxOrderTimer > guestController.currentOrder.orderTime / 2)
            {
                GuestManager.overallSatisfaction += maxSatisfactionPerOrder/2;
                hit.collider.gameObject.GetComponent<AudioManager>().Play("Up");
                guestController.SatisfactionUp();
            }
            else if (guestController.maxOrderTimer < guestController.currentOrder.orderTime / 2)
            {
                float valueNormalized = Mathf.InverseLerp(0, (guestController.currentOrder.orderTime / 4), guestController.maxOrderTimer);

                GuestManager.overallSatisfaction += (maxSatisfactionPerOrder / 4) + ((maxSatisfactionPerOrder / 4) * valueNormalized);
                hit.collider.gameObject.GetComponent<AudioManager>().Play("Up");
                guestController.SatisfactionUp();
            }
        }
        else if (!(foodItem.rightSideCooked && foodItem.leftSideCooked) && !foodItem.sausageBurnt)
        {
            GuestManager.overallSatisfaction -= maxSatisfactionPerOrder * 0.1f;
            hit.collider.gameObject.GetComponent<AudioManager>().Play("Down");
            guestController.SatisfactionDown();

            //guestController.animator.SetTrigger("Angry");
        }
        else if (foodItem.sausageBurnt)
        {
            GuestManager.overallSatisfaction -= satsifactionDropBurntFood;
            hit.collider.gameObject.GetComponent<AudioManager>().Play("Down");
            guestController.SatisfactionDown();

            //guestController.animator.SetTrigger("Angry");
        }


        ////Debug.Log(guestController.maxOrderTimer);
    }

}
