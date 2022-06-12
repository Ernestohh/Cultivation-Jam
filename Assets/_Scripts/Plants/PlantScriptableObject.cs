using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Plants
{
    [CreateAssetMenu(fileName = "_PlantData", menuName = "Plants/PlantData")]
    public class PlantScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string PlantName { get; set; }
        [field: SerializeField] public int GrowTime { get; set; }

        [field: SerializeField]
        [field: Range(0f, 1)]
        public float BaseSicknessChance { get; set; }
        
        [field: SerializeField] public List<string> PositivePlantEffects { get; set; }
        [field: SerializeField] public List<string> NegativePlantEffects { get; set; }
        
        [field: SerializeField] public Mesh PlantMesh { get; set; }
        [field: SerializeField] public Material PlantMaterial { get; set; }
        
    }
}