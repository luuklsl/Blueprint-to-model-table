﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
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
    List<GameObject> walls_to_snap;


    List<GameObject> building_walls;


    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    // Start is called before the first frame update 
    void Start()
    {
        walls_to_snap = new List<GameObject>();

        building_walls = new List<GameObject>();
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


                //On click of the left mouse-button changes the layers to "Ignore Raycast" 
                //to avoid having the line tilt upwards.  
                StartWallPoint.layer = 2;
                EndWallPoint.layer = 2;
                IntermediateWall.layer = 2;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            if (Physics.Raycast(ray, out hitInfo))
            {
                setEnd(hitInfo.point);

            }
        }
        else if (creating == true)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                adjust(hitInfo.point);
            }

        }
        //Destroys individual wall segments 

        //Destroys only if object is block, probably should update when NOT terrain 
        else if (Input.GetMouseButton(1))
        {

            if (Physics.Raycast(ray, out hitInfo))
            {
                RemoveObjectNear(hitInfo.point);
                name = hitInfo.transform.gameObject.name;
                if (name != "Terrain" && name != "Grid")
                {
                    Destroy(hitInfo.transform.gameObject);
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

        //if (Input.GetKeyDown(KeyCode.P)) 
        //{ 
        //    nearestWallSnapping = !nearestWallSnapping; 
        //    if (GameObject.FindGameObjectsWithTag("walls_to_snap").Length == 0) 
        //    { 
        //        nearestWallSnapping = false; 
        //        Debug.Log("set some walls before activating wall snapping"); 
        //    } 
        //} 

    }

    private void RemoveObjectNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);

    }

    void setStart(Vector3 clickpoint)
    {
        creating = true;
        StartWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);


        IntermediateWall = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);


        
        //if (nearestWallSnapping) 
        //{ 
        //    StartWallPoint.transform.position = closestWallTo(clickpoint) 
        //        .transform.position; 
        //} 

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


        //After the left click is released kills the intermediateWall object and 
        //adjusts the start and end 'poles' to look at eachother,  
        //also adjusts the wall inbetween 
        Destroy(IntermediateWall);
        StartWallPoint.transform.LookAt(EndWallPoint.transform.position);
        EndWallPoint.transform.LookAt(StartWallPoint.transform.position);

        //Calculates the distance between start and end wall points 
        float distance = Vector3.Distance(StartWallPoint.transform.position,
            EndWallPoint.transform.position);

        //Creates subsequent segments and places them one after the other in order to create 
        //a line after the left-click is released. The number of segments is equal to the  
        //distance between the start and end points. 

        //The +1f is used to replace the EndWallPoint (which is repositioned to default) 
        //with an individual wall segment 
        float increment = 1f;

        for (float i = 0; i < (distance + increment); i++)
        {

            GameObject building_wall = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position + (increment * i)
            * StartWallPoint.transform.forward, StartWallPoint.transform.rotation);

            building_wall.tag = "building_wall";
            building_walls.Add(building_wall);

            //Converts the layers to "Default" in order to be able to delete the objects 
            //Does not set Start and EndWallPoint to 'default' layer to avoid deleting them 
            //and crashing the game. Instead it just repositions them to their default 
            //position 
            building_wall.layer = 0;



            //Lifts the wall segments upwards so that they are on the same level
            //with the grid
            building_wall.transform.position = new Vector3(building_wall.transform.position.x,
                1.5f, building_wall.transform.position.z);
        }



        //setEndWalls(); 

        //Resets the position of the StartWallPoint and EndWallPoint to avoid  
        //deleting them 
        Vector3 initial_position = new Vector3(-0.106f, 1.5f, -0.77f);
        StartWallPoint.transform.position = initial_position;
        EndWallPoint.transform.position = initial_position;


    }


    //GameObject closestWallTo(Vector3 worldPoint) 
    //{ 
    //    GameObject closest = null; 
    //    float distance = Mathf.Infinity; 
    //    float currentDistance = Mathf.Infinity; 
    //    foreach (GameObject w in walls_to_snap) 
    //    { 

    //        currentDistance = Vector3.Distance(worldPoint, w.transform.position); 
    //        if (currentDistance < distance) 
    //        { 
    //            distance = currentDistance; 
    //            closest = w; 
    //        } 
    //    } 
    //    return closest; 
    //} 


    //void setEndWalls() 
    //{ 
    //    GameObject w1 = (GameObject)Instantiate(IntermediateWallPrefab, 
    //        StartWallPoint.transform.position, StartWallPoint.transform.rotation); 
    //    GameObject w2 = (GameObject)Instantiate(IntermediateWallPrefab, 
    //        EndWallPoint.transform.position, EndWallPoint.transform.rotation); 
    //    w1.tag = "walls_to_snap"; 
    //    w2.tag = "walls_to_snap"; 
    //    w1.layer = 0; 
    //    w2.layer = 0; 
    //    walls_to_snap.Add(w1); 
    //    walls_to_snap.Add(w2); 

    //} 


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

        placeanIntermediateWallforVisual();
    }


    void placeanIntermediateWallforVisual()
    {
        //Creates a scalable intermediate wall to complete the line. This object is later 
        //deleted and replaced by individual objects representing wall segments. 

        //Adjusts the start and end 'poles' to look at eachother,  
        //also adjusts the wall inbetween. 
        StartWallPoint.transform.LookAt(EndWallPoint.transform.position);
        EndWallPoint.transform.LookAt(StartWallPoint.transform.position);

        float distance = Vector3.Distance(StartWallPoint.transform.position,
            EndWallPoint.transform.position);

        IntermediateWall.transform.position = StartWallPoint.transform.position
            + (distance * 0.5f) * StartWallPoint.transform.forward;

        IntermediateWall.transform.rotation = StartWallPoint.transform.rotation;

        IntermediateWall.transform.localScale = new Vector3(IntermediateWall.transform.localScale.x
            , IntermediateWall.transform.localScale.y, distance);




    }

}
