using UnityEngine;
using System.Collections;

public class ShootArrow : MonoBehaviour {
	public GameObject arrow;
	// Use this for initialization
	void Start () {
	}

	/// <summary>
	/// Called when Normal Bow attack is used. Spawns one arrow 
	/// </summary>
	public void FireArrow(){
		/// Spawn arrow and assign it damage
		GameObject a = Instantiate (arrow, transform.position, transform.rotation) as GameObject;
		SenderDamage sender = a.GetComponent<SenderDamage> ();

		if (sender == null) {
			print ("SENDER IS NULL");
		} else {
			sender.setStats(GetComponentInParent<MainPlayerController>().getStats ());
		}
	}
	// Update is called once per frame
	void Update () {
	}
}
