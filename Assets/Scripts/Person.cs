using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour
{

		public enum Job
		{
				BAKER,
				HUNTER,
				BLACKSMITH,
				DOCTOR,
				SHAMAN,
				SCHOOL_CHILD,
				WORKER
		}

		public enum AgeBracket
		{
				INFANT, //0-10
				TEEN,//11-20
				ADULT,//21-35
				MATURE,//36-50
				ELDER,//51+
				NOT_VALID
		}

		public Job job;
		public int age;
		public bool isSick = false;
		public bool isPregnant = false;
		public bool isContagious = false;
		private AgeBracket ageBracket;
		private float sacrificeValue = 0;
		public bool isFemale = false;
		public string christianName = "";
		private string family = "";
		private float duration = 15.0f;
		private Vector3 wayPoint;
		private float range = 40.0f;
		private Vector3 spawnLocation = Vector3.zero;
		private Town town;
		private Hut hut ;
		private float tensionValue;

		public void Start ()
		{
				this.ageBracket = GetAgeBracketFromAge ();
				this.sacrificeValue = GetSacrificeValue ();
				this.town = GameObject.FindObjectOfType<Town> () as Town;
				this.Wander ();
				if (gameObject.transform.parent != null) {
						this.hut = gameObject.transform.parent.GetComponent<Hut> ();
				} else {
						hut = new Hut ();
				}				
				this.tensionValue = GetTensionValue ();
		}

		public void SetFamily (string family)
		{
				this.family = family;
		}

		public float GetFamilyWoe ()
		{
				return hut.GetWoe ();
		}

		public string GetFamily ()
		{
				return family;
		}


		public float GetSacrificeValue ()
		{
				float sacrificeValue = 0.0f;
				sacrificeValue += this.GetJobWeighting ();
				sacrificeValue += this.GetAgeWeighting ();
				if (isSick) {
						sacrificeValue--;
						if (isContagious) {
								sacrificeValue--;
						}
				}
				if (isPregnant) {
						sacrificeValue *= 1.5f;
				}
				return sacrificeValue;
		}

		public float GetTensionValue ()
		{
				float tensionValue = 0.0f;
				tensionValue += GetJobTension ();
				tensionValue += GetGenderTension ();
				tensionValue += GetAgeTension ();
				return tensionValue;
	
		}

		public void UpdateFamilyWoe ()
		{
				hut.AddWoe ();
		}

		private float GetJobTension ()
		{
				float jobTension = 0.0f;
				int population = town.GetPopulation ();
				int myColleagues = town.GetNumberJobSpecialist (job);
				int needed = job == Job.WORKER ? Mathf.FloorToInt (population / 5.0f) : Mathf.FloorToInt (population / 15.0f);
				if (!(myColleagues > needed)) {
						jobTension = 2 ^ (needed - myColleagues);
				}
				print ("job tension : " + jobTension);
				return jobTension;
		}

		private float GetAgeTension ()
		{
				float ageTension = 0.0f;
				if (this.ageBracket.Equals (AgeBracket.INFANT)) {
						ageTension = 6.0f;
				}
				if (this.ageBracket.Equals (AgeBracket.TEEN)) {
						ageTension = 3.0f;
				}
				print ("age tension : " + ageTension);
				return ageTension;
		}

		private float GetGenderTension ()
		{
				float genderTension = 0.0f;
				Person[] adults = town.GetAdults ();
				int females = 0;
				foreach (Person adult in adults) {
						if (adult.isFemale) {
								females++;
						}
				}
				if (adults.Length != 0) {
						float percentage = 100.0f * (females / adults.Length);
						float deviation = Mathf.Abs (50.0f - percentage);
						if (deviation > 20.0f) {
								genderTension = deviation - 20.0f;
						}
				}

				return genderTension;
			
		}

		private int GetAgeWeighting ()
		{
				switch (ageBracket) {
				case AgeBracket.INFANT:
						return -2;
				case AgeBracket.TEEN:
						return -1;
				case AgeBracket.ADULT:
						return 2;
				case AgeBracket.MATURE:
						return 0;
				}
				return 1;
		}

		public bool isAdult ()
		{
				return AgeBracket.ADULT.Equals (this.ageBracket);
		}

		private AgeBracket GetAgeBracketFromAge ()
		{
				if (age < 11) {
						return AgeBracket.INFANT;
				} else if (age > 10 && age < 21) {
						return AgeBracket.TEEN;
				} else if (age > 20 && age < 36) {
						return AgeBracket.ADULT;
				} else if (age > 35 && age < 51) {
						return AgeBracket.MATURE;
				}
				return AgeBracket.ELDER;
		}

		private int GetJobWeighting ()
		{
				if (this.job.Equals (Job.WORKER)) {
						return 1;
				} else {
						return 2;
				}
		}

		public string GetProfession ()
		{
				switch (job) {
				case Job.BAKER:
						return "baker";
				case Job.BLACKSMITH:
						return "blacksmith";
				case Job.DOCTOR:
						return "doctor";
				case Job.HUNTER:
						return "hunter";
				case Job.SHAMAN:
						return "shaman";
				}
				return "worker";

		}


		void Update ()
		{
				gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, wayPoint, 1 / (duration * (Vector3.Distance (gameObject.transform.position, wayPoint))));
				if ((transform.position - wayPoint).magnitude < 3) {
						Wander ();
				}
		}
	
		private void Wander ()
		{
				wayPoint = new Vector3 (Random.Range (transform.position.x - range, transform.position.x + range), Random.Range (transform.position.z - range, transform.position.z + range), 0);
				CheckBoundaries ();

				wayPoint.z = 0;
		}

		void CheckBoundaries ()
		{
				if (wayPoint.x > 100.0f) {
						wayPoint.x = 100.0f;
				}
				if (wayPoint.x < -100.0f) {
						wayPoint.x = -100.0f;
				}
				if (wayPoint.y < -40.0f) {
						wayPoint.y = -40.0f;
				}
				if (wayPoint.y > 40.0f) {
						wayPoint.y = 40.0f;
				}
		}

		void  OnCollisionEnter2D (Collision2D collision)
		{
				Wander ();

		}


}
