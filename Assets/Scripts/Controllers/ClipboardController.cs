using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardController : MonoBehaviour
{

    public GameObject orderText;

    public GameObject orderTextHoldPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        orderText.transform.position = orderTextHoldPos.transform.position;
        orderText.transform.eulerAngles = orderTextHoldPos.transform.eulerAngles;
    }
}
