using System;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    
    public class PlayerController : MonoBehaviour
    {
        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();
        }
        
        private void Update()
        {
            if(_health.isDead)
                return;
            if (InteractWithCombat())
                return;
            if(InteractWithMovement())
                return;
            Debug.Log("nothing to do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if(target == null) continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1);
                }
            }
            return hasHit;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }



    } 
}
    