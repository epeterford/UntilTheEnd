using UnityEngine;
using System.Collections;

public class MainPlayerController : MonoBehaviour {
	
	public Animator animator;
	public Transform[] weapons;
	public static int currentWeapon;
	float rotationSpeed = 30;
	Vector3 inputVec;
	public float distance = 10;
	bool isMoving;
	public static Vector3 position;
	ShootArrow shootArrow;
	ShootMultiArrow shootMultiArrow;
	public static Vector3 lookTarget;
	public bool attacking = false;
	public Transform cursorResponse;
	public static Vector3 mousePosition;
	public Vector3 cursorPos;
	private CharacterController controller;
	private Vector3 velocity = new Vector3(0,0,-200);
	private StatsSheet_Player stats;
	public static bool completedLevel = false;
	private CharacterController cc;
	public float knockbackTime;
	private Rigidbody rb;
	public static bool canBowSpecial;
	public static bool canSwordSpecial;
	public StatsSheet_Player getStats(){ return stats; }
	public uiDisplay ui;
	public static bool isDead = false;
	public ParticleSystem dust;
	public float dashDist = 20f;
	public float dashTime = 0.3f;
	private float dashTimer = 0f;
	private bool dashing = false;
	public static bool usingController = false;
	public float swordAttackRoF;
	public float bowAttackRoF;
	public static bool bowSpecialUnlocked;
	public static bool swordSpecialunlocked;
	public audioPlay soundFX;
	public bool concealed;

	public SphereCollider swordAoE;

	void Start()
	{
		/// Initializations
		soundFX = GetComponent<audioPlay>();
		swordAoE = GameObject.Find ("SwordColl").GetComponent<SphereCollider> ();
		cameraShake.shake = 0;
		isDead = false;
		stats = GameObject.Find ("DungeonMaster").GetComponent<DungeonMaster> ().playerStats;
		ui = GameObject.FindGameObjectWithTag ("UI").GetComponent<uiDisplay> ();
		shootArrow = gameObject.GetComponentInChildren<ShootArrow> ();
		shootMultiArrow = gameObject.GetComponentInChildren<ShootMultiArrow> ();
		changeWeapon (0);
		controller = this.gameObject.GetComponent<CharacterController> ();
		ReceiverDamage receiver = GetComponent<ReceiverDamage> ();

		if (receiver == null) {
			print ("RECEIVER IS NULL");
		} else {
			receiver.setStats(stats);
		}

		SenderDamage[] sender = GetComponentsInChildren<SenderDamage> ();
		if (sender != null) {
			foreach(SenderDamage sd in sender){
				Debug.Log (sd.gameObject.name);
				sd.setStats(stats);
			}
		}
		ui.UpdateHealth(stats.getCurHealth(), stats.getMaxHealth());
		cc = gameObject.GetComponent<CharacterController> ();
		rb = gameObject.GetComponent<Rigidbody> ();

		concealed = false;
	}

	// Fires Arrow
	public void CallShootArrow()
	{
		shootArrow.FireArrow ();
	}

	// Fires Multi Arrrows
	public void CallShootMultiArrow()
	{
		shootMultiArrow.FireArrow ();
	}

	// Weapon Special Cooldowns
	IEnumerator CoolDown(string weapon)
	{
		StartCoroutine(GameObject.Find("_MainHUDCanvas").GetComponent<uiDisplay>().SpecialCooldown());

		if (weapon == "sword") {
			canSwordSpecial=false;
			yield return new WaitForSeconds(5f);
			canSwordSpecial=true;
		} 
		else if (weapon == "bow") 
		{
			canBowSpecial=false;
			yield return new WaitForSeconds(5f);
			canBowSpecial=true;
		}
	}

