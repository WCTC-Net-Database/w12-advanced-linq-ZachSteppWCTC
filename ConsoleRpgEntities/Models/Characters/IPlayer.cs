﻿using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Equipments;

namespace ConsoleRpgEntities.Models.Characters;

public interface IPlayer
{
    int Id { get; set; }
    string Name { get; set; }

    ICollection<Ability> Abilities { get; set; }

    void Attack(ITargetable target);
    void UseAbility(IAbility ability, ITargetable target);

    Inventory GetInventory();
    void UseInventoryItem(string targetName);

    void EquipInventoryItem(string targetName);
}
