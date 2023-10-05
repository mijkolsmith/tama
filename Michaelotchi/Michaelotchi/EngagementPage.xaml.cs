namespace Michaelotchi;

public partial class EngagementPage : ContentPage
{
	public string TitleText { get; set; } = "\n \n";
	public string CountText { get; set; } = "\n";
	int count;
	Creature Creature { get; set; }

	public EngagementPage(Creature creature)
	{
		BindingContext = this;

		InitializeComponent();

		Creature = creature;
		TitleText = $"\n Play juffen with {Creature.Name}! \n Press the juf button every time a 7 will occur in the next number.";
	}

	public void OnCounterClicked(object sender, EventArgs e)
	{
		if (Count(false))
		{
			string displayText = $"Clicked {count} time";
			displayText += (count == 1 ? "" : "s") + "\n";
			CountText = displayText;
		}
		else ResetCount();
	}

	public void OnJufClicked(object sender, EventArgs e)
	{
		if (Count(true))
		{
			if (Creature.Tired > 10)
			{
				CountText = $"Juf! {Creature.Name} liked that. \n";
				Creature.Engagement += 2;
			}
			else CountText = "Juf!";
		}
		else ResetCount();
	}

	public bool Count(bool juf)
	{
		count++;
		if (count % 7 == 0 || count.ToString().Contains('7'))
		{
			return juf;
		}
		return !juf;
	}

	public void ResetCount()
	{
		if (Creature.Tired > 0)
		{
			Creature.Tired -= 5;
		}

		CountText = $"WRONG! {Creature.UserName} is clearly a bad pet owner. \n";
		count = 0;
	}
}