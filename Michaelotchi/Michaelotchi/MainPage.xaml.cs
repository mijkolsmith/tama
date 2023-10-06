namespace Michaelotchi
{
	public partial class MainPage : ContentPage
	{
		public bool displayDebugInfo = false;

		public string HeaderTitle { get; set; } = "Welcome to Michaelotchi";
		public string SubTitle { get; set; } = "How's Michael doing?";
		public string DebugText { get; set; }
		public Creature Creature { get; set; }
		private bool timersStarted = false;

		public int hungerTickIntervalSeconds = 4; //1200;
		public float hungerTickValue = 1;
		public string HungerText => "Hunger: " +
			(displayDebugInfo ? Creature?.Hunger : Creature?.Hunger switch
			{
				>= 75 => "Not hungry",
				>= 50 => "Could eat",
				>= 25 => "Hungry",
				>= 10 => "Extremely hungry",
				>= 0 => "Dying of hunger",
				_ => ""
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
			(displayDebugInfo ? Creature?.Thirst : Creature?.Thirst switch
			{
				>= 75 => "Not thirsty",
				>= 50 => "Could drink",
				>= 25 => "Thirsty",
				>= 10 => "Extremely thirsty",
				>= 0 => "Dying of thirst",
				_ => ""
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
			(displayDebugInfo ? Creature?.Engagement : Creature?.Engagement switch
			{
				>= 90 => "Overstimulated",
				>= 75 => "Extremely happy",
				>= 50 => "Happy",
				>= 25 => "Bored",
				>= 10 => "Extremely bored",
				>= 0 => "Bored to death",
				_ => ""
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

		public int lonelinessTickIntervalSecondsClosed = 1500;
		public int lonelinessTickIntervalSecondsOpened = 20;
		public float lonelinessTickValue = 1;
		public string LonelinessText => "Loneliness: " +
			(displayDebugInfo ? Creature?.Loneliness : Creature?.Loneliness switch
			{
				>= 90 => "Feeling abandoned",
				>= 75 => "Extremely lonely",
				>= 50 => "Lonely",
				>= 25 => "Happy",
				>= 0 => "Loved",
				_ => ""
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

		public int energyTickIntervalSeconds = 2; //500;
		public float energyTickValue = 1;
		public string EnergyText => "Energy: " +
			(displayDebugInfo ? Creature?.Tired : Creature?.Tired switch
			{
				>= 75 => "Energized",
				>= 50 => "Rested",
				>= 25 => "Tired",
				>= 10 => "Extremely tired",
				>= 0 => "Sleeping",
				_ => ""
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
			bool loadedCreature = await TryLoadCreature();

			if (loadedCreature)
				await StartTimers();
			else Respawn();
		}

		private async Task<bool> StartTimers()
		{
			if (timersStarted)
				return false;

			timersStarted = true;

			// Update creature according to elapsed time outside app
			float elapsedSeconds = Preferences.Get("elapsedTime", 0);

			if (elapsedSeconds != -1)
			{
				int hungerTicks = (int)MathF.Floor(elapsedSeconds / hungerTickIntervalSeconds);

				if (displayDebugInfo)
				{
					DateTime sleepTime = Preferences.Get("sleepTime", DateTime.Now);
					DateTime wakeTime = Preferences.Get("wakeTime", DateTime.Now);
					DebugText +=
						$"Elapsed Seconds: {elapsedSeconds}" +
						$"\nHunger Before: {Creature.Hunger} " +
						$"\n Hunger Ticks: {hungerTicks}" +
						$"\nSleep Time: {sleepTime.Minute}:{sleepTime.Second}" +
						$"\nWake Time: {wakeTime.Minute}:{wakeTime.Second}";
				}

				hungerTickValue *= hungerTicks;
				HungerTick(null, null);

				if (displayDebugInfo)
				{
					DebugText += $"\nHunger After: {Creature.Hunger}";
				}

				int thirstTicks = (int)MathF.Floor(elapsedSeconds / thirstTickIntervalSeconds);
				thirstTickValue *= thirstTicks;
				ThirstTick(null, null);

				int engagementTicks = (int)MathF.Floor(elapsedSeconds / engagementTickIntervalSeconds);
				engagementTickValue *= engagementTicks;
				EngagementTick(null, null);

				int lonelinessTicks = (int)MathF.Floor(elapsedSeconds / lonelinessTickIntervalSecondsClosed);
				lonelinessTickValue *= lonelinessTicks;
				LonelinessTickUp();

				int energyTicks = (int)MathF.Floor(elapsedSeconds / energyTickIntervalSeconds);
				energyTickValue *= energyTicks;
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

			//Loneliness timer (reversed compared to when app is closed)
			System.Timers.Timer lonelinessTimer = new()
			{
				Interval = lonelinessTickIntervalSecondsOpened * 1000,
				AutoReset = true
			};
			lonelinessTimer.Elapsed += LonelinessTickDown;
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

			return true;
		}

		#region Tick Methods
		private async void HungerTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Hunger > 0)
			{
				Creature.Hunger -= hungerTickValue;
				OnPropertyChanged(nameof(HungerText));
				OnPropertyChanged(nameof(HungerColor));
			}

			hungerTickValue = 1;

			if (Creature.Hunger <= 0)
				Respawn();
		}

		private async void ThirstTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Thirst > 0)
			{
				Creature.Thirst -= thirstTickValue;
				OnPropertyChanged(nameof(ThirstText));
				OnPropertyChanged(nameof(ThirstColor));
			}
			
			thirstTickValue = 1;

			if (Creature.Thirst <= 0)
				Respawn();
		}

		private async void EngagementTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Engagement > 0)
			{
				Creature.Engagement = MathF.Max(Creature.Engagement - engagementTickValue, 0);
				OnPropertyChanged(nameof(EngagementText));
				OnPropertyChanged(nameof(EngagementColor));
			}

			engagementTickValue = 1;

			if (Creature.Engagement < 20 || Creature.Engagement > 80)
			{
				energyTickValue *= .5f;
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		private async void LonelinessTickUp()
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Loneliness < 100)
			{
				Creature.Loneliness = MathF.Min(Creature.Loneliness + lonelinessTickValue, 100);
				OnPropertyChanged(nameof(LonelinessText));
				OnPropertyChanged(nameof(LonelinessColor));
			}

			lonelinessTickValue = 1;

			if (Creature.Loneliness > 50)
			{
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		private async void LonelinessTickDown(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Loneliness > 100)
			{
				Creature.Loneliness = MathF.Max(Creature.Loneliness - lonelinessTickValue, 0);
				OnPropertyChanged(nameof(LonelinessText));
				OnPropertyChanged(nameof(LonelinessColor));
			}

			if (Creature.Loneliness > 50)
			{
				thirstTickValue *= 2f;
				hungerTickValue *= 2f;
			}
		}

		private async void EnergyTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Creature == null)
				await TryLoadCreature();

			if (Creature.Tired < 100)
			{
				Creature.Tired = MathF.Min(Creature.Tired + energyTickValue, 100);
				OnPropertyChanged(nameof(EnergyText));
				OnPropertyChanged(nameof(EnergyColor));
			}

			energyTickValue = 1;
		}
		#endregion

		private void LoadNewCreaturePage() =>
			Navigation.PushAsync(new NewCreaturePage());

		private void LoadHungerPage(object sender, EventArgs e) =>
			Navigation.PushAsync(new HungerPage(Creature));

		private void LoadThirstPage(object sender, EventArgs e) =>
			Navigation.PushAsync(new ThirstPage(Creature));

		private async void LoadEngagementPage(object sender, EventArgs e)
		{
			if (Creature.Tired > 10)
			{
				await Navigation.PushAsync(new EngagementPage(Creature));
			}
		}

		private void KillCreature(object sender, EventArgs e) => Respawn();

		private async void Respawn()
		{
			if (Creature != null)
			{
				IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
				await creatureDataStore.DeleteItem(Creature);
			}

			LoadNewCreaturePage();
		}

		private async Task<bool> TryLoadCreature() 
		{
			//Preferences.Clear();
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
			int creatureId = Preferences.Get("creatureId", -1);

			if (creatureId == -1)
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

			SubTitle = Creature.Name + " is happy to see you!";
			return true;
		}

		protected override async void OnAppearing()
		{
			await TryLoadCreature();
			StartTimers();

			SubTitle = Creature.Name + " is happy to see you!";

			base.OnAppearing();
		}

		private void DoNothing(object sender, EventArgs e) { }
	}
}