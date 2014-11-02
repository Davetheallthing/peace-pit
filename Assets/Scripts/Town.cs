using UnityEngine;
using System.Collections.Generic;
using System;

public class Town : MonoBehaviour
{

		public string townName = "Town Name";		
		public Pit pitPrebab;
		public List<Hut> huts;
		private float time = 0.0f;
		private int seconds = 0;
		private int year = 0;
		private float tensionMeter = 0.0f;
		private float tensionBarLength = 0.0f;
		private float maxTensionMeter = 300.0f;
		public int annualSacrificeCounter = 0;
		private Pit pit;

		public void Start ()
		{
				pit = Instantiate (pitPrebab) as Pit;
				pit.transform.parent = this.transform;
				foreach (Hut hut in huts) {
						Hut myHut = Instantiate (hut, hut.position, Quaternion.identity) as Hut;
						myHut.transform.parent = this.gameObject.transform;
				}

		}

		public void OnGUI ()
		{
				float bestRating = 0.0f;
				Person bestPerson = null;

				if (pit != null && pit.isGameOver ()) {
						GUI.Box (new Rect (400, 400, 300, 30), "GAME OVER");
				} else {
						GUI.Box (new Rect (20, 20, 300, 30), townName);
						GUI.Box (new Rect (20, 55, 300, 30), "Year: " + year + " : " + "Timer: " + seconds);
						GUI.Label (new Rect (20, 125, 150, 30), "Tension meter: " + tensionMeter + "/" + maxTensionMeter);
						GUI.Box (new Rect (170, 125, tensionBarLength / 3, 30), "");

				}
				

				foreach (Hut hut in huts) {
						float iterate = 0.0f;
						bestPerson = hut.villagers [0];
						foreach (Person person in hut.villagers) {
								iterate += 100.0f;
								float sf = person.GetSacrificeValue ();
								if (sf > bestRating) {
										bestRating = sf;
										bestPerson = person;
								}
								
						}
				}
//				GUI.Box (new Rect (20, 400, 400, 30), "Best person to kill: " + bestPerson.christianName + " " + bestPerson.name);
				if (bestPerson != null) {
						print ("Best person to kill: " + bestPerson.christianName + " " + bestPerson.name);
				}
				

		}


		public void Update ()
		{
				time += Time.deltaTime;
				if (time > 10) {
						IncreaseYear ();
						time = 0.0f;
				}
				seconds = Mathf.FloorToInt (time);
				tensionBarLength = (Screen.width / 2) * (tensionMeter / maxTensionMeter);
				if (GetPopulation () < 1 || tensionMeter > maxTensionMeter) {
						pit.setGameOver (true);
				}

		}

		public void IncreaseYear ()
		{
				year++;
				pit.setFeedNeeded (true);
				annualSacrificeCounter = 0;
				DecreaseFamilyWoeCounter ();
				tensionMeter -= 10;
				if (tensionMeter < 0) {
						tensionMeter = 0;
				}
				
		}

		public int GetPopulation ()
		{
				int population = 0;
				foreach (Hut hut in huts) {
						foreach (Person person in hut.villagers)
								population++;				
				}
				return population;
		}

		public int GetNumberJobSpecialist (Person.Job job)
		{
				int specialists = 0;
				foreach (Person villager in GetVillagers ()) {
						if (villager.job == job) {
								specialists++;
						}
				}
				return specialists;
		}

		public Person[] GetVillagers ()
		{
				return GameObject.FindObjectsOfType<Person> ();
		}

		public Person[] GetAdults ()
		{
				List<Person> adults = new List<Person> ();
				foreach (Person person in GetVillagers()) {
						if (person.isAdult ()) {
								adults.Add (person);
						}
				}
				return adults.ToArray ();
		}

		public float GetGlobalSacrificeValue ()
		{
				float sacrificeValue = 0.0f;
				foreach (Hut hut in huts) {
						foreach (Person person in hut.villagers)
								sacrificeValue += person.GetSacrificeValue ();	
				}
				return sacrificeValue;
		}
	
		void DecreaseFamilyWoeCounter ()
		{
				foreach (Hut hut in GameObject.FindObjectsOfType<Hut>()) {
						if (hut.isMourning ()) {
								hut.DecreaseWoe ();
						}	
				}
		}
	
		public void UpdateTensionMeter (Person victim)
		{
				tensionMeter += victim.GetTensionValue ();
				tensionMeter += victim.GetFamilyWoe ();
				tensionMeter += 3 ^ annualSacrificeCounter;			
		}
		
	

	
		public string[] SelectVillager ()
		{
				Person[] villagers = GetVillagers ();
				string[] content = new string[villagers.Length];
				int i = 0;
				foreach (Person person in villagers) {
						content [i] = person.christianName + " " + person.GetFamily ();
						i++;
				}
				return content;

		}

		public Person GetVillagerByName (string name)
		{

				string[] names = name.Split (' ');
				Person[] villagers = GetVillagers ();
				foreach (Person villager in villagers) {
						if (villager.GetFamily ().Equals (names [1]) && villager.christianName.Equals (names [0])) {
								return villager;
						}
				}
				throw new Exception ("Cannot find villager " + name);
		}
		

	
}