	// On Player Collisions
	void OnTriggerEnter(Collider other) 
	{
		/// Pick Up potion
		if (other.gameObject.tag == "potion")
		{
			soundFX.MedpackPickup();
			Destroy(other.gameObject);
			PlayerPrefs.SetInt("potions", PlayerPrefs.GetInt("potions")+1);
		}

		/// Proceed to next level
		if (other.gameObject.tag == "nextLevel") 
		{
			if(other.gameObject.GetComponent<TeleportScript>().waveComplete){
				//PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level")+1);
				completedLevel=true;
				levelChanger.fadeToLevel=true;

			}
		}
	}

	// On Death
	public void Death()
	{
		GameObject dm = GameObject.Find ("DungeonMaster");

		if(dm!=null){
			/// Play death animation
			cameraShake.shake = 3f;
			dm.GetComponent<DungeonMaster>().handleOnDeathPlayer();
			bringUp.playerAssigned=false;
			PlayerPrefs.SetInt("minDunLength",0);

			// Fade to end game level
			levelChanger.fadeToEndGame=true;
		}

	}
	void Update() // every frame
	{
		if (!swordAoE) 
		{
			swordAoE = GameObject.Find ("SwordColl").GetComponent<SphereCollider> ();
		}

		/// CHECK IF BOW SPECIAL AND SWORD SPECIAL IS UNLOCKED
		if (bowSpecialUnlocked != true) {

			canBowSpecial = false;
		}
		if (swordSpecialunlocked != true) {

			canSwordSpecial = false;

		}

		/// SWORD SPECIAL AOE RADIUS
		if (PlayerPrefs.GetInt ("AoE") == 2) {
			
			swordAoE.radius = 45;
			
		} else if (PlayerPrefs.GetInt ("AoE") == 3) {
			
			swordAoE.radius = 60;
		}
		else{
			swordAoE.radius=30;
			
		}

		/// SWORD ROF
		if (PlayerPrefs.GetInt ("SwordRoF") == 1) {

			swordAttackRoF = 1.25f;
		} else if (PlayerPrefs.GetInt ("SwordRoF") == 2) {

			swordAttackRoF = 1.5f;
		} else if (PlayerPrefs.GetInt ("SwordRoF") == 3) {
			
			swordAttackRoF = 2;
		} else {
			swordAttackRoF = 1;

		}

		/// BOW ROF
		if (PlayerPrefs.GetInt ("BowRoF") == 1) {
			
			bowAttackRoF = 1.5f;
		} else if (PlayerPrefs.GetInt ("BowRoF") == 2) {
			
			bowAttackRoF = 1.75f;
		} else if (PlayerPrefs.GetInt ("BowRoF") == 3) {
			
			bowAttackRoF = 2;
		} else {
			bowAttackRoF = 1.25f;
			
		}
		/// PLAYER AGILITY
		if (PlayerPrefs.GetInt ("Agility") == 1 && animator.GetBool("Moving") && animator.GetBool("Running") && animator.GetBool("attacking")!=true) {
			
			animator.speed = 1.25f;
		} else if (PlayerPrefs.GetInt ("Agility") == 2 && animator.GetBool("Moving") && animator.GetBool("Running")&& animator.GetBool("attacking")!=true) {
			
			animator.speed = 1.5f;
		} else if (PlayerPrefs.GetInt ("Agility") == 3 && animator.GetBool("Moving") && animator.GetBool("Running") && animator.GetBool("attacking")!=true) {
			animator.speed = 2;
		} else if(animator.GetBool("attacking")!=true) {
			animator.speed = 1;
			
		}

		/// *** DASH ***
		if (Input.GetButtonDown ("Dash") && uiDisplay.stamina[0].fillAmount>=1 && !isDead) // If dash pressed and player has enough stamina 
		{
			if(!dashing){
				soundFX.Dash();

				// Reduce stamina
				uiDisplay.stamina[0].fillAmount -=1;

				dashing = true;
				dust.enableEmission = true;
				dust.Play();
			}
		}

		/// Dashing particle and length
		if (dashing) 
		{
			dashTimer += Time.deltaTime;
			if(dashTimer < dashTime && Time.deltaTime < 1f)
			{
				Dash(dashDist*Time.deltaTime/dashTime);
			}
			else if(Time.deltaTime>1f)
			{
				Dash(dashDist);
				dashTimer = 0;
				dashing = false;
				dust.enableEmission = false;
			}
			else
			{
				Dash(dashDist*Time.deltaTime/dashTime);
				dashTimer = 0;
				dashing = false;
				dust.enableEmission = false;
			}

		}

		/// *** USE POTION ***
		if (Input.GetButtonDown ("UsePotion") && !isDead) 
		{
			if (PlayerPrefs.GetInt ("potions") > 0 && stats.getCurHealth()!=stats.getMaxHealth())  // If player has a potion to use and health is not full
			{
				soundFX.MedpackUse();
				PlayerPrefs.SetInt("potions", PlayerPrefs.GetInt("potions")-1);
				stats.setCurHealth (stats.getCurHealth()+(stats.getMaxHealth()/4));
				ui.UpdateHealth(stats.getCurHealth(), stats.getMaxHealth());
			}
		}

		// If player's health falls <=0
		if (stats.getCurHealth () <= 0 && isDead==false) 
		{
			isDead=true;
			animator.SetTrigger("Death");
			uiDisplay.deathUI.SetActive(true);
		}

		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (uiDisplay.cursor.transform.position);
		if (Physics.Raycast (ray, out hit)) 
		{
			
			lookTarget = hit.point;
		}

		//Get input from controls
		float z = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("JoystickMoveY");
		float x = (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("JoystickMoveX"));
		inputVec = new Vector3(x, 0, z);
		
		//Apply inputs to animator
		animator.SetFloat("Input X", x);
		animator.SetFloat("Input Z", z);

		// If player is inputing movement through controller
		if (Input.GetAxisRaw ("JoystickMoveX") != 0 || Input.GetAxisRaw ("JoystickMoveY") != 0) 
		{
			usingController = true;
		} 
		// Otherwise, if player is inputting movement through keyboard
		else if(Input.GetAxisRaw("Horizontal") !=0 || Input.GetAxisRaw("Vertical") !=0)
		{
			usingController=false;

		}

		if ((x != 0 || z != 0) && !attacking && !isDead && animator.GetBool("attacking")!=true)  // If player is moving
		{
			//set that character is moving
			animator.SetBool("Moving", true);
			animator.SetBool("Running", true);
			isMoving = true;
		}
		else
		{
			//character is not moving
			animator.SetBool("Moving", false);
			animator.SetBool("Running", false);
			isMoving = false;
		}

		/// *** NORMAL ATTACK ***
		if (Input.GetButtonDown("Fire1") && !attacking && !isDead && gameManager.showUpgradeMenu!=true && animator.GetBool("attacking")!=true) // if player uses normal attack
		{ 
			uiDisplay UI = GameObject.Find ("_MainHUDCanvas").GetComponent<uiDisplay>();

			// Face player towards mouse position
			arrow.arrowTarget = lookTarget;
			mousePosition = lookTarget;

			// Spawn particle effect on mouse location at the time of attack
			Instantiate(cursorResponse, lookTarget, Quaternion.identity);
			cursorPos = cursorResponse.position;
			lookTarget.y = this.transform.position.y;

			//Play attack animation
			if(currentWeapon==0) // if sword is active
			{
				// Play sword attack animation
				animator.SetTrigger("Attack1Trigger");
			}
			else // otherwise
			{
				// Face player towards mouse and play Bow attack animation
				transform.LookAt(lookTarget);
				animator.SetTrigger("BowAttackTrigger");
			}


		}

		/// *** SPECIAL ATTACK ***
		if (Input.GetButtonDown ("Fire2") && !attacking && !isDead && gameManager.showUpgradeMenu!=true && animator.GetBool("attacking")!=true) // if player uses special attack
		{
			/// Play attack animation
			if(currentWeapon==0 && canSwordSpecial) // if sword is active and sword special attack is available
			{
				animator.SetBool("attacking", true);
				arrow.arrowTarget = lookTarget;
				
				mousePosition = new Vector3(Input.mousePosition.x, this.transform.position.y, Input.mousePosition.z);
				
				//Attack Cooldown
				uiDisplay UI = GameObject.Find ("_MainHUDCanvas").GetComponent<uiDisplay>();
				
				
				
				//Face player towards mouse position
				Instantiate(cursorResponse, lookTarget, Quaternion.identity);
				cursorPos = cursorResponse.position;
				lookTarget.y = this.transform.position.y;
				transform.LookAt(lookTarget);

				// Play sword special animation
				animator.SetTrigger("SwordSpecial");

				// Start sword special cooldown
				StartCoroutine(CoolDown("sword"));

			}
			else if(currentWeapon==1 && canBowSpecial) // otherwise, if bow is active and bow special attack is available
			{
				animator.SetBool("attacking", true);
				arrow.arrowTarget = lookTarget;
				
				mousePosition = new Vector3(Input.mousePosition.x, this.transform.position.y, Input.mousePosition.z);
				
				//Attack Cooldown
				uiDisplay UI = GameObject.Find ("_MainHUDCanvas").GetComponent<uiDisplay>();

				//Face player towards mouse position
				Instantiate(cursorResponse, lookTarget, Quaternion.identity);
				cursorPos = cursorResponse.position;
				lookTarget.y = this.transform.position.y;
				transform.LookAt(lookTarget);

				// Play bow special animation
				animator.SetTrigger("BowMultiAttackTrigger");

				// Start bow special cooldown
				StartCoroutine(CoolDown("bow"));
			}
		}

		/// *** WEAPON SWAP ***
		if (Input.GetButtonDown ("WeaponSwap") && !attacking && !isDead && gameManager.showUpgradeMenu!=true)
		{
			if(currentWeapon==0)
			{
				changeWeapon(1);
			}
			else
			{
				changeWeapon(0);
			}
		}

		// If player not attacking, update movement
		if (Input.GetButtonDown ("Fire1") == false) 
		{
			UpdateMovement ();  
		}
	}

