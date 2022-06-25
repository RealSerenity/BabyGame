using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectPredict : MonoBehaviour
{
    public bool hasCollision = false;

    private void Start()
    {
        //Destroy(this.gameObject,.03f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ProjectPredict>() == null)
        {
            transform.rotation = Quaternion.Euler(other.gameObject.transform.rotation.x,transform.rotation.y,transform.rotation.z);
        }
    }
}
