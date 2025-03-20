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
    ObservableCollection<ColorChanger> svgColorChanger = new();
    public ObservableCollection<ColorChanger> SVGColorChanger { get { return svgColorChanger; } }

	ObservableCollection<Preview> colorPreview = new();
	public ObservableCollection<Preview> ColorPreview { get { return colorPreview; } }

	string[] svgs;
	string filePath;

    public MainPage()
	{
		InitializeComponent();

		mainPage.BackgroundColor = Color.FromRgba("#121212");

        colorPreview.Add(new Preview() { FromColor = Colors.Blue, ToColor = Colors.White });
        colorPreviewCollectionView.ItemsSource = colorPreview;

        svgColorChanger.Add(new ColorChanger() { FromId = $"{svgColorChanger.Count}-From", ToId = $"{svgColorChanger.Count}-To" });
        colorInputCollectionView.ItemsSource = svgColorChanger;
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
				foreach (Preview ColorPreview in colorPreview)
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
        colorPreview.Add(new Preview() { FromColor = Colors.Blue, ToColor = Colors.White });
        colorPreviewCollectionView.ItemsSource = colorPreview;

        svgColorChanger.Add(new ColorChanger() { FromId = $"{svgColorChanger.Count}-From", ToId = $"{svgColorChanger.Count}-To" });
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
				TempFromColor[i] = colorPreview[i].FromColor;
				TempToColor[i] = colorPreview[i].ToColor;
            }
			catch
			{
				// Handle Error
			}
		}

        // Dumb workaround to update color of BoxView
        int temp = colorPreview.Count; // Store value before resetting
        colorPreview.Clear();
        for (int i = 0; i < temp; i++)
        {
            colorPreview.Add(new Preview() { FromColor = TempFromColor[i], ToColor = TempToColor[i] });
        }
        colorPreviewCollectionView.ItemsSource = colorPreview;
    }

	void OnTextCompleted(object sender, EventArgs e)
	{
		// To-Do
	}
}
