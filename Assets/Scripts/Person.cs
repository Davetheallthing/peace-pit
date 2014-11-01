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
		public string family;
		public bool isSick = false;
		public bool isPregnant = false;
		public bool isContagious = false;
		public AgeBracket ageBracket;
		public float sacrificeValue = 0;
		public bool isFemale = false;

		public void Start ()
		{
				this.ageBracket = GetAgeBracketFromAge ();
				this.sacrificeValue = GetSacrificeValue ();
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


}
