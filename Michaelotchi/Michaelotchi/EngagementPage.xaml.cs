namespace Michaelotchi;

public partial class EngagementPage : ContentPage
{
	public string BtnText { get; set; } = "Click Here";
	int count;

	public EngagementPage()
	{
		BindingContext = this;

		InitializeComponent();
	}

	public async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		string displayNumber;
		if (count % 7 == 0 || count.ToString().Contains('7'))
		{
			displayNumber = "juf";
		}
		else displayNumber = count.ToString();

		string displayText = $"Clicked {displayNumber} time";
		displayText += count == 1 ? "" : "s";

		BtnText = displayText;

		// Animations
		await CounterBtn.RelRotateTo(90.0, 1000, Easing.SpringIn);
		CounterBtn.TranslateTo(.0, -90.0, 1000);
	}
}