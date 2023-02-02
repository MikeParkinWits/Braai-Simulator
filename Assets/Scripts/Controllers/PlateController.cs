using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{

    public bool plateStackedOntop = false;
    public GameObject spawnPos;
    public GameObject foodPos;

    public GameObject stackedPlateObject;
    public GameObject foodPlacedObject;

    public bool foodPlacedOntop = false;

    public bool plateDoneTut = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GameObject layerOneParent = this.transform.parent.gameObject;

        if (!plateDoneTut && layerOneParent.name == "PlacePoint")
        {
            PlaceableController layerTwoParent = layerOneParent.transform.parent.gameObject.GetComponent<PlaceableController>();

            if (!layerTwoParent.guestOnly)
            {
                plateDoneTut = true;
                //Debug.Log("YEAH");
            }
        }
    }
}
