using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;
        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null; 
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();

            if (!lookupTable[characterClass].ContainsKey(stat))
            {
                return 0;
            }

            float[] levels = lookupTable[characterClass][stat];
            
            if (levels.Length == 0)
            {
                return 0;
            }
            if (levels.Length < level)
            {
                return levels[levels.Length - 1];
            }
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if(lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats )
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookUpTable;
            }
        }



        // Classes ------------------------------------------------------------------------------
        
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            
        }
        
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        
        

    }
}
