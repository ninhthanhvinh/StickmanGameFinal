using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new(0, 2, -10);
    Vector3 velocity;
    private void LateUpdate()
    {
        if (target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, .5f);
        }
    }
}
