// Clase que representa un upgrade que puede ser comprado múltiples veces para aumentar el CPS.
public class Upgrade
{
	// Nombre del upgrade.
	public string Name { get; }

	// Costo base del upgrade (el precio inicial).
	public int BaseCost { get; }

	// Cuánto aumenta el CPS cada vez que se compra este upgrade.
	public int CPSIncrease { get; }

	// Cuántas veces se ha comprado este upgrade.
	public int TimesPurchased { get; set; } = 0;

	// Multiplicador de precio: cada compra aumenta el precio en un 15%.
	public float PriceMultiplier { get; } = 1.15f;

	// Constructor para inicializar el upgrade.
	public Upgrade(string name, int baseCost, int cpsIncrease, float priceMultiplier = 1.15f)
	{
		Name = name;
		BaseCost = baseCost;
		CPSIncrease = cpsIncrease;
		PriceMultiplier = priceMultiplier;
	}

	// Calcula el costo actual del upgrade según cuántas veces fue comprado.
	public int CurrentCost =>
		(int)System.Math.Ceiling(BaseCost * System.Math.Pow(PriceMultiplier, TimesPurchased));
}
