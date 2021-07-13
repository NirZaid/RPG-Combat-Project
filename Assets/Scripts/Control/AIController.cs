using System;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        private Fighter _fighter;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (InAttackRange() && _fighter.CanAttack(player))
            {
                _fighter.Attack(player);
            }
            else
            {
                _fighter.Cancel();
            }
            
        }
        
        private bool InAttackRange()
        {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }
    }
        
        
}

       


