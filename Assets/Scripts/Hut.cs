using UnityEngine;
using System.Collections.Generic;

public class Hut : MonoBehaviour
{

		public List<Person> villagers;
		public Vector3 position;
		public string family;
		private float woe = 0.0f;
		private int woeCounter = 0;
		

		public void Start ()
		{
				this.transform.position = position;
				//spawnLocation = Vector3.one * 2.0f;
				foreach (Person villager in villagers) {
						Person person = Instantiate (villager) as Person;
						person.SetFamily (family);
						person.transform.parent = this.gameObject.transform;
				}
		}

		public void AddWoe ()
		{
				woeCounter++;
				recalculateWoe ();
		}


		public float GetWoe ()
		{
				return woe;
		
		}

		public void DecreaseWoe ()
		{
				woeCounter--;
				recalculateWoe ();
		
		}

		public void recalculateWoe ()
		{
				woe += 2 ^ woeCounter;
		}

		public bool isMourning ()
		{
				return woeCounter > 0;
		}

//		private void SpawnNextPerson (Person person)
//		{
//				Instantiate (person, spawnLocation, Quaternion.identity);
//				spawnLocation -= Vector3.one * 2.0f;
//		}

		void OnMouseOver ()
		{

				GUI.Button (new Rect (10, 10, 100, 20), new GUIContent ("Click me", "This is the tooltip"));
				GUI.Label (new Rect (10, 40, 100, 40), GUI.tooltip);

//				foreach (Person person in villagers) {
//			string playerInfo = "name: " + person.christianName + " " + person.name + "\n" +
//				"age: " + person.age + "\n" + "profession: " + person.GetProfession () + "\n";
//			GUI.Box (new Rect (20, 220 + iterate, 300, 50),GUIContent ("Click me", "This is the tooltip"));
//						iterate += 100.0f;
//						float sf = person.GetSacrificeValue ();
//						if (sf > bestRating) {
//								bestRating = sf;
//								bestPerson = person;
//			
//						}							
//
		}

}
