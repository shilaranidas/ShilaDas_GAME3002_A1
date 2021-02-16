using UnityEngine.Assertions;
using UnityEngine;
using TMPro;
using System.Collections;

public class BallPhysics : MonoBehaviour
{
    public TMP_Text scoreValuetxt;
    public TMP_Text ballLefttxt;
    [SerializeField]
    private float m_fInputDeltaVal = 0.001f;
    [SerializeField]
    private Vector3 m_vTargetPos;
    [SerializeField]
    private Vector3 m_vInitialVel;
    [SerializeField]
    private Vector3 m_vInitialVelocity = Vector3.zero;
    [SerializeField]
    private bool m_bDebugKickBall = false;
    public int score = 0;
    public int BallLeft = 5;
    private Rigidbody m_rb = null;
    private GameObject m_TargetDisplay = null;

    private bool m_bIsGrounded = true;

    private float m_fDistanceToTarget = 0f;

    private Vector3 vDebugHeading;
    //Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Houston, we've got a problem here! No Rigidbody attached");
       // startPosition= transform.position;
        CreateTargetDisplay();

    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //m_rb.velocity = Vector3.zero;
        //m_rb.MovePosition(startPosition);
        
        if (other.gameObject.tag == "Boundary")
        {
            this.gameObject.GetComponent<Rigidbody>().velocity *= -1;
            Debug.Log("boundary");     
        }
        else
        {
            if (other.gameObject.tag == "Goal")
            {
                Debug.Log("goal");
                score++;
            }
            if (other.gameObject.tag == "Wall")
            {
                Debug.Log("wall");
            }
            if (other.gameObject.tag == "outofField")
            {
                Debug.Log("outfField");
            }
            transform.position = GameObject.Find("BallStartPosition").transform.position;
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
 
    }
   
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{

        //   
        //}

        //if (Input.GetMouseButtonUp(1))
        //{
        //    m_rb.velocity = Vector3.zero;
        //    m_rb.MovePosition(startPosition);
        //}

        ////Rotate ball with mouse
        //float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        //float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        
        //transform.localRotation = Quaternion.Euler(new Vector4(-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z));
       // Debug.Log("rot" + transform.localRotation);
        HandleUserInput();
        //// GetLandingPosition();
        if (m_TargetDisplay != null && m_bIsGrounded)
        {
            m_TargetDisplay.transform.position = m_vTargetPos;
            Debug.Log("change" + m_TargetDisplay.transform.position+" "+m_vTargetPos);
            vDebugHeading = m_vTargetPos - transform.position;
        }

        if (m_bDebugKickBall && m_bIsGrounded)
        {
            m_bDebugKickBall = false;
            if (m_TargetDisplay != null && m_bIsGrounded)
            {
                m_TargetDisplay.transform.position = GetLandingPosition();
            }
            // OnKickBall();
        }
        scoreValuetxt.text = score.ToString();
        ballLefttxt.text = BallLeft.ToString();
    }
    public Material myM;
    private void CreateTargetDisplay()
    {
        m_TargetDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_TargetDisplay.transform.position = new Vector3(0, .5f, 10);
        m_TargetDisplay.GetComponent<MeshRenderer>().material = myM;
        m_vTargetPos =new Vector3(0, .5f, 10);
        
        m_TargetDisplay.transform.localScale = new Vector3(.3f, 0.1f, .3f);
        m_TargetDisplay.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        //m_TargetDisplay.GetComponent<Renderer>().material.color = Color.red;
        m_TargetDisplay.GetComponent<Collider>().enabled = false;
    }

    //public void OnKickBall()
    //{
    //    // H = Vi^2 * sin^2(theta) / 2g
    //    // R = 2Vi^2 * cos(theta) * sin(theta) / g

    //    // Vi = sqrt(2gh) / sin(tan^-1(4h/r))
    //    // theta = tan^-1(4h/r)

    //    // Vy = V * sin(theta)
    //    // Vz = V * cos(theta)
    //    Debug.Log("Kick "+ m_TargetDisplay.transform.position+ " "+m_vTargetPos+" "+ transform.position);
    //    //m_rb = GetComponent<Rigidbody>();
    //    //m_rb.AddRelativeForce(0, 100, 100);
    //    m_fDistanceToTarget = (m_TargetDisplay.transform.position - transform.position).magnitude;
    //    float fMaxHeight = m_TargetDisplay.transform.position.y;
    //    float fRange = (m_fDistanceToTarget * 2);
    //    float fTheta = Mathf.Atan((4 * fMaxHeight) / (fRange));

    //    float fInitVelMag = Mathf.Sqrt((2 * Mathf.Abs(Physics.gravity.y) * fMaxHeight)) / Mathf.Sin(fTheta);

    //    m_vInitialVel.y = fInitVelMag * Mathf.Sin(fTheta);
    //    m_vInitialVel.z = fInitVelMag * Mathf.Cos(fTheta);

    //    m_rb.velocity = m_vInitialVel;
    //    BallLeft--;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + vDebugHeading, transform.position);
        //Gizmos.DrawLine(transform.position,);
    }
    private Vector3 GetLandingPosition()
    {
        float fTime = 2f * (0f - m_vInitialVelocity.y / Physics.gravity.y);

        Vector3 vFlatVel = m_vInitialVelocity;
        vFlatVel.y = 0f;
        vFlatVel *= fTime;

        return m_vTargetPos + vFlatVel;
    }
    public void OnLaunchProjectile()
    {
        if (!m_bIsGrounded)
        {
            return;
        }

        m_TargetDisplay.transform.position = GetLandingPosition();
        m_bIsGrounded = false;

        transform.LookAt(m_TargetDisplay.transform.position, Vector3.up);

        m_rb.velocity = m_vInitialVelocity;
    }

    private void HandleUserInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //m_projectile.
            m_bDebugKickBall = true;
            OnLaunchProjectile();
           
        }

        if (Input.GetKey(KeyCode.W))
        {
            m_vTargetPos.z += m_fInputDeltaVal;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_vTargetPos.z -= m_fInputDeltaVal;
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_vTargetPos.x += m_fInputDeltaVal;
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_vTargetPos.x -= m_fInputDeltaVal;
        }
        if (Input.GetKey(KeyCode.R))
        {
            m_vTargetPos.y += m_fInputDeltaVal;
        }

        if (Input.GetKey(KeyCode.F))
        {
            m_vTargetPos.y -= m_fInputDeltaVal;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_rb.MovePosition(new Vector3(m_rb.transform.position.x , m_rb.transform.position.y, m_rb.transform.position.z + m_fInputDeltaVal));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_rb.MovePosition(new Vector3(m_rb.transform.position.x , m_rb.transform.position.y, m_rb.transform.position.z - m_fInputDeltaVal));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_rb.MovePosition(new Vector3(m_rb.transform.position.x + m_fInputDeltaVal, m_rb.transform.position.y, m_rb.transform.position.z));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_rb.MovePosition(new Vector3(m_rb.transform.position.x - m_fInputDeltaVal, m_rb.transform.position.y, m_rb.transform.position.z));
        }
    }
}
