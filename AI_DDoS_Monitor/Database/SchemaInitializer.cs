using Npgsql;
using System;

namespace AI_DDoS_Monitor.Database
{
    public static class SchemaInitializer
    {
        public static void CreateSchema()
        {
            using var conn = new NpgsqlConnection(DbConnection.ConnectionString);
            conn.Open();

            var sql = @"
                CREATE SCHEMA IF NOT EXISTS ai_ddos;

                CREATE TYPE ai_ddos.attack_type AS ENUM ('syn_flood', 'udp_flood', 'http_flood', 'slowloris');
                CREATE TYPE ai_ddos.status AS ENUM ('pending', 'running', 'completed', 'failed');

                CREATE TABLE ai_ddos.datasets (
                    id SERIAL PRIMARY KEY,
                    name TEXT NOT NULL UNIQUE,
                    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
                    size_mb NUMERIC(10, 2) CHECK (size_mb > 0),
                    tags TEXT[]
                );

                CREATE TABLE ai_ddos.experiments (
                    id SERIAL PRIMARY KEY,
                    name TEXT NOT NULL,
                    attack_type ai_ddos.attack_type NOT NULL,
                    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
                    is_active BOOLEAN DEFAULT TRUE
                );

                CREATE TABLE ai_ddos.runs (
                    id SERIAL PRIMARY KEY,
                    experiment_id INT NOT NULL REFERENCES ai_ddos.experiments(id) ON DELETE CASCADE,
                    dataset_id INT NOT NULL REFERENCES ai_ddos.datasets(id) ON DELETE RESTRICT,
                    status ai_ddos.status DEFAULT 'pending',
                    started_at TIMESTAMP,
                    ended_at TIMESTAMP,
                    accuracy NUMERIC(5, 4) CHECK (accuracy >= 0 AND accuracy <= 1),
                    config JSONB
                );";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}