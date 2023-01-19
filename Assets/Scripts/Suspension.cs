using UnityEngine;

public class Suspension : MonoBehaviour 
{
	private Rigidbody rbody;

	[Header("Suspension")]
	public float restlength;
	public float springTravel;
	public float springStiffness;
	public float damperStiffness;

	private float maxLength;
	private float minLength;
	private float lastFrameLength;
	private float springLength;	
	private float springVelocity;
	[SerializeField]
	private float springForce;
	private float damperForce;

	private float Fx;
	public static float Fy;

	RaycastHit hit;

	[Header("Wheel")]
	public float wheelRadius;

	private Vector3 SuspensionForce;
	private Vector3 wheelVelocityLS;

	[Header("Skid")]
	public GameObject skid;
	public GameObject nkcobj;
	public New_Kart_Controller nkc;

	private void Start() {
		rbody = transform.root.GetComponent<Rigidbody>();
		minLength = restlength - springTravel;
		maxLength = restlength + springTravel;	
		nkc = nkcobj.GetComponent<New_Kart_Controller>();
	}

	private void Update() {
		//서스펜션 포스
		//Debug.DrawRay(transform.position, -transform.up * springLength, Color.green);
		//휠의 속력
		//Debug.DrawRay(hit.point, wheelVelocityLS.z * transform.forward , Color.yellow);
		//구심력
		//Debug.DrawRay(hit.point, wheelVelocityLS.x * -transform.right, Color.red);
		//합력
		//Debug.DrawRay(hit.point, SuspensionForce + Fy * -transform.right, Color.blue);
	}

	private void FixedUpdate()
	{
		if(Physics.Raycast(transform.position, -transform.up, out hit, maxLength + wheelRadius)){
			lastFrameLength = springLength;
			springLength = hit.distance - wheelRadius;
			springVelocity = (lastFrameLength - springLength)/Time.fixedDeltaTime;
			springLength = Mathf.Clamp(springLength, minLength, maxLength);
			springForce = springStiffness * (restlength - springLength);
			damperForce = damperStiffness * springVelocity;
			SuspensionForce = (springForce+damperForce) * transform.up;

			wheelVelocityLS = transform.InverseTransformDirection(rbody.GetPointVelocity(hit.point)); 
			Fx = wheelVelocityLS.z;
			//구심력
			Fy = wheelVelocityLS.x * springForce / 3;
			if(nkc.ratio >= 0.2f){
				Fy = Mathf.Lerp(Fy, Fy/6f, 15f * Time.fixedDeltaTime);				
				// Fy /= 6;
			}
			rbody.AddForceAtPosition(Fy * -transform.right, hit.point);
			rbody.AddForceAtPosition(SuspensionForce, hit.point);

			if(skid != null){
				skid.transform.position = hit.point;
			}			
		}
	}
}
