using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] materials;
    Material selectedMaterial = null;
    private float mouseWheelRotation;
    // Update is called once per frame

    void Update()
    {
        ChooseMaterial();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButton(0))
            {
                hitInfo.collider.gameObject.GetComponent<Renderer>().material = selectedMaterial;
            }
        }
        
    }

    private void ChooseMaterial()
    {
        //Debug.Log(Input.mouseScrollDelta);
        //mouseWheelRotation += Input.mouseScrollDelta.y;
        //selectedMaterial = materials[(int)mouseWheelRotation%materials.Length];
        selectedMaterial = MaterialPicker.selectedMaterial;
    }
}
