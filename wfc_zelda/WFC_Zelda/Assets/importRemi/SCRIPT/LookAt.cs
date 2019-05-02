using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform LookAtTransform;
    public void Start()
    {
        LookAtTransform = Camera.main.transform;
    }
    public void Update()
    {
        Vector3 direction = (transform.position - LookAtTransform.position).normalized;
        //create the rotation we need to be in to look at the target
        Vector3 _lookRotation = Quaternion.LookRotation(direction).eulerAngles;
        transform.rotation = Quaternion.Euler(_lookRotation.x, 0, 0);
    }
}
