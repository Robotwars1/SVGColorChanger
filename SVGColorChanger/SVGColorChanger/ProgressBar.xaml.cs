namespace SVGColorChanger;

public partial class ProgressBar : ContentView
{
	private int ItemCount = 0;
	private int CurrentItem = 1;

	public ProgressBar()
	{
		InitializeComponent();
	}

	public void Setup(int _ItemCount)
	{
		ItemCount = _ItemCount;
	}

	public void Increment()
	{
		// When bar is filled, dont fill it more
		if (CurrentItem == ItemCount)
		{
			return;
		}

		CurrentItem++;

		ColumnDefinition FilledArea = new(new GridLength(CurrentItem, GridUnitType.Star));
		ColumnDefinition UnfilledArea = new(new GridLength(ItemCount - CurrentItem, GridUnitType.Star));

		ProgressGrid.ColumnDefinitions = new ColumnDefinitionCollection(FilledArea, UnfilledArea);
	}
}
