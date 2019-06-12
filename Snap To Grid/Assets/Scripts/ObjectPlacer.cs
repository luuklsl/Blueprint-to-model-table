using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;

    //[SerializeField]
    //private KeyCode newObjectHotkey = KeyCode.A;

    private GameObject currentPlaceableObject;

    private GameObject lookedAt = null;

    private float mouseWheelRotation;

    private void Update()
    {


        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
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

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {   
            if (hitInfo.collider.gameObject.name != "Terrain")
            {
                if (lookedAt != hitInfo.collider.gameObject && lookedAt != null)
                {
                    lookedAt.GetComponent<Renderer>().enabled = true;
                }
                hitInfo.collider.gameObject.GetComponent<Renderer>().enabled = false;
                lookedAt = hitInfo.collider.gameObject;
                //print(hitInfo.collider.gameObject.name);
                //hitInfo.collider.gameObject.GetComponent<Renderer>().enabled = false;
                currentPlaceableObject.transform.position = hitInfo.collider.gameObject.transform.position;
                currentPlaceableObject.transform.rotation = hitInfo.collider.gameObject.transform.rotation;
                
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