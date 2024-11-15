using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using ConsoleRpgEntities.Models.Equipments;

namespace ConsoleRpgEntities.Models.Characters
{
    public class Player : ITargetable, IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }

        // Foreign key
        public int? EquipmentId { get; set; }

        // Navigation properties
        public virtual Inventory Inventory { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual ICollection<Ability> Abilities { get; set; }

        public void Attack(ITargetable target)
        {
            // Player-specific attack logic
            Console.WriteLine($"{Name} attacks {target.Name} with a {Equipment.Weapon.Name} dealing {Equipment.Weapon.Attack} damage!");
            target.Health -= Equipment.Weapon.Attack;
            System.Console.WriteLine($"{target.Name} has {target.Health} health remaining.");

        }

        public void UseAbility(IAbility ability, ITargetable target)
        {
            if (Abilities.Contains(ability))
            {
                ability.Activate(this, target);
            }
            else
            {
                Console.WriteLine($"{Name} does not have the ability {ability.Name}!");
            }
        }

        public void UseInventoryItem(string targetName)
        {
            // Currently set to be a roundabout way to equip items if the item is a type of equipment.
            // Another potential implementation of "use" could be "inspect": adding a description of every item that is read out if it is used.
            Item item = Inventory.FindSingleItemByName(targetName);
            if (item != null)
            {
                switch (item.Type)
                {
                    case "Weapon" or "Armor":
                        EquipInventoryItem(item.Name);
                        break;
                    case "Potion":
                        UseConsumable(item);
                        break;
                    // Further item use could include notes, or single-use / weapons / affliction.
                    /*
                    case "Note":
                        Console.WriteLine(item.Text);
                        break;
                    */
                }
            }
            else
            {
                Console.WriteLine("Item not found in inventory.");
            }
        }

        public void UseConsumable(Item item)
        {
            Console.WriteLine($"{Name} uses the {item.Name}");
            Inventory.Remove(item);
        }

        public void EquipInventoryItem(string targetName) 
        {
            // Could add rechargable consumables as an equipment type, I tried something similar in Week 11
            Item item = Inventory.FindSingleItemByName(targetName);
            if (item != null)
                switch (item.Type)
                {
                    case ("Weapon"):
                        if (Equipment.Weapon != null)
                            Inventory.Add(Equipment.Weapon);
                        Equipment.Weapon = item;
                        Inventory.Remove(item);
                        Console.WriteLine($"{item.Name} equipped as Weapon.");
                        break;
                    case ("Armor"):
                        if (Equipment.Armor != null)
                            Inventory.Add(Equipment.Armor);
                        Equipment.Armor = item;
                        Inventory.Remove(item);
                        Console.WriteLine($"{item.Name} equipped as Armor.");
                        break;

                }
            else
            {
                Console.WriteLine("Item not found in inventory.");
            }
        }

        public Inventory GetInventory()
        {
            return Inventory;
        }
    }
}
