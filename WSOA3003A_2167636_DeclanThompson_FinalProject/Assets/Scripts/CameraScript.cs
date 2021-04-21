using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset;
    void Update()
    {
        gameObject.GetComponent<Transform>();
        gameObject.transform.position = Player.position + offset;
    }
}
