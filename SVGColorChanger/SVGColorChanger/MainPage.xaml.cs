using System.Collections.ObjectModel;

namespace SVGColorChanger;

public partial class MainPage : ContentPage
{
	public class ColorChanger
	{
		public string PlaceholderText { get; set; }
	}

    // Creates Collections of each class to populate each CollectionView
    ObservableCollection<ColorChanger> svgColorChanger = new ObservableCollection<ColorChanger>();
    public ObservableCollection<ColorChanger> SVGColorChanger { get { return svgColorChanger; } }

    public MainPage()
	{
		InitializeComponent();

		mainPage.BackgroundColor = Color.FromRgba("#121212");

		svgColorChanger.Add(new ColorChanger() { PlaceholderText = "Press to type" });
        colorInputCollectionView.ItemsSource = svgColorChanger;
    }

	void OnApplyButtonClicked(object sender, EventArgs e)
	{

	}
}
