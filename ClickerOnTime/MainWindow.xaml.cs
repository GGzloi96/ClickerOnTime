using ClickerOnTime.ClassesForTimeClicker;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClickerOnTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CController controller;
        private DispatcherTimer timer;
        private DateTime lastTick;
        private double timeLeft;
        private const double GameDuration = 30.0; // длительность раунда
        private bool isRunning = false;


        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            double delta = (now - lastTick).TotalSeconds;
            lastTick = now;

            // Защита от «скачков» после паузы окна
            if (delta > 0.3) delta = 0.3;

            timeLeft -= delta;

            if (timeLeft <= 0)
            {
                EndGame();
                return;
            }

            // Обновляем логику и перерисовываем сцену
            controller.update(delta);
            Redraw();

            // Обновляем интерфейс
            ScoreText.Text = $"Score: {controller.GetPoints():0}";
            TimeText.Text = $"Time: {timeLeft:0,0}s";
        }

        private void Redraw()
        {
            GameCanvas.Children.Clear();

            foreach (var obj in controller.GetObjects())
            {
                var sprite = obj.getSprite();
                GameCanvas.Children.Add(sprite);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning) return;

            isRunning = true;
            StartButton.IsEnabled = false;

            // Создаём контроллер под текущий размер холста
            controller = new CController(
                spawnRate: 2.0,
                startTime: 0,
                sceneSize: new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight)
            );
            controller.ObjectDied += ShowDeathMessage;

            GameCanvas.Children.Clear();

            timeLeft = GameDuration;
            lastTick = DateTime.Now;
            timer.Start();
        }

        private void ShowDeathMessage(double x, double y)
        {
            // Создаём текст
            TextBlock label = new TextBlock
            {
                Text = "ЛОХ!",
                Foreground = System.Windows.Media.Brushes.Red,
                FontSize = 14,
                FontWeight = System.Windows.FontWeights.Bold
            };

            // Ставим рядом с местом, где был кружок (чуть ниже и правее)
            Canvas.SetLeft(label, x + 5);
            Canvas.SetTop(label, y + 5);

            // Добавляем на холст
            MessageCanvas.Children.Add(label);

            // === НОВОЕ: через 1 секунду удаляем надпись ===
            var removeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            removeTimer.Tick += (s, e) =>
            {
                MessageCanvas.Children.Remove(label);
                removeTimer.Stop();
            };
            removeTimer.Start();
        }

        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isRunning) return;

            Point p = e.GetPosition(GameCanvas);
            controller.mouseClick(p);
        }

        private void EndGame()
        {
            timer.Stop();
            isRunning = false;

            StartButton.IsEnabled = true;
            StartButton.Content = "Start";
            TimeText.Text = "Time: 0,0s";
        }
    }
}