using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class New_Kart_Controller : MonoBehaviour
{
    [Header("Kart Stat")]
    public float maxSpeed;
    public float turnSpeed;

    public int driftEscapeConstants;

    [Header("Kart position")]
    public bool isdrift = false;
    public Transform steeringTransform;
    public Transform driftTransform;
    private Rigidbody rBody;
    Vector3 wheelVelocityLS;
    public float ratio;
    public int speedmetre;

    //UI
    public UI_SPD uI_SPD;
    public Text text;

    //booster
    public circle_gauge_contorller circle_Gauge_Contorller;
    public float booster_time = 2f;
    bool booster_on = false;
    float drift_start_time = 0f;
    bool drift_start_time_cor = false;
    bool new_cut_cor = false;

    // Start is called before the first frame update
    void Start()
    {
        rBody = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && speedmetre >= 30){
            isdrift = true;
        }else{
            isdrift = false;
        }

        //KartRay
        Debug.DrawRay(transform.position, wheelVelocityLS.z * transform.forward, Color.cyan);
        Debug.DrawRay(transform.position, wheelVelocityLS.x * transform.right, Color.blue);
        Ratio();

        //spdcheck
        speedmetre = Mathf.RoundToInt(wheelVelocityLS.magnitude * 6.8f);
        spd(speedmetre);


        if (booster_on == false && Input.GetKeyDown(KeyCode.LeftControl) 
            && circle_Gauge_Contorller.booster > 0)
        {
            StartCoroutine("boosteron");
        }

    }

    private void FixedUpdate() {
        //wheelVelocityLS
        wheelVelocityLS = transform.InverseTransformDirection(rBody.velocity); 

        //old accel
        // if(!isdrift)

        
        
        if(booster_on == true)
        {
            
            rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 2500*1.3f);
            //circle_Gauge_Contorller.booster -= 1;
        }
        else if (speedmetre<=200 && booster_on == false)
        {
            rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 2500);
        }


        //new accel
        //rBody.AddRelativeForce(Vector3.forward * Input.GetAxisRaw("Vertical") * 1000);

        //old Steer
         //if(!isdrift)
            //rBody.AddForceAtPosition(transform.right*Input.GetAxisRaw("Horizontal")*3, steeringTransform.position, ForceMode.Acceleration);
         //if(isdrift)
            //rBody.AddForceAtPosition(transform.right*Input.GetAxisRaw("Horizontal")*10 * -1, driftTransform.position, ForceMode.Acceleration);


        if(isdrift){
            if (!drift_start_time_cor)
            {
                StartCoroutine("booster_timer");
            }
            // rBody.AddForceAtPosition(transform.right*Input.GetAxisRaw("Horizontal")*10 * -1, driftTransform.position, ForceMode.Acceleration);
            if(wheelVelocityLS.z <= 0){
                wheelVelocityLS.z = -1f * wheelVelocityLS.z;
                rBody.velocity = transform.TransformDirection(wheelVelocityLS);
                rBody.angularVelocity = new Vector3(rBody.angularVelocity.x, Mathf.Lerp(rBody.angularVelocity.y,0,8 * Time.fixedDeltaTime), rBody.angularVelocity.z);
            }
            rBody.AddTorque(new Vector3(0,Input.GetAxisRaw("Horizontal")*220,0));
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis("Horizontal") * 10, transform.eulerAngles.z), turnSpeed * Time.fixedDeltaTime);
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis("Horizontal") * 10, transform.eulerAngles.z), turnSpeed * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, Input.GetAxisRaw("Horizontal") * 100 * Time.deltaTime, 0));

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow) && new_cut_cor == false)
                {
                    new_cut_cor = true;
                    rBody.AddTorque(new Vector3(0, (-1) * 220, 0));
                }
                else
                {
                    new_cut_cor = false;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("right key");
                if (Input.GetKey(KeyCode.RightArrow) && new_cut_cor == false)
                {
                    Debug.Log("new cut on");
                    new_cut_cor = true;
                    rBody.AddTorque(new Vector3(0,  -2200 , 0));
                }
                else
                {
                    new_cut_cor = false;
                }
            }
            else
            {
                new_cut_cor = false;
            }

            new_cut_cor = false;

        }
        else
        {
            drift_start_time = 0f;
        }

        //Debug.Log(rBody.angularVelocity.y);
    }

    private void OnDrawGizmos() {
        if(!isdrift)
            Gizmos.DrawSphere(steeringTransform.position, 0.1f);
        if(isdrift)
            Gizmos.DrawSphere(driftTransform.position, 0.1f);
    }

    public void Ratio(){
        float x = Mathf.Abs(wheelVelocityLS.x);
        float z = Mathf.Abs(wheelVelocityLS.z);
        
        ratio = (x) / (x + z);
        text.text = Mathf.RoundToInt(ratio*100).ToString("000");
    }

    public void spd(int val){
        uI_SPD.updText(val);
    }


    private IEnumerator boosteron()
    {
        Debug.Log("on");
        booster_on = true;
        circle_Gauge_Contorller.booster -= 1;
        //rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 2500 * 1.3f);
        yield return new WaitForSeconds(booster_time);
        
        StopCoroutine("boosteron");
        booster_on = false;
        
    }

    private IEnumerator booster_timer()
    {
        drift_start_time_cor = true;
        yield return new WaitForSeconds(0.01f);
        drift_start_time += 0.01f;
        StopCoroutine("booster_timer");
        drift_start_time_cor = false;


    }

}
