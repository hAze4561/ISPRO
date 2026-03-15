using System;
using System.Diagnostics;

namespace BackpackApp.Debugging
{
    public class ExecutionTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _operationName;
        private bool _disposed = false;

        /// <summary>
        /// Конструктор. Запускает таймер и логирует начало операции
        /// </summary>
        /// <param name="operationName">Название операции для измерения</param>
        public ExecutionTimer(string operationName)
        {
            _operationName = operationName;
            _stopwatch = Stopwatch.StartNew();
            DebugLogger.Log($"Начало операции: {operationName}");
        }

        /// <summary>
        /// Останавливает таймер и логирует время выполнения операции
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _stopwatch.Stop();
                DebugLogger.Log($"Операция '{_operationName}' завершена за {_stopwatch.ElapsedMilliseconds} мс");
                _disposed = true;
            }
        }
    }
}