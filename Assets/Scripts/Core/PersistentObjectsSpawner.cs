
using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjectPrefab;
        private static bool hasSpawned;
        
        private void Awake()
        {
            if(hasSpawned) return;

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }


    }
}

