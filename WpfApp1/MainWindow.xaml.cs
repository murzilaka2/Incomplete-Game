using System.Numerics;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Rectangle> itemRemover = new List<Rectangle>();
        Random r = new Random();
        ImageBrush playerImage = new ImageBrush();
        ImageBrush starImage = new ImageBrush();
        Rect playerHitBox;
        int speed = 15;
        int playerSpeed = 10;
        int starCounter = 30;
        int powerModed = 200;
        double score, i, carNum;
        bool moveLeft, moveRight, gameOver, powerMode;
        public MainWindow()
        {
            InitializeComponent();
            myCanvas.Focus();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void StartGame()
        {
            speed = 8;
            gameTimer.Start();
            moveLeft = false;
            moveRight = false;
            gameOver = false;
            powerMode = false;
            score = 0;
            scoreText.Content = "Survived: 00 Seconds";
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car.png"));
            starImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/carPowerMode.png"));
            player.Fill = playerImage;
            myCanvas.Background = Brushes.Gray;
            foreach (var item in myCanvas.Children.OfType<Rectangle>())
            {
                if (item.Tag.ToString() == "Car")
                {
                    Canvas.SetTop(item, (r.Next(100, 400) * -1));
                    Canvas.SetLeft(item, (r.Next(00, 430)));
                    ChangeCars(item);
                }
                if (item.Tag.ToString() == "star")
                {
                    itemRemover.Add(item);
                }
            }
            itemRemover.Clear();
            
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            score += .05;
            starCounter -= 1;
            scoreText.Content = $"Survived: {score.ToString("#.#")} Seconds";
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            if (moveLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }
            if (starCounter < 1)
            {
                MakeStar();
                starCounter = r.Next(600, 900);
            }

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if (x.Tag.ToString() == "roadMarks")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }
                }
                if (x.Tag.ToString() == "Car")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                    if (Canvas.GetTop(x) > 500)
                    {
                        ChangeCars(x);
                    }
                    Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width - 50, x.Height - 50);
                    if (playerHitBox.IntersectsWith(carHitBox) && powerMode)
                    {
                        ChangeCars(x);
                    }
                    else if (playerHitBox.IntersectsWith(carHitBox) && powerMode == false)
                    {
                        gameTimer.Stop();
                        scoreText.Content = "Pree Enter to play...";
                        gameOver = true;
                    }
                }
                if (x.Tag.ToString() == "star")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerHitBox.IntersectsWith(starHitBox))
                    {
                        itemRemover.Add(x);
                        powerMode = true;
                        //...
                    }
                    if (Canvas.GetTop(x) > 400)
                    {
                        itemRemover.Add(x);
                    }
                }
            }

            if (powerMode)
            {
                //...
                PowerUp();
                //...
            }
            else
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car.png"));
                myCanvas.Background = Brushes.Gray;
            }
            foreach (var item in itemRemover)
            {
                myCanvas.Children.Remove(item);
            }

            if (score >= 10 && score < 20)
            {
                speed = 12;
            }
            else if (score < 30)
            {
                speed = 14;
            }
            else if (score < 50)
            {
                speed = 16;
            }
            else if (score < 80)
            {
                speed = 18;
            }
            //....
        }

        private void PowerUp()
        {
            
        }

        private void ChangeCars(Rectangle x)
        {
            carNum = r.Next(1, 6);
            ImageBrush carImage = new ImageBrush();
            switch (carNum)
            {
                case 1:
                    {
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car.png"));
                        break;
                    }
                case 2:
                    {
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car2.png"));
                        break;
                    }
                case 3:
                    {
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car3.png"));
                        break;
                    }
                case 4:
                    {
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car4.png"));
                        break;
                    }
                case 5:
                    {
                        carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/car5.png"));
                        break;
                    }
            }
            x.Fill = carImage;
            Canvas.SetTop(x, (r.Next(100,400) * -1));
            Canvas.SetLeft(x, (r.Next(0,430)));
        }

        private void MakeStar()
        {
            Rectangle star = new Rectangle
            {
                Height = 40,
                Width = 50,
                Tag = "star",
                Fill = starImage
            };

            Canvas.SetLeft(star, r.Next(0,430));
            Canvas.SetTop(star, r.Next(100,400) * -1);
            myCanvas.Children.Add(star);
        }

        private void myCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void myCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if (e.Key == Key.Enter && gameOver)
            {
                StartGame();
            }
        }
    }
}