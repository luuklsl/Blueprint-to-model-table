using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    private GameObject currentPlaceableObject = null;

    private GameObject lookedAt1 = null;
    private GameObject lookedAt2 = null;
    private GameObject block1 = null;
    private GameObject block2 = null;

    private float mouseWheelRotation;

    private void Update()
    {
            MoveCurrentObjectToMouse();
    }

    private void HandleNewObject()
    {
        if (currentPlaceableObject == null)
        {
            currentPlaceableObject = Instantiate(placeableObjectPrefab);
            currentPlaceableObject.GetComponentInChildren<Renderer>().material = MaterialPicker.selectedMaterial;
        }
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo1;
        RaycastHit hitInfo2;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hitInfo1))
            {
                if (hitInfo1.collider.gameObject.layer == 10)
                {
                    block1 = hitInfo1.collider.gameObject;
                }
                else
                {
                    return;
                }

                if (Physics.Raycast(block1.transform.position, block1.transform.forward, out hitInfo2))
                {
                    if (hitInfo2.distance < 2 && hitInfo2.collider.gameObject.layer == 10)
                    {
                        block2 = hitInfo2.collider.gameObject;
                    }
                    else
                    {
                        return;
                    }

                    HandleNewObject();
                    currentPlaceableObject.transform.position = block1.transform.position;
                    currentPlaceableObject.transform.rotation = block1.transform.rotation;
                    //Debug.DrawRay(block1.transform.position, block1.transform.forward * 1, Color.red);
                    if (lookedAt1 != block1 && lookedAt1 != null)
                    {
                        lookedAt1.GetComponent<Renderer>().enabled = true;
                    }
                    if (lookedAt2 != block2 && lookedAt2 != null)
                    {
                        lookedAt2.GetComponent<Renderer>().enabled = true;
                    }
                    block1.GetComponent<Renderer>().enabled = false;
                    block2.GetComponent<Renderer>().enabled = false;
                    lookedAt1 = block1;
                    lookedAt2 = block2;
                }        
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log(block1.name);
            if (lookedAt1 != block1 && lookedAt1 != null)
            {
                lookedAt1.GetComponent<Renderer>().enabled = true;
            }
            if (lookedAt2 != block2 && lookedAt2 != null)
            {
                lookedAt2.GetComponent<Renderer>().enabled = true;
            }
            if (block1 != null && block1.layer != 9)
            {
                Destroy(block1);
                if (lookedAt1 != null)
                {
                    Destroy(lookedAt1);
                }
            }
            if (block2 != null && block1.layer != 9)
            {
                Destroy(block2);
                if (lookedAt2 != null)
                {
                    Destroy(lookedAt2);
                }
            }
            currentPlaceableObject = null;
        }
    }

    private void RotateFromMouseWheel()
    {
        Debug.Log(Input.mouseScrollDelta);
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }
}