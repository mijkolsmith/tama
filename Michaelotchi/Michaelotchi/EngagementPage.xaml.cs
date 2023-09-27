namespace Michaelotchi;

public partial class EngagementPage : ContentPage
{
	public string ButtonText { get; set; } = "Click Here";
	int count;

	public EngagementPage(Creature creature)
	{
		BindingContext = this;

		InitializeComponent();
	}

	public void OnCounterClicked(object sender, EventArgs e)
	{
		if (Count(false))
		{
			string displayText = $"Clicked {count} time";
			displayText += count == 1 ? "" : "s";

			ButtonText = displayText;
		}
		else ResetCount();

		// Animations
		//await CounterBtn.RelRotateTo(90.0, 1000, Easing.SpringIn);
		//CounterBtn.TranslateTo(.0, 90.0, 1000);
	}

	public void OnJufClicked(object sender, EventArgs e)
	{
		if (Count(true))
		{
			ButtonText = "juf";
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
		count = 0;
	}
}