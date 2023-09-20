namespace Michaelotchi
{
	public partial class MainPage : ContentPage //,INotifyPropertyChanged
	{
		public string HeaderTitle { get; set; } = "Welcome to Michaelotchi";

		public float hungerValue = 100; // perfect at 100, decreases slowly
		public int hungerTickIntervalSeconds = 1200;
		public float hungerTickValue = 1;
		public string HungerText => hungerValue switch
		{
			>= 75 => "Not hungry",
			>= 50 => "Could eat",
			>= 25 => "Hungry",
			>= 10 => "Extremely hungry",
			>= 0 => "Dying of hunger",
			_ => ""
		};

		public float thirstValue = 100; // perfect at 100, decreases slowly
		public int thirstTickIntervalSeconds = 1000;
		public float thirstTickValue = 1;
		public string ThirstText => thirstValue switch
		{
			>= 75 => "Not thirsty",
			>= 50 => "Could drink",
			>= 25 => "Thirsty",
			>= 10 => "Extremely thirsty",
			>= 0 => "Dying of thirst",
			_ => ""
		};

		public float engagementValue = 50; // perfect at 50, decreases very slowly
		public int engagementTickIntervalSeconds = 2500;
		public float engagementTickValue = 1;
		public string EngagementText => engagementValue switch
		{
			>= 90 => "Overstimulated",
			>= 75 => "Extremely happy",
			>= 50 => "Happy",
			>= 25 => "Bored",
			>= 10 => "Extremely bored",
			>= 0 => "Bored to death",
			_ => ""
		};

		public float lonelinessValue = 0; // perfect at 0, increases very slowly
		public int lonelinessTickIntervalSeconds = 2000;
		public float lonelinessTickValue = 1;
		public string LonelinessText => lonelinessValue switch
		{
			>= 90 => "Feeling abandoned",
			>= 75 => "Extremely lonely",
			>= 50 => "Lonely",
			>= 25 => "Happy",
			>= 0 => "Loved",
			_ => ""
		};

		public float energyValue = 100; // perfect at 100, increases slowly
		public int energyTickIntervalSeconds = 500;
		public float energyTickValue = 1;
		public string EnergyText => energyValue switch
		{
			>= 75 => "Energized",
			>= 50 => "Rested",
			>= 25 => "Tired",
			>= 10 => "Extremely tired",
			>= 0 => "Sleeping",
			_ => ""
		};

		public MainPage()
		{
			BindingContext = this;

			InitializeComponent();

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
		}

		public void HungerTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (hungerValue > 0) hungerValue -= hungerTickValue;

			if (hungerValue < 0)
			{ 
				//Die
			}
		}

		public void ThirstTick(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (thirstValue > 0) thirstValue -= thirstTickValue;

			if (thirstValue < 0)
			{
				//Die
			}
		}

		public void EngagementTick(object sender, System.Timers.ElapsedEventArgs e)
		{

		}

		public void LonelinessTick(object sender, System.Timers.ElapsedEventArgs e)
		{

		}

		public void EnergyTick(object sender, System.Timers.ElapsedEventArgs e)
		{

		}

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

		private void DoNothing(object sender, EventArgs e) { }
	}
}