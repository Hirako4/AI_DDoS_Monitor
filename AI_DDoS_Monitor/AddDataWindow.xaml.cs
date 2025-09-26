using System;
using System.Windows;
using Npgsql;
using AI_DDoS_Monitor.Database;

namespace AI_DDoS_Monitor.Windows
{
    public partial class AddDataWindow : Window
    {
        public AddDataWindow()
        {
            InitializeComponent();
        }

        private void AddDatasetBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                using var transaction = conn.BeginTransaction();
                try
                {
                    var sql = @"
                        INSERT INTO ai_ddos.datasets (name, size_mb, tags)
                        VALUES (@name, @size_mb, @tags)
                        RETURNING id";

                    using var cmd = new NpgsqlCommand(sql, conn, transaction);
                    cmd.Parameters.AddWithValue("@name", DatasetName.Text);
                    cmd.Parameters.AddWithValue("@size_mb", decimal.Parse(SizeMb.Text));
                    cmd.Parameters.AddWithValue("@tags", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, Tags.Text.Split(','));

                    var id = cmd.ExecuteScalar();
                    transaction.Commit();

                    MessageBox.Show($"Датасет добавлен с ID: {id}");
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddExperimentBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                var sql = @"
            INSERT INTO ai_ddos.experiments (name, attack_type)
            VALUES (@name, @attack_type::ai_ddos.attack_type)
            RETURNING id";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", ExperimentName.Text);
                cmd.Parameters.AddWithValue("@attack_type", AttackType.Text); // Просто строка

                var id = cmd.ExecuteScalar();

                MessageBox.Show($"Эксперимент добавлен с ID: {id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AddRunBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на пустые поля
                if (string.IsNullOrWhiteSpace(RunExperimentId.Text))
                {
                    MessageBox.Show("Введите ID эксперимента.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(RunDatasetId.Text))
                {
                    MessageBox.Show("Введите ID датасета.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(RunStatus.Text))
                {
                    MessageBox.Show("Выберите статус.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(RunAccuracy.Text))
                {
                    MessageBox.Show("Введите точность (0-1).");
                    return;
                }

                // Попытка преобразования
                if (!int.TryParse(RunExperimentId.Text, out int experimentId))
                {
                    MessageBox.Show("ID эксперимента должно быть целым числом.");
                    return;
                }

                if (!int.TryParse(RunDatasetId.Text, out int datasetId))
                {
                    MessageBox.Show("ID датасета должно быть целым числом.");
                    return;
                }

                if (!decimal.TryParse(RunAccuracy.Text, out decimal accuracy) || accuracy < 0 || accuracy > 1)
                {
                    MessageBox.Show("Точность должна быть числом от 0 до 1.");
                    return;
                }

                using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
                conn.Open();

                var sql = @"
                    INSERT INTO ai_ddos.runs (experiment_id, dataset_id, status, accuracy)
                    VALUES (@experiment_id, @dataset_id, @status::ai_ddos.status, @accuracy)
                    RETURNING id";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@experiment_id", experimentId);
                cmd.Parameters.AddWithValue("@dataset_id", datasetId);
                cmd.Parameters.AddWithValue("@status", RunStatus.Text);
                cmd.Parameters.AddWithValue("@accuracy", accuracy);

                var id = cmd.ExecuteScalar();

                MessageBox.Show($"Запуск добавлен с ID: {id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}