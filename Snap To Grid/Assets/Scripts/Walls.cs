using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{

    private Grid grid;

    bool creating;
    public GameObject StartWallPoint;
    public GameObject EndWallPoint;
    
    GameObject IntermediateWall;
    public GameObject IntermediateWallPrefab;

    bool xDirectionSnapping;
    bool zDirectionSnapping;

    bool nearestWallSnapping;
    List<GameObject> walls;


    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        walls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) 
        {
           
            if (Physics.Raycast(ray, out hitInfo))
            {
                setStart(hitInfo.point);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {


            if (Physics.Raycast(ray, out hitInfo))
            { 
                setEnd(hitInfo.point);
            }
        }
        else
        {
            if (creating == true) { 
                if (Physics.Raycast(ray, out hitInfo))
                    {
                        adjust(hitInfo.point);
                    }
            }
        }


        //From here on down, specific snapping functions can be triggered with keys
        if (Input.GetKey(KeyCode.X))
        {
            zDirectionSnapping = true;
        }
        else
        {
            zDirectionSnapping = false;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            xDirectionSnapping = true;
        }
        else
        {
            xDirectionSnapping = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            nearestWallSnapping = !nearestWallSnapping;
            if (GameObject.FindGameObjectsWithTag("Wall").Length == 0)
            {
                nearestWallSnapping = false;
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


    void setStart(Vector3 clickpoint)
    {
        creating = true;
        StartWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);
        IntermediateWall = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);

        if (nearestWallSnapping)
        {
            StartWallPoint.transform.position = closestWallTo(clickpoint)
                .transform.position;
        }
    }

    void setEnd(Vector3 clickpoint)
    {
        //Set creating to false and take the endpoint from a raycast
        creating = false;
        EndWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);

        //Some special cases that might happen
        if (xDirectionSnapping)
        {
            EndWallPoint.transform.position = new Vector3(StartWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, EndWallPoint.transform.position.z);
        }
        if (zDirectionSnapping)
        {
            EndWallPoint.transform.position = new Vector3(EndWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, StartWallPoint.transform.position.z);
        }
        setEndWalls();
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


    void adjust(Vector3 clickpoint)
    {
        EndWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);

        if (xDirectionSnapping)
        {
            EndWallPoint.transform.position = new Vector3(StartWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, EndWallPoint.transform.position.z);
        }
        if (zDirectionSnapping)
        {
            EndWallPoint.transform.position = new Vector3(EndWallPoint.transform.position.x,
                EndWallPoint.transform.position.y, StartWallPoint.transform.position.z);
        }

        adjustIntermediateWall();
    }


    void adjustIntermediateWall()
    {
        //Adjusts the start and end 'poles' to look at eachother, also adjusts the wall inbetween.

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


    //Vector3 getWorldPoint()
    //{
    //    Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        return hit.point;
    //    }
    //    return Vector3.zero;
    //}
    
}
