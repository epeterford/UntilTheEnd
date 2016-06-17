using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class upgradeMenu : MonoBehaviour {
	private bool isPaused=false;
	public Animator bowPanelAnim;
	public Animator swordPanelAnim;
	public Animator playerPanelAnim;
	Color fullUpgrade = new Color (0, 255, 97, 255);
	Color inProgress = new Color (255, 255, 255, 255);
	private Image bowUpgrades;
	private Image swordUpgrades;
	private Image playerUpgrades;
	private Button swordDamageBtn;
	private Button swordRoFBtn;
	private Button swordSpecialBtn;

	private Button bowDamageBtn;
	private Button bowRoFBtn;
	private Button bowSpecialBtn;

	private Button healthBtn;
	private Button staminaBtn;
	private Button agilityBtn;

	private AudioSource upgradeSound;
	private AudioSource fullUpgradeSound;
	private Text currentPoints;

	///UPGRADE PROGRESS
		//BOW
	private Text multiShotProgress;
	private Text bowDamageProgress;
	private Text bowRoFProgress;

		//PLAYER
	private Text healthProgress;
	private Text staminaProgress;
	private Text agilityProgress;

		//SWORD
	private Text swordDamageProgress;
	private Text swordRoFProgress;
	private Text aoeProgress;

	//UPGRADE DESCRIPTION
	private Text nextLevelDescription;
	private Text upgradeCost; 
	private Text upgradeDescription;
	public GameObject descriptionBG;
	public static StatsSheet_Player stats;

	///UPGRADE COSTS
	private int swordDamageCost;
	private int swordRofCost;
	private int swordSpecialCost;

	private int healthCost;
	private int staminaCost;
	private int agilityCost;

	private int bowDamageCost;
	private int bowRoFCost;
	private int bowSpecialCost; 
	public EventSystem eventSystem;

	// Use this for initialization
	void Start () 
	{
		if (gameManager.showUpgradeMenu) 
		{
			eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
			stats = GameObject.Find ("DungeonMaster").GetComponent<DungeonMaster> ().playerStats;
			upgradeSound = GameObject.Find ("UpgradeAudio").GetComponent<AudioSource> ();
			fullUpgradeSound = GameObject.Find ("FullUpgradeAudio").GetComponent<AudioSource> ();
			swordDamageBtn = GameObject.Find ("SwordDamage").GetComponent<Button> ();
			swordRoFBtn = GameObject.Find ("SwordRoF").GetComponent<Button> ();
			swordSpecialBtn = GameObject.Find ("AoE").GetComponent<Button> ();

			bowDamageBtn = GameObject.Find ("BowDamage").GetComponent<Button> ();
			bowRoFBtn = GameObject.Find ("BowRoF").GetComponent<Button> ();
			bowSpecialBtn = GameObject.Find ("MultiShot").GetComponent<Button> ();

			healthBtn = GameObject.Find ("Health").GetComponent<Button> ();
			staminaBtn = GameObject.Find ("Stamina").GetComponent<Button> ();
			agilityBtn = GameObject.Find ("Agility").GetComponent<Button> ();

			bowUpgrades = GameObject.Find ("BowUpgrades").GetComponent<Image> ();
			swordUpgrades = GameObject.Find ("SwordUpgrades").GetComponent<Image> ();
			playerUpgrades = GameObject.Find ("PlayerUpgrades").GetComponent<Image> ();
			descriptionBG = GameObject.Find ("DescriptionBG");
			descriptionBG.SetActive (false);
			upgradeDescription = GameObject.Find ("UpgradeDescription").GetComponent<Text> ();
			nextLevelDescription = GameObject.Find ("NextLevelDescription").GetComponent<Text> ();
			upgradeCost = GameObject.Find ("UpgradeCost").GetComponent<Text> ();
			currentPoints = GameObject.Find("PlayerSkills").transform.Find("CurrentPoints").GetComponent<Text>();

			//BOW
			multiShotProgress = GameObject.Find ("MultiShotProgress").GetComponent<Text> ();
			bowDamageProgress = GameObject.Find ("BowDamageProgress").GetComponent<Text> ();
			bowRoFProgress = GameObject.Find ("BowRoFProgress").GetComponent<Text> ();

			//PLAYER
			healthProgress = GameObject.Find ("HealthProgress").GetComponent<Text> ();
			staminaProgress = GameObject.Find ("StaminaProgress").GetComponent<Text> ();
			agilityProgress = GameObject.Find ("AgilityProgress").GetComponent<Text> ();

			//SWORD
			swordDamageProgress = GameObject.Find ("SwordDamageProgress").GetComponent<Text> ();
			swordRoFProgress = GameObject.Find ("SwordRoFProgress").GetComponent<Text> ();
			aoeProgress = GameObject.Find ("AoEProgress").GetComponent<Text> ();
		}

	}
	void Awake(){
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

	}
	public void OnEnable()
	{
		GameObject swordDamageStart;
		swordDamageStart = GameObject.Find("SwordDamage");
		if(gameManager.showUpgradeMenu){
			eventSystem.SetSelectedGameObject(swordDamageStart); 
		}
	}

	/// <summary>
	/// When Hovering over an upgrade panel
	/// Scale panel up
	/// </summary>
	/// <param name="panel">Panel.</param>
	public void overPanel(string panel)
	{
		if (panel == "bow") 
		{
			bowPanelAnim.GetComponent<Animator> ().SetTrigger ("scaleUp");

		}
		else if (panel == "player") 
		{
			playerPanelAnim.GetComponent<Animator> ().SetTrigger ("scaleUp");
		}
		else if (panel == "sword") 
		{
			swordPanelAnim.GetComponent<Animator> ().SetTrigger ("scaleUp");
		}

	}
	/// <summary>
	/// When Exiting Hover on an upgrade panel
	/// Scale panel down
	/// </summary>
	/// <param name="panel">Panel.</param>
	public void exitPanel(string panel){
		
		if (panel == "bow") 
		{
			bowPanelAnim.GetComponent<Animator> ().SetTrigger ("scaleDown");
		} 
		else if (panel == "player")
		{
			playerPanelAnim.GetComponent<Animator> ().SetTrigger ("scaleDown");
		}
		else if (panel == "sword") 
		{
			swordPanelAnim.GetComponent<Animator> ().SetTrigger ("scaleDown");
		}
	}
	/// <summary>
	/// Displays description for each upgrade to player
	/// </summary>
	/// <param name="upgrade">Upgrade.</param>
	public void GetDescription(string upgrade){

		descriptionBG.SetActive (true);

		/// BOW
		if (upgrade == "multiShot") {
			if (PlayerPrefs.GetInt ("MultiShot") ==0) 
			{
				upgradeDescription.color = Color.grey;
				upgradeDescription.text = "MULTI-SHOT: Fires One Additional Arrow.";
				nextLevelDescription.text = "Unlock Attack";
				upgradeCost.text = "Cost: " + bowSpecialCost;
			}
			else if (PlayerPrefs.GetInt ("MultiShot") ==1)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "MULTI-SHOT: Fires One Additional Arrow";
				nextLevelDescription.text = "Next Level: Fires two additional arrows.";
				upgradeCost.text = "Cost: " + bowSpecialCost;
			}
			else if (PlayerPrefs.GetInt ("MultiShot") ==2)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "MULTI-SHOT: Fires Two Additional Arrows";
				nextLevelDescription.text = "Next Level: Fires three additional arrows.";
				upgradeCost.text = "Cost: " + bowSpecialCost;
			}
			else if (PlayerPrefs.GetInt ("MultiShot") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "MULTI-SHOT: Fires Three Additional Arrows";
				upgradeCost.text = " ";
				nextLevelDescription.text = " ";
			}
		}
		if (upgrade == "bowDamage") 
		{
			if (PlayerPrefs.GetInt ("BowDamage") ==0) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Bow Damage";
				nextLevelDescription.text = "Next Level: Bow Damage +10%";
				upgradeCost.text = "Cost: " + bowDamageCost;
			}
			else if (PlayerPrefs.GetInt ("BowDamage") ==1)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Bow Damage [+10%]";
				nextLevelDescription.text = "Next Level: Bow Damage +20%";
				upgradeCost.text = "Cost: " + bowDamageCost;
			}
			else if (PlayerPrefs.GetInt ("BowDamage") ==2) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Bow Damage [+20%]";
				nextLevelDescription.text = "Next Level: Bow Damage +30%";
				upgradeCost.text = "Cost: " + bowDamageCost;
			}
			else if (PlayerPrefs.GetInt ("BowDamage") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Bow Damage [+30%]";
				upgradeCost.text = " ";
				nextLevelDescription.text = " ";
			}
		}
		if (upgrade == "bowRoF") 
		{
			if (PlayerPrefs.GetInt ("BowRoF") ==0) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Bow Rate of Fire";
				nextLevelDescription.text = "Next Level: Bow RoF +10%";
				upgradeCost.text = "Cost: " + bowRoFCost;
			}
			else if (PlayerPrefs.GetInt ("BowRoF") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Bow Rate of Fire [+10%]";
				nextLevelDescription.text = "Next Level: Bow RoF +20%";
				upgradeCost.text = "Cost: " + bowRoFCost;
			}else if (PlayerPrefs.GetInt ("BowRoF") ==2) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Bow Rate of Fire [+20%]";
				nextLevelDescription.text = "Next Level: Bow RoF +30%";
				upgradeCost.text = "Cost: " + bowRoFCost;
			}
			else if (PlayerPrefs.GetInt ("BowRoF") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Bow Rate of Fire [+30%]";
				nextLevelDescription.text = " ";
				upgradeCost.text = " ";
			}
		}

		/// PLAYER
		if (upgrade == "health") 
		{
			if (PlayerPrefs.GetInt ("Health") ==0) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Maximum Health [100%]";
				nextLevelDescription.text = "Next Level: Increase Max Health to 110%";
				upgradeCost.text = "Cost: " + healthCost;
			}
			else if (PlayerPrefs.GetInt ("Health") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Maximum Health [110%]";
				nextLevelDescription.text = "Next Level: Increase Max Health to 120%";
				upgradeCost.text = "Cost: " + healthCost;
			}
			else if (PlayerPrefs.GetInt ("Health") ==2) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Maximum Health [120%]";
				nextLevelDescription.text = "Next Level: Increase Max Health to 130%";
				upgradeCost.text = "Cost: " + healthCost;
			}
			else if (PlayerPrefs.GetInt ("Health") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Maximum Health [130%]";
				upgradeCost.text = " ";
				nextLevelDescription.text = " ";
			}
		}
		if (upgrade == "stamina") 
		{
			if (PlayerPrefs.GetInt ("Stamina") ==0)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Stamina Refills In 12 seconds.";
				nextLevelDescription.text = "Next Level: Stamina Refills in 10 seconds.";
				upgradeCost.text = "Cost: " + staminaCost;
			}
			else if (PlayerPrefs.GetInt ("Stamina") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Stamina Refills In 10 seconds.";
				nextLevelDescription.text = "Next Level: Stamina Refills In 7 seconds.";
				upgradeCost.text = "Cost: " + staminaCost;
			}
			else if (PlayerPrefs.GetInt ("Stamina") ==2)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Stamina Refills In 7 seconds.";
				nextLevelDescription.text = "Next Level: Stamina Refills In 5 seconds.";
				upgradeCost.text = "Cost: " + staminaCost;
			}
			else if (PlayerPrefs.GetInt ("Stamina") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Stamina Refills In 5 seconds.";
				upgradeCost.text = " ";
				nextLevelDescription.text = " ";
			}
		}
		if (upgrade == "agility")
		{
			if (PlayerPrefs.GetInt ("Agility") ==0) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Movement Speed";
				nextLevelDescription.text = "Next Level: Movement Speed +10%";
				upgradeCost.text = "Cost: " + agilityCost;
			}
			else if (PlayerPrefs.GetInt ("Agility") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Movement Speed [+10%]";
				nextLevelDescription.text = "Next Level: Movement Speed +20%";
				upgradeCost.text = "Cost: " + agilityCost;
			}
			else if (PlayerPrefs.GetInt ("Agility") ==2) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Movement Speed [+20%]";
				nextLevelDescription.text = "Next Level: Movement Speed +30%";
				upgradeCost.text = "Cost: " + agilityCost;;
			}
			else if (PlayerPrefs.GetInt ("Agility") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Movement Speed [+30%]";
				nextLevelDescription.text = " ";
				upgradeCost.text = " ";
			}
		}

		/// SWORD
		if (upgrade == "swordDamage") 
		{
			if (PlayerPrefs.GetInt ("SwordDamage") ==0) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Sword Damage";
				nextLevelDescription.text = "Next Level: Sword Damage +10%";
				upgradeCost.text = "Cost: " + swordDamageCost;
			}
			else if (PlayerPrefs.GetInt ("SwordDamage") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Sword Damage [+10%]";
				nextLevelDescription.text = "Next Level: Sword Damage +20%";
				upgradeCost.text = "Cost: " + swordDamageCost;
			}
			else if (PlayerPrefs.GetInt ("SwordDamage") ==2) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Sword Damage [+20%]";
				nextLevelDescription.text = "Next Level: Sword Damage +30%";
				upgradeCost.text = "Cost: " + swordDamageCost;
			}
			else if (PlayerPrefs.GetInt ("SwordDamage") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Sword Damage [+30%]";
				nextLevelDescription.text = " ";
				upgradeCost.text = " ";
			}
		}
		if (upgrade == "swordRoF") 
		{
			if (PlayerPrefs.GetInt ("SwordRoF") ==0) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Sword Rate of Fire";
				nextLevelDescription.text = "Next Level: Sword RoF +10%";
				upgradeCost.text = "Cost: " + swordRofCost;
			}
			else if (PlayerPrefs.GetInt ("SwordRoF") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Sword Rate of Fire [+10%]";
				nextLevelDescription.text = "Next Level: Sword RoF +20%";
				upgradeCost.text = "Cost: " + swordRofCost;
			}
			else if (PlayerPrefs.GetInt ("SwordRoF") ==2)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "Sword Rate of Fire [+20%]";
				nextLevelDescription.text = "Next Level: Sword RoF +30%";
				upgradeCost.text = "Cost: " + swordRofCost;;
			}
			else if (PlayerPrefs.GetInt ("SwordRoF") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "Sword Rate of Fire [+30%]";
				nextLevelDescription.text = " ";
				upgradeCost.text = " ";
			}
		}
		if (upgrade == "aoe") 
		{
			if (PlayerPrefs.GetInt ("AoE") ==0) 
			{
				upgradeDescription.color = Color.gray;
				upgradeDescription.text = "SPIN ATTACK: Do a whirling spin attack to hit multiple enemies!";
				nextLevelDescription.text = "Unlock Attack";
				upgradeCost.text = "Cost: " + swordSpecialCost;
			}
			else if (PlayerPrefs.GetInt ("AoE") ==1) 
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "SPIN ATTACK: Do a whirling spin attack to hit multiple enemies!";
				nextLevelDescription.text = "Next Level: Spin Attack radius +50%";
				upgradeCost.text = "Cost: " + swordSpecialCost;
			}
			else if (PlayerPrefs.GetInt ("AoE") ==2)
			{
				upgradeDescription.color = Color.white;
				upgradeDescription.text = "SPIN ATTACK: Do a whirling spin attack to hit multiple enemies!\n\n[Radius +50%]";
				nextLevelDescription.text = "Next Level: Spin Attack radius +100%";
				upgradeCost.text = "Cost: " + swordSpecialCost;
			}
			else if (PlayerPrefs.GetInt ("AoE") ==3) 
			{
				upgradeDescription.color = Color.cyan;
				upgradeDescription.text = "SPIN ATTACK: Do a whirling spin attack to hit multiple enemies!\n\n[Radius +100%]";
				nextLevelDescription.text = " ";
				upgradeCost.text = " ";
			}
		}
	}

	/// <summary>
	/// Removes Upgrade Description
	/// </summary>
	public void ExitDescription()
	{
		descriptionBG.SetActive (false);
		upgradeDescription.text = " ";
		upgradeCost.text = " ";
		nextLevelDescription.text = " ";

	}

	/// <summary>
	/// Upgrades specified skill
	/// Checks if player can afford upgrade,
	/// 	If so, unlock upgrade and update accordingly 
	/// 	Plays sound on each upgrade
	/// </summary>
	/// <param name="upgrade">Upgrade.</param>
	public void Upgrade(string upgrade)
	{
		/// BOW
		if (upgrade == "multiShot") // Multishot
		{
			if(PlayerPrefs.GetInt("CurrentPoints")>bowSpecialCost)
			{
				PlayerPrefs.SetInt("MultiShot", PlayerPrefs.GetInt("MultiShot")+1);
				MainPlayerController.bowSpecialUnlocked = true;
				MainPlayerController.canBowSpecial = true;

				if(PlayerPrefs.GetInt("MultiShot")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowSpecialCost);
					bowSpecialCost=2000;
				}
				else if(PlayerPrefs.GetInt("MultiShot")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowSpecialCost);
					bowSpecialCost=3000;
					stats.setRangedDamage (stats.getRangedDamage()*1.20f);
				}
				else if(PlayerPrefs.GetInt("MultiShot")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowSpecialCost);
				}
			}

			if(PlayerPrefs.GetInt("MultiShot")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}

		if (upgrade == "bowDamage") // Bow Damage
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>bowDamageCost)
			{
				PlayerPrefs.SetInt("BowDamage", PlayerPrefs.GetInt("BowDamage")+1);
			
				if(PlayerPrefs.GetInt("BowDamage")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowDamageCost);
					bowDamageCost=2000;
					stats.setRangedDamage (stats.getRangedDamage()*1.10f);
				}
				else if(PlayerPrefs.GetInt("BowDamage")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowDamageCost);
					bowDamageCost=3000;
					stats.setRangedDamage (stats.getRangedDamage()*1.20f);
				}
				else if(PlayerPrefs.GetInt("BowDamage")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowDamageCost);
					stats.setRangedDamage (stats.getRangedDamage()*1.30f);
				}
			}
			if(PlayerPrefs.GetInt("BowDamage")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}
		if (upgrade == "bowRoF") // Bow Rate of Fire
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>bowRoFCost)
			{
				PlayerPrefs.SetInt("BowRoF", PlayerPrefs.GetInt("BowRoF")+1);
			
				if(PlayerPrefs.GetInt("BowRoF")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowRoFCost);
					bowRoFCost=2000;
				}
				else if(PlayerPrefs.GetInt("BowRoF")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowRoFCost);
					bowRoFCost=3000;
				}
				else if(PlayerPrefs.GetInt("BowRoF")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - bowRoFCost);
				}
			}
			if(PlayerPrefs.GetInt("BowRoF")==3){
				fullUpgradeSound.Play();
			}
			else{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}

		//PLAYER
		if (upgrade == "health") // Health
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>healthCost)
			{
				PlayerPrefs.SetInt("Health", PlayerPrefs.GetInt("Health")+1);
			
				if(PlayerPrefs.GetInt("Health")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - healthCost);
					healthCost=2000;
					stats.setMaxHealth (stats.getMaxHealth()*1.10f);
				}
				else if(PlayerPrefs.GetInt("Health")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - healthCost);
					healthCost=3000;
					stats.setMaxHealth (stats.getMaxHealth()*1.10f);
				}
				else if(PlayerPrefs.GetInt("Health")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - healthCost);
					stats.setMaxHealth (stats.getMaxHealth()*1.1f);
				}
			}
			if(PlayerPrefs.GetInt("Health")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}
		if (upgrade == "stamina") // Stamina
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>staminaCost)
			{
				PlayerPrefs.SetInt("Stamina", PlayerPrefs.GetInt("Stamina")+1);
			
				if(PlayerPrefs.GetInt("Stamina")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - staminaCost);
					staminaCost=2000;
				}
				else if(PlayerPrefs.GetInt("Stamina")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - staminaCost);
					staminaCost=3000;
				}
				else if(PlayerPrefs.GetInt("Stamina")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - staminaCost);
				}
			}
			if(PlayerPrefs.GetInt("Stamina")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}
		if (upgrade == "agility") // Agility
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>agilityCost)
			{
				PlayerPrefs.SetInt("Agility", PlayerPrefs.GetInt("Agility")+1);
			
				if(PlayerPrefs.GetInt("Agility")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - agilityCost);
					agilityCost=2000;
				}
				else if(PlayerPrefs.GetInt("Agility")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - agilityCost);
					agilityCost=3000;
				}
				else if(PlayerPrefs.GetInt("Agility")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - agilityCost);
				}
			}
			if(PlayerPrefs.GetInt("Agility")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}

		//SWORD
		if (upgrade == "swordDamage") // Sword Damage
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>swordDamageCost)
			{
				PlayerPrefs.SetInt("SwordDamage", PlayerPrefs.GetInt("SwordDamage")+1);
			
				if(PlayerPrefs.GetInt("SwordDamage")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordDamageCost);
					swordDamageCost=2000;
					stats.setDamage (stats.getDamage("Attack_Player_Sword")*1.10f);
				}
				else if(PlayerPrefs.GetInt("SwordDamage")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordDamageCost);
					swordDamageCost=3000;
					stats.setDamage (stats.getDamage("Attack_Player_Sword")*1.20f);
				}
				else if(PlayerPrefs.GetInt("SwordDamage")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordDamageCost);
					stats.setDamage (stats.getDamage("Attack_Player_Sword")*1.30f);
				}
			}
			if(PlayerPrefs.GetInt("SwordDamage")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}
		if (upgrade == "swordRoF") // Sword Rate of Fire
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>swordRofCost)
			{
				PlayerPrefs.SetInt("SwordRoF", PlayerPrefs.GetInt("SwordRoF")+1);
			
				if(PlayerPrefs.GetInt("SwordRoF")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordRofCost);
					swordRofCost=2000;
				}
				else if(PlayerPrefs.GetInt("SwordRoF")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordRofCost);
					swordRofCost=3000;
				}
				else if(PlayerPrefs.GetInt("SwordRoF")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordRofCost);
				}
			}
			if(PlayerPrefs.GetInt("SwordRoF")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}
		if (upgrade == "aoe") // Area of Effect Radius
		{

			if(PlayerPrefs.GetInt("CurrentPoints")>swordSpecialCost)
			{
				PlayerPrefs.SetInt("AoE", PlayerPrefs.GetInt("AoE")+1);
				MainPlayerController.swordSpecialunlocked=true;
				MainPlayerController.canSwordSpecial = true;

				if(PlayerPrefs.GetInt("AoE")==1)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordSpecialCost);
					swordSpecialCost=2000;

				}
				else if(PlayerPrefs.GetInt("AoE")==2)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordSpecialCost);
					swordSpecialCost=3000;
				
				}
				else if(PlayerPrefs.GetInt("AoE")==3)
				{
					PlayerPrefs.SetInt("CurrentPoints", PlayerPrefs.GetInt("CurrentPoints") - swordSpecialCost);
				
				}
			}
			if(PlayerPrefs.GetInt("AoE")==3)
			{
				fullUpgradeSound.Play();
			}
			else
			{
				upgradeSound.Play();
			}
			GetDescription(upgrade);
		}
	}

	/// <summary>
	/// Update called each frame
	/// This is used to display progress for each upgrade available
	/// Each skill can only be upgraded max 3 times
	/// </summary>
	void Update () {
		currentPoints.text = "" + PlayerPrefs.GetInt ("CurrentPoints");
		/// BOW
		//MULTISHOT
		if (PlayerPrefs.GetInt ("MultiShot") > 3) {
			PlayerPrefs.SetInt ("MultiShot", 0);
			GetDescription ("multiShot");
		}
		multiShotProgress.text = PlayerPrefs.GetInt ("MultiShot") + "/3";
		if (PlayerPrefs.GetInt ("MultiShot") == 3) {
			multiShotProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("MultiShot") < 1) {
			multiShotProgress.color = Color.white;
		} else {
			multiShotProgress.color = Color.yellow;
		}

		//BOW DAMAGE
		if (PlayerPrefs.GetInt ("BowDamage") > 3) {
			PlayerPrefs.SetInt ("BowDamage", 0);
			GetDescription ("bowDamage");
		}
		bowDamageProgress.text = PlayerPrefs.GetInt ("BowDamage") + "/3";
		if (PlayerPrefs.GetInt ("BowDamage") == 3) {
			bowDamageProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("BowDamage") < 1) {
			bowDamageProgress.color = Color.white;
		} else {
			bowDamageProgress.color = Color.yellow;
		}

		//BOW RoF
		if (PlayerPrefs.GetInt ("BowRoF") > 3) {
			PlayerPrefs.SetInt ("BowRoF", 0);
			GetDescription ("bowRoF");
		}
		bowRoFProgress.text = PlayerPrefs.GetInt ("BowRoF") + "/3";
		if (PlayerPrefs.GetInt ("BowRoF") == 3) {
			bowRoFProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("BowRoF") < 1) {
			bowRoFProgress.color = Color.white;
		} else {
			bowRoFProgress.color = Color.yellow;
		}

		/// PLAYER
		//HEALTH
		if (PlayerPrefs.GetInt ("Health") > 3) {
			PlayerPrefs.SetInt ("Health", 0);
			GetDescription ("health");
		}
		healthProgress.text = PlayerPrefs.GetInt ("Health") + "/3";
		if (PlayerPrefs.GetInt ("Health") == 3) {
			healthProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("Health") < 1) {
			healthProgress.color = Color.white;
		} else {
			healthProgress.color = Color.yellow;
		}

		//STAMINA
		if (PlayerPrefs.GetInt ("Stamina") > 3) {
			PlayerPrefs.SetInt ("Stamina", 0);
			GetDescription ("stamina");
		}
		staminaProgress.text = PlayerPrefs.GetInt ("Stamina") + "/3";
		if (PlayerPrefs.GetInt ("Stamina") == 3) {
			staminaProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("Stamina") < 1) {
			staminaProgress.color = Color.white;
		} else {
			staminaProgress.color = Color.yellow;
		}

		//AGILITY
		if (PlayerPrefs.GetInt ("Agility") > 3) {
			PlayerPrefs.SetInt ("Agility", 0);
			GetDescription ("agility");
		}
		agilityProgress.text = PlayerPrefs.GetInt ("Agility") + "/3";
		if (PlayerPrefs.GetInt ("Agility") == 3) {
			agilityProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("Agility") < 1) {
			agilityProgress.color = Color.white;
		} else {
			agilityProgress.color = Color.yellow;
		}

		/// SWORD
		//SWORD DAMAGE
		if (PlayerPrefs.GetInt ("SwordDamage") > 3) {
			PlayerPrefs.SetInt ("SwordDamage", 0);
			GetDescription ("swordDamage");
		}
		swordDamageProgress.text = PlayerPrefs.GetInt ("SwordDamage") + "/3";
		if (PlayerPrefs.GetInt ("SwordDamage") == 3) {
			swordDamageProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("SwordDamage") < 1) {
			swordDamageProgress.color = Color.white;
		} else {
			swordDamageProgress.color = Color.yellow;
		}

		//SWORD RoF
		if (PlayerPrefs.GetInt ("SwordRoF") > 3) {
			PlayerPrefs.SetInt ("SwordRoF", 0);
			GetDescription ("swordRoF");
		}
		swordRoFProgress.text = PlayerPrefs.GetInt ("SwordRoF") + "/3";
		if (PlayerPrefs.GetInt ("SwordRoF") == 3) {
			swordRoFProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("SwordRoF") < 1) {
			swordRoFProgress.color = Color.white;
		} else {
			swordRoFProgress.color = Color.yellow;
		}

		//AoE
		if (PlayerPrefs.GetInt ("AoE") > 3) {
			PlayerPrefs.SetInt ("AoE", 0);
			GetDescription ("aoe");
		}
		aoeProgress.text = PlayerPrefs.GetInt ("AoE") + "/3";
		if (PlayerPrefs.GetInt ("AoE") == 3) {
			aoeProgress.color = Color.green;
		} else if (PlayerPrefs.GetInt ("AoE") < 1) {
			aoeProgress.color = Color.white;
		} else {
			aoeProgress.color = Color.yellow;
		}

		if (PlayerPrefs.GetInt ("SwordDamage") == 3) {

			swordDamageBtn.interactable = false;

		} else if (PlayerPrefs.GetInt ("SwordDamage") == 2) {
			swordDamageCost = 3000;

		} else if (PlayerPrefs.GetInt ("SwordDamage") == 1) {
			swordDamageCost = 2000;

		} else {
			swordDamageCost = 1020;

		}
		if (PlayerPrefs.GetInt ("SwordRoF") == 3) {

			swordRoFBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("SwordRoF") == 2) {
			swordRofCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("SwordRoF") == 1) {
			swordRofCost = 2000;
			
		} else {
			swordRofCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("AoE") == 3) {

			swordSpecialBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("AoE") == 2) {
			swordSpecialCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("AoE") == 1) {
			swordSpecialCost = 2000;
			
		} else {
			swordSpecialCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("BowDamage") == 3) {
			
			bowDamageBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("BowDamage") == 2) {
			bowDamageCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("BowDamage") == 1) {
			bowDamageCost = 2000;
			
		} else {
			bowDamageCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("BowRoF") == 3) {
			
			bowRoFBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("BowRoF") == 2) {
			bowRoFCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("BowRoF") == 1) {
			bowRoFCost = 2000;
			
		} else {
			bowRoFCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("MultiShot") == 3) {
			
			bowSpecialBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("MultiShot") == 2) {
			bowSpecialCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("MultiShot") == 1) {
			bowSpecialCost = 2000;
			
		} else {
			bowSpecialCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("Health") == 3) {
			
			healthBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("Health") == 2) {
			healthCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("Health") == 1) {
			healthCost = 2000;
			
		} else {
			healthCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("Stamina") == 3) {
			
			staminaBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("Stamina") == 2) {
			staminaCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("Stamina") == 1) {
			staminaCost = 2000;
			
		} else {
			staminaCost = 1020;
			
		}
		if (PlayerPrefs.GetInt ("Agility") == 3) {
			
			agilityBtn.interactable=false;
			
		}
		else if (PlayerPrefs.GetInt ("Agility") == 2) {
			agilityCost = 3000;
			
		} else if (PlayerPrefs.GetInt ("Agility") == 1) {
			agilityCost = 2000;
			
		} else {
			agilityCost = 1020;
			
		}
	}
}
