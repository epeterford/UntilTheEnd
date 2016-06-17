using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class uiDisplay : MonoBehaviour {
	public Image health;
	public static Image[] stamina = new Image[4];
	public Text potionCount;
	public Text dayCounter;
	Vector3 inputVec;
	public Animator dayCounterAnim;
	public TeleportScript teleport;
	//public Transform[] attackIcons;
	private bool offScreen = false;
	public Image healthOverlay;
	public Text target;
	public Color c;

	public static Image cursor; 
	Vector3 JoystickOffset = Vector2.zero;
	Vector3 mousePosition;
	Vector3 viewPos;
	public float staminaRefillTime;

	//ATTACK ICONS
	public Image[] activeWeapon = new Image[2];
	public Image[] currentInputIcons = new Image[4];
	public Image[] attackIcons;

	//SPECIAL COOLDOWN
	public Color swordSpecialCD;
	public Color bowSpecialCD;
	public Text specialCD;

	public DungeonMaster dm;
	public Spawner_Final waveCount;
	public static GameObject deathUI;
	public Text interactText;

	// Use this for initialization
	void Start () {
		dm = GameObject.Find ("DungeonMaster").GetComponent<DungeonMaster> ();
		teleport = GameObject.FindGameObjectWithTag ("nextLevel").GetComponent<TeleportScript>();
		interactText = GameObject.Find("InteractionText").GetComponent<Text>();
		deathUI = GameObject.Find ("DeathUI");
		target = GameObject.Find ("Target").GetComponent<Text> ();
		cursor = GameObject.Find ("Cursor").GetComponent<Image> ();
		potionCount = GameObject.Find ("PotionCount").GetComponent<Text> ();
		dayCounter = GameObject.Find ("DayCounter").transform.Find("DAY").GetComponent<Text> ();
		dayCounterAnim = GameObject.Find ("DayCounter").GetComponent<Animator> ();
		waveCount = GameObject.Find ("Spawner_Final").GetComponent<Spawner_Final> ();
		stamina [0] = GameObject.Find ("MainPanel").transform.Find("STAMINA").transform.Find ("SbBar").GetComponent<Image> ();
		stamina [1] = GameObject.Find ("MainPanel").transform.Find("STAMINA").transform.Find ("SbFull").GetComponent<Image> ();
		stamina [2] = GameObject.Find ("MainPanel").transform.Find("STAMINA").transform.Find ("Sb23").GetComponent<Image> ();
		stamina [3] = GameObject.Find ("MainPanel").transform.Find("STAMINA").transform.Find ("Sb13").GetComponent<Image> ();

		activeWeapon[0] = GameObject.Find ("AttackPanel").transform.Find ("SwordActive").GetComponent<Image> ();//Sword Active
		activeWeapon[1] = GameObject.Find ("AttackPanel").transform.Find ("BowActive").GetComponent<Image> (); //Bow Active

		currentInputIcons[0] = GameObject.Find ("AttackPanel").transform.Find ("LeftClick").GetComponent<Image> (); //Left Click
		currentInputIcons[1] = GameObject.Find ("AttackPanel").transform.Find ("RightClick").GetComponent<Image> (); //Right Click
		currentInputIcons[2] = GameObject.Find ("AttackPanel").transform.Find ("R1").GetComponent<Image> (); //R1
		currentInputIcons[3] = GameObject.Find ("AttackPanel").transform.Find ("R2").GetComponent<Image> (); //R2

		attackIcons[0] = GameObject.Find ("AttackPanel").transform.Find ("SwordAtk").GetComponent<Image> (); //Sword Atk
		attackIcons[1] = GameObject.Find ("AttackPanel").transform.Find ("SwordSpec").GetComponent<Image> (); //Sword Special
		attackIcons[2] = GameObject.Find ("AttackPanel").transform.Find ("BowAtk").GetComponent<Image> (); //Bow Atk
		attackIcons[3] = GameObject.Find ("AttackPanel").transform.Find ("BowSpec").GetComponent<Image> (); //Bow Special

		specialCD = GameObject.Find ("AttackPanel").transform.Find ("SpecialCD").GetComponent<Text>();
		swordSpecialCD = attackIcons [1].color;
		bowSpecialCD = attackIcons [3].color;
		specialCD.text = "";

		c = healthOverlay.color;

		deathUI.SetActive (false);
	}

	// Stamina refill
	IEnumerator RefillStamina(float time){

		float startTime = 0;
		while (startTime<time) 
		{
			startTime += Time.deltaTime;
			stamina[0].fillAmount = startTime/time;

			yield return null;
		}
	}

	// Special attack cooldown
	public IEnumerator SpecialCooldown(){
		float time = 1;
		float startTime = 6;
		while (startTime>time) 
		{
			startTime -= Time.deltaTime;
			specialCD.text = startTime/time + "";
			yield return null;
		}
		specialCD.text = "";
	}

	// Update player's current health
	public void UpdateHealth(float currentHealth, float maxHealth){
		health.fillAmount = currentHealth/maxHealth;
		
	}
	public void DestroyDayCounter(){
		Destroy (dayCounter);

	}
	// Update is called once per frame
	void Update () {
		/// If player is within certain distance of an object that can be interacted with
		if(interactions.distance<5)
		{
			interactText.text = "Interact";
		}else{
			interactText.text = "";

		}

		//ATTACK PANEL ICONS
		if (MainPlayerController.currentWeapon == 0) // If sword is currently active
		{
			/// Show sword sprites and hide bow sprites
			activeWeapon [0].enabled = true;
			activeWeapon [1].enabled = false;

			attackIcons [0].enabled = true;
			attackIcons [1].enabled = true;
			attackIcons [2].enabled = false;
			attackIcons [3].enabled = false;

		} 
		else // otherwise
		{
			/// Show bow sprites and hide sword sprites
			activeWeapon [0].enabled = false;
			activeWeapon [1].enabled = true;
			
			attackIcons [0].enabled = false;
			attackIcons [1].enabled = false;
			attackIcons [2].enabled = true;
			attackIcons [3].enabled = true;

		}

		if(MainPlayerController.usingController!=true) // If player is using a controller for input
		{
			/// Show controller sprites and hide keyboard/mouse sprites
			currentInputIcons [0].enabled = true;
			currentInputIcons [1].enabled = true;
			currentInputIcons [2].enabled = false;
			currentInputIcons [3].enabled = false;

		}
		else // otherwise
		{
			/// Show keyboard/mouse sprites and hide controller sprites
			currentInputIcons [0].enabled = false;
			currentInputIcons [1].enabled = false;
			currentInputIcons [2].enabled = true;
			currentInputIcons [3].enabled = true;
		}


		///STAMINA REFILL TIME
		if (PlayerPrefs.GetInt ("Stamina") == 1) 
		{
			staminaRefillTime = 10f;
		} 
		else if (PlayerPrefs.GetInt ("Stamina") == 2) 
		{
			staminaRefillTime = 7f;
		} 
		else if (PlayerPrefs.GetInt ("Stamina") == 3) 
		{
			staminaRefillTime = 5;
		} 
		else {
			staminaRefillTime = 12;
			
		}

		/// Display whether bow special attack is available to use
		if (MainPlayerController.canBowSpecial != true) 
		{
			bowSpecialCD.a = .4f;
			attackIcons[3].color = bowSpecialCD;
		}
		else 
		{
			bowSpecialCD.a = 1f;
			attackIcons[3].color = bowSpecialCD;
		}

		/// Display whether sowrd special attack is available to use
		if (MainPlayerController.canSwordSpecial != true) 
		{
			swordSpecialCD.a = .4f;
			attackIcons[1].color = swordSpecialCD;	
		}
		else 
		{
			swordSpecialCD.a = 1f;
			attackIcons[1].color = swordSpecialCD;
		}

		/// Update player's current objective 
		if (teleport.waveComplete) // If all waves have been completed
		{
			target.text = "Proceed to Next Day";
		}
		else if (waveCount.currentWave > 0) // otherwise, if player has reached wave system but has yet to complete all waves
		{
			// Display current wave
			target.text = "Complete Wave " + waveCount.currentWave;
		}
		else if ((dm.getTotalEnemies () - dm.getKillCount ()) <= 0) // otherwise, if required number of enemies have been killed in the level to move on to wave system
		{
			target.text = "Proceed to Teleporter";

		}
		else if ((dm.getTotalEnemies () - dm.getKillCount ()) > 0) // otherwise, player still needs to kill required number of enemies
		{
			target.text = "Target: " + (dm.getTotalEnemies () - dm.getKillCount ()).ToString ();
		}

		JoystickOffset = new Vector3(Input.GetAxis("JoystickX"), Input.GetAxis("JoystickY"), 0);

		//IS PLAYER USING MOUSE
		if ((Input.GetAxis("Mouse X")!=0 || Input.GetAxis("Mouse Y")!=0) && !offScreen)
		{ 

			cursor.transform.position = Input.mousePosition;

		} 
		else if(!offScreen)  //IS PLAYER USING JOYSTICK
		{
			cursor.transform.position += JoystickOffset * 7;// MOVE CURSOR WITH JOYSTICK
		}

		/// Keep player's mouse/cursor on screen
		if (cursor.transform.position.x > Screen.width) 
		{
			offScreen=true;
			cursor.transform.position = new Vector3 (cursor.transform.position.x - 5, cursor.transform.position.y, 0);

		} 
		else if (cursor.transform.position.x < 0) 
		{
			offScreen=true;
			cursor.transform.position = new Vector3 (cursor.transform.position.x + 5, cursor.transform.position.y, 0);
		} 
		else if (cursor.transform.position.y > Screen.height) 
		{
			offScreen=true;
			cursor.transform.position = new Vector3 (cursor.transform.position.x, cursor.transform.position.y - 5, 0);
			
		} 
		else if (cursor.transform.position.y < 0) 
		{
			offScreen=true;
			cursor.transform.position = new Vector3 (cursor.transform.position.x, cursor.transform.position.y + 5, 0);
		} 
		else 
		{
			offScreen = false;
		}

		// Display current Day(level) the player is on
		if (dayCounter != null) {

			dayCounter.text = "Day " + PlayerPrefs.GetInt ("level").ToString ();
		}

		//POTION
		potionCount.text = "x" + PlayerPrefs.GetInt ("potions");

		/// HEALTH SPRITES - update what sprites are visible depending on amount of Health
		if (health.fillAmount < .51f && health.fillAmount > .25f) 
		{
			c.a = 128;
			healthOverlay.color=c;
		} 
		else if (health.fillAmount < .26f) 
		{
			c.a = 255;
			healthOverlay.color=c;
		} 
		else
		{
			c.a = 0;
			healthOverlay.color=c;
		}

		// STAMINA REFILL
		if (stamina[0].fillAmount ==0) 
		{
			StartCoroutine("RefillStamina" , staminaRefillTime);
		}

		/// STAMINA SPRITES - update what sprites are visible depending on amount of Stamina 
		if (stamina [0].fillAmount == 1) 
		{

			stamina [1].enabled = true;
			stamina [2].enabled = false;
			stamina [3].enabled = false;

		}
		else if (stamina [0].fillAmount < 1 && stamina [0].fillAmount > .65f) 
		{
			
			stamina [1].enabled = false;
			stamina [2].enabled = true;
			stamina [3].enabled = false;
			
		} 
		else if (stamina [0].fillAmount >.32f && stamina [0].fillAmount < .66f) 
		{
			
			stamina [1].enabled = false;
			stamina [2].enabled = false;
			stamina [3].enabled = true;
			
		} 
		else 
		{
			stamina [1].enabled = false;
			stamina [2].enabled = false;
			stamina [3].enabled = false;

		}
	}
}
