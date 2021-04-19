using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int enemySize = 20;
        const int EnemySpeed = 100;
        const int playerSpeed = 4;
        const double barTop = 7;
        const double PlayerMoveLength = 4;
        const double PlayerShotRange = 150;
        const double shotSpeed = 5;
        private int playerPoints = 0;
        private Random rng = new Random();
        private List<Enemy> Enemies = new List<Enemy>();
        private Player player = new Player();
        private DispatcherTimer gameTickTimer = new DispatcherTimer();
        private DispatcherTimer playerTimer = new DispatcherTimer();
        private Canvas enemyHalf = new Canvas() { Width = 700, Height = 200, };
        private enum EnemyDirection { Left, Right, Up, Down };
        private enum PlayerDirection { Left, Right, Up, Down, Space, None };
        private PlayerDirection playerDirection = PlayerDirection.None;
        private EnemyDirection enemyDirection = EnemyDirection.Left;
        private SolidColorBrush enemyColor = Brushes.Black;
        public MainWindow()
        {
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            playerTimer.Tick += new EventHandler(MovePlayer);
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            DrawEnemies();
            MoveEnemies();
        }

        private bool isReach()
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                if (Canvas.GetLeft(Enemies[i].UiElement) >= Canvas.GetLeft(player.UiElement) && Canvas.GetLeft(Enemies[i].UiElement) + Enemies[i].UiElement.DesiredSize.Width <= Canvas.GetLeft(player.UiElement) + player.UiElement.DesiredSize.Width)
                    return true;
            }

            return false;
        }
        private async void AttackEnemies()
        {
            if (Enemies.Count > 0)
            {

            }
        }

        public void DrawEnemies()
        {
            foreach (Enemy enemies in Enemies)
            {
                if (enemies.UiElement == null)
                {
                    enemies.UiElement = new Ellipse
                    {
                        Width = enemySize,
                        Height = enemySize,
                        Fill = enemyColor,
                        StrokeThickness = 2,
                        Stroke = Brushes.Red
                    };
                    enemyHalf.Children.Add(enemies.UiElement);
                    Canvas.SetLeft(enemies.UiElement, rng.Next(0, (int)enemyHalf.ActualWidth));
                    Canvas.SetTop(enemies.UiElement, rng.Next(0, (int)enemyHalf.ActualHeight));
                    if (enemies.Healthbar == null)
                    {
                        enemies.Healthbar = new Rectangle
                        {
                            Width = enemies.Health / 5,
                            Height = 5,
                            Fill = Brushes.Green
                        };
                        Canvas.SetLeft(enemies.Healthbar, Canvas.GetLeft(enemies.UiElement));
                        Canvas.SetTop(enemies.Healthbar, Canvas.GetTop(enemies.UiElement) - barTop);
                        enemyHalf.Children.Add(enemies.Healthbar);
                    }

                }
            }

            if (Enemies.Count > 0)
            {
                for (int i = Enemies.Count - 1; i >= 0; i--)
                {
                    if (Enemies[i].Health <= 0)
                    {
                        Enemies[i].Health = 0;
                        playerPoints += Enemies[i].PointWorth;
                        playerScore.Content = string.Format("Player score: {0}", playerPoints);
                        enemyHalf.Children.Remove(Enemies[i].UiElement);
                        enemyHalf.Children.Remove(Enemies[i].Healthbar);
                        Enemies.RemoveAt(i);
                    }
                }
            }
        }
        public void DrawPlayer()
        {

        }
        private async void Jump()
        {
            double nextY = Canvas.GetTop(player.UiElement);
            double JumpHeight = 15;
            double GravitySpeed = 1;
            bool isJumping = true;
            if (Canvas.GetTop(player.UiElement) == gameField.ActualHeight - 50)
            {
                while (isJumping)
                {
                    nextY -= JumpHeight;
                    Canvas.SetTop(player.UiElement, nextY);
                    Canvas.SetTop(player.Healthbar, Canvas.GetTop(player.UiElement) - barTop);
                    if (JumpHeight <= 0)
                    {
                        while (isJumping)
                        {
                            nextY += JumpHeight;
                            Canvas.SetTop(player.UiElement, nextY);
                            Canvas.SetTop(player.Healthbar, Canvas.GetTop(player.UiElement) - barTop);
                            JumpHeight += GravitySpeed;
                            if (Canvas.GetTop(player.UiElement) >= gameField.ActualHeight - 50)
                            {
                                Canvas.SetTop(player.UiElement, gameField.ActualHeight - 50);
                                Canvas.SetTop(player.Healthbar, Canvas.GetTop(player.UiElement) - barTop);
                                isJumping = false;
                            }
                            await Task.Delay(1);
                        }
                    }
                    JumpHeight -= GravitySpeed;
                    await Task.Delay(1);
                }
            }

        }
        private async void MovePlayer(object sender, EventArgs e)
        {
            double playerUiX = player.UiElement.DesiredSize.Width;
            double playerUiY = player.UiElement.DesiredSize.Height;
            double nextX = Canvas.GetLeft(player.UiElement);

            if (Keyboard.IsKeyDown(Key.Up))
            {
                Jump();
            }

            if (Keyboard.IsKeyDown(Key.Left))
            {
                await Task.Delay(1);
                nextX -= PlayerMoveLength;
                Canvas.SetLeft(player.UiElement, nextX);
                Canvas.SetLeft(player.Healthbar, nextX);
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                await Task.Delay(1);
                nextX += PlayerMoveLength;
                Canvas.SetLeft(player.UiElement, nextX);
                Canvas.SetLeft(player.Healthbar, nextX);
            }
            if (Keyboard.IsKeyDown(Key.Space) && Enemies.Count > 0)
            {
                await Task.Delay(1);
                Rectangle shot = new Rectangle() { Width = 5, Height = 5, Fill = Brushes.Black };
                double shotPosX = Canvas.GetLeft(player.UiElement), shotPosY = Canvas.GetTop(player.UiElement);
                double midPosX = shotPosX + (playerUiX / 2), midPosY = shotPosY + (playerUiY / 2);
                bool enemyHit = false;

                Canvas.SetTop(shot, midPosY);
                Canvas.SetLeft(shot, midPosX);
                for (int i = 0; i < Enemies.Count; i++)
                {
                    gameField.Children.Add(shot);
                    while (!enemyHit)
                    {
                        if (Canvas.GetLeft(shot) < Canvas.GetLeft(Enemies[i].UiElement))
                        {
                            midPosX += shotSpeed;
                        }

                        if (Canvas.GetLeft(shot) > Canvas.GetLeft(Enemies[i].UiElement))
                        {
                            midPosX -= shotSpeed;
                        }

                        if (Canvas.GetTop(shot) < Canvas.GetTop(Enemies[i].UiElement))
                        {
                            midPosY += shotSpeed;
                        }

                        if (Canvas.GetTop(shot) > Canvas.GetTop(Enemies[i].UiElement))
                        {
                            midPosY -= shotSpeed;
                        }

                        Canvas.SetLeft(shot, midPosX);
                        Canvas.SetTop(shot, midPosY);
                        await Task.Delay(10);
                        if (Enemies.Count > 0)
                        {
                            if (Canvas.GetLeft(shot) > Canvas.GetLeft(Enemies[i].UiElement) && Canvas.GetLeft(shot) < Canvas.GetLeft(Enemies[i].UiElement) + Enemies[i].UiElement.DesiredSize.Width && Canvas.GetTop(shot) > Canvas.GetTop(Enemies[i].UiElement) && Canvas.GetTop(shot) < Canvas.GetTop(Enemies[i].UiElement) + Enemies[i].UiElement.DesiredSize.Height)
                            {
                                Enemies[i].Health -= player.Damage;
                                if (Enemies[i].Health > 0)
                                {
                                    enemyHalf.Children.Remove(Enemies[i].Healthbar);
                                    Enemies[i].Healthbar = new Rectangle { Width = Enemies[i].Health / 5, Height = 5, Fill = Brushes.Green };
                                    Canvas.SetLeft(Enemies[i].Healthbar, Canvas.GetLeft(Enemies[i].UiElement));
                                    Canvas.SetTop(Enemies[i].Healthbar, Canvas.GetTop(Enemies[i].UiElement) - barTop);
                                    enemyHalf.Children.Add(Enemies[i].Healthbar);
                                }
                                enemyHit = true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    gameField.Children.Remove(shot);
                }
            }
        }
        public async void MoveEnemies()
        {
            double EnemyMoveLength = 4;
            foreach (Enemy enemies in Enemies)
            {
                int move = rng.Next(4);
                double lastX = Canvas.GetLeft(enemies.UiElement), lastY = Canvas.GetTop(enemies.UiElement);
                if (move == 0)
                {
                    enemyDirection = EnemyDirection.Left;
                }
                else if (move == 1)
                {
                    enemyDirection = EnemyDirection.Down;
                }
                else if (move == 2)
                {
                    enemyDirection = EnemyDirection.Right;
                }
                else if (move == 3)
                {
                    enemyDirection = EnemyDirection.Up;
                }


                switch (enemyDirection)
                {
                    case EnemyDirection.Left:
                        if (lastX <= enemyHalf.ActualWidth - enemyHalf.ActualWidth)
                        {
                            lastX += EnemyMoveLength;
                            Canvas.SetLeft(enemies.UiElement, lastX);
                            Canvas.SetLeft(enemies.Healthbar, lastX);
                        }
                        else
                        {
                            lastX -= EnemyMoveLength;
                            Canvas.SetLeft(enemies.UiElement, lastX);
                            Canvas.SetLeft(enemies.Healthbar, lastX);
                        }
                        break;
                    case EnemyDirection.Down:
                        if (lastY >= enemyHalf.ActualHeight)
                        {
                            lastY -= EnemyMoveLength;
                            Canvas.SetTop(enemies.UiElement, lastY);
                            Canvas.SetTop(enemies.Healthbar, lastY - barTop);
                        }
                        else
                        {
                            lastY += EnemyMoveLength;
                            Canvas.SetTop(enemies.UiElement, lastY);
                            Canvas.SetTop(enemies.Healthbar, lastY - barTop);
                        }
                        break;
                    case EnemyDirection.Right:
                        if (lastX >= enemyHalf.ActualWidth)
                        {
                            lastX -= EnemyMoveLength;
                            Canvas.SetLeft(enemies.UiElement, lastX);
                            Canvas.SetLeft(enemies.UiElement, lastX);
                        }
                        else
                        {
                            lastX += EnemyMoveLength;
                            Canvas.SetLeft(enemies.UiElement, lastX);
                            Canvas.SetLeft(enemies.Healthbar, lastX);
                        }
                        break;
                    case EnemyDirection.Up:
                        if (lastY <= enemyHalf.ActualHeight - enemyHalf.ActualHeight)
                        {
                            lastY += EnemyMoveLength;
                            Canvas.SetTop(enemies.UiElement, lastY);
                            Canvas.SetTop(enemies.Healthbar, lastY - barTop);
                        }
                        else
                        {
                            lastY -= EnemyMoveLength;
                            Canvas.SetTop(enemies.UiElement, lastY);
                            Canvas.SetTop(enemies.Healthbar, lastY - barTop);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (isReach())
            {
                if (gameTickTimer.IsEnabled)
                {
                    for (int i = Enemies.Count - 1; i >= 0; i--)
                    {
                        if (Canvas.GetLeft(Enemies[i].UiElement) >= Canvas.GetLeft(player.UiElement) && Canvas.GetLeft(Enemies[i].UiElement) + Enemies[i].UiElement.DesiredSize.Width <= Canvas.GetLeft(player.UiElement) + player.UiElement.DesiredSize.Width)
                        {
                            Rectangle enemyShot = new Rectangle() { Width = 5, Height = 5, Fill = Brushes.Black };
                            gameField.Children.Add(enemyShot);
                            Canvas.SetLeft(enemyShot, Canvas.GetLeft(Enemies[i].UiElement) + Enemies[i].UiElement.DesiredSize.Width / 2);
                            Canvas.SetTop(enemyShot, Canvas.GetTop(Enemies[i].UiElement));
                            for (double j = Canvas.GetTop(enemyShot); j <= gameField.ActualHeight; j++)
                            {
                                await Task.Delay(1);
                                Canvas.SetTop(enemyShot, j);
                            }
                        }

                    }
                }
            }
        }

        public void StartNewGame()
        {
            int StartEnemies = 10;
            player.Damage = 2;
            player.Health = 100;
            player.UiElement = new Rectangle() { Width = 50, Height = 50, Fill = Brushes.Green, StrokeThickness = 5, Stroke = Brushes.Black };
            player.Healthbar = new Rectangle() { Width = player.Health / 2, Height = 5, Fill = Brushes.Green };
            gameField.Children.Add(player.UiElement);
            gameField.Children.Add(player.Healthbar);
            gameField.Children.Add(enemyHalf);
            Grid.SetRow(enemyHalf, 0);

            Canvas.SetLeft(player.UiElement, 10);
            Canvas.SetTop(player.UiElement, gameField.ActualHeight - 50);
            Canvas.SetLeft(player.Healthbar, Canvas.GetLeft(player.UiElement));
            Canvas.SetTop(player.Healthbar, Canvas.GetTop(player.UiElement) - barTop);
            //-Test Area--------------------------------------------------------------------------//

            // -----------------------------------------------------------------------------------//
            for (int i = StartEnemies; i > 0; i--)
            {
                Enemies.Add(item: new Enemy() { Health = 100, PointWorth = rng.Next(3, 5) });
            }

            playerTimer.Interval = TimeSpan.FromMilliseconds(playerSpeed);
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(EnemySpeed);

            playerTimer.Start();
            gameTickTimer.IsEnabled = true;
        }
        public void ResetGame()
        {
            for(int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].UiElement = null;
                Enemies[i].Healthbar = null;
            }
            Enemies.Clear();
            gameField.Children.Clear();
            playerPoints = 0;
            Canvas.SetLeft(player.UiElement, 10);
            Canvas.SetTop(player.UiElement, gameField.ActualHeight - 50);
            playerScore.Content = string.Format("Player score: {0}", playerPoints);
            playerTimer.Stop();
            gameTickTimer.IsEnabled = false;
        }



        private void Window_ContentRendered(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Enemies.Add(item: new Enemy() { Health = 100, PointWorth = rng.Next(3, 5) });
        }

        private void Reset_Game(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void New_Game(object sender, RoutedEventArgs e)
        {
            ResetGame();
            StartNewGame();
        }
    }
}
