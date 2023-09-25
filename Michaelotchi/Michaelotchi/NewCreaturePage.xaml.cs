namespace Michaelotchi
{
	public partial class NewCreaturePage : ContentPage
	{
		private const float defaultHungerValue = 90f;
		private const float defaultThirstValue = 90f;
		private const float defaultEngagementValue = 40f;
		private const float defaultLonelinessValue = 0f;
		private const float defaultEnergyValue = 100f;

		private int creatureCount = 0;
		public string CreatureCount => creatureCount + creatureCount switch
		{
			1 => "st",
			2 => "nd",
			3 => "rd",
			>= 4 => "th",
			_ => ""
		};

		private string name;
		public string CreatureCreatedText { get; set; } = "";

		public NewCreaturePage()
		{
			InitializeComponent();

			creatureCount = Preferences.Get("creatureCount", 0);
		}

		private void OnEntryCompleted(object sender, EventArgs e)
		{
			name = ((Entry)sender).Text;
		}

		public void NewCreature(object sender, EventArgs e)
		{
			creatureCount++;

			IDataStore <Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

			creatureDataStore.CreateItem(
				new Creature(	name,
								defaultHungerValue,
								defaultThirstValue,
								defaultEngagementValue,
								defaultLonelinessValue,
								defaultEnergyValue,
								creatureCount));

			CreatureCreatedText = "Creature succesfully created!";
		}
	}
}