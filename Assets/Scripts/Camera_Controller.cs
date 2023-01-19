using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public float rotsmoothconstants;
    public float smoothconstants;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, smoothconstants * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, rotsmoothconstants * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }
}
