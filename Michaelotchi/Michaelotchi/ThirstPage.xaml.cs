namespace Michaelotchi;

public partial class ThirstPage : ContentPage
{
	Creature Creature { get; set; }
	public string TitleText { get; set; }
	public string NotificationText { get; set; }

	public ThirstPage(Creature creature)
	{
		BindingContext = this;
		InitializeComponent();

		Creature = creature;
		TitleText = $"Give {Creature.Name} water!";
	}

	public void WaterCreature(object sender, EventArgs e)
	{
		if (Creature.Thirst < 90 && Creature.Tired > 10)
		{
			Creature.Thirst += 10;
			Creature.Tired -= 5;
			NotificationText = $"{Creature.Name} was given water!";
		}
		else NotificationText = $"{Creature.Name} is not thirsty.";
	}
}
