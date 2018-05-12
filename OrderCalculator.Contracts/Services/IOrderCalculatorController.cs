using System;

namespace OrderCalculator.Contracts.Services
{
	public interface IOrderCalculatorController : IDisposable
	{
		int Execute();
	}
}
