using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yAnchor : MonoBehaviour
{

    private float originalY;

    // Start is called before the first frame update
    void Start()
    {
        originalY = this.transform.position.y;
        Debug.Log(originalY);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3 (transform.position.x, originalY, transform.position.z);
    }
}
