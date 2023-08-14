using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform mainCamera;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        //transform.LookAt(mainCamera);
        //transform.Rotate(0, 180, 0);
    }
}
