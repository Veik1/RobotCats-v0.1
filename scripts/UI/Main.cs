using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Control
{

	[Export] public Button CatButton;
	[Export] public Label CatCounterLabel;
	[Export] public Label CPSLabel;
	[Export] public VBoxContainer UpgradesContainer;
	[Export] public VBoxContainer FactoriesContainer;
	[Export] public HBoxContainer FactoryRow1;
	[Export] public HBoxContainer FactoryRow2;
	[Export] public HBoxContainer FactoryRow3;

	// instancia de la lógica de negocio.
	private CatManager _catManager = new CatManager();

	// diccionarios para asociar upgrades y fábricas con sus botones.
	private Dictionary<Upgrade, Button> _upgradeButtons = new Dictionary<Upgrade, Button>();
	private Dictionary<Factory, Button> _factoryButtons = new Dictionary<Factory, Button>();

	// diccionario para mapear el nombre de la fábrica a cada SVG.
	private readonly Dictionary<string, string> _factorySvgPaths = new()
	{
		{ "Mini Factory", "res://assets/factories/mini_factory.svg" },
		{ "Simple Factory", "res://assets/factories/simple_factory.svg" },
		{ "Ultrasupermega Factory", "res://assets/factories/ultrasupermega_factory.svg" }
	};

	public override void _Ready()
	{
		// conecta el evento del botón de gatos.
		CatButton.Pressed += OnCatButtonPressed;

		// conecta el evento de actualización de estado.
		_catManager.OnStateChanged += UpdateLabels;

		// actualiza las etiquetas al iniciar.
		UpdateLabels(_catManager.CatCount, _catManager.CatsPerSecond);


		foreach (var upgrade in _catManager.AvailableUpgrades)
		{
			var button = new Button
			{
				Text = GetUpgradeButtonText(upgrade)
			};
			// funcionalidad para evitar que el contenedor se amplie
			button.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
			button.CustomMinimumSize = new Vector2(200, 0);
			button.AutowrapMode = TextServer.AutowrapMode.Word;
			button.Pressed += () => OnUpgradeButtonPressed(upgrade);
			UpgradesContainer.AddChild(button);
			_upgradeButtons[upgrade] = button;
		}

		foreach (var factory in _catManager.AvailableFactories)
		{
			var button = new Button
			{
				Text = GetFactoryButtonText(factory)
			};
			button.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
			button.CustomMinimumSize = new Vector2(200, 0);
			button.AutowrapMode = TextServer.AutowrapMode.Word;
			button.Pressed += () => OnFactoryButtonPressed(factory);
			FactoriesContainer.AddChild(button);
			_factoryButtons[factory] = button;
		}

		UpdateFactoryVisuals();
	}

	public override void _Process(double delta)
	{
		_catManager.Update(delta);
	}

	private void OnCatButtonPressed()
	{
		_catManager.AddCats(_catManager.CatsPerClick);
	}

	private void OnUpgradeButtonPressed(Upgrade upgrade)
	{
		if (_catManager.TryBuyUpgrade(upgrade))
		{
			_upgradeButtons[upgrade].Text = GetUpgradeButtonText(upgrade);
		}
	}

	private void OnFactoryButtonPressed(Factory factory)
	{
		if (_catManager.TryBuyFactory(factory))
		{
			_factoryButtons[factory].Text = GetFactoryButtonText(factory);
			UpdateFactoryVisuals();
		}
	}

	// update de labels con nuevos costos y cps
	private void UpdateLabels(int catCount, int catsPerSecond)
	{
		CatCounterLabel.Text = $"Cats: {catCount}";
		CPSLabel.Text = $"CPS: {catsPerSecond}";
	}

	private string GetUpgradeButtonText(Upgrade upgrade)
	{
		return $"{upgrade.Name} (+{upgrade.CPSIncrease} CPS) - {upgrade.CurrentCost} Cats (x{upgrade.TimesPurchased})";
	}

	private string GetFactoryButtonText(Factory factory)
	{
		return $"{factory.Name} (+{factory.CPSIncrease} CPS) - {factory.CurrentCost} Cats (x{factory.TimesPurchased})";
	}

	// update de visuales de fabrica, mostrando hasta 10 fabricas por tipo
	private void UpdateFactoryVisuals()
	{
		FactoryRow1.QueueFreeChildren();
		FactoryRow2.QueueFreeChildren();
		FactoryRow3.QueueFreeChildren();

		foreach (var factory in _catManager.AvailableFactories)
		{
			HBoxContainer targetRow = null;
			if (factory.Name == "Mini Factory")
				targetRow = FactoryRow1;
			else if (factory.Name == "Simple Factory")
				targetRow = FactoryRow2;
			else if (factory.Name == "Ultrasupermega Factory")
				targetRow = FactoryRow3;

			if (targetRow == null)
				continue;

			int count = Math.Min(factory.TimesPurchased, 10);
			for (int i = 0; i < count; i++)
			{
				var textureRect = new TextureRect
				{
					Texture = GD.Load<Texture2D>(_factorySvgPaths[factory.Name]),
					StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
					CustomMinimumSize = new Vector2(48, 48) // se ajusta el tamaño según la UI
				};
				targetRow.AddChild(textureRect);
			}
		}
	}
}

// extensión para limpiar todos los hijos de un Node (por ejemplo, un HBoxContainer)
public static class NodeExtensions
{
	public static void QueueFreeChildren(this Node node)
	{
		foreach (var child in node.GetChildren())
			(child as Node)?.QueueFree();
	}
}
