namespace Michaelotchi;

public partial class ThirstPage : ContentPage
{
	Creature Creature { get; set; }

	public ThirstPage(Creature creature)
	{
		InitializeComponent();

		Creature = creature;
	}

	public void WaterCreature(object sender, EventArgs e)
	{
		Creature.Thirst += 10;
	}
}