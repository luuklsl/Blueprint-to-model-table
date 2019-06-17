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
                name = hitInfo.transform.gameObject.name;
                if (name != "Terrain" && name != "Grid")
                {
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }
}
