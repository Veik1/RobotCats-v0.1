using System;
using System.Collections.Generic;

// Clase que maneja la lógica principal del juego: gatos, CPS y upgrades.
public class CatManager
{
	public int CatCount { get; private set; } = 0;
	public int CatsPerSecond { get; private set; } = 0;
	private float _catAccumulator = 0f;
	public int CatsPerClick { get; private set; } = 1;

	public List<Upgrade> AvailableUpgrades { get; } = new List<Upgrade>
	{
		new Upgrade("Click", 1, 1),
		new Upgrade("Clicker Cat", 10, 1),
		new Upgrade("Mega Cat", 50, 5),
		new Upgrade("T-800 Cat", 200, 20)
	};
	
	public List<Factory> AvailableFactories { get; } = new List<Factory>
	{
		new Factory("Mini Factory", 20, 3),
		new Factory("Simple Factory", 85, 10),
		new Factory("Ultrasupermega Factory", 200, 20)
	};

	// evento para notificar cambios en la cantidad de gatos o CPS.
	public event Action<int, int> OnStateChanged;

	public void AddCats(int amount)
	{
		CatCount += amount;
		OnStateChanged?.Invoke(CatCount, CatsPerSecond);
	}

	// Actualiza el estado cada frame, generando gatos automáticos según el CPS.
	public void Update(double delta)
	{
		if (CatsPerSecond > 0)
		{
			_catAccumulator += (float)delta * CatsPerSecond;
			if (_catAccumulator >= 1f)
			{
				int catsToAdd = (int)_catAccumulator;
				AddCats(catsToAdd);
				_catAccumulator -= catsToAdd;
			}
		}
	}

	public bool TryBuyUpgrade(Upgrade upgrade)
	{
		int cost = upgrade.CurrentCost;
		// restricción de costo
		if (CatCount >= cost)
		{
			CatCount -= cost;
			if (upgrade.Name == "Click")
				CatsPerClick += upgrade.CPSIncrease;
			else
				CatsPerSecond += upgrade.CPSIncrease;
			upgrade.TimesPurchased++;
			OnStateChanged?.Invoke(CatCount, CatsPerSecond);
			return true;
		}
		return false;
	}
	
	public bool TryBuyFactory(Factory factory)
	{
		int cost = factory.CurrentCost;
		// restricción de costo
		if (CatCount >= cost)
		{
			CatCount -= cost;
			CatsPerSecond += factory.CPSIncrease;
			factory.TimesPurchased++;
			OnStateChanged?.Invoke(CatCount, CatsPerSecond);
			return true;
		}
		return false;
	}
}
