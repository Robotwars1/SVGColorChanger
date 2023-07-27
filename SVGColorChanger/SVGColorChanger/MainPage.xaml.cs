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
		public Color FromColor { get; set; }
		public Color ToColor { get; set; }
	}

    // Creates Collections of each class to populate each CollectionView
    ObservableCollection<ColorChanger> svgColorChanger = new ObservableCollection<ColorChanger>();
    public ObservableCollection<ColorChanger> SVGColorChanger { get { return svgColorChanger; } }

    public MainPage()
	{
		InitializeComponent();

		mainPage.BackgroundColor = Color.FromRgba("#121212");

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
        svgColorChanger.Add(new ColorChanger() { PlaceholderText = "Press to type", FromId = $"{svgColorChanger.Count}-From", ToId = $"{svgColorChanger.Count}-To" });
        colorInputCollectionView.ItemsSource = svgColorChanger;
	}

	void OnRemoveColorButtonClicked(object sender, EventArgs e)
	{
		svgColorChanger.RemoveAt(svgColorChanger.Count - 1);
        colorInputCollectionView.ItemsSource = svgColorChanger;
    }

	void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		// Dumb workaround to update color of BoxView
		int a = svgColorChanger.Count;
		svgColorChanger.Clear();
		for (int i = 0; i < a; i++)
		{
			svgColorChanger.Add(new ColorChanger() { PlaceholderText = "Press to type", FromId = $"{svgColorChanger.Count}-From", ToId = $"{svgColorChanger.Count}-To", FromColor = Colors.Blue });
		}
	}

	void OnTextCompleted(object sender, EventArgs e)
	{
		// To-Do
	}

	/*
	void OnEditorTextChanged(object sender, TextChangedEventArgs e)
	{
		Console.WriteLine("aaaaaaaaaa");
		/*
        for (int i = 0; i < svgColorChanger.Count; i++)
		{
            if (((Editor)sender).ClassId == svgColorChanger[i].FromId)
			{
				try
				{
                    svgColorChanger[i].FromColor = Color.FromArgb(((Editor)sender).Text);
                }
				catch 
				{
					// To-Do
				}
				
			}
			else if (((Editor)sender).ClassId == svgColorChanger[i].ToId)
			{
				try
				{
                    svgColorChanger[i].ToColor = Color.FromArgb(((Editor)sender).Text);
                }
				catch
				{
					// To-Do
				}
			}
		}
		
	}
	*/
}
