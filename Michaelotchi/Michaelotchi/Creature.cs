using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michaelotchi
{
	public class Creature
	{
		public string name;
		public float hungerValue; // perfect at 100, decreases slowly
		public float thirstValue; // perfect at 100, decreases slowly
		public float engagementValue; // perfect at 50, decreases very slowly
		public float lonelinessValue; // perfect at 0, increases very slowly
		public float energyValue; // perfect at 100, increases slowly
		public int creatureCount;

		public Creature(string name, float hungerValue, float thirstValue, float engagementValue, float lonelinessValue, float energyValue, int creatureCount)
		{
			this.name = name;
			this.hungerValue = hungerValue;
			this.thirstValue = thirstValue;
			this.engagementValue = engagementValue;
			this.lonelinessValue = lonelinessValue;
			this.energyValue = energyValue;
			this.creatureCount = creatureCount;
		}
	}
}
