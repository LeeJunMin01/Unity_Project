using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    Transform cam;
    void Start()
    {
        cam = Camera.main.transform;
    }


    void Update()
    {
        //항상 카메라를 정면으로 바라볼 수 있도록 로테이션 값이 조정되게 함
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);   
    }
}
