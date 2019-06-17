using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    //[SerializeField]
    //private KeyCode newObjectHotkey = KeyCode.A;

    private GameObject currentPlaceableObject;

    private GameObject lookedAt1 = null;
    private GameObject lookedAt2 = null;

    private float mouseWheelRotation;

    private void Update()
    {
        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            //RotateFromMouseWheel();
            //ReleaseIfClicked();
        }
        else
        {
            HandleNewObject();
        }
    }

    private void HandleNewObject()
    {
        if (currentPlaceableObject != null)
        {
            Destroy(currentPlaceableObject);
        }
        else
        {
            currentPlaceableObject = Instantiate(placeableObjectPrefab);
        }
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo1;
        RaycastHit hitInfo2;

        if (Physics.Raycast(ray, out hitInfo1))
        {
            GameObject block1 = hitInfo1.collider.gameObject;
            if (Physics.Raycast(block1.transform.position, block1.transform.forward, out hitInfo2))
            {
                GameObject block2 = hitInfo2.collider.gameObject;

                if (block1.name != "Terrain")
                {
                    Debug.DrawRay(block1.transform.position, block1.transform.forward * 1, Color.red);

                    if (lookedAt1 != block1 && lookedAt1 != null)
                    {
                        lookedAt1.GetComponent<Renderer>().enabled = true;
                        lookedAt2.GetComponent<Renderer>().enabled = true;
                    }
                    block1.GetComponent<Renderer>().enabled = false;
                    block2.GetComponent<Renderer>().enabled = false;
                    lookedAt1 = block1;
                    lookedAt2 = block2;
                    currentPlaceableObject.transform.position = block1.transform.position;
                    currentPlaceableObject.transform.rotation = block1.transform.rotation;

                    if (Input.GetMouseButtonDown(0))
                    {
                        currentPlaceableObject = null;
                        Destroy(block1);
                        Destroy(block2);
                    }

                }
            }
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