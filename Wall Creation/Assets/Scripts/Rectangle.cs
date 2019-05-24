using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour
{
    bool creating;
    public GameObject StartWallPoint;
    public GameObject EndWallPoint;

    public GameObject IntermediateWallPrefab;

    GameObject IntermediateWall_top;
    GameObject IntermediateWall_right;
    GameObject IntermediateWall_bottom;
    GameObject IntermediateWall_left;


    //experimenting
    GameObject GuidingPoint_1;
    GameObject GuidingPoint_2;


    // Start is called before the first frame update
    void Start()
    {
        
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
            if(creating)
            {
                adjust();
            }
        }
    }

    void setStart()
    {
        creating = true;
        StartWallPoint.transform.position = getWorldPoint();
        
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
    void setEnd()
    {
        creating = false;
        EndWallPoint.transform.position = getWorldPoint();
    }
    void adjust()
    {
        EndWallPoint.transform.position = getWorldPoint();
        adjustIntermediateWall();

    }
    void adjustIntermediateWall()
    {

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



