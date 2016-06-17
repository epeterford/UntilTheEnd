using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class scoreKeeper : MonoBehaviour {
	public string[] names;
	public int[] scores = {};
	public Text[]scoresToText;
	public Text[]namesToText;
	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey("Score1Name") != true) // If there isn't a saved score
		{
			/// Set default scores
			names [0] = "...";
			names [1] = "...";
			names [2] = "...";
			names [3] = "...";
			names [4] = "...";
			scores [0] = 0;
			scores [1] = 0;
			scores [2] = 0;
			scores [3] = 0;
			scores [4] = 0;
		} 
		else //otherwise
		{
			/// Get and assign saved scores
			names[0] = PlayerPrefs.GetString("Score1Name");
			names[1] = PlayerPrefs.GetString("Score2Name");
			names[2] = PlayerPrefs.GetString("Score3Name");
			names[3] = PlayerPrefs.GetString("Score4Name");
			names[4] = PlayerPrefs.GetString("Score5Name");
			scores[0] = PlayerPrefs.GetInt("Score1Score");
			scores[1] = PlayerPrefs.GetInt("Score2Score");
			scores[2] = PlayerPrefs.GetInt("Score3Score");
			scores[3] = PlayerPrefs.GetInt("Score4Score");
			scores[4] = PlayerPrefs.GetInt("Score5Score");


		}

		if (PlayerPrefs.HasKey ("AddedName") == true) 
		{
			AddScore();
		}

	}

	public void AddScore()
	{
		int score = PlayerPrefs.GetInt ("AddedScore");
		UpdateScores (PlayerPrefs.GetString("AddedName"), score);
	}
	public void ResetScoreboard(){

		names [0] = "...";
		names [1] = "...";
		names [2] = "...";
		names [3] = "...";
		names [4] = "...";
		scores [0] = 0;
		scores [1] = 0;
		scores [2] = 0;
		scores [3] = 0;
		scores [4] = 0;
		PlayerPrefs.DeleteAll ();
	}

	/// <summary>
	/// Sorts scores from highest to lowest
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="score">Score.</param>
	public void UpdateScores(string name, int score){

		for (int i=0; i<5; i++) {
			if(score>scores[i]){
				int j=4;
				while(j>i){
					scores[j] = scores[j-1];
					names[j] = names[j-1];
					j--;
				}
				scores[i] = score;
				names[i] = name;
				break;
			}
		}
		PlayerPrefs.SetInt ("Score1Score", scores [0]);
		PlayerPrefs.SetInt ("Score2Score", scores [1]);
		PlayerPrefs.SetInt ("Score3Score", scores [2]);
		PlayerPrefs.SetInt ("Score4Score", scores [3]);
		PlayerPrefs.SetInt ("Score5Score", scores [4]);
		PlayerPrefs.SetString ("Score1Name", names [0]);
		PlayerPrefs.SetString ("Score2Name", names [1]);
		PlayerPrefs.SetString ("Score3Name", names [2]);
		PlayerPrefs.SetString ("Score4Name", names [3]);
		PlayerPrefs.SetString ("Score5Name", names [4]);
		PlayerPrefs.DeleteKey ("AddedName");

	}
	// Update is called once per frame
	void Update () {

		for (int i=0; i<scores.Length; i++) {
			namesToText[i].text = "" + names[i];
			scoresToText[i].text = ""+ scores[i];

		}
	}
}