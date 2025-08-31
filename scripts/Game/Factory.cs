// Clase que representa un upgrade que puede ser comprado múltiples veces para aumentar el CPS.
public class Factory
{

	public string Name { get; }
	public int BaseCost { get; }
	public int CPSIncrease { get; }
	public int TimesPurchased { get; set; } = 0;
	public float PriceMultiplier { get; } = 1.15f;

	// Constructor para inicializar el upgrade.
	public Factory(string name, int baseCost, int cpsIncrease, float priceMultiplier = 1.15f)
	{
		Name = name;
		BaseCost = baseCost;
		CPSIncrease = cpsIncrease;
		PriceMultiplier = priceMultiplier;
	}

	public int CurrentCost =>
		(int)System.Math.Ceiling(BaseCost * System.Math.Pow(PriceMultiplier, TimesPurchased));
}
