using UnityEngine;
using System.Collections;

public class ShootMultiArrow : MonoBehaviour {
	public GameObject arrow;
	// Use this for initialization
	void Start () {
	}

	/// <summary>
	/// Called when Special Bow attack is used.  Spawns X number of arrows depending on player's current upgrade progress for MultiShot skill
	/// </summary>
	/// <returns>The arrows.</returns>
	IEnumerator FireArrows()
	{
		// Fire first arrow and assign it damage
		GameObject a = Instantiate (arrow, transform.position, transform.rotation) as GameObject;
		SenderDamage sender1 = a.GetComponent<SenderDamage> ();

		if (sender1 == null) {
			print ("SENDER IS NULL");
		} else {
			sender1.setStats(GetComponentInParent<MainPlayerController>().getStats ());
		}

		// If player has at least first upgrade
		if (PlayerPrefs.GetInt ("MultiShot") >= 1) {
			/// Fire second arrow and assign it damage
			yield return new WaitForSeconds (.3f);
			GameObject b = Instantiate (arrow, transform.position, transform.rotation) as GameObject;
			SenderDamage sender2 = b.GetComponent<SenderDamage> ();

			if (sender2 == null) {
				print ("SENDER IS NULL");
			} else {
				sender2.setStats (GetComponentInParent<MainPlayerController> ().getStats ());
			}

			// If player has at least the second upgrade
			if(PlayerPrefs.GetInt("MultiShot")>=2){

				/// Fire third arrow and assign it damage
				yield return new WaitForSeconds (.3f);
				GameObject c = Instantiate (arrow, transform.position, transform.rotation) as GameObject;
				SenderDamage sender3 = c.GetComponent<SenderDamage> ();
				if (sender3 == null) {
					print ("SENDER IS NULL");
				} else {
					sender3.setStats (GetComponentInParent<MainPlayerController> ().getStats ());
				}

				// If player has the third upgrade
				if(PlayerPrefs.GetInt("MultiShot")>=3){

					/// Fire fourth arrow and assign it damage
					yield return new WaitForSeconds (.3f);
					GameObject d = Instantiate (arrow, transform.position, transform.rotation) as GameObject;
					SenderDamage sender4 = d.GetComponent<SenderDamage> ();
					if (sender4 == null) {
						print ("SENDER IS NULL");
					} else {
						sender4.setStats (GetComponentInParent<MainPlayerController> ().getStats ());
					}
				}
				else{
					yield return 0;
				}
			}
			else{

				yield return 0;
			}
		} else {

			yield return 0;
		}

	}

	public void FireArrow()
	{
		StartCoroutine ("FireArrows");
	}
	// Update is called once per frame
	void Update () {
	}
}
