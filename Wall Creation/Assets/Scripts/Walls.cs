using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{

    bool creating;
    public GameObject StartWallPoint;
    public GameObject EndWallPoint;
    
    GameObject IntermediateWall;
    public GameObject IntermediateWallPrefab;

    bool xSnapping;
    bool zSnapping;

    bool wallSnapping;
    List<GameObject> walls;

   


    // Start is called before the first frame update
    void Start()
    {
        walls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void getInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            setStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            setEnd();
        }
        else
        {
            if (creating == true)
            {
                adjust();
            }
        }

        if (Input.GetKey(KeyCode.X))
        {
            zSnapping = true;
        }
        else
        {
            zSnapping = false;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            xSnapping = true;
        }
        else
        {
            xSnapping = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            wallSnapping = !wallSnapping;
            if (GameObject.FindGameObjectsWithTag("Wall").Length == 0)
            {
                wallSnapping = false;
                Debug.Log("set some walls before activating pole snapping");
            }
        }

    }

    //new
    Vector3 gridSnap(Vector3 originalPosition)
    {
        int granularity = 1;
        Vector3 snappedPosition = new Vector3(Mathf.Floor(originalPosition.x/granularity)
            * granularity, originalPosition.y, Mathf.Floor(originalPosition.z/granularity)
            * granularity);
        return snappedPosition;
    }

    void setStart()
    {
        creating = true;
        StartWallPoint.transform.position = gridSnap(getWorldPoint());
        IntermediateWall = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);

        if (wallSnapping)
        {
            StartWallPoint.transform.position = closestWallTo(getWorldPoint())
                .transform.position;
        }
    }

    GameObject closestWallTo(Vector3 worldPoint)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        float currentDistance = Mathf.Infinity;
        foreach (GameObject w in walls)
        {
            currentDistance = Vector3.Distance(worldPoint, w.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = w;
            }
        }
        return closest;
    }


    void setEnd()
    {
        creating = false;
        EndWallPoint.transform.position = gridSnap(getWorldPoint());
        if (xSnapping)
        {
            EndWallPoint.transform.position = new Vector3(StartWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, EndWallPoint.transform.position.z);
        }
        if (zSnapping)
        {
            EndWallPoint.transform.position = new Vector3(EndWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, StartWallPoint.transform.position.z);
        }
        setEndWalls();
    }

    void setEndWalls()
    {
        GameObject w1 = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, StartWallPoint.transform.rotation);
        GameObject w2 = (GameObject)Instantiate(IntermediateWallPrefab,
            EndWallPoint.transform.position, EndWallPoint.transform.rotation);
        w1.tag = "Wall";
        w2.tag = "Wall";
        walls.Add(w1);
        walls.Add(w2);
    }

    void adjust()
    {
        EndWallPoint.transform.position = gridSnap(getWorldPoint());

        if (xSnapping)
        {
            EndWallPoint.transform.position = new Vector3(StartWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, EndWallPoint.transform.position.z);
        }
        if (zSnapping)
        {
            EndWallPoint.transform.position = new Vector3(EndWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, StartWallPoint.transform.position.z);
        }

        adjustIntermediateWall();
    }

    void adjustIntermediateWall()
    {
        StartWallPoint.transform.LookAt(EndWallPoint.transform.position);
        EndWallPoint.transform.LookAt(StartWallPoint.transform.position);
        float distance = Vector3.Distance(StartWallPoint.transform.position,
            EndWallPoint.transform.position);
        IntermediateWall.transform.position = StartWallPoint.transform.position + 
            (distance/2) * StartWallPoint.transform.forward;
        IntermediateWall.transform.rotation = StartWallPoint.transform.rotation;
        IntermediateWall.transform.localScale = new Vector3(IntermediateWall.transform.localScale.x
            , IntermediateWall.transform.localScale.y, distance);
 
    }

    Vector3 getWorldPoint()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    
}
