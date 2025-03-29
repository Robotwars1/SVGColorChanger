using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SVGColorChanger;

public partial class MainPage : ContentPage
{
	public class ColorChange
	{
		public string FromEntryID { get; set; }
        public Color FromColor { get; set; }
        public string ToEntryID { get; set; }
        public Color ToColor { get; set; }
    }

    public ObservableCollection<ColorChange> ColorChanges = [];

	string[] SVGs;

    public MainPage()
	{
		InitializeComponent();

        ColorChanges.Add(new ColorChange() { FromEntryID = $"{ColorChanges.Count}-From", FromColor = Colors.Blue, ToEntryID = $"{ColorChanges.Count}-To", ToColor = Colors.White });
        ColorChangesCollectionView.ItemsSource = ColorChanges;
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
		progressBar.Setup(SVGs.Length);

        // Repeat for every .svg file
        for (int i = 0; i < SVGs.Length; i++)
		{
			string NewText = "";

			// Read the entire file before chaning it
			IEnumerable<string> Lines = File.ReadLines(SVGs[i]);

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
				foreach (ColorChange ColorChange in ColorChanges)
				{
                    // Do said color-switch
                    NewLine = NewLine.Replace($"fill:{ColorChange.FromColor.ToHex().ToLower()}", $"fill:{ColorChange.ToColor.ToHex().ToLower()}");
				}

				// Save the new line for writing
				NewText += NewLine + "\n";
			}

            // Write the new text to the file
            File.WriteAllText(SVGs[i], NewText);

            // Update progress-bar
            progressBar.Increment();
			Debug.WriteLine($"Finished object nr:{i}");
        }
    }

    async void OnFolderSelectClicked(object sender, EventArgs e)
	{
		// Select the folder
		var Result = await FolderPicker.PickAsync(default);

        // Find all .svg files and put them in an array
        SVGs = Directory.GetFiles(Result.Folder.Path, "*.svg");

		// Update info-labels text
		folderNameLabel.Text = Result.Folder.Name;
		filePathLabel.Text = Result.Folder.Path;
		filesFoundLabel.Text = $"{SVGs.Length}";
	}

	void OnAddColorButtonClicked(object sender, EventArgs e)
	{
        ColorChanges.Add(new ColorChange() { FromEntryID = $"{ColorChanges.Count}-From", FromColor = Colors.Blue, ToEntryID = $"{ColorChanges.Count}-To", ToColor = Colors.White });
        ColorChangesCollectionView.ItemsSource = ColorChanges;
    }

	void OnRemoveColorButtonClicked(object sender, EventArgs e)
	{
		if (ColorChanges.Count > 0)
		{
			ColorChanges.RemoveAt(ColorChanges.Count - 1);
            ColorChangesCollectionView.ItemsSource = ColorChanges;
        }
    }

	void OnTextChanged(object sender, TextChangedEventArgs e)
	{
        for (int i = 0; i < ColorChanges.Count; i++)
		{
			if (((Entry)sender).ClassId == ColorChanges[i].FromEntryID)
			{
				try
				{
                    ColorChanges[i].FromColor = Color.FromArgb(e.NewTextValue);
				}
				catch 
				{
					// Handle Error
				}
			}
			else if (((Entry)sender).ClassId == ColorChanges[i].ToEntryID)
			{
                try
                {
                    ColorChanges[i].ToColor = Color.FromArgb(e.NewTextValue);
                }
                catch
                {
                    // Handle Error
                }
            }
        }

        ColorChangesCollectionView.ItemsSource = ColorChanges;
    }
}
