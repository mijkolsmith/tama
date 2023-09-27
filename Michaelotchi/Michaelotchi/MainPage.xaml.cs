using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace Michaelotchi
{
	public partial class MainPage : ContentPage //,INotifyPropertyChanged
	{
		public string HeaderTitle { get; set; } = "Welcome to Michaelotchi";

		public Creature Creature { get; set; }

		public int hungerTickIntervalSeconds = 1200;
		public float hungerTickValue = 1;
		public string HungerText => Creature?.Hunger switch
		{
			>= 75 =>	"Not hungry",
			>= 50 =>	"Could eat",
			>= 25 =>	"Hungry",
			>= 10 =>	"Extremely hungry",
			>= 0 =>		"Dying of hunger",
			_ =>		""
		};

		public int thirstTickIntervalSeconds = 1000;
		public float thirstTickValue = 1;
		public string ThirstText => Creature?.Thirst switch
		{
			>= 75 =>	"Not thirsty",
			>= 50 =>	"Could drink",
			>= 25 =>	"Thirsty",
			>= 10 =>	"Extremely thirsty",
			>= 0 =>		"Dying of thirst",
			_ =>		""
		};

		public int engagementTickIntervalSeconds = 2500;
		public float engagementTickValue = 1;
		public string EngagementText => Creature?.Boredom switch
		{
			>= 90 =>	"Overstimulated",
			>= 75 =>	"Extremely happy",
			>= 50 =>	 "Happy",
			>= 25 =>	"Bored",
			>= 10 =>	"Extremely bored",
			>= 0 =>		"Bored to death",
			_ =>		""
		};

		public int lonelinessTickIntervalSeconds = 1500;
		public float lonelinessTickValue = 1;
		public string LonelinessText => Creature?.Loneliness switch
		{
			>= 90 =>	"Feeling abandoned",
			>= 75 =>	"Extremely lonely",
			>= 50 =>	"Lonely",
			>= 25 =>	"Happy",
			>= 0 =>		"Loved",
			_ =>		""
		};
		public int energyTickIntervalSeconds = 500;
		public float energyTickValue = 1;
		public string EnergyText => Creature?.Tired switch
		{
			>= 75 =>	"Energized",
			>= 50 =>	"Rested",
			>= 25 =>	"Tired",
			>= 10 =>	"Extremely tired",
			>= 0 =>		"Sleeping",
			_ =>		""
		};
		
		public MainPage()
		{
			BindingContext = this;

			InitializeComponent();

			LoadCreatureAtStartup();
		}

		public async void LoadCreatureAtStartup()
		{
			if (!await TryLoadCreature()) 
				LoadNewCreaturePage();

			#region Start Timers
			//Hunger timer
			System.Timers.Timer hungerTimer = new System.Timers.Timer()
			{
				Interval = hungerTickIntervalSeconds * 1000,
				AutoReset = true
			};
			hungerTimer.Elapsed += HungerTick;
			hungerTimer.Start();

			//Thirst timer
			System.Timers.Timer thirstTimer = new System.Timers.Timer()
			{
				Interval = thirstTickIntervalSeconds * 1000,
				AutoReset = true
			};
			thirstTimer.Elapsed += ThirstTick;
			thirstTimer.Start();

			//Engagement timer
			System.Timers.Timer engagementTimer = new System.Timers.Timer()
			{
				Interval = engagementTickIntervalSeconds * 1000,
				AutoReset = true
			};
			engagementTimer.Elapsed += ThirstTick;
			engagementTimer.Start();

			//Loneliness timer
			System.Timers.Timer lonelinessTimer = new System.Timers.Timer()
			{
				Interval = lonelinessTickIntervalSeconds * 1000,
				AutoReset = true
			};
			lonelinessTimer.Elapsed += ThirstTick;
			lonelinessTimer.Start();

			//Energy timer
			System.Timers.Timer energyTimer = new System.Timers.Timer()
			{
				Interval = energyTickIntervalSeconds * 1000,
				AutoReset = true
			};
			energyTimer.Elapsed += ThirstTick;
			energyTimer.Start();
			#endregion
		}

		#region Tick Methods
		public void HungerTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
			{ 
				TryLoadCreature(); 
			}

			if (Creature.Hunger > 0) Creature.Hunger -= hungerTickValue;
			hungerTickValue = 1;

			if (Creature.Hunger < 0)
			{
				Die();
			}
		}

		public void ThirstTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.Thirst > 0) Creature.Thirst -= thirstTickValue;
			thirstTickValue = 1;

			if (Creature.Thirst < 0)
			{
				Die();
			}
		}

		public void EngagementTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.Engagement > 0) Creature.Engagement -= engagementTickValue;

			if (Creature.Engagement < 20 || Creature.Engagement > 80)
			{
				energyTickValue *= .5f;
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		public void LonelinessTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.Loneliness < 100) Creature.Loneliness += lonelinessTickValue;

			if (Creature.Loneliness > 50)
			{
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		public void EnergyTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.Tired < 100) Creature.Tired = MathF.Min(Creature.Tired + energyTickValue, 100);
		}
		#endregion
		
		private void LoadNewCreaturePage()
		{
			Navigation.PushAsync(new NewCreaturePage());
		}

		private void LoadHungerPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new HungerPage(Creature));
		}

		private void LoadThirstPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ThirstPage(Creature));
		}

		private void LoadEngagementPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new EngagementPage(Creature));
		}

		private async void Die()
		{
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			await creatureDataStore.DeleteItem(Creature);
			await Navigation.PushAsync(new NewCreaturePage());
		}

		private async Task<bool> TryLoadCreature() 
		{
			//Preferences.Clear();
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			int creatureId = Preferences.Get("creatureId", 0);

			if (creatureId == 0)
			{
				return false;
			}
			else
			{
				Creature = await creatureDataStore.ReadItem(creatureId);
				if (Creature == null)
				{
					return false;
				}
			}

			return true;
		}

		private void DoNothing(object sender, EventArgs e) { }
	}
}