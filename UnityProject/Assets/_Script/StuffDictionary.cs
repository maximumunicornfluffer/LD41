using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu]
    public class StuffDictionary : ScriptableObject
    {
        public Stuff m_corpsePrefab;
        public Stuff m_corpse2Prefab;
        public Stuff m_corpse3Prefab;
        public Stuff m_potatoPrefab;
        public Stuff m_ashesPrefab;
        public Stuff m_waterBucketPrefab;
        public Stuff m_friesPrefab;

        public Stuff Instantiate(StuffType type, StuffSubType subType, Transform parent)
        {
            switch (type)
            {
                case StuffType.Cinder:
                    return Object.Instantiate(m_ashesPrefab, parent, false);
                case StuffType.Corpes:
                    switch (subType)
                    {
                        case StuffSubType.Corpse1: return Object.Instantiate(m_corpsePrefab, parent, false);
                        case StuffSubType.Corpse2: return Object.Instantiate(m_corpse2Prefab, parent, false);
                        case StuffSubType.Corpse3: return Object.Instantiate(m_corpse3Prefab, parent, false);
                    }
                    break;
                case StuffType.Fries:
                    return Object.Instantiate(m_friesPrefab, parent, false);
                case StuffType.Potatoes:
                    return Object.Instantiate(m_potatoPrefab, parent, false);
                case StuffType.Water:
                    return Object.Instantiate(m_waterBucketPrefab, parent, false);
            }

            return null;
        }
    }
}