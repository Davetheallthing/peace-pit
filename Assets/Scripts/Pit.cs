using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour
{

		private float warMeter = 0.0f;
		private float maxWarMeter = 100.0f;
		private bool feedNeeded = false;
		private float time = 0.0f;
		private float warBarLength = 0.0f;
		private bool gameOver = false;
		private Town town;
		private bool feeding = false;

	

		public void OnGUI ()
		{
				
				if (!gameOver) {
						GUI.Label (new Rect (20, 90, 150, 30), "War meter: " + warMeter + "/" + maxWarMeter);
						GUI.Box (new Rect (170, 90, warBarLength, 30), "");
						if (feedNeeded) {
								GUI.Box (new Rect (20, 175, 100, 30), "FEED ME!!!");	
						}
						if (GUI.Button (new Rect (20, 140, 50, 30), "FEED")) {
								feeding = true;
						}
						if (feeding) {
								string[] content = town.SelectVillager ();
								//int personIdx = GUILayout.SelectionGrid (new Rect (120, 140, 100, 100), 0, content, 1);4
								int personIdx = GUILayout.SelectionGrid (-1, content, 1);
								if (personIdx != -1) {
										string personName = content [personIdx];
										Person victim = town.GetVillagerByName (personName);
										Feed (victim);											
								}
				
						}
				}
		}




		public void Feed (Person victim)
		{
//				print (victim.christianName);
				feedNeeded = false;
				feeding = false;
				warMeter -= victim.GetSacrificeValue ();
				if (warMeter < 0.0f) {
						warMeter = 0.0f;
				}
				victim.UpdateFamilyWoe ();
				town.annualSacrificeCounter++;
				town.UpdateTensionMeter (victim);	
				Destroy (victim.gameObject);
			
		}
	
		void Start ()
		{
				this.warMeter = 0.0f;
				this.maxWarMeter = 100.0f;
				this.feedNeeded = false;
				this.gameOver = false;
				this.warBarLength = 0.0f;
				this.town = this.transform.parent.GetComponent<Town> ();
		}

		void Update ()
		{
				if (feedNeeded) {
						time += Time.deltaTime;
						if (time > 1) {
								time = 0.0f;
								warMeter++;
						}
				}
				warBarLength = Screen.width / 2 * warMeter / maxWarMeter;
				if (warMeter == maxWarMeter) {
						this.gameOver = true;	
				}
		}

		public bool isGameOver ()
		{
				return gameOver;
		}

		public void setGameOver (bool isGameOver)
		{
				this.gameOver = isGameOver;
		}
		public void setFeedNeeded (bool feedNeeded)
		{
				this.feedNeeded = feedNeeded;
		}

}