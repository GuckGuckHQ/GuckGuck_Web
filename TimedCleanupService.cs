using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GuckGuck;

public class TimedCleanupService : IHostedService, IDisposable
{
	private readonly ILogger<TimedHostedService> _logger;
	private Timer _timer;
	private readonly string _wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

	public TimedCleanupService(ILogger<TimedHostedService> logger)
	{
		_logger = logger;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Timed Background Service is starting.");

		_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(12));

		return Task.CompletedTask;
	}

	private void DoWork(object state)
	{
		_logger.LogInformation("Timed Background Service is working.");

		var now = DateTime.UtcNow;
		var directories = Directory.GetDirectories(_wwwrootPath);

		foreach (var dir in directories)
		{
			var dirName = Path.GetFileName(dir);
			if (DateTime.TryParseExact(dirName.Substring(dirName.Length - 17), "yyyyMMddHHmmssfff", null, System.Globalization.DateTimeStyles.None, out var dirTimestamp))
			{
				if ((now - dirTimestamp).TotalHours > 12)
				{
					try
					{
						Directory.Delete(dir, true);
						_logger.LogInformation($"Deleted directory: {dir}");
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Error deleting directory: {dir}");
					}
				}
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Timed Background Service is stopping.");

		_timer?.Change(Timeout.Infinite, 0);

		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}