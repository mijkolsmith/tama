using Newtonsoft.Json;

namespace Michaelotchi
{
	public partial class MainPage : ContentPage //,INotifyPropertyChanged
	{
		public string HeaderTitle { get; set; } = "Welcome to Michaelotchi";

		public Creature Creature { get; set; }

		public int hungerTickIntervalSeconds = 1200;
		public float hungerTickValue = 1;
		public string HungerText => Creature?.hungerValue switch
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
		public string ThirstText => Creature?.thirstValue switch
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
		public string EngagementText => Creature?.engagementValue switch
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
		public string LonelinessText => Creature?.lonelinessValue switch
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
		public string EnergyText => Creature?.energyValue switch
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

			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			Creature = creatureDataStore.ReadItem();

			if (Creature == null)
			{
				Navigation.PushAsync(new NewCreaturePage());
			}
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
			if (Creature.hungerValue > 0) Creature.hungerValue -= hungerTickValue;
			hungerTickValue = 1;

			if (Creature.hungerValue < 0)
			{
				Die();
			}
		}

		public void ThirstTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.thirstValue > 0) Creature.thirstValue -= thirstTickValue;
			thirstTickValue = 1;

			if (Creature.thirstValue < 0)
			{
				Die();
			}
		}

		public void EngagementTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.engagementValue > 0) Creature.engagementValue -= engagementTickValue;

			if (Creature.engagementValue < 20 || Creature.engagementValue > 80)
			{
				energyTickValue *= .5f;
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		public void LonelinessTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.lonelinessValue < 100) Creature.lonelinessValue += lonelinessTickValue;

			if (Creature.lonelinessValue > 50)
			{
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		public void EnergyTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature.energyValue < 100) Creature.energyValue = MathF.Min(Creature.energyValue + energyTickValue, 100);
		}
		#endregion

		private void LoadHungerPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new HungerPage());
		}

		private void LoadThirstPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ThirstPage());
		}

		private void LoadEngagementPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new EngagementPage());
		}

		private void Die()
		{
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			creatureDataStore.DeleteItem(Creature);
			Navigation.PushAsync(new NewCreaturePage());
		}

		// Temp to bind to temp buttons
		private void DoNothing(object sender, EventArgs e) { }
	}
}