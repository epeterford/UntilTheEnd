using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class menuScript : MonoBehaviour {
	private Canvas quitMenu;
	public Animator sidePanelAnim;
	public Animator titlePanelAnim;
	public Animator creditsPanelAnim;
	public Animator scoreboardPanelAnim;
	public Animator playerSelectAnim;
	public Animator quitAnim;
	public Animator playGameBtn;
	public Animator controlsAnim;
	public Animator panelShakeBobAnim;
	public Animator panelShakeSusyAnim;
	private Canvas creditsMenu;
	private Canvas playerSelectMenu;
	private Canvas controlsMenu;
	private Canvas scoreboardMenu;
	private Button startBtn;
	private Button controlBtn;
	private Button scoreboardBtn;
	private Button creditsBtn;
	private Button quitGameBtn;
	private Button yesQuitBtn;
	private Button noQuitBtn;
	public bool chooseBob = false;
	public bool chooseSusy = false;
	public AudioClip badSound;
	public AudioClip goodSound;
	public AudioSource audio;
	public Image bobPanel;
	public Image susyPanel;
	private GameObject gamepadImage;
	private GameObject keyboardImage;
	private Text controlsAsk;
	private bool atGamepad=true;
	public EventSystem eventSystem;
	private bool usingController=false;
	// Use this for initialization

	void Start () 
	{
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		Cursor.visible = true;
		PlayerPrefs.SetInt ("level", 1);
		PlayerPrefs.SetInt ("potions", 0);
		controlsAsk = GameObject.FindGameObjectWithTag ("ControlsAsk").GetComponent<Text>();
		gamepadImage = GameObject.FindGameObjectWithTag ("Gamepad");
		keyboardImage = GameObject.FindGameObjectWithTag ("Keyboard");
		gamepadImage.SetActive (true);
		keyboardImage.SetActive (false);
		audio = GetComponent<AudioSource>();
		bobPanel = GameObject.Find ("BobPanel").GetComponent<Image>();
		susyPanel = GameObject.Find("SusyPanel").GetComponent<Image>();
		playerSelectMenu = GameObject.Find ("PlayerSelectMenu").GetComponent<Canvas> ();
		controlsMenu = GameObject.Find ("ControlsMenu").GetComponent<Canvas> ();
		creditsMenu = GameObject.Find ("CreditsMenu").GetComponent<Canvas> ();
		scoreboardMenu = GameObject.Find ("ScoreBoardMenu").GetComponent<Canvas> ();
		//sideMenu = GameObject.Find ("SideMenu").GetComponent<Canvas> ();
		quitMenu = GameObject.Find ("QuitMenu").GetComponent<Canvas> ();
		startBtn = GameObject.Find ("Begin").GetComponent<Button> ();
		quitGameBtn = GameObject.Find ("Quit").GetComponent<Button> ();
		scoreboardBtn  = GameObject.Find ("Scoreboard").GetComponent<Button> ();
		creditsBtn = GameObject.Find ("Credits").GetComponent<Button> ();
		controlBtn = GameObject.Find ("Controls").GetComponent<Button> ();
		yesQuitBtn = GameObject.Find ("Yes").GetComponent<Button> ();
		noQuitBtn = GameObject.Find ("No").GetComponent<Button> ();
		quitMenu.enabled = false;
		creditsMenu.enabled = false;
		playerSelectMenu.enabled = false;
		scoreboardMenu.enabled = false;
		controlsMenu.enabled = false;
	}

	/// <summary>
	/// Select between male or female character
	/// </summary>
	/// <param name="character">Character.</param>
	public void ChooseCharacter(string character)
	{
		// Display play game button when character is selected
		playGameBtn.GetComponent<Animator> ().SetTrigger ("showBtn");

		if (character == "bob")
		{
			chooseBob=true;
			chooseSusy=false;

		}
		else if (character == "susy")
		{
			chooseSusy=true;
			chooseBob=false;

		}

	}


	public void QuitMenu()
	{
		if(usingController)
		{
			eventSystem.SetSelectedGameObject (GameObject.Find("Yes"));
		}

		/// Display Quit menu with buttons
		quitMenu.enabled = true;
		noQuitBtn.enabled = true;
		yesQuitBtn.enabled = true;

		// Play animation
		quitAnim.GetComponent<Animator> ().SetTrigger ("showPanel");

		/// Hide other butons
		scoreboardBtn.enabled = false;
		creditsBtn.enabled = false;
		startBtn.enabled = false;
		controlBtn.enabled = false;
		quitGameBtn.enabled = false;
	}

	// Swap between different Controls for game
	public void ChooseController() 
	{
		atGamepad = !atGamepad;
		if (atGamepad) 
		{
			controlsAsk.text="Using Keyboard and Mouse?";
			gamepadImage.SetActive (true);
			keyboardImage.SetActive (false);

		} else
		{
			controlsAsk.text="Using Gamepad?";
			gamepadImage.SetActive (false);
			keyboardImage.SetActive (true);
		}
	}

	/// <summary>
	/// Display and enable selected menu
	/// Using Animator to slide selected menu into frame
	/// </summary>
	/// <param name="menu">Menu.</param>
	public void SlideMenu(string menu)
	{
		if (menu == "controls") 
		{
			controlsMenu.enabled = true;
			controlsAnim.GetComponent<Animator> ().SetTrigger ("showPanel");
			if(usingController)
			{
				eventSystem.SetSelectedGameObject(GameObject.Find("ControlsExit"));
			}
		} 
		else if (menu == "playerSelect") 
		{
			playerSelectMenu.enabled=true;
			playerSelectAnim.GetComponent<Animator>().SetTrigger("showPanel");
			if(usingController)
			{
				eventSystem.SetSelectedGameObject(GameObject.Find("PlayerSelectExit"));
			}
		}
		else if (menu == "credits") 
		{
			if(usingController)
			{
				eventSystem.SetSelectedGameObject(GameObject.Find("CreditsExit"));
			}
			creditsMenu.enabled=true;
			creditsPanelAnim.GetComponent<Animator>().SetTrigger("showPanel");
		}
		else if (menu == "scoreboard")
		{
			if(usingController)
			{
				eventSystem.SetSelectedGameObject(GameObject.Find("ScoreboardExit"));
			}
			scoreboardMenu.enabled=true;
			scoreboardPanelAnim.GetComponent<Animator>().SetTrigger("showPanel");
		}

		// Hide the main menu and side panel
		titlePanelAnim.GetComponent<Animator> ().SetTrigger ("hidePanel");
		sidePanelAnim.GetComponent<Animator> ().SetTrigger ("hidePanel");

		quitMenu.enabled = false;
		startBtn.enabled = true;
		quitGameBtn.enabled = true;
	}

	/// <summary>
	/// Hides and disables selected menu
	/// Using Animator to slide selected menu out of frame
	/// </summary>
	/// <param name="menu">Menu.</param>
	public void ExitMenu(string menu)
	{
		if (menu == "controls")
		{
			if(usingController)
			{
				eventSystem.SetSelectedGameObject (GameObject.Find ("Controls"));
			}
			controlsMenu.enabled = true;
			controlsAnim.GetComponent<Animator> ().SetTrigger ("hidePanel");
		} 
		else if (menu == "playerSelect") {
			if(usingController)
			{
				eventSystem.SetSelectedGameObject (GameObject.Find ("Begin"));
			}
			chooseBob=false;
			chooseSusy=false;
			playerSelectMenu.enabled=true;
			playerSelectAnim.GetComponent<Animator>().SetTrigger("hidePanel");
		}
		else if (menu == "credits") 
		{
			if(usingController)
			{
				eventSystem.SetSelectedGameObject (GameObject.Find ("Credits"));
			}
			creditsMenu.enabled=true;
			creditsPanelAnim.GetComponent<Animator>().SetTrigger("hidePanel");
		}
		else if (menu == "scoreboard") 
		{
			if(usingController)
			{
				eventSystem.SetSelectedGameObject (GameObject.Find ("Scoreboard"));
			}
			scoreboardMenu.enabled=true;
			scoreboardPanelAnim.GetComponent<Animator>().SetTrigger("hidePanel");
		}

		// Show the main menu and side panel
		sidePanelAnim.GetComponent<Animator> ().SetTrigger ("showPanel");
		titlePanelAnim.GetComponent<Animator> ().SetTrigger ("showPanel");

		quitMenu.enabled = false;
		startBtn.enabled = true;
		quitGameBtn.enabled = true;
	}

	/// <summary>
	/// If pressed No when prompted to confirm to quit the game or not
	/// </summary>
	public void NoPress()
	{
		quitAnim.GetComponent<Animator> ().SetTrigger ("hidePanel");
		if(usingController)
		{
			eventSystem.SetSelectedGameObject (GameObject.Find ("Quit"));
		}
		controlBtn.enabled = true;
		scoreboardBtn.enabled = true;
		creditsBtn.enabled = true;
		noQuitBtn.enabled = false;
		yesQuitBtn.enabled = false;
		startBtn.enabled = true;
		quitGameBtn.enabled = true;
	}

	/// <summary>
	/// If pressed Yes when prompted to confirm to quit the game or not
	/// </summary>
	public void ExitGame()
	{
		Application.Quit ();
	}
	/// <summary>
	/// Start game button pressed
	/// </summary>
	public void StartGame()
	{
		if (!chooseBob && !chooseSusy) // if no character has been selected
		{
			/// Play sound and animation
			audio.PlayOneShot (badSound); 
			panelShakeBobAnim.GetComponent<Animator> ().SetTrigger ("shakeIt");
			panelShakeSusyAnim.GetComponent<Animator> ().SetTrigger ("shakeIt");
		} 
		else // otherwise
		{

			if(chooseBob)
			{
				PlayerPrefs.SetString("PlayerSelect", "Bob");
			}
			else
			{
				PlayerPrefs.SetString("PlayerSelect", "Susy");
			}

			/// Setup defaults for variables
			PlayerPrefs.SetInt ("potions", 0);
			PlayerPrefs.SetInt("branches", 0);
			PlayerPrefs.SetInt("branchDepth",0);
			PlayerPrefs.SetInt ("level", 1);
			PlayerPrefs.DeleteKey ("CurrentPoints");
			PlayerPrefs.DeleteKey ("SwordDamage");
			PlayerPrefs.DeleteKey("SwordRoF");
			PlayerPrefs.DeleteKey("AoE");
			PlayerPrefs.DeleteKey ("BowDamage");
			PlayerPrefs.DeleteKey("BowRoF");
			PlayerPrefs.DeleteKey("MultiShot");
			PlayerPrefs.DeleteKey ("Health");
			PlayerPrefs.DeleteKey("Stamina");
			PlayerPrefs.DeleteKey("Agility");

			// Play Start Game sound
			audio.PlayOneShot (goodSound);

			// Call fade to level
			levelChanger.fadeToLevel = true;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// If using a controller to navigate menu
		if(Input.GetAxis("DPadY")!=0)
		{
			usingController = true;

		}
		/// Update sprite attributes for character panels
		if (chooseBob) 
		{
			bobPanel.color = UnityEngine.Color.green;
			susyPanel.color = UnityEngine.Color.white;
		} 
		else if (chooseSusy) 
		{
			bobPanel.color = UnityEngine.Color.white;
			susyPanel.color = UnityEngine.Color.green;
		} 
		else 
		{
			bobPanel.color = UnityEngine.Color.white;
			susyPanel.color = UnityEngine.Color.white;
		}
	}
}
