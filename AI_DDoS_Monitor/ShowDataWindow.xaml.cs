using System.Data;
using System.Windows;
using Npgsql;
using AI_DDoS_Monitor.Database;

namespace AI_DDoS_Monitor.Windows
{
    public partial class ShowDataWindow : Window
    {
        public ShowDataWindow()
        {
            InitializeComponent();
            LoadDatasets();
            LoadExperiments();
            LoadRuns();
            LoadSchemas();
        }

        private void LoadDatasets()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                var sql = "SELECT id, name, size_mb, tags FROM ai_ddos.datasets";

                using var cmd = new NpgsqlCommand(sql, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                DatasetsGrid.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке датасетов: {ex.Message}");
            }
        }

        private void LoadExperiments()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                var sql = "SELECT id, name, attack_type, created_at, is_active FROM ai_ddos.experiments";

                using var cmd = new NpgsqlCommand(sql, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                ExperimentsGrid.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке экспериментов: {ex.Message}");
            }
        }

        private void LoadRuns()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                var sql = @"
                    SELECT r.id, e.name AS experiment_name, d.name AS dataset_name, r.status, r.accuracy
                    FROM ai_ddos.runs r
                    JOIN ai_ddos.experiments e ON r.experiment_id = e.id
                    JOIN ai_ddos.datasets d ON r.dataset_id = d.id";

                using var cmd = new NpgsqlCommand(sql, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                RunsGrid.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке запусков: {ex.Message}");
            }
        }

        private void LoadSchemas()
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                var sql = "SELECT schema_name FROM information_schema.schemata WHERE schema_name NOT IN ('information_schema', 'pg_catalog')";

                using var cmd = new NpgsqlCommand(sql, conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                SchemasGrid.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке схем: {ex.Message}");
            }
        }
    }
}