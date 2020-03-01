using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject lookAtThisObject;  

    void Update()
    {
        Vector3 lookAtPoint = lookAtThisObject.transform.position;
        lookAtPoint.y = transform.position.y; 
        transform.LookAt(lookAtPoint);
    }
}
