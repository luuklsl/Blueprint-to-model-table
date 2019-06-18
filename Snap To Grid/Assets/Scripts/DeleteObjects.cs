using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                int layer = hitInfo.transform.gameObject.layer;
                if (layer == 10 || layer == 9)
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }
}
