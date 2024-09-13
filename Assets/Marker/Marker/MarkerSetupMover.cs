using Others;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarkerSetupMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("MarkerReferencePoint").GetComponent<Transform>().parent = transform;
        GameObject.Find("MarkerReferencePoint").GetComponent<Transform>().SetPositionAndRotation(new Vector3(), Quaternion.identity);
    }
}
