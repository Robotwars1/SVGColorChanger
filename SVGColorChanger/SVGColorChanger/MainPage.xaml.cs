using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SVGColorChanger;

public partial class MainPage : ContentPage
{
	public class ColorChanger
	{
		public string FromId { get; set; }
		public string ToId { get; set; }
	}

	public class Preview
	{
        public Color FromColor { get; set; }
        public Color ToColor { get; set; }
    }

    // Creates Collections of each class to populate each CollectionView
    public ObservableCollection<ColorChanger> SVGColorChanger = new();
	public ObservableCollection<Preview> ColorPreview = new();

	string[] svgs;
	string filePath;

    public MainPage()
	{
		InitializeComponent();

		mainPage.BackgroundColor = Color.FromRgba("#121212");

        ColorPreview.Add(new Preview() { FromColor = Colors.Blue, ToColor = Colors.White });
        colorPreviewCollectionView.ItemsSource = ColorPreview;

        SVGColorChanger.Add(new ColorChanger() { FromId = $"{SVGColorChanger.Count}-From", ToId = $"{SVGColorChanger.Count}-To" });
        colorInputCollectionView.ItemsSource = SVGColorChanger;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Window.MinimumWidth = 1280;
        Window.MaximumWidth = 1280;
        Window.MinimumHeight = 720;
        Window.MaximumHeight = 720;
        Window.Title = "SVGColorChanger";
    }

    void OnApplyButtonClicked(object sender, EventArgs e)
	{
		progressBar.Setup(svgs.Length);

        // Repeat for every .svg file
        for (int i = 0; i < svgs.Length; i++)
		{
			string NewText = "";

			// Read the entire file before chaning it
			IEnumerable<string> Lines = File.ReadLines(svgs[i]);

            // Go through the file line by line, replacing colors as found
            // If line has #xxxxxx (hex code color), replace with new hex code
            foreach (string Line in Lines)
            {
				// If Line is empty or just newline, ignore it for the switched file
				if (Line == "" || Line == "\n")
				{
					continue;
				}

				string NewLine = Line;

				// Go through every color-switch
				foreach (Preview ColorPreview in ColorPreview)
				{
                    // Do said color-switch
                    NewLine = NewLine.Replace($"fill:{ColorPreview.FromColor.ToHex().ToLower()}", $"fill:{ColorPreview.ToColor.ToHex().ToLower()}");
				}

				// Save the new line for writing
				NewText += NewLine + "\n";
			}

            // Write the new text to the file
            File.WriteAllText(svgs[i], NewText);

            // Update progress-bar
            progressBar.Increment();
			Debug.WriteLine($"Finished object nr:{i}");
        }
    }

    async void OnFolderSelectClicked(object sender, EventArgs e)
	{
		// Select the folder
		var folder = await FolderPicker.PickAsync(default);

		// Find all .svg files and put them in an array
		svgs = Directory.GetFiles(folder.Folder.Path, "*.svg");

        // Save FilePath for changing .svg files
        filePath = folder.Folder.Path;

		// Update info-labels text
		folderNameLabel.Text = folder.Folder.Name;
		filePathLabel.Text = folder.Folder.Path;
		filesFoundLabel.Text = $"{svgs.Length}";
	}

	void OnAddColorButtonClicked(object sender, EventArgs e)
	{
        ColorPreview.Add(new Preview() { FromColor = Colors.Blue, ToColor = Colors.White });
        colorPreviewCollectionView.ItemsSource = ColorPreview;

        SVGColorChanger.Add(new ColorChanger() { FromId = $"{SVGColorChanger.Count}-From", ToId = $"{SVGColorChanger.Count}-To" });
        colorInputCollectionView.ItemsSource = SVGColorChanger;
    }

	void OnRemoveColorButtonClicked(object sender, EventArgs e)
	{
		if (ColorPreview.Count > 0)
		{
            ColorPreview.RemoveAt(ColorPreview.Count - 1);
            colorPreviewCollectionView.ItemsSource = ColorPreview;

            SVGColorChanger.RemoveAt(SVGColorChanger.Count - 1);
            colorInputCollectionView.ItemsSource = SVGColorChanger;
        }
    }

	void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		// Arrays to temporarily store colors
        Color[] TempFromColor = new Color[SVGColorChanger.Count];
        Color[] TempToColor = new Color[SVGColorChanger.Count];
		
        // Set color-input into ObservableCollection FromColor and ToColor
        for (int i = 0; i < SVGColorChanger.Count; i++)
		{
			if (((Entry)sender).ClassId == SVGColorChanger[i].FromId)
			{
				try
				{
					ColorPreview[i].FromColor = Color.FromArgb(e.NewTextValue);
				}
				catch 
				{
					// Handle Error
				}
			}
			else if (((Entry)sender).ClassId == SVGColorChanger[i].ToId)
			{
                try
                {
                    ColorPreview[i].ToColor = Color.FromArgb(e.NewTextValue);
                }
                catch
                {
                    // Handle Error
                }
            }
			try
			{
				// Temporary stores color(s)
				TempFromColor[i] = ColorPreview[i].FromColor;
				TempToColor[i] = ColorPreview[i].ToColor;
            }
			catch
			{
				// Handle Error
			}
		}

        // Dumb workaround to update color of BoxView
        int temp = ColorPreview.Count; // Store value before resetting
        ColorPreview.Clear();
        for (int i = 0; i < temp; i++)
        {
            ColorPreview.Add(new Preview() { FromColor = TempFromColor[i], ToColor = TempToColor[i] });
        }
        colorPreviewCollectionView.ItemsSource = ColorPreview;
    }

	void OnTextCompleted(object sender, EventArgs e)
	{
		// To-Do
	}
}
