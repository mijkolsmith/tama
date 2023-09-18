namespace Michaelotchi
{
	public partial class MainPage : ContentPage //,INotifyPropertyChanged
	{
		public string HeaderTitle { get; set; } = "Welcome to Michaelotchi";


		public int hungerValue; // perfect at 100, decreases slowly
		public string HungerText { get; set; } = "Hunger";
		public int thirstValue; // perfect at 100, decreases slowly
		public string ThirstText { get; set; } = "Thirst";
		public int engagementValue; // perfect at 50, decreases slowly
		public string EngagementText { get; set; } = "Happiness";
		public int lonelinessValue; // perfect at 0, increases slowly
		public string LonelinessText { get; set; } = "Loneliness";
		public int energyValue; // perfect at 100, increases slowly
		public string EnergyText { get; set; } = "Energy";

		public MainPage()
		{
			BindingContext = this;

			InitializeComponent();
		}

		/*private void OnCounterClicked(object sender, EventArgs e)
		{
			int randomNumber = Random.Shared.Next(0, 100);

			if (HeaderTitle == "Michael")
				HeaderTitle = "Maat";
			else if (randomNumber < 80)
				HeaderTitle = "Michael";
			else HeaderTitle = "kenker";
			// ? SemanticScreenReader.Announce(CounterBtn.Text);
		}*/
		private void LoadGamePage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new GamePage());
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
			Navigation.PushAsync(new GamePage());
		}
	}
}