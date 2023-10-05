namespace Michaelotchi;

public partial class HungerPage : ContentPage
{
	Creature Creature { get; set; }
	public string TitleText { get; set; }
	public string NotificationText { get; set; }

	public HungerPage(Creature creature)
	{
		BindingContext = this;
		InitializeComponent();

		Creature = creature;
		TitleText = $"Feed {Creature.Name}!";
	}

	public void FeedCreature(object sender, EventArgs e)
	{
		if (Creature.Hunger < 90 && Creature.Tired > 10)
		{
			Creature.Hunger += 10;
			Creature.Tired -= 5;
			NotificationText = $"{Creature.Name} was fed!";
		}
		else NotificationText = $"{Creature.Name} is not hungry.";
	}
}
