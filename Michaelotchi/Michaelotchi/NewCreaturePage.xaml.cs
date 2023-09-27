namespace Michaelotchi
{
	public partial class NewCreaturePage : ContentPage
	{
		private const float defaultHungerValue = 90f;
		private const float defaultThirstValue = 90f;
		private const float defaultEngagementValue = 40f;
		private const float defaultLonelinessValue = 0f;
		private const float defaultEnergyValue = 100f;

		public string CreatureCountText { get => "What should your " + CreatureCount + "creature be called?"; }
		private int creatureCount = 0;
		public string CreatureCount => creatureCount + creatureCount switch
		{
			1 => "st",
			2 => "nd",
			3 => "rd",
			>= 4 => "th",
			_ => ""
		};

		private string Name { get; set; }
		private string UserName { get; set; }
		public string CreatureCreatedText { get; set; } = "";

		public NewCreaturePage()
		{
			InitializeComponent();

			creatureCount = Preferences.Get("creatureCount", 0);
		}

		private void OnUserNameEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			UserName = e.NewTextValue;
		}

		private void OnNameEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			Name = e.NewTextValue;
		}

		public async void NewCreature(object sender, EventArgs e)
		{
			creatureCount++;

			IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

			if (await creatureDataStore.CreateItem(new Creature(
					Name,
					UserName,
					defaultHungerValue,
					defaultThirstValue,
					defaultEngagementValue,
					defaultLonelinessValue,
					defaultEnergyValue,
					creatureCount)))
			{
				int id = Preferences.Get("creatureId", 0);
				CreatureCreatedText = $"Creature succesfully created with id: {id}";
			}
			else CreatureCreatedText = "error occured";
		}
	}
}