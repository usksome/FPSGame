using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);   // 현재 위에서 카메라와 같은 방향을 더한 것.
    }
}
