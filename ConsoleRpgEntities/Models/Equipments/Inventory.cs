using ConsoleRpgEntities.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models.Equipments
{
    public class Inventory
    {
        public int Id { get; set; }

        // Foreign key
        public int PlayerId { get; set; }

        // Navigation properties
        public virtual Player Player { get; set; }
        public virtual ICollection<Item> Items { get; set; }

        public void FindByName(string targetName) 
        {
            var items = Items.Where(item => item.Name.ToLower().Contains(targetName.ToLower()));
            Console.WriteLine("Search Results: ");
            Display(items.ToList());
        }

        public Item FindSingleItemByName(string targetName)
        {
            Item item = Items.Where(item => item.Name.ToLower() == targetName.ToLower()).FirstOrDefault();
            return item;
        }

        public void ListByType() 
        {
            var items = Items.ToList().GroupBy(item => item.Type);
            foreach (var group in items)
            {
                Console.WriteLine($"Type: {group.Key}");
                Display(group.ToList());
            }
        }

        public void SortByProperty(string property) 
        {
            List<Item> items = new List<Item>();
            switch (property)
            {
                case "attack":
                    items = Items.OrderByDescending(item => item.Attack).ToList();
                    Console.WriteLine("Sorted by Attack:");
                    break;
                case "defense":
                    items = Items.OrderByDescending(item => item.Defense).ToList();
                    Console.WriteLine("Sorted by Defense:");
                    break;
                case "name":
                    items = Items.OrderBy(item => item.Name).ToList();
                    Console.WriteLine("Sorted by Name:");
                    break;
                case "weight":
                    items = Items.OrderByDescending(item => item.Weight).ToList();
                    Console.WriteLine("Sorted by Weight:");
                    break;
                case "value":
                    items = Items.OrderByDescending(item => item.Value).ToList();
                    Console.WriteLine("Sorted by Value:");
                    break;
            }
            Display (items);
        }

        public void Add(Item item) 
        {
            Items.Add(item);
            Console.WriteLine($"{item.Name} added to inventory.");
        }

        public void Remove(Item item)
        {
            Items.Remove(item);
            Console.WriteLine($"{item.Name} removed from inventory.");
        }

        public int Size() { return Items.Count; }

        public void Display (List<Item> list)
        {
            if (list.Count > 0)
            {
                Console.WriteLine("{0,-20} | {1,-10} | {2,-10} | {3,-10} | {4,-10}",
                "Name", "Attack", "Defense", "Weight", "Value", "Attack");

                Console.WriteLine(new string('-', 70)); // Separator line
                foreach (var item in list)
                {
                    Console.WriteLine("{0,-20} | {1,-10} | {2,-10} | {3,-10} | {4,-10}",
                        item.Name, item.Attack, item.Defense, item.Weight, item.Value);
                }
            }
            else
            {
                Console.WriteLine("No Items.");
            }
        }
    }

}
