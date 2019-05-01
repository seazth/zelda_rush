using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contrainte : MonoBehaviour
{
    public Transform other;

    void LateUpdate()
    {
        other.position = transform.position;
        other.rotation = transform.rotation;
    }
}
