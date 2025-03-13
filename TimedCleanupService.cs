using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace GuckGuck;

public class TimedCleanupService : IHostedService, IDisposable
{
	private Timer _timer;
	private readonly string _wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

	public Task StartAsync(CancellationToken cancellationToken)
	{
		Console.WriteLine("Timed Background Service is starting.");

		_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(12));

		return Task.CompletedTask;
	}

	private void DoWork(object state)
	{
		Console.WriteLine("Timed Background Service is working.");

		var now = DateTime.UtcNow;
		string[] directories;

		try
		{
			directories = Directory.GetDirectories(_wwwrootPath);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error retrieving directories: {ex.Message}");
			return;
		}

		foreach (var dir in directories)
		{
			var dirName = Path.GetFileName(dir);
			if (dirName.Length >= 17 && DateTime.TryParseExact(dirName.Substring(dirName.Length - 17), "yyyyMMddHHmmssfff", null, System.Globalization.DateTimeStyles.None, out var dirTimestamp))
			{
				if ((now - dirTimestamp).TotalHours > 12)
				{
					try
					{
						if (Directory.Exists(dir))
						{
							Directory.Delete(dir, true);
							Console.WriteLine($"Deleted directory: {dir}");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error deleting directory: {dir}, {ex.Message}");
					}
				}
			}
			else
			{
				Console.WriteLine($"Directory name does not contain a valid timestamp: {dir}");
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		Console.WriteLine("Timed Background Service is stopping.");

		_timer?.Change(Timeout.Infinite, 0);

		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}