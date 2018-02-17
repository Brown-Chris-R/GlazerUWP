using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GlazerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const decimal WIDTH_MIN = 0.5m;
        private const decimal WIDTH_MAX = 5.0m;
        private const decimal HEIGHT_MIN = 0.75m;
        private const decimal HEIGHT_MAX = 3.0m;
        private const decimal METERS_TO_FEET = 3.2808398m;

        public MainPage()
        {
            this.InitializeComponent();
            // Initialize a new empty Dictionary instance
            Dictionary<String, string> colors = new Dictionary<string, string>();

            // Put some key value pairs into the dictionary
            colors.Add("Black", "Black");
            colors.Add("Brown", "Brown");
            colors.Add("Blue", "Blue");

            // Finally, Specify the ComboBox items source
            TintColor.ItemsSource = colors;

            // Specify the ComboBox items text and value
            TintColor.SelectedValuePath = "Value";
            TintColor.DisplayMemberPath = "Key";

            // Set the Default item
            TintColor.SelectedIndex = 0;

            CalculationDate.Text = "";

            // Set the wood frame image and text
            WoodImage.Source = new BitmapImage(new Uri("https://cdn.dick-blick.com/items/188/91/18891-1006-2ww-m.jpg", UriKind.Absolute));
            WoodImage.Visibility = Visibility.Collapsed;
            WoodNeeded.Text = "";
            // Set the glass image and text
            GlassImage.Source = new BitmapImage(new Uri("https://5.imimg.com/data5/MC/CK/MY-24724854/glass-sheet-500x500.jpg", UriKind.Absolute));
            GlassImage.Visibility = Visibility.Collapsed;
            GlassNeeded.Text = "";

        }

        private void WidthInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((!(e.Key == Windows.System.VirtualKey.Number0) && !(e.Key == Windows.System.VirtualKey.Number1) &&
                 !(e.Key == Windows.System.VirtualKey.Number2) && !(e.Key == Windows.System.VirtualKey.Number3) &&
                 !(e.Key == Windows.System.VirtualKey.Number4) && !(e.Key == Windows.System.VirtualKey.Number5) &&
                 !(e.Key == Windows.System.VirtualKey.Number6) && !(e.Key == Windows.System.VirtualKey.Number7) &&
                 !(e.Key == Windows.System.VirtualKey.Number8) && !(e.Key == Windows.System.VirtualKey.Number9) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad0) && !(e.Key == Windows.System.VirtualKey.NumberPad1) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad2) && !(e.Key == Windows.System.VirtualKey.NumberPad3) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad4) && !(e.Key == Windows.System.VirtualKey.NumberPad5) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad6) && !(e.Key == Windows.System.VirtualKey.NumberPad7) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad8) && !(e.Key == Windows.System.VirtualKey.NumberPad9) &&
                 !(e.Key.ToString() == "190") && !(e.Key == Windows.System.VirtualKey.Decimal) && !(e.Key == Windows.System.VirtualKey.Tab)))
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        private void HeightInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((!(e.Key == Windows.System.VirtualKey.Number0) && !(e.Key == Windows.System.VirtualKey.Number1) &&
                 !(e.Key == Windows.System.VirtualKey.Number2) && !(e.Key == Windows.System.VirtualKey.Number3) &&
                 !(e.Key == Windows.System.VirtualKey.Number4) && !(e.Key == Windows.System.VirtualKey.Number5) &&
                 !(e.Key == Windows.System.VirtualKey.Number6) && !(e.Key == Windows.System.VirtualKey.Number7) &&
                 !(e.Key == Windows.System.VirtualKey.Number8) && !(e.Key == Windows.System.VirtualKey.Number9) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad0) && !(e.Key == Windows.System.VirtualKey.NumberPad1) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad2) && !(e.Key == Windows.System.VirtualKey.NumberPad3) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad4) && !(e.Key == Windows.System.VirtualKey.NumberPad5) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad6) && !(e.Key == Windows.System.VirtualKey.NumberPad7) &&
                 !(e.Key == Windows.System.VirtualKey.NumberPad8) && !(e.Key == Windows.System.VirtualKey.NumberPad9) &&
                 !(e.Key.ToString() == "190") && !(e.Key == Windows.System.VirtualKey.Decimal) && !(e.Key == Windows.System.VirtualKey.Tab)))
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }

        }

        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog("");

            // Validate Width
            string widthErrorMessage = "";
            var width = WidthInput.Text;
            bool widthIsValid = ValidWidth(width, out widthErrorMessage);
            if (!widthIsValid)
            {
                messageDialog.Content = widthErrorMessage;
                // Show the message dialog
                await messageDialog.ShowAsync();
            }

            //Validate Height
            string heightErrorMessage = "";
            var height = HeightInput.Text;
            bool heightIsValid = ValidHeight(height, out heightErrorMessage);
            if (!heightIsValid)
            {
                messageDialog.Content = heightErrorMessage;
                // Show the message dialog
                await messageDialog.ShowAsync();
            }

            var tintColor = TintColor.SelectedValue.ToString();
            int quantity = Convert.ToInt16(Quantity.Value);

            if (widthIsValid && heightIsValid)
            {
                CalculationDate.Text = "Materials calculation performed on " + DateTime.Now.ToString("M/d/yyyy");

                decimal woodNeeded = (2 * (Convert.ToDecimal(width) * Convert.ToDecimal(height)) * METERS_TO_FEET) * quantity;
                WoodNeeded.Text = "You will need " + woodNeeded + " feet of wood.";

                WoodImage.Visibility = Visibility.Visible;

                decimal glassNeeded = (2 * (Convert.ToDecimal(width) * Convert.ToDecimal(height))) * quantity;
                GlassNeeded.Text = "You will need " + glassNeeded + " square meters of " + tintColor + " glass.";

                GlassImage.Visibility = Visibility.Visible;
            }

        }

        public bool ValidWidth(string width, out string errorMessage)
        {
            decimal widthNumber;

            // Confirm that the width field is not empty.
            if (width == "")
            {
                errorMessage = "Width is required.";
                return false;
            }

            if (decimal.TryParse(width, out widthNumber))
            {
                // check that the width is between min and max
                if (widthNumber >= WIDTH_MIN && widthNumber <= WIDTH_MAX)
                {
                    errorMessage = "";
                    return true;
                }
                errorMessage = "Width must be between " + WIDTH_MIN +
                    " meters and " + WIDTH_MAX + " meters.";
                return false;
            }
            else
            {
                errorMessage = "Width must be a valid number.";
                return false;
            }
        }

        public bool ValidHeight(string height, out string errorMessage)
        {
            decimal heightNumber;

            // Confirm that the width field is not empty.
            if (height == "")
            {
                errorMessage = "Height is required.";
                return false;
            }

            // check that the height is a valid number
            if (decimal.TryParse(height, out heightNumber))
            {
                // check that the height is between min and max
                if (heightNumber >= HEIGHT_MIN && heightNumber <= HEIGHT_MAX)
                {
                    errorMessage = "";
                    return true;
                }
                errorMessage = "Height must be between " + HEIGHT_MIN +
                    " meters and " + HEIGHT_MAX + " meters.";
                return false;
            }
            else
            {
                errorMessage = "Height must be a valid number.";
                return false;
            }
        }
    }
}
