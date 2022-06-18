using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Plants
{
    [CreateAssetMenu(fileName = "_PlantData", menuName = "Plants/PlantData")]
    public class PlantScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string PlantName { get; set; }
        [field: SerializeField] public int AmountOfDaysToGrow { get; set; }
        [field: SerializeField] public List<int> AmountOfGrowthStages { get; set; }
        [field: SerializeField]
        [field: Range(0f, 1)]
        public float BaseSicknessChance { get; set; }
        
        [field: SerializeField] public List<string> PositivePlantEffects { get; set; }
        [field: SerializeField] public List<string> NegativePlantEffects { get; set; }
        
        [field: SerializeField] public List<Mesh> PlantMeshes { get; set; }
        [field: SerializeField] public Material PlantMaterial { get; set; }
        
    }
}

public class GrowthStages
{
    public int AmountOfDaysToReachPlantStage, GrowthStage;
}