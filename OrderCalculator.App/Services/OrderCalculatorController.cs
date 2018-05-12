using OrderCalculator.App.Parser;
using OrderCalculator.Common;
using OrderCalculator.Contracts.Services;
using OrderCalculator.Framework.Extensions;
using OrderCalculator.Framework.Logging;
using System;
using System.Linq;

namespace OrderCalculator.App.Services
{
	public class OrderCalculatorController : IOrderCalculatorController
	{
		private string[] _args;

		public OrderCalculatorController(string[] args)
		{
			this._args = args;
		}

		public int Execute()
		{
			try
			{
				if (this._args == null || !this._args.Any())
				{
					var value = this.ReadArgumentsFromConsole();

					this._args = value.Split(new[] { Constants.WhitespaceSeparator }, StringSplitOptions.None);
				}

				var argument = ArgumentParser.Parse(this._args);

				using (IOrderCalculatorService service = new OrderCalculatorService(argument))
				{
					var result = service.Calculate();

					Console.WriteLine($"Total: {result}");
				}
			}
			catch (Exception ex)
			{
				this.HandleException(ex);

				return ExitCode.Error.GetHashCode();
			}

			return ExitCode.Success.GetHashCode();
		}

		private string ReadArgumentsFromConsole()
		{
			const int MaximumNumberOfAttempts = 3;

			string value = null;

			bool shouldRetry = false;
			int numberOfAttempts = 0;

			do
			{
				if (numberOfAttempts >= MaximumNumberOfAttempts)
				{
					throw new Exception($"Maximum number of attempts reached ({MaximumNumberOfAttempts}). Please try again later.");
				}

				Console.WriteLine("Please enter a command line.");

				Console.WriteLine("Command format example: CalculateOrder CatalogFilePath Product1 QuantityProduct1 Product2 QuantityProduct2...");

				value = Console.ReadLine();

				shouldRetry = string.IsNullOrWhiteSpace(value);

				if (shouldRetry)
				{
					numberOfAttempts++;
				}

			} while (shouldRetry);

			return value;
		}

		private void HandleException(Exception ex)
		{
			this.LogException(ex);

			var friendlyMessage = this.GetFriendlyMessage(ex);

			Console.WriteLine(friendlyMessage);
		}

		private void LogException(Exception ex)
		{
			try
			{
				Log4netHelper.All(x => x.Error(ex.ToString()));
			}
			catch { }
		}

		private string GetFriendlyMessage(Exception ex)
		{
			var messages = ex.InnerExceptions().Where(e => e != null && !string.IsNullOrWhiteSpace(e.Message)).Select(e => e.Message);

			var friendlyMessage = string.Join(Environment.NewLine, messages);

			return friendlyMessage;
		}

		#region IDisposable Members

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					if (this._args != null)
					{
						Array.Clear(this._args, 0, this._args.Length);
						this._args = null;
					}
				}
			}

			this._disposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable Members
	}
}
