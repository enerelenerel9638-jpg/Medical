using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicialUse.Components
{
    public enum KeyboardType
    {
        Digit,
        English,
        Mongolian,
        EnglishUpper,
        EnglishLower
    }

    public sealed partial class Keyboard : UserControl
    {
        public static readonly DependencyProperty KeyboardTypeProperty =
        DependencyProperty.Register(
            nameof(KeyboardType),
            typeof(KeyboardType),
            typeof(Keyboard),
            new PropertyMetadata(KeyboardType.Digit, OnKeyboardTypeChanged));

        public KeyboardType KeyboardType
        {
            get => (KeyboardType)GetValue(KeyboardTypeProperty);
            set => SetValue(KeyboardTypeProperty, value);
        }

        private static void OnKeyboardTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Keyboard)d;
            var newKeyboardType = (KeyboardType)e.NewValue;
            control.SetKeyboardType(newKeyboardType);
        }

        public KeyboardType CurrentKeyboardType { get; private set; }

        public event Action<string> KeyPressed;
        private readonly List<Border> removeKeyboardKey = new List<Border>();
        private readonly List<Border> chosenKeyboardKey = new List<Border>();
        private static readonly Color customBackgroundColor = Color.FromArgb(255, 171, 195, 223);

        private readonly string[] numericKeys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "Del" };
        private readonly string[] englishAlphabet = {
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
        "Q","W","E","R","T","Y","U","I","O","P"
        ,"A","S","D","F","G","H","J","K","L","Z"
        ,"X","C","V","B","N","M","@",".","Del"
        };

        private readonly string[] englishAlphabetUpper = {
        "1","2","3","4","5","6","7","8","9","0",
        "Q","W","E","R","T","Y","U","I","O","P",
        "A","S","D","F","G","H","J","K","L","Z",
        "CapsLock","X","C","V","B","N","M","Del"
        };

        private readonly string[] englishAlphabetLower = {
        "1","2","3","4","5","6","7","8","9","0",
        "q","w","e","r","t","y","u","i","o","p",
        "a","s","d","f","g","h","j","k","l","z",
        "CapsLock","x","c","v","b","n","m","Del"
        };

        private readonly string[] alphabetMongolian = {
         "Ф","Ц","У","Ж","Э","Н","Г","Ш","Ү","З",
         "К","Ъ","Й","Ы","Б","Ө","А","Х","Р","О",
         "Л","Д","П","Я","Ч","Ё","С","М","И","Т",
         "Ь","В","Ю","Del","Е","Щ",
        };

        public Keyboard()
        {
            this.InitializeComponent();
            SetKeyboardType(KeyboardType.Digit);
        }

        public void SetKeyboardType(KeyboardType type)
        {
            CurrentKeyboardType = type;
            ClearKeyboard();
            RunVirtualKeyboard();
        }

        private void ClearKeyboard()
        {
            keyboardRow1.Children.Clear();
            keyboardRow2.Children.Clear();
            keyboardRow3.Children.Clear();
            keyboardRow4.Children.Clear();
            removeKeyboardKey.Clear();
            chosenKeyboardKey.Clear();
        }

        private void RunVirtualKeyboard()
        {
            string[] keys;

            switch (CurrentKeyboardType)
            {
                case KeyboardType.Digit:
                    keys = numericKeys;
                    break;
                case KeyboardType.Mongolian:
                    keys = alphabetMongolian;
                    break;
                case KeyboardType.English:
                    keys = englishAlphabet;
                    break;
                case KeyboardType.EnglishLower:
                    keys = englishAlphabetLower;
                    break;
                case KeyboardType.EnglishUpper:
                    keys = englishAlphabetUpper;
                    break;
                default:
                    keys = numericKeys;
                    break;
            }

            for (int i = 0; i < keys.Length; i++)
            {
                Border keyboardKey = CreateKeyboardKey(keys[i], i.ToString());
                AddKeyToGrid(keyboardKey, i);
            }
        }

        private Border CreateKeyboardKey(string key, string uid)
        {
            int width = 60;

            switch (CurrentKeyboardType)
            {
                case KeyboardType.Digit:
                    width = key == "Del" && CurrentKeyboardType == KeyboardType.Digit ? 295 : 145;
                    break;
                case KeyboardType.English:
                    width = key == "Del" ? 125 : 60;
                    break;
                case KeyboardType.Mongolian:
                    width = key == "Del" ? 315 : 60;
                    break;
            }

            Border keyboardKey = new Border
            {
                Width = width,
                Height = 60,
                Margin = new Thickness(2, 3, 2, 3),
                Background = new SolidColorBrush(customBackgroundColor),
                CornerRadius = new CornerRadius(5),
                Tag = key,
                Name = "Key" + uid
            };

            keyboardKey.MouseDown += KeyboardPressed;

            TextBlock text = new TextBlock
            {
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 23,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Colors.Black),
                Text = key
            };

            if (key.Equals("Del"))
            {
                try
                {
                    Image image = new Image
                    {
                        Height = 40,
                        Source = new BitmapImage(new Uri("pack://application:,,,/images/delete-icon.png"))
                    };
                    keyboardKey.Child = image;
                }
                catch
                {
                    keyboardKey.Child = text;
                }
            }
            else if (key.Equals("CapsLock"))
            {
                try
                {
                    Image image = new Image
                    {
                        Height = 25,
                        Source = new BitmapImage(new Uri("pack://application:,,,/images/up-arrow.png"))
                    };
                    keyboardKey.Child = image;
                }
                catch
                {
                    keyboardKey.Child = text;
                }
            }
            else
            {
                keyboardKey.Child = text;
            }

            return keyboardKey;
        }

        private void AddKeyToGrid(Border keyboardKey, int index)
        {
            if (CurrentKeyboardType == KeyboardType.Digit)
            {
                if (index < 3)
                {
                    keyboardRow1.Children.Add(keyboardKey);
                }
                else if (index < 6)
                {
                    keyboardRow2.Children.Add(keyboardKey);
                }
                else if (index < 9)
                {
                    keyboardRow3.Children.Add(keyboardKey);
                }
                else
                {
                    keyboardRow4.Children.Add(keyboardKey);
                }
            }
            else if (CurrentKeyboardType == KeyboardType.English)
            {
                // English keyboard
                if (index < 10)
                {
                    keyboardRow1.Children.Add(keyboardKey);
                }
                else if (index < 20)
                {
                    keyboardRow2.Children.Add(keyboardKey);
                }
                else if (index < 30)
                {
                    keyboardRow3.Children.Add(keyboardKey);
                }
                else
                {
                    keyboardRow4.Children.Add(keyboardKey);
                }
            }
            else
            {
                // Mongolian keyboard
                if (index < 10)
                {
                    keyboardRow1.Children.Add(keyboardKey);
                }
                else if (index < 20)
                {
                    keyboardRow2.Children.Add(keyboardKey);
                }
                else if (index < 30)
                {
                    keyboardRow3.Children.Add(keyboardKey);
                }
                else if (index < 39)
                {
                    keyboardRow4.Children.Add(keyboardKey);
                }
            }

            removeKeyboardKey.Add(keyboardKey);
            chosenKeyboardKey.Add(keyboardKey);
        }

        private void KeyboardPressed(object sender, MouseButtonEventArgs e)
        {
            Border btn = sender as Border;
            string key = (string)btn.Tag;
            string index = btn.Name.Substring(3);

            if (key.Equals("Del"))
            {
                KeyPressed?.Invoke("BACKSPACE");
            }
            else if (key.Equals("CapsLock"))
            {
                KeyPressed?.Invoke("CAPSLOCK");
            }
            else
            {
                KeyPressed?.Invoke(key);
            }
        }
    }
}
