using Microsoft.Maui;
using System.Reflection.Metadata.Ecma335;

namespace Michaelotchi
{
	public partial class MainPage : ContentPage
	{
		public bool displayDebugStats = true;

		public string HeaderTitle { get; set; } = "Welcome to Michaelotchi";
		public string SubTitle { get; set; } = "How's Michael doing?";
		public string DebugText { get; set; }
		public Creature Creature { get; set; }
		private bool timersStarted = false;

		public int hungerTickIntervalSeconds = 5;//1200;
		public float hungerTickValue = 1;
		public string HungerText => "Hunger: " + 
			(displayDebugStats ? Creature?.Hunger : Creature?.Hunger switch
		{
			>= 75 =>	"Not hungry",
			>= 50 =>	"Could eat",
			>= 25 =>	"Hungry",
			>= 10 =>	"Extremely hungry",
			>= 0 =>		"Dying of hunger",
			_ =>		""
		});
		public Color HungerColor => Creature?.Hunger switch
		{
			>= 75 =>	Colors.Green,
			>= 50 =>	Colors.LightGreen,
			>= 25 =>	Colors.Orange,
			>= 10 =>	Colors.Yellow,
			>= 0 =>		Colors.Red,
			_ =>		Colors.White
		};

		public int thirstTickIntervalSeconds = 1000;
		public float thirstTickValue = 1;
		public string ThirstText => "Thirst: " + 
			(displayDebugStats ? Creature?.Thirst : Creature?.Thirst switch
		{
			>= 75 =>	"Not thirsty",
			>= 50 =>	"Could drink",
			>= 25 =>	"Thirsty",
			>= 10 =>	"Extremely thirsty",
			>= 0 =>		"Dying of thirst",
			_ =>		""
		});
		public Color ThirstColor => Creature?.Thirst switch
		{
			>= 75 =>	Colors.Green,
			>= 50 =>	Colors.LightGreen,
			>= 25 =>	Colors.Orange,
			>= 10 =>	Colors.Yellow,
			>= 0 =>		Colors.Red,
			_ =>		Colors.White
		};

		public int engagementTickIntervalSeconds = 2500;
		public float engagementTickValue = 1;
		public string EngagementText => "Happiness: " + 
			(displayDebugStats ? Creature?.Engagement : Creature?.Engagement switch
		{
			>= 90 =>	"Overstimulated",
			>= 75 =>	"Extremely happy",
			>= 50 =>	"Happy",
			>= 25 =>	"Bored",
			>= 10 =>	"Extremely bored",
			>= 0 =>		"Bored to death",
			_ =>		""
		});
		public Color EngagementColor => Creature?.Engagement switch
		{
			>= 90 =>	Colors.Red,
			>= 75 =>	Colors.Orange,
			>= 60 =>	Colors.LightGreen,
			>= 40 =>	Colors.Green,
			>= 25 =>	Colors.LightGreen,
			>= 10 =>	Colors.Orange,
			>= 0 =>		Colors.Red,
			_ =>		Colors.White
		};

		public int lonelinessTickIntervalSeconds = 1500;
		public float lonelinessTickValue = 1;
		public string LonelinessText => "Loneliness: " + 
			(displayDebugStats ? Creature?.Loneliness : Creature?.Loneliness switch
		{
			>= 90 =>	"Feeling abandoned",
			>= 75 =>	"Extremely lonely",
			>= 50 =>	"Lonely",
			>= 25 =>	"Happy",
			>= 0 =>		"Loved",
			_ =>		""
		});
		public Color LonelinessColor => Creature?.Loneliness switch
		{
			>= 90 =>	Colors.Red,
			>= 75 =>	Colors.Yellow,
			>= 50 =>	Colors.Orange,
			>= 25 =>	Colors.LightGreen,
			>= 0 =>		Colors.Green,
			_ =>		Colors.White
		};

		public int energyTickIntervalSeconds = 5; //500;
		public float energyTickValue = 1;
		public string EnergyText => "Energy: " + 
			(displayDebugStats ? Creature?.Tired : Creature?.Tired switch
		{
			>= 75 =>	"Energized",
			>= 50 =>	"Rested",
			>= 25 =>	"Tired",
			>= 10 =>	"Extremely tired",
			>= 0 =>		"Sleeping",
			_ =>		""
		});
		public Color EnergyColor => Creature?.Tired switch
		{
			>= 75 => Colors.Green,
			>= 50 => Colors.LightGreen,
			>= 25 => Colors.Orange,
			>= 10 => Colors.Yellow,
			>= 0 => Colors.Red,
			_ => Colors.White
		};

		public MainPage()
		{
			BindingContext = this;

			InitializeComponent();

			LoadCreatureAtStartup();
		}

		private async void LoadCreatureAtStartup()
		{
			// Try and get the creature, if it's still alive
			(bool, bool) loadedCreature = await TryLoadCreature();

			if (loadedCreature.Item1)
				StartTimers();
			else LoadNewCreaturePage(loadedCreature.Item2);
		}

