using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using BackpackApp.Models;

namespace BackpackApp.Debugging
{
    public static class DebugLogger
    {
        [Conditional("DEBUG")]
        public static void Log(string message)
        {
            string logMessage = $"[DEBUG] {DateTime.Now:HH:mm:ss.fff}: {message}";
            Debug.WriteLine(logMessage);
            LogToFile(logMessage);
        }

        [Conditional("DEBUG")]
        public static void LogItems(List<Item> items, string stage)
        {
            if (items == null || items.Count == 0)
            {
                Log($"{stage}: список предметов пуст");
                return;
            }

            Log($"{stage}:");
            foreach (var item in items)
            {
                Log($" - {item.Name}: вес {item.Weight}, стоимость {item.Cost}");
            }
        }

        [Conditional("DEBUG")]
        public static void LogSqlQuery(string query, SqlParameter[] parameters = null)
        {
            Log($"[SQL] {query}");
            if (parameters != null && parameters.Length > 0)
            {
                Log("[SQL] Параметры:");
                foreach (var param in parameters)
                {
                    Log($" - {param.ParameterName}: {param.Value}");
                }
            }
        }

        private static void LogToFile(string message)
        {
            try
            {
                string logPath = Path.Combine(
                    Application.StartupPath,
                    "debug_log.txt");
                File.AppendAllText(logPath, message + Environment.NewLine);
            }
            catch
            {
                // Игнорируем ошибки записи в файл
            }
        }
    }
}