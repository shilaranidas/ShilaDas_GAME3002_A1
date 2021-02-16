using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public TMP_Text scoreValuetxt;
    public TMP_Text ballLefttxt;
    public static int score = 0;
    public static int BallLeft = 5;
    public Rigidbody ballPrefabs;
   // public Transform startPos;
    public GameObject cursor;
    public Transform shootPoint;
    public static bool kickBall = false;
    public static bool reset = false;
    public LayerMask layer;
    private Camera cam;
    public AudioSource snd;
    Rigidbody obj;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        obj = Instantiate(ballPrefabs, shootPoint.position, Quaternion.identity);
       // startPos = ballPrefabs.transform;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchProjectile();
        scoreValuetxt.text = score.ToString();
        ballLefttxt.text = BallLeft.ToString();
    }
    void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point + Vector3.up * .1f;
            Vector3 Vo = CalculateVelocity(hit.point, shootPoint.position, 1f);
            transform.rotation = Quaternion.LookRotation(Vo);
            if (Input.GetMouseButtonDown(0))
            {
                snd.Play();
                // GameObject.Find("displayBall").SetActive(false);
                obj.transform.position = shootPoint.position;
               
                obj.velocity = Vo;
                BallLeft--;
                
                kickBall = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                // GameObject.Find("displayBall").SetActive(true);
                Destroy(obj);
                reset = true;
                 obj = Instantiate(ballPrefabs, shootPoint.position, Quaternion.identity);
                obj.velocity = Vector3.zero;
                obj.rotation = Quaternion.identity;
                
                obj.transform.position = shootPoint.position;
                  
            }
        }
        else
        {
            cursor.SetActive(false);
           // GameObject.Find("displayBall").SetActive(true);
        }
    }
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;
        //create a float the represent the distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;
        float Vxz = Sxz / time;
        float Vy = Sy / time + .5f * Mathf.Abs(Physics.gravity.y) * time;
        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
    }
}
