using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour
{
    private Grid grid;

    bool creating;
    public GameObject StartWallPoint;
    public GameObject EndWallPoint;

    //GuidingPoint_1 is inline with StartWallPoint, GuidingPoint_2 is inline with
    //EndWallPoint
    GameObject GuidingPoint_1;
    GameObject GuidingPoint_2;

    public GameObject IntermediateWallPrefab;


    //These gameobjects are create only as visual guides and are subsequently replaced
    //by individual wall segments
    GameObject IntermediateWall_top;
    GameObject IntermediateWall_right;
    GameObject IntermediateWall_bottom;
    GameObject IntermediateWall_left;


    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
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
                GuidingPoint_1.layer = 2;
                GuidingPoint_2.layer = 2;
                IntermediateWall_top.layer = 2;
                IntermediateWall_right.layer = 2;
                IntermediateWall_bottom.layer = 2;
                IntermediateWall_left.layer = 2;

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            if (Physics.Raycast(ray, out hitInfo))
            {
                setEnd(hitInfo.point);
            }
        }

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
        else
        {
            if (creating)
            {
                if (Physics.Raycast(ray, out hitInfo))
                {
                    adjust(hitInfo.point);
                }
            }
        }
    }

    private void RemoveObjectNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);


    }

    void setStart(Vector3 clickpoint)
    {
        creating = true;
        StartWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);
        
        GuidingPoint_1 = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);
        GuidingPoint_2 = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);


        IntermediateWall_top = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);
        IntermediateWall_right = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);
        IntermediateWall_bottom = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);
        IntermediateWall_left = (GameObject)Instantiate(IntermediateWallPrefab,
            StartWallPoint.transform.position, Quaternion.identity);
    }
    void setEnd(Vector3 clickpoint)
    {
        creating = false;
        EndWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);

        //Destroys the intermediate walls used for visual and replaces them with wall segments
        Destroy(IntermediateWall_top);
        Destroy(IntermediateWall_right);
        Destroy(IntermediateWall_bottom);
        Destroy(IntermediateWall_left);

        //Top segment
        //Calculates the distance between start and end wall points
        float distance_top = Vector3.Distance(StartWallPoint.transform.position,
            GuidingPoint_1.transform.position);

        //Creates subsequent segments and places them one after the other in order to create
        //a line after the left-click is released. The number of segments is equal to the 
        //distance between the start and end points.
        for (float i = 0; i < distance_top; i++)
        {
            float increment = 1f;

            //Checks for direction of creation for the segments
            if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z 
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (StartWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (-StartWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                 & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (StartWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (-StartWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

        }
        //Right segment
        float distance_right = Vector3.Distance(StartWallPoint.transform.position,
            GuidingPoint_2.transform.position);

        for (float i = 0; i < distance_right; i++)
        {
            float increment = 1f;

            if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                GuidingPoint_1.transform.position + (increment * i)
                * (-StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                GuidingPoint_2.transform.position + (increment * i)
                * (StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                GuidingPoint_1.transform.position + (increment * i)
                * (StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
           & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

       }

        //Bottom segment
        float distance_bottom = Vector3.Distance(EndWallPoint.transform.position,
            GuidingPoint_2.transform.position);

        for (float i = 0; i < distance_bottom; i++)
        {
            float increment = 1f;

            if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                EndWallPoint.transform.position + (increment * i)
                * (-EndWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                EndWallPoint.transform.position + (increment * i)
                * (EndWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                EndWallPoint.transform.position + (increment * i)
                * (-EndWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                EndWallPoint.transform.position + (increment * i)
                * (EndWallPoint.transform.right), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

        }

        //Left segment
        float distance_left = Vector3.Distance(EndWallPoint.transform.position,
            GuidingPoint_1.transform.position);

        for (float i = 0; i < distance_left; i++)
        {
            float increment = 1f;

            if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (-StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z > EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                EndWallPoint.transform.position + (increment * i)
                * (EndWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x < EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                StartWallPoint.transform.position + (increment * i)
                * (StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

            else if (StartWallPoint.transform.position.z < EndWallPoint.transform.position.z
                & StartWallPoint.transform.position.x > EndWallPoint.transform.position.x)
            {
                GameObject wall_between = (GameObject)Instantiate(IntermediateWallPrefab,
                EndWallPoint.transform.position + (increment * i)
                * (-StartWallPoint.transform.forward), StartWallPoint.transform.rotation);

                //Converts the layers to "Default" in order to be able to delete the objects
                wall_between.layer = 0;
            }

        }

        //Converts the layers to "Default" in order to be able to delete the objects
        //Does not set Start and EndWallPoints to default to avoid deleting them.
        //Instead it justs repositions them to default position outside of the grid
        //StartWallPoint.layer = 0;
        //EndWallPoint.layer = 0;
        GuidingPoint_1.layer = 0;
        GuidingPoint_2.layer = 0;

        //Resets the position of the StartWallPoint and EndWallPoint to avoid 
        //deleting them
        Vector3 initial_position = new Vector3(-0.106f, 1.25f, -0.77f);
        StartWallPoint.transform.position = initial_position;
        EndWallPoint.transform.position = initial_position;

    }
    void adjust(Vector3 clickpoint)
    {
        EndWallPoint.transform.position = grid.GetNearestPointOnGrid(clickpoint);
        placeIntermediateWallsforVisual();

    }
    void placeIntermediateWallsforVisual()
    {
        //Position the 2 guiding points one at each remaining rectangle cornerns
        Vector3 GuidingPoint_1_pos = new Vector3(EndWallPoint.transform.position.x,
            StartWallPoint.transform.position.y, StartWallPoint.transform.position.z);

        GuidingPoint_1.transform.position = GuidingPoint_1_pos;
        
        Vector3 GuidingPoint_2_pos = new Vector3(StartWallPoint.transform.position.x,
           StartWallPoint.transform.position.y, EndWallPoint.transform.position.z);

        GuidingPoint_2.transform.position = GuidingPoint_2_pos;
        

        //Top wall

        //Positioning distance
        Vector3 distance_top = new Vector3
            ((StartWallPoint.transform.position.x + GuidingPoint_1.transform.position.x)/2,
            (StartWallPoint.transform.position.y + GuidingPoint_1.transform.position.y)/2,
            (StartWallPoint.transform.position.z + GuidingPoint_1.transform.position.z)/2);

        IntermediateWall_top.transform.position = distance_top ;

        //Scale distance
        float distance_top_s = Vector3.Distance(StartWallPoint.transform.position,
            GuidingPoint_1.transform.position);
        
        IntermediateWall_top.transform.localScale = new Vector3(distance_top_s,
             IntermediateWall_top.transform.localScale.y, IntermediateWall_top.transform.localScale.z);

        //Right wall

        //Positioning distance
        Vector3 distance_right = new Vector3
            ((EndWallPoint.transform.position.x + GuidingPoint_1.transform.position.x) / 2,
            (EndWallPoint.transform.position.y + GuidingPoint_1.transform.position.y) / 2,
            (EndWallPoint.transform.position.z + GuidingPoint_1.transform.position.z) / 2);

        IntermediateWall_right.transform.position = distance_right;

        //Scale distance
        float distance_right_s = Vector3.Distance(EndWallPoint.transform.position,
            GuidingPoint_1.transform.position);

        IntermediateWall_right.transform.localScale = new Vector3(IntermediateWall_right.transform.localScale.x,
             IntermediateWall_right.transform.localScale.y, distance_right_s);

        //Bottom wall

        //Positioning distance
        Vector3 distance_bottom = new Vector3
            ((EndWallPoint.transform.position.x + GuidingPoint_2.transform.position.x) / 2,
            (EndWallPoint.transform.position.y + GuidingPoint_2.transform.position.y) / 2,
            (EndWallPoint.transform.position.z + GuidingPoint_2.transform.position.z) / 2);

        IntermediateWall_bottom.transform.position = distance_bottom;

        //Scale distance
        float distance_bottom_s = Vector3.Distance(EndWallPoint.transform.position,
            GuidingPoint_2.transform.position);

        IntermediateWall_bottom.transform.localScale = new Vector3(distance_bottom_s,
             IntermediateWall_bottom.transform.localScale.y, IntermediateWall_bottom.transform.localScale.z);

        //Left wall

        //Positioning distance
        Vector3 distance_left = new Vector3
            ((StartWallPoint.transform.position.x + GuidingPoint_2.transform.position.x) / 2,
            (StartWallPoint.transform.position.y + GuidingPoint_2.transform.position.y) / 2,
            (StartWallPoint.transform.position.z + GuidingPoint_2.transform.position.z) / 2);

        IntermediateWall_left.transform.position = distance_left;

        //Scale distance
        float distance_left_s = Vector3.Distance(StartWallPoint.transform.position,
            GuidingPoint_2.transform.position);

        IntermediateWall_left.transform.localScale = new Vector3(IntermediateWall_left.transform.localScale.x,
             IntermediateWall_left.transform.localScale.y, distance_left_s);

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



