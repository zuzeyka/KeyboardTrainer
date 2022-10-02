using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private string charsCaseSensetive = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
        private string charsCaseUnsensitive = "qwertyuiopasdfghjklzxcvbnm";
        private bool isShiftPressed = false;
        private string inputText = "";
        private string enteredText = "";
        private Random random = new Random();
        private Dictionary<Key, char> keyCharDict;
        private Dictionary<char, Rectangle> charRect;
        private int failsCounter = 0;
        private DispatcherTimer timer;
        private DateTime start;
        private int charsCount = 0;
        private int speed = 0;
        public MainWindow()
        {
            InitializeComponent();

            // initial dict keys-chars
            keyCharDict = new Dictionary<Key, char>();
            for (int i = 44; i <= 69; i++)
                keyCharDict.Add((Key)i, (char)(i + 21));

            // init dict chars-rects
            charRect = new Dictionary<char, Rectangle>();
            charRect.Add('q', qR);
            charRect.Add('`', apostropheR);
            charRect.Add('w', wR); 
            charRect.Add('e', eR);
            charRect.Add('r', rR);
            charRect.Add('t', tR);
            charRect.Add('y', yR);
            charRect.Add('u', uR);
            charRect.Add('i', iR);
            charRect.Add('o', oR);
            charRect.Add('p', pR);
            charRect.Add('a', aR);
            charRect.Add('s', sR);
            charRect.Add('d', dR);
            charRect.Add('f', fR);
            charRect.Add('g', gR);
            charRect.Add('h', hR);
            charRect.Add('j', jR); 
            charRect.Add('k', kR);
            charRect.Add('l', lR);
            charRect.Add('z', zR);
            charRect.Add('x', xR);
            charRect.Add('c', cR);
            charRect.Add('v', vR);
            charRect.Add('b', bR);
            charRect.Add('n', nR);
            charRect.Add('m', mR);

            // init timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            speed = (int)(charsCount / ((DateTime.Now - start).TotalMinutes));
            SpeedTB.Text = "Speed: " + speed.ToString() + "chars/min";
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                isShiftPressed = true;

            if (keyCharDict.ContainsKey(e.Key))
            {
                if (caseSensCB.IsChecked == true)
                {
                    if ((keyCharDict[e.Key] == inputText[0] && isShiftPressed) || keyCharDict[e.Key] == Convert.ToChar(inputText[0] - 32))
                    {
                        charRect[inputText.ToLower()[0]].StrokeThickness = 3;
                        charRect[inputText.ToLower()[0]].Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#4A2940");
                        enteredText += inputText[0];
                        inputText = inputText.Remove(0, 1);
                        inputTextTB.Text = inputText;
                        if (enteredText.Length == 32)
                            enteredText = enteredText.Remove(0, 1);
                        inputedTextTB.Text = enteredText;
                        charsCount++;
                        if (inputText.Length > 0)
                        {
                            charRect[inputText.ToLower()[0]].StrokeThickness = 5;
                            charRect[inputText.ToLower()[0]].Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#00FF00");
                        }
                        else
                        {
                            MessageBox.Show("Well Done");
                            StopButton_Click(null, null);
                        }
                    }
                    else
                    {
                        failsCounter++;
                        FailsCountTB.Text = "Fails: " + failsCounter.ToString();
                    }
                }
                else
                {
                    if (keyCharDict[e.Key] == inputText.ToUpper()[0] || keyCharDict[e.Key] == Convert.ToChar(inputText[0]))
                    {
                        charRect[inputText.ToLower()[0]].StrokeThickness = 3;
                        charRect[inputText.ToLower()[0]].Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#4A2940");
                        enteredText += inputText[0];
                        inputText = inputText.Remove(0, 1);
                        inputTextTB.Text = inputText;
                        if (enteredText.Length == 32)
                            enteredText = enteredText.Remove(0, 1);
                        inputedTextTB.Text = enteredText;
                        charsCount++;
                        if (inputText.Length > 0)
                        {
                            charRect[inputText.ToLower()[0]].StrokeThickness = 5;
                            charRect[inputText.ToLower()[0]].Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#00FF00");
                        }
                        else
                        {
                            MessageBox.Show("Well Done");
                            StopButton_Click(null, null);
                        }
                    }
                    else
                    {
                        failsCounter++;
                        FailsCountTB.Text = "Fails: " + failsCounter.ToString();
                    }
                }
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                isShiftPressed = false;
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultySlider.IsEnabled = false;
            for (var i = 0; i < DifficultySlider.Value; i++)
            {
                if(caseSensCB.IsChecked == true)
                    inputText += charsCaseSensetive[random.Next(charsCaseSensetive.Length)];
                else
                    inputText += charsCaseUnsensitive[random.Next(charsCaseUnsensitive.Length)];
            }
            inputTextTB.Text = inputText;
            caseSensCB.IsEnabled = false;
            start = DateTime.Now;
            timer.Start();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            DifficultySlider.IsEnabled = true;
            inputText = "";
            enteredText = "";
            inputTextTB.Text = inputText;
            inputedTextTB.Text = enteredText;
            failsCounter = 0;
            FailsCountTB.Text = "Fails: " + failsCounter.ToString();
            caseSensCB.IsEnabled = true;
            timer.Stop();
            charsCount = 0;
        }

        private void DifficultySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Difficulty.Text = "Difficulty :" + Convert.ToInt16(DifficultySlider.Value).ToString();
        }
    }
}
