using UnityEngine;
using System.Collections.Generic;

public class Town : MonoBehaviour
{

		public List<Hut> hut;
		public int year = 0;
		private float time = 0.0f;
		private int seconds = 0;

		public void OnGUI ()
		{
				
				GUI.Box (new Rect (20, 20, 100, 30), "Timer: " + seconds);
				GUI.Box (new Rect (20, 40, 100, 30), "Year: " + year);

		}


		public void Update ()
		{
				time += Time.deltaTime;
				if (time > 10) {
						IncreaseYear ();
						time = 0.0f;
				}
				seconds = Mathf.FloorToInt (time);

		}

		public void IncreaseYear ()
		{
				year++;
		}

	
}
