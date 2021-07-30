using System;
using GameDevTV.Inventories;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private float raycastRadius = 1f;
        [SerializeField] private CursorMapping[] cursorMappings;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private int numberOfAbilities = 6;

        private ActionStore _actionStore;
        private Health _health;
        private bool isDraggingUI = false;

        private void Awake()
        {
            _actionStore = GetComponent<ActionStore>();
            _health = GetComponent<Health>();
        }
        
        private void Update()
        {
            if (InteractWithUI())
            {
                SetCursor(CursorType.UI);
                return;
            }
            if (_health.isDead)
            {
                SetCursor(CursorType.None);
                return;
            }
            UseAbilities();
            
            if (InteractWithComponent())
            {
                return;
            }
            if (InteractWithMovement())
            {
                return;
            }
            SetCursor(CursorType.None);
        }

        private void UseAbilities()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    _actionStore.Use(i, gameObject);
                }
            }
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        RaycastHit[] RayCastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                return true;
            }

            if (isDraggingUI)
            {
                return true;
            }

            return false;

        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                    return mapping;
            }

            return cursorMappings[0];
        }
        
        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target))
                {
                    return false;
                }
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1);
                }
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit)
            {
                return false;
            }
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh)
            {
                return false;
            }
            target = navMeshHit.position;
            return true;
        }

     
        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }



    } 
}
    