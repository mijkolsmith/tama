namespace Michaelotchi;

public partial class HungerPage : ContentPage
{
	Creature Creature { get; set; }

	public HungerPage(Creature creature)
	{
		InitializeComponent();

		Creature = creature;
	}

	public void FeedCreature(object sender, EventArgs e)
	{
		Creature.Hunger += 10;
	}
}