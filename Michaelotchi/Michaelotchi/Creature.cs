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

		private float hungerValue;
		public float Hunger // perfect at 100, decreases slowly
		{
			get => hungerValue;
			set
			{
				hungerValue = value;
			}
		}

		private float thirstValue;
		public float Thirst // perfect at 100, decreases slowly
		{
			get => thirstValue;
			set
			{
				thirstValue = value;
				UpdateCreature();
			}
		}

		public float Engagement // perfect at 50, decreases very slowly
		{
			get
			{
				return Stimulated;
			}
			set 
			{
				Boredom = 100 - value;
				Stimulated = value;
				UpdateCreature();
			} 
		}
		
		private float boredomValue;
		public float Boredom // perfect at 50, increases very slowly according to engagement
		{
			get => boredomValue;
			set => boredomValue = value;
		}

		private float stimulatedValue;
		public float Stimulated // perfect at 50, decreases very slowly according to engagement
		{ 
			get => stimulatedValue;
			set => stimulatedValue = value;
		}

		private float lonelinessValue;
		public float Loneliness // perfect at 0, increases very slowly
		{
			get => lonelinessValue;
			set
			{
				lonelinessValue = value;
				UpdateCreature();
			}
		}

		private float tiredValue;
		public float Tired // perfect at 100, increases slowly
		{ 
			get => tiredValue; 
			set
			{
				tiredValue = value; 
				UpdateCreature();
			}
		}

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

		public async Task<bool> UpdateCreature()
		{
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			return await creatureDataStore.UpdateItem(this);
		}
	}
}
