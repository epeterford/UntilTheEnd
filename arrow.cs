using UnityEngine;
using System.Collections;

public class arrow : MonoBehaviour {
	private float speed = 2f;
	private bool shouldMove=true;
	public static Vector3 arrowTarget;
	private Rigidbody rb;
	private SenderDamage dmg;
	public GameObject arrowParticle;
	public GameObject arrowCollision;
	// Use this for initialization
	void Start () {
		transform.LookAt(arrowTarget);
		rb = gameObject.GetComponent<Rigidbody> ();
		dmg = gameObject.GetComponent<SenderDamage> ();
		arrowParticle = transform.FindChild("Arrow Particle").gameObject;
		arrowCollision = transform.FindChild("Arrow Collision").gameObject;
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "Player") // If collides with something other than player
		{
			arrowParticle.SetActive(false);
			arrowCollision.SetActive(true);
			Destroy(this.gameObject, .2f);

		}
	}
	// Update is called once per frame
	void Update () {
		Destroy (this.gameObject, 3f);
		if (shouldMove) {
			rb.AddForce (transform.forward * speed, ForceMode.Impulse); // Move arrow forward for three seconds

		}
		else{
			rb.velocity = Vector3.zero;
		}
	}
}
