namespace Michaelotchi;

public partial class ThirstPage : ContentPage
{
	Creature Creature { get; set; }
	public string NotificationText { get; set; }

	public ThirstPage(Creature creature)
	{
		InitializeComponent();

		Creature = creature;
	}

	public void WaterCreature(object sender, EventArgs e)
	{
		if (Creature.Thirst < 90 && Creature.Tired > 10)
		{
			Creature.Thirst += 10;
			Creature.Tired -= 5;
			NotificationText = "Michael was given water!";
		}
		else NotificationText = "Michael is not thirsty.";
	}

	protected override async void OnDisappearing()
	{
		IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
		await creatureDataStore.UpdateItem(Creature);

		base.OnDisappearing();
	}
}