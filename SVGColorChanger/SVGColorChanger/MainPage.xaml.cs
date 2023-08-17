using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;

namespace SVGColorChanger;

public partial class MainPage : ContentPage
{
	public class ColorChanger
	{
		public string PlaceholderText { get; set; }
		public string FromId { get; set; }
		public string ToId { get; set; }
	}

	public class Preview
	{
        public Color FromColor { get; set; }
        public Color ToColor { get; set; }
    }

    // Creates Collections of each class to populate each CollectionView
    ObservableCollection<ColorChanger> svgColorChanger = new ObservableCollection<ColorChanger>();
    public ObservableCollection<ColorChanger> SVGColorChanger { get { return svgColorChanger; } }

	ObservableCollection<Preview> colorPreview = new ObservableCollection<Preview>();
	public ObservableCollection<Preview> ColorPreview { get { return colorPreview; } }

    public MainPage()
	{
		InitializeComponent();

		mainPage.BackgroundColor = Color.FromRgba("#121212");

        colorPreview.Add(new Preview() { FromColor = Colors.Blue, ToColor = Colors.White });
        colorPreviewCollectionView.ItemsSource = colorPreview;

        svgColorChanger.Add(new ColorChanger() { PlaceholderText = "Press to type", FromId = $"{svgColorChanger.Count}-From", ToId = $"{svgColorChanger.Count}-To" });
        colorInputCollectionView.ItemsSource = svgColorChanger;
    }

	void OnApplyButtonClicked(object sender, EventArgs e)
	{

	}

	async void OnFolderSelectClicked(object sender, EventArgs e)
	{
		// Select the folder
		var folder = await FolderPicker.PickAsync(default);

		// Find all .svg files and put them in an array
		string[] svgs = Directory.GetFiles(folder.Folder.Path, "*.svg");

		// Update info-labels text
		folderNameLabel.Text = folder.Folder.Name;
		filePathLabel.Text = folder.Folder.Path;
		filesFoundLabel.Text = $"{svgs.Count()}";
	}

	void OnAddColorButtonClicked(object sender, EventArgs e)
	{
        colorPreview.Add(new Preview() { FromColor = Colors.Blue, ToColor = Colors.White });
        colorPreviewCollectionView.ItemsSource = colorPreview;

        svgColorChanger.Add(new ColorChanger() { PlaceholderText = "Press to type", FromId = $"{svgColorChanger.Count}-From", ToId = $"{svgColorChanger.Count}-To" });
        colorInputCollectionView.ItemsSource = svgColorChanger;
    }

	void OnRemoveColorButtonClicked(object sender, EventArgs e)
	{
        colorPreview.RemoveAt(colorPreview.Count - 1);
        colorPreviewCollectionView.ItemsSource = colorPreview;

        svgColorChanger.RemoveAt(svgColorChanger.Count - 1);
        colorInputCollectionView.ItemsSource = svgColorChanger;
    }

	void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		// Arrays to temporarily store colors
        Color[] TempFromColor = new Color[svgColorChanger.Count];
        Color[] TempToColor = new Color[svgColorChanger.Count];

        // Set color-input into ObservableCollection FromColor and ToColor
        for (int i = 0; i < svgColorChanger.Count; i++)
		{
			if (((Entry)sender).ClassId == svgColorChanger[i].FromId)
			{
				try
				{
					colorPreview[i].FromColor = Color.FromArgb(e.NewTextValue);
				}
				catch 
				{
					// Handle Error
				}
			}
			else if (((Entry)sender).ClassId == svgColorChanger[i].ToId)
			{
                try
                {
                    colorPreview[i].ToColor = Color.FromArgb(e.NewTextValue);
                }
                catch
                {
                    // Handle Error
                }
            }
			try
			{
				// Temporary stores color(s)
				TempFromColor[i] = Colors.Blue; //colorPreview[i].FromColor;
				TempToColor[i] = Colors.Blue; //colorPreview[i].ToColor;
            }
			catch
			{
				// Handle Error
			}
		}

        // Dumb workaround to update color of BoxView
		colorPreview.Clear();
		for (int i = 0; i < colorPreview.Count; i++)
		{
			//colorPreview.Add(new Preview() { FromColor = TempFromColor[i], ToColor = TempToColor[i] });
			colorPreview.Add(new Preview() { FromColor = Colors.Red, ToColor = Colors.Red });
        }
		colorPreviewCollectionView.ItemsSource = colorPreview;
    }

	void OnTextCompleted(object sender, EventArgs e)
	{
		// To-Do
	}
}
