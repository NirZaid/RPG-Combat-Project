using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.Attributes;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] private Transform respawnLocation;
        [SerializeField] private float respawnDelay = 3f;
        [SerializeField] private float fadeTime = 0.2f;
        [SerializeField] private float healthRegenPercentage = 20;
        [SerializeField] private float enemyRespawnHealthPercentage;


        private void Awake()
        {
            GetComponent<Health>().onDie.AddListener(Respawn);
        }

        private void Start()
        {
            if (GetComponent<Health>().IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            
            yield return new WaitForSeconds(respawnDelay);
            Debug.Log("Finished Waiting");
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            RespawnPlayer();
            ResetEnemies();
            
            savingWrapper.Save();
            yield return fader.FadeIn(fadeTime);

        }

        private void ResetEnemies()
        {
            foreach (AIController aiController in FindObjectsOfType<AIController>())
            {
                Health health = aiController.GetComponent<Health>();
                if (health && !health.IsDead())
                {
                    aiController.Reset();
                    health.Heal(health.GetMaxHealthPoints() * enemyRespawnHealthPercentage/100);
                }
            }
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            Health health = GetComponent<Health>();
            health.Heal(healthRegenPercentage * health.GetMaxHealthPoints() / 100);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta );
            }
        }
    }
}


