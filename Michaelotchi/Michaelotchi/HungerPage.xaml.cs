namespace Michaelotchi;

public partial class HungerPage : ContentPage
{
	Creature Creature { get; set; }
	public string NotificationText { get; set; }

	public HungerPage(Creature creature)
	{
		BindingContext = this;
		InitializeComponent();

		Creature = creature;
	}

	public void FeedCreature(object sender, EventArgs e)
	{
		if (Creature.Hunger < 90 && Creature.Tired > 10)
		{
			Creature.Hunger += 10;
			Creature.Tired -= 5;
			NotificationText = "Michael was fed!";
		}
		else NotificationText = "Michael is not hungry.";
	}

	protected override async void OnDisappearing()
	{
		IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
		await creatureDataStore.UpdateItem(Creature);

		base.OnDisappearing();
	}
}