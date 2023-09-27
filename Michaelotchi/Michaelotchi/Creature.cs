using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michaelotchi
{
	public class Creature
	{
		public int Id { get; set; }
		public string Name {  get; set; }
		public string UserName { get; set; }
		public float Hunger { get; set; } // perfect at 100, decreases slowly
		public float Thirst { get; set; } // perfect at 100, decreases slowlý
		private float engagement;
		public float Engagement {
			get => engagement;
			set 
			{
				engagement = value;
				Boredom = 100 - value;
				Stimulated = value;
			} 
		} // perfect at 50, decreases very slowly
		public float Boredom { get; set; } // perfect at 50, increases very slowly according to engagement
		public float Stimulated { get; set; } // perfect at 50, decreases very slowly according to engagement
		public float Loneliness { get; set; } // perfect at 0, increases very slowly
		public float Tired { get; set; } // perfect at 100, increases slowly
		public int creatureCount;

		public Creature(string name, 
						string userName, 
						float hungerValue, 
						float thirstValue, 
						float engagementValue, 
						float lonelinessValue, 
						float energyValue, 
						int creatureCount)
		{
			Name = name;
			UserName = userName;
			Hunger = hungerValue;
			Thirst = thirstValue;
			Engagement = engagementValue;
			Loneliness = lonelinessValue;
			Tired = energyValue;
			this.creatureCount = creatureCount;
		}
	}
}
