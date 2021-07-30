using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click", order = 0)]
        
    public class DelayedClickTargeting : TargetingStrategy
    {
        
        private PlayerController playerConroller;
        [SerializeField] private Texture2D cursorTexture;
        [SerializeField] private Vector2 cursorHotspot;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float areaEffectRadius;
        [SerializeField] private Transform targetingPrefab;

        private Transform targetingPrefabInstance;
        
        
        public override void StartTargeting(AbilityData data, Action finished)
        {
            playerConroller =  data.GetUser().GetComponent<PlayerController>();
            playerConroller.StartCoroutine(Targeting(data, playerConroller, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;
            if (targetingPrefabInstance == null)
            {
                targetingPrefabInstance = Instantiate(targetingPrefab);
            }
            else
            {
                targetingPrefabInstance.gameObject.SetActive(true);
            }

            targetingPrefabInstance.localScale = new Vector3(areaEffectRadius * 2, 1, areaEffectRadius * 2);


            while (!data.IsCanceled())
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycastHit.point;
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        break;
                    }
                }
                yield return null;
            }
            playerController.enabled = true;
            targetingPrefabInstance.gameObject.SetActive(false);
            finished();

        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            {
                RaycastHit[] hits = Physics.SphereCastAll(point, areaEffectRadius, Vector3.up, 0);
                foreach (var hit in hits)
                {
                    yield return hit.collider.gameObject;
                }
                
            }
        }
    }
}