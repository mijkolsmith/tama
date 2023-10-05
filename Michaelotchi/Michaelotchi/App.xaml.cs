namespace Michaelotchi
{
	public partial class App : Application
	{
		DateTime sleepTime;
		DateTime wakeTime;
		TimeSpan elapsedTime;

		public App()
		{
			InitializeComponent();

			DependencyService.RegisterSingleton<IDataStore<Creature>>(new RemoteCreatureDataStore());

            MainPage = new AppShell();
		}

		protected override void OnStart()
		{
			wakeTime = DateTime.Now;
			sleepTime = Preferences.Get(nameof(sleepTime), wakeTime);

			elapsedTime = wakeTime - sleepTime;

			Preferences.Set("wakeTime", wakeTime);

			Preferences.Set("elapsedTime", elapsedTime.Seconds);
			base.OnStart();
		}

		protected override void OnSleep()
		{
			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

			sleepTime = DateTime.Now;
			Preferences.Set(nameof(sleepTime), sleepTime);

			base.OnSleep();
		}

		protected override void OnResume()
		{
			wakeTime = DateTime.Now;
			sleepTime = Preferences.Get(nameof(sleepTime), wakeTime);

			elapsedTime = wakeTime - sleepTime;

			Preferences.Set("wakeTime", wakeTime);

			Preferences.Set("elapsedTime", elapsedTime.Seconds);

			base.OnResume();
		}
	}
}