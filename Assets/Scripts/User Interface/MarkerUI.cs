using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerUI : MonoBehaviour
{
    public PickUpController pickUpController;

    public GameObject[] markerPosition = new GameObject[4];
    public GameObject marker;
    private GameObject[] markerUI = new GameObject[4];    

    private void Start()
    {
        markerUI[0] = Instantiate(marker, markerPosition[0].transform.position, Quaternion.identity, markerPosition[0].transform);
        markerUI[1] = Instantiate(marker, markerPosition[1].transform.position, Quaternion.identity, markerPosition[1].transform);
        markerUI[2] = Instantiate(marker, markerPosition[2].transform.position, Quaternion.identity, markerPosition[2].transform);
        markerUI[3] = Instantiate(marker, markerPosition[3].transform.position, Quaternion.identity, markerPosition[3].transform);
    }

    private void Update()
    {
        if (pickUpController.braaiPositionStatus[0] == true)
        {
            markerPosition[0].SetActive(true);
        }
        else if (pickUpController.braaiPositionStatus[0] == false)
        {
            markerPosition[0].SetActive(false);
        }

        if (pickUpController.braaiPositionStatus[1] == true)
        {
            markerPosition[1].SetActive(true);
        }
        else if (pickUpController.braaiPositionStatus[1] == false)
        {
            markerPosition[1].SetActive(false);
        }

        if (pickUpController.braaiPositionStatus[2] == true)
        {
            markerPosition[2].SetActive(true);
        }
        else if (pickUpController.braaiPositionStatus[2] == false)
        {
            markerPosition[2].SetActive(false);
        }

        if (pickUpController.braaiPositionStatus[3] == true)
        {
            markerPosition[3].SetActive(true);
        }
        else if (pickUpController.braaiPositionStatus[3] == false)
        {
            markerPosition[3].SetActive(false);
        }
    }

    
}
