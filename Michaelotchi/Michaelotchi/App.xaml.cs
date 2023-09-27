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

		protected override void OnSleep()
		{
			base.OnSleep();

			sleepTime = DateTime.Now;
			Preferences.Set(nameof(sleepTime), sleepTime);
		}

		protected override void OnResume()
		{
			base.OnResume();

			wakeTime = DateTime.Now;
			sleepTime = Preferences.Get(nameof(sleepTime), wakeTime);

			elapsedTime = wakeTime - sleepTime;
		}
	}
}