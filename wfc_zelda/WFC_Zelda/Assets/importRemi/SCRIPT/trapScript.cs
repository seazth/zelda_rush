using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapScript : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject arrow;
  int chrono = 300;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   // Debug.Log("Chrono : "+ chrono);

    timer();
    }
  void timer()
  {
    if (chrono <= 0) {
      Debug.Log("Fleche lancé ");
      Instantiate(arrow, new Vector3(0, 0, 0), Quaternion.identity);
      chrono = 300;
    }
    else {
      chrono--;
    }
  }
}