	/// Get current attack speed depending on RoF upgrades
	public void GetAttackSpeed(int weapon) 
	{
		if (weapon == 0) 
		{
			Debug.Log ("SwordAttackRoF: " + swordAttackRoF);
			animator.SetBool ("attacking", true);
			attacking = true;
			animator.speed = swordAttackRoF;
		} 
		else if (weapon == 1)
		{
			Debug.Log ("BowAttackRoF: " + bowAttackRoF);
			animator.SetBool ("attacking", true);
			attacking = true;
			animator.speed = bowAttackRoF;

		} 
		else 
		{

			animator.SetBool ("attacking", true);
			attacking = true;
		}


	}

	public void ResetAnimatorSpeed()
	{
		animator.SetBool ("attacking", false);
		attacking = false;
		animator.speed = 1;

	}

	/// Switch active weapon
	public void changeWeapon (int num) 
	{
		currentWeapon = num;

		for (int i = 0; i<weapons.Length; i++) {

			if(i==num)
			{
				weapons[i].gameObject.SetActive(true);
			}
			else
			{
				weapons[i].gameObject.SetActive(false);
			}
		}

	}

	/// Moves the player forward in the direction they are facing
	 void Dash(float dashAmount) 
	{

		Vector3 forward = new Vector3();
		
		forward = transform.TransformDirection (Vector3.forward);
		float curSpeed = 20 * dashAmount;
		controller.SimpleMove(forward * curSpeed);

	}

	/// Face character along input direction
	void RotateTowardsMovementDir()  
	{
		if (inputVec != Vector3.zero && attacking==false && !isDead && animator.GetBool("attacking")!=true)
		{
		
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}
	
	float UpdateMovement()
	{
		Vector3 motion = inputVec;  //get movement input from controls
		
		//reduce input for diagonal movement
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
		
		RotateTowardsMovementDir();  //if not strafing, face character along input direction
		
		return inputVec.magnitude;  //return a movement value for the animator, not currently used
	}
}