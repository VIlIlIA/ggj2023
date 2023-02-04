using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "WorldGenNoiseSetting")]
    public class WorldGenNoise : UnityEngine.ScriptableObject
    {
        [field: Range(0.01f, 0.9f)]
        [field: SerializeField]
        public float startFequency { get; private set; }

        [field: Min(1)]
        [field: SerializeField]
        public int octave { get; private set; }

        [field: Min(0)]
        [field: SerializeField]
        public float presistance { get; private set; }

        [field: Min(0)]
        [field: SerializeField]
        public float frequencyModifier { get; private set; }

        [field: SerializeField] public Vector2Int offset { get; private set; }

        [field: SerializeField] public int noiseRangeMin { get; private set; }
        [field: SerializeField] public int noiseRangeMax { get; private set; }
    }
}