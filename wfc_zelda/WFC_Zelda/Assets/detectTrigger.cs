using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectTrigger : MonoBehaviour
{
  public GameObject Quad;
  public float floor;
  public bool isFirst;
    // Start is called before the first frame update
    void Start()
    {
    Quad.transform.position = new Vector3(0, 50, 0);

  }

  // Update is called once per frame
  void Update()
    {
        
    }

  void modifyQuad()
  {
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player") {
      Debug.Log("TEst");
      Quad.transform.position = new Vector3(0, floor, 0);
      if (isFirst) {
        GameObject.Find("VILLAGE").SetActive(false);

      }
      //Quad.transform.Translate(1, floor, 1);
    }
  }
}
