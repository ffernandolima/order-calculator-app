using System;

namespace OrderCalculator.Contracts.Services
{
	public interface IOrderCalculatorService : IDisposable
	{
		decimal? Calculate();
	}
}
