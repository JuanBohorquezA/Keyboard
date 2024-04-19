using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Keyboard
{
    /// <summary>
    /// Lógica de interacción para Keyboard.xaml
    /// </summary>
    public partial class KeyboardComponent : UserControl
    {
        private TextBox? _currentTextBox;

        private bool _isFilled = true;
        private bool _isNumeric = true;


        public KeyboardComponent()
        {

            InitializeComponent();
            
        }




        #region TouchEvents
        private void HandleKeyboardType(object sender, MouseEventArgs e)
        {
            Button parent = (Button)sender;
            _currentTextBox!.Text += parent.Content.ToString();
        }

        private void HandleKeyboardDelete(object sender, MouseEventArgs e)
        {
            _currentTextBox!.Text = !string.IsNullOrEmpty(_currentTextBox.Text) ? _currentTextBox.Text.Substring(0, _currentTextBox.Text.Length - 1) : _currentTextBox.Text;
        }

        private void HandleKeyboardEnter(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            _currentTextBox = new TextBox();
        }
        private void HandleKeyboardUpperCase(object sender, MouseEventArgs e)
        {
            Button buttonSender = (Button)sender;
            buttonSender.ContentTemplate = _isFilled ? (DataTemplate)Application.Current.Resources["ArrowFilled"] : (DataTemplate)Application.Current.Resources["Arrow"];
            var contentString = KeyboardGrid.Children.OfType<Grid>().SelectMany(grid => grid.Children.OfType<Button>()).Where(button => button.Content is string);


            foreach (var button in contentString)
            {
                string? content = button.Content as string;
                if (content == null) continue;
                if (double.TryParse(content, out _)) return;
                button.Content = _isFilled ? content.ToUpper() : content.ToLower();
            }
            _isFilled = !_isFilled;


        }

        private void UpdateKeyboardContent(object sender, MouseEventArgs e)
        {
            var contentString = KeyboardGrid.Children.OfType<Grid>().SelectMany(grid => grid.Children.OfType<Button>()).Where(button => button.Content is string);
            List<string> keys = getKeys();

            int index = 0;
            foreach (var button in contentString)
            {
                string? content = button.Content as string;
                if (content == null || button.Name == "space") continue;
                button.Content = keys[index];
                index++;
            }
            _isNumeric = !_isNumeric;
        }

        #endregion




        #region Focus events
        public void InitializeKeyboard(FrameworkElement element)
        {
            (element.GetChildObjects().FirstOrDefault() as Panel)?.Children.Add(this);
            foreach (var textBox in element.FindChildren<TextBox>())
            {
                textBox.GotFocus += TextBox_GotFocus;
            }
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentTextBox = (TextBox)sender;
            KeyboardSetDimensionsAndPosition();

        }



        #endregion



        #region KeyboardServices



        private void KeyboardSetDimensionsAndPosition()
        {
            // Obtener las coordenadas del TextBox en relación con la ventana principal
            Point textBoxScreenCoordinates = _currentTextBox!.PointToScreen(new Point(0, 0));

            // Calcular la posición Y del teclado virtual 5 píxeles por debajo del TextBox
            double offsetY = textBoxScreenCoordinates.Y * 0.1 + _currentTextBox.ActualHeight + 5;

            // Establecer la posición del teclado virtual
            this.RenderTransform = new TranslateTransform(0, offsetY);

            // Mostrar el teclado virtual
            this.Visibility = Visibility.Visible;

            // Establecer el tamaño del teclado virtual
            this.Width = _currentTextBox.Width * 0.8;
            this.Height = this.Width * 0.4;
        }
        private List<string> getKeys()
        {
            var keys = new List<string>();

            List<string> words = new List<string>()
            {
                "q","w","e","r","t","y","u","i","o","p",
                "a","s","d","f","g","h","j","k","l","ñ",
                "z","x","c","v","b","n","m",
                ",","@","."
            };

            List<string> numsAndSymbols = new List<string>
            {
                "1","2","3","4","5","6","7","8","9","0",
                "@","#","$","_","&","-","+","(",")","/",
                "*","\"","\'",":",";","!","?",
                ",","@","."
            };

            keys = _isNumeric ? numsAndSymbols : words;

            return keys;
        }

        #endregion


    }
}
