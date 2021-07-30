using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Abilities;
using UnityEngine;


public class CooldownStore : MonoBehaviour
{
   private Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
   private Dictionary<InventoryItem, float> initialCooldownTimes = new Dictionary<InventoryItem, float>();
   
   public void StartCooldown(InventoryItem inventoryItem, float cooldownTime)
   {
      cooldownTimers[inventoryItem] = cooldownTime;
      initialCooldownTimes[inventoryItem] = cooldownTime;
   }

   private void Update()
   {

      List<InventoryItem> inventoryItems = new List<InventoryItem>(cooldownTimers.Keys);
      foreach (InventoryItem item in inventoryItems)
      {
         cooldownTimers[item] -= Time.deltaTime;
         if (cooldownTimers[item] < 0)
         {
            cooldownTimers.Remove(item);
            initialCooldownTimes.Remove(item);
         }
      }
   }

   public float GetTimeRemaining(InventoryItem inventoryItem)
   {
      if (!cooldownTimers.ContainsKey(inventoryItem))
      {
         return 0;
      }

      return cooldownTimers[inventoryItem];
   }

   public float GetFractionRemaining(InventoryItem inventoryItem)
   {
      if (inventoryItem == null)
      {
         return 0;
      }

      if (!cooldownTimers.ContainsKey(inventoryItem))
      {
         return 0;
      }

      return cooldownTimers[inventoryItem] / initialCooldownTimes[inventoryItem];
   }
}
