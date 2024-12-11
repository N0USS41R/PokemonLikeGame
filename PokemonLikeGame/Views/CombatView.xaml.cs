using ExerciceMonster.Models;
using ExerciceMonster.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExerciceMonster.Views
{
    public partial class CombatView : UserControl, INotifyPropertyChanged
    {
        private readonly string _connectionString;
        private Monster _playerMonster;
        private Monster _enemyMonster;
        private ObservableCollection<Spell> _playerSpells = new ObservableCollection<Spell>();
        private int _playerHP;
        private int _enemyHP;

        
        private double _playerPokemonX;
        private double _playerPokemonY;
        private double _enemyPokemonX;
        private double _enemyPokemonY;

        public event PropertyChangedEventHandler PropertyChanged;
        private ImageBrush _backgroundImageBrush;
        private bool _isFightInitiated = false;

        public CombatView()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ExerciceMonsterContext"].ConnectionString;

            InitializeComponent();
            DataContext = this;
            LoadPlayerMonster();
            LoadEnemyMonster();
            InitializePositions();
            AttackCommand = new RelayCommand(Attack, CanAttack);
            RestartCombatCommand = new RelayCommand(RestartCombat);
            
            SetBackgroundImage(@"..\images\background.jpg");

            
            this.Loaded += (s, e) => this.Focus();
        }

        #region Position Properties

        public double PlayerPokemonX
        {
            get => _playerPokemonX;
            set
            {
                _playerPokemonX = value;
                OnPropertyChanged();
            }
        }

        public double PlayerPokemonY
        {
            get => _playerPokemonY;
            set
            {
                _playerPokemonY = value;
                OnPropertyChanged();
            }
        }

        public double EnemyPokemonX
        {
            get => _enemyPokemonX;
            set
            {
                _enemyPokemonX = value;
                OnPropertyChanged();
            }
        }

        public double EnemyPokemonY
        {
            get => _enemyPokemonY;
            set
            {
                _enemyPokemonY = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Existing Properties

        public ImageBrush BackgroundImageBrush
        {
            get => _backgroundImageBrush;
            set
            {
                _backgroundImageBrush = value;
                OnPropertyChanged("BackgroundImageBrush");
            }
        }

        public Monster PlayerMonster
        {
            get => _playerMonster;
            set
            {
                _playerMonster = value;
                OnPropertyChanged();
            }
        }

        public Monster EnemyMonster
        {
            get => _enemyMonster;
            set
            {
                _enemyMonster = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Spell> PlayerSpells
        {
            get => _playerSpells;
            set
            {
                _playerSpells = value;
                OnPropertyChanged();
            }
        }

        public int PlayerHP
        {
            get => _playerHP;
            set
            {
                _playerHP = value;
                OnPropertyChanged();
            }
        }

        public int EnemyHP
        {
            get => _enemyHP;
            set
            {
                _enemyHP = value;
                OnPropertyChanged();
            }
        }

        public ICommand AttackCommand { get; private set; }
        public ICommand RestartCombatCommand { get; private set; }

        #endregion

        #region Initialization Methods

        private void InitializePositions()
        {
            // Initialize player's Pokémon position (e.g., bottom-left)
            PlayerPokemonX = 50;
            PlayerPokemonY = 350;

    
        }

        #endregion

        #region Event Handlers

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isFightInitiated || SelectedPokemon == null)
                return; 

            const double moveStep = 10;

            if (SelectedPokemon == PlayerMonster) // Move player's Pokémon
            {
                switch (e.Key)
                {
                    case Key.Left:
                        PlayerPokemonX = Math.Max(PlayerPokemonX - moveStep, 0);
                        break;
                    case Key.Right:
                        PlayerPokemonX = Math.Min(PlayerPokemonX + moveStep, GameCanvas.ActualWidth - PlayerPokemonImage.Width);
                        break;
                    case Key.Up:
                        PlayerPokemonY = Math.Max(PlayerPokemonY - moveStep, 0);
                        break;
                    case Key.Down:
                        PlayerPokemonY = Math.Min(PlayerPokemonY + moveStep, GameCanvas.ActualHeight - PlayerPokemonImage.Height);
                        break;
                }
            }
            else if (SelectedPokemon == EnemyMonster) // Move enemy's Pokémon
            {
                switch (e.Key)
                {
                    case Key.Left:
                        EnemyPokemonX = Math.Max(EnemyPokemonX - moveStep, 0);
                        break;
                    case Key.Right:
                        EnemyPokemonX = Math.Min(EnemyPokemonX + moveStep, GameCanvas.ActualWidth - EnemyPokemonImage.Width);
                        break;
                    case Key.Up:
                        EnemyPokemonY = Math.Max(EnemyPokemonY - moveStep, 0);
                        break;
                    case Key.Down:
                        EnemyPokemonY = Math.Min(EnemyPokemonY + moveStep, GameCanvas.ActualHeight - EnemyPokemonImage.Height);
                        break;
                }
            }

            CheckProximity();
        }

        private Monster _selectedPokemon;

        public Monster SelectedPokemon
        {
            get => _selectedPokemon;
            set
            {
                _selectedPokemon = value;
                OnPropertyChanged(); // Notify UI of the change
            }
        }
        private void PlayerPokemonImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Check which Pokémon was clicked (either player or enemy)
            if (sender == PlayerPokemonImage)
            {
                SelectedPokemon = PlayerMonster;  // Select the player's Pokémon
            }
            else if (sender == EnemyPokemonImage)
            {
                SelectedPokemon = EnemyMonster;  // Select the enemy's Pokémon
            }

            this.Focus();
        }


        private void FightButton_Click(object sender, RoutedEventArgs e)
        {
            _isFightInitiated = true;
            FightButton.Visibility = Visibility.Collapsed;
            CombatInterface.Visibility = Visibility.Visible;
        }

        #endregion

        #region Proximity Detection

        private void CheckProximity()
        {
            if (SelectedPokemon == null)
                return;

            double distance = 0;

            if (SelectedPokemon == PlayerMonster) // Checking distance if player selected
            {
                distance = GetDistance(PlayerPokemonX, PlayerPokemonY, EnemyPokemonX, EnemyPokemonY);
            }
            else if (SelectedPokemon == EnemyMonster) // Checking distance if enemy selected
            {
                distance = GetDistance(EnemyPokemonX, EnemyPokemonY, PlayerPokemonX, PlayerPokemonY);
            }

            const double proximityThreshold = 100;

            if (distance <= proximityThreshold)
            {
                FightButton.Visibility = Visibility.Visible;
            }
            else
            {
                FightButton.Visibility = Visibility.Collapsed;
            }
        }


        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        #endregion

        #region Combat Methods

        private bool CanAttack(object parameter)
        {
            return EnemyHP > 0 && PlayerHP > 0;
        }

        private void Attack(object parameter)
        {
            if (parameter is Spell spell)
            {
                // Player attacks enemy
                EnemyHP -= spell.Damage;
                if (EnemyHP <= 0)
                {
                    EnemyHP = 0;
                    MessageBox.Show("You have defeated the enemy monster!", "Victory", MessageBoxButton.OK, MessageBoxImage.Information);
                    EndCombat();
                    return;
                }

                // Enemy attacks back
                EnemyAttack();
            }
        }

        private void EnemyAttack()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Fetch a random spell for the enemy monster
                var query = "SELECT TOP 1 s.Damage FROM MonsterSpell ms JOIN Spell s ON ms.SpellID = s.ID WHERE ms.MonsterID = @MonsterID ORDER BY NEWID()";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MonsterID", EnemyMonster.ID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PlayerHP -= reader.GetInt32(0);
                            if (PlayerHP <= 0)
                            {
                                PlayerHP = 0;
                                MessageBox.Show("Your monster has been defeated!", "Defeat", MessageBoxButton.OK, MessageBoxImage.Information);
                                EndCombat();
                            }
                        }
                    }
                }
            }
        }

        private void RestartCombat(object parameter)
        {
            LoadEnemyMonster();
            EnemyHP = EnemyMonster.Health;
            PlayerHP = PlayerMonster.Health;
            CombatInterface.Visibility = Visibility.Collapsed;
            _isFightInitiated = false;
            this.Focus(); // Return focus for movement
        }

        private void EndCombat()
        {
            CombatInterface.Visibility = Visibility.Collapsed;
            _isFightInitiated = false;
            this.Focus(); // Return focus for movement
        }

        #endregion

        #region Loading Methods

        private void LoadPlayerMonster()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Query to fetch the first player's monster
                var query = @"
                    SELECT TOP 1 m.ID, m.Name, m.Health, m.ImageURL
                    FROM PlayerMonster pm
                    JOIN Monster m ON pm.MonsterID = m.ID";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PlayerMonster = new Monster
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Health = reader.GetInt32(2),
                            ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };

                        PlayerHP = PlayerMonster.Health;
                    }
                }

                // Fetch spells for the player's monster
                query = "SELECT s.ID, s.Name, s.Damage FROM MonsterSpell ms JOIN Spell s ON ms.SpellID = s.ID WHERE ms.MonsterID = @MonsterID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MonsterID", PlayerMonster.ID);

                    using (var reader = command.ExecuteReader())
                    {
                        PlayerSpells.Clear();
                        while (reader.Read())
                        {
                            PlayerSpells.Add(new Spell
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Damage = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
        }

        private void LoadEnemyMonster()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT TOP 1 ID, Name, Health, ImageURL
                    FROM Monster
                    ORDER BY NEWID()";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        EnemyMonster = new Monster
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Health = (int)(reader.GetInt32(2) * 1.10), // +10% HP
                            ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };

                        EnemyHP = EnemyMonster.Health;

                        EnemyPokemonX = 650;
                        EnemyPokemonY = 200;
                    }
                }
            }
        }

        #endregion

        #region Background Image

        private void SetBackgroundImage(string relativePath)
        {
             var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var assemblyPath = Path.GetDirectoryName(assemblyLocation);

            var fullPath = Path.GetFullPath(Path.Combine(assemblyPath, relativePath));

            try
            {
                var image = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                BackgroundImageBrush = new ImageBrush(image)
                {
                    Stretch = Stretch.Fill
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading background image: {ex.Message}");
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
