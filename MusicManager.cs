using UnityEngine;
using System.Collections;

public class MusicManager: MonoBehaviour{

	public AudioClip[] myAudioClip;
	private AudioSource myAudioSource;
	private int currentClip = 1;
	private bool atEndGame = false;
	private bool testBool = true;
	private static MusicManager instance = null;
	private float audioFadePoint;
	private float audioVolume = .3f;

	public static MusicManager Instance { 
		get {return instance;}
	}
	void Start()
	{
		currentClip = 0;
		myAudioSource = GetComponent<AudioSource>();
	}
	void Awake()
	{
		if(instance !=null && instance !=this)
		{
			Destroy (this.gameObject); 
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	/// <summary>
	/// Wait for end of music clip
	/// Once clip ended, fade out
	/// </summary>
	/// <returns>The for end.</returns>
	IEnumerator WaitForEnd()
	{
		testBool = false;
		yield return new WaitForSeconds(myAudioSource.clip.length - audioFadePoint);
		StartCoroutine("FadeOut");
	}

	/// <summary>
	/// Fade music out
	/// </summary>
	/// <returns>The out.</returns>
	IEnumerator FadeOut()
	{
		while(audioVolume>0)
		{
			audioVolume -=.05f * Time.deltaTime;
			myAudioSource.volume = audioVolume;
			yield return null;
		}
		testBool=true;
	}

	void Update()
	{
		if(Application.loadedLevel == 4)
		{
			Destroy(this.gameObject);
		}
		while(testBool)
		{
			myAudioSource.clip = myAudioClip[currentClip];
			myAudioSource.Play();
			audioFadePoint = .1f*myAudioSource.clip.length;
			StartCoroutine("WaitForEnd");
			audioVolume = .3f;
			myAudioSource.volume = audioVolume;
			currentClip ++;

			if(currentClip>= myAudioClip.Length)
			{
				currentClip = 0;
			}
		}
	}
}
