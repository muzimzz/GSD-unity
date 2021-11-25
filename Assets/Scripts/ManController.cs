using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    float rotSpeed = 3.0f;

    void Update()
    {
        transform.Rotate(0, 0, this.rotSpeed);
    }

    
}