		private void StartTimers()
		{
			if (timersStarted)
				return;

			timersStarted = true;

			// Update creature according to elapsed time outside app
			float elapsedSeconds = Preferences.Get("elapsedTime", 0);
			DebugText = "Elapsed Seconds: " + elapsedSeconds;
			if (elapsedSeconds != -1)
			{
				int hungerTicks = (int)MathF.Floor(elapsedSeconds / hungerTickIntervalSeconds);
				DebugText += "\nHunger Ticks: " + hungerTicks;

				DateTime sleepTime = Preferences.Get("sleepTime", DateTime.Now);
				DateTime wakeTime = Preferences.Get("wakeTime", DateTime.Now);

				DebugText += "\nSleep Time: " + sleepTime.Second;
				DebugText += "\nWake Time: " + wakeTime.Second;
				for (; hungerTicks > 0; hungerTicks--)
					HungerTick(null, null);

				for (int thirstTicks = (int)MathF.Floor(elapsedSeconds / thirstTickIntervalSeconds); thirstTicks > 0; thirstTicks--)
					ThirstTick(null, null);

				for (int engagementTicks = (int)MathF.Floor(elapsedSeconds / engagementTickIntervalSeconds); engagementTicks > 0; engagementTicks--)
					EngagementTick(null, null);

				for (int lonelinessTicks = (int)MathF.Floor(elapsedSeconds / lonelinessTickIntervalSeconds); lonelinessTicks > 0; lonelinessTicks--)
					LonelinessTick(null, null);

				for (int energyTicks = (int)MathF.Floor(elapsedSeconds / energyTickIntervalSeconds); energyTicks > 0; energyTicks--)
					EnergyTick(null, null);
			}

			// Start the in-app timers
			#region Start Timers
			//Hunger timer
			System.Timers.Timer hungerTimer = new()
			{
				Interval = hungerTickIntervalSeconds * 1000,
				AutoReset = true
			};
			hungerTimer.Elapsed += HungerTick;
			hungerTimer.Start();

			//Thirst timer
			System.Timers.Timer thirstTimer = new()
			{
				Interval = thirstTickIntervalSeconds * 1000,
				AutoReset = true
			};
			thirstTimer.Elapsed += ThirstTick;
			thirstTimer.Start();

			//Engagement timer
			System.Timers.Timer engagementTimer = new()
			{
				Interval = engagementTickIntervalSeconds * 1000,
				AutoReset = true
			};
			engagementTimer.Elapsed += EngagementTick;
			engagementTimer.Start();

			//Loneliness timer
			System.Timers.Timer lonelinessTimer = new()
			{
				Interval = lonelinessTickIntervalSeconds * 1000,
				AutoReset = true
			};
			lonelinessTimer.Elapsed += LonelinessTick;
			lonelinessTimer.Start();

			//Energy timer
			System.Timers.Timer energyTimer = new()
			{
				Interval = energyTickIntervalSeconds * 1000,
				AutoReset = true
			};
			energyTimer.Elapsed += EnergyTick;
			energyTimer.Start();
			#endregion
		}

		#region Tick Methods
		public async void HungerTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Hunger > 0) 
				Creature.Hunger -= hungerTickValue;
			
			hungerTickValue = 1;

			if (Creature.Hunger <= 0)
				Die();
		}

		public async void ThirstTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Thirst > 0)
				Creature.Thirst -= thirstTickValue;
			
			thirstTickValue = 1;

			if (Creature.Thirst <= 0)
				Die();
		}

		public async void EngagementTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Engagement > 0) 
				Creature.Engagement -= engagementTickValue;

			if (Creature.Engagement < 20 || Creature.Engagement > 80)
			{
				energyTickValue *= .5f;
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		public async void LonelinessTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Loneliness < 100) 
				Creature.Loneliness += lonelinessTickValue;

			if (Creature.Loneliness > 50)
			{
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		public async void EnergyTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Tired < 100) 
				Creature.Tired = MathF.Min(Creature.Tired + energyTickValue, 100);
		}
		#endregion
		
		private void LoadNewCreaturePage(bool died)
		{
			Navigation.PushAsync(new NewCreaturePage(died));
		}

		private void LoadHungerPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new HungerPage(Creature));
		}

		private void LoadThirstPage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ThirstPage(Creature));
		}

		private async void LoadEngagementPage(object sender, EventArgs e)
		{
			if (Creature.Tired > 10)
			{
				await Navigation.PushAsync(new EngagementPage(Creature));
			}
		}

		private async void Die()
		{
			if (Preferences.Get("creatureId", -1) == -1)
				return;

			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			await creatureDataStore.DeleteItem(Creature);

			LoadNewCreaturePage(true);
		}

		private async Task<(bool, bool)> TryLoadCreature() 
		{
			//Preferences.Clear();
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			int creatureId = Preferences.Get("creatureId", -1);

			if (creatureId == -1)
			{
				return (false, false);
			}
			else
			{
				Creature = await creatureDataStore.ReadItem(creatureId);
				if (Creature == null)
				{
					return (false, false);
				}
				if (!CreatureIsAlive())
				{
					LoadNewCreaturePage(true);
					return (false, true);
				}
			}

			SubTitle = Creature.Name + " is happy to see you!";
			return (true, false);
		}

		private bool CreatureIsAlive()
		{
			return Creature.Hunger > 0 && Creature.Thirst > 0;
		}

		protected override async void OnAppearing()
		{
			await TryLoadCreature();
			StartTimers();

			SubTitle = Creature.Name + " is happy to see you!";

			base.OnAppearing();
		}

		protected override async void OnDisappearing()
		{
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			await creatureDataStore.UpdateItem(Creature);

			base.OnDisappearing();
		}

		private void DoNothing(object sender, EventArgs e) { }
	}
}