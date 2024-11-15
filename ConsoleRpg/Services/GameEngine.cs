using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;

    private IPlayer _player;
    private IMonster _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }

    private void GameLoop()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Attack");
            _outputManager.WriteLine("2. Inventory");
            _outputManager.WriteLine("3. Quit");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AttackCharacter();
                    break;
                case "2":
                    ViewInventory(); 
                    break;
                case "3":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose an option.", ConsoleColor.Red);
                    break;
            }
        }
    }

    private void AttackCharacter()
    {
        if (_goblin is ITargetable targetableGoblin)
        {
            _player.Attack(targetableGoblin);
            _player.UseAbility(_player.Abilities.First(), targetableGoblin);
            Random random = new Random();
            int randomNumber = random.Next(1, 137);
            _player.GetInventory().Add(_context.Items.Where(item => item.Id == randomNumber).FirstOrDefault());
        }
    }

    private string GetTargetItemName()
    {
        _outputManager.WriteLine("Enter Item Name:", ConsoleColor.Cyan);
        _outputManager.Display();
        return Console.ReadLine();
    }
    private void ViewInventory()
    {
        if (_player.GetInventory().Size() > 0)
        {
            _outputManager.WriteLine("Inventory Management:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Search for Item by Name");
            _outputManager.WriteLine("2. List Items by Type");
            _outputManager.WriteLine("3. Sort Items");
            _outputManager.WriteLine("4. Equip Item");
            _outputManager.WriteLine("5. Use Item");
            _outputManager.WriteLine("6. Discard Item");
            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _player.GetInventory().FindByName(GetTargetItemName());
                    break;
                case "2":
                    _player.GetInventory().ListByType();
                    break;
                case "3":
                    SortInventory(_player.GetInventory());
                    break;
                case "4":
                    _player.EquipInventoryItem(GetTargetItemName());
                    break;
                case "5":
                    _player.UseInventoryItem(GetTargetItemName());
                    break;
                case "6":
                    _player.GetInventory().Remove(_player.GetInventory().FindSingleItemByName(GetTargetItemName()));
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose an option.", ConsoleColor.Red);
                    break;
            }
        }
        else
        {
            Console.WriteLine("Inventory is empty. Attack monsters to find new items.");
        }
    }

    private void SortInventory(Inventory inventory)
    {
        _outputManager.WriteLine("Sort Options:", ConsoleColor.Cyan);
        _outputManager.WriteLine("1. Name");
        _outputManager.WriteLine("2. Value");
        _outputManager.WriteLine("3. Weight");
        _outputManager.WriteLine("4. Attack");
        _outputManager.WriteLine("5. Defense");
        _outputManager.Display();

        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                inventory.SortByProperty("name");
                break;
            case "2":
                inventory.SortByProperty("value");
                break;
            case "3":
                inventory.SortByProperty("weight");
                break;
            case "4":
                inventory.SortByProperty("attack");
                break;
            case "5":
                inventory.SortByProperty("defense");
                break;
            default:
                _outputManager.WriteLine("Invalid selection.", ConsoleColor.Red);
                break;
        }
    }

    private void SetupGame()
    {
        _player = _context.Players.FirstOrDefault();
        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);
        // Load monsters into random rooms 
        LoadMonsters();

        // Pause before starting the game loop
        Thread.Sleep(500);
        GameLoop();
    }

    private void LoadMonsters()
    {
        _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
    }

}
