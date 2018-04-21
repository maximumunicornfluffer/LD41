using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu]
    public class StuffDictionary : ScriptableObject
    {
        public Stuff m_corpsePrefab;
        public Stuff m_potatoPrefab;
        public Stuff m_ashesPrefab;
        public Stuff m_waterBucketPrefab;
        public Stuff m_friesPrefab;

        public Stuff Instantiate(StuffType type, Transform parent)
        {
            switch (type)
            {
                case StuffType.Cinder:
                    return Object.Instantiate(m_ashesPrefab, parent, false);
                case StuffType.Corpes:
                    return Object.Instantiate(m_corpsePrefab, parent, false);
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