using System.Windows;
using AI_DDoS_Monitor.Database;
using AI_DDoS_Monitor.Windows;

namespace AI_DDoS_Monitor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ToggleThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            app.SwitchTheme();
        }
        private void CreateSchemaBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SchemaInitializer.CreateSchema();
                MessageBox.Show("Схема успешно создана!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddDataWindow();
            addWindow.ShowDialog();
        }

        private void ShowDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var showWindow = new ShowDataWindow();
            showWindow.Show();
        }

        private void TestConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new Npgsql.NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();
                var sql = "SELECT COUNT(*) FROM ai_ddos.runs";
                using var cmd = new Npgsql.NpgsqlCommand(sql, conn);
                var count = cmd.ExecuteScalar();
                MessageBox.Show($"Подключение успешно. В таблице runs: {count} записей.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }
    }
}