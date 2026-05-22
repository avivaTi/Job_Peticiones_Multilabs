using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio.services
{
    class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }
        public void WriteinLog(string cadena)
        {
            _logger.LogWarning(cadena);
        }
    }
}
