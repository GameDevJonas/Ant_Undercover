using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        if (GetComponentInParent<PlayerUI>().isLocalPlayer)
        {
            this.gameObject.SetActive(false);
        }
    }
}
