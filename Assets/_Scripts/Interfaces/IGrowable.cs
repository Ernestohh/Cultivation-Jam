using System.Collections;
using _Scripts.Plants;
using UnityEngine.Events;

namespace _Scripts.Interfaces
{
    public interface IGrowable
    {
        bool IsGrowing { get; set; }
        bool IsSick { get; set; }
        bool IsHarvestable { get; set; }
        PlantScriptableObject PlantScriptableObject { get; set; }
        public UnityEvent OnGrowingHealthy { get; set; }
        public UnityEvent OnGrowingSick { get; set; }
        public UnityEvent OnFullyGrownHealthy { get; set; }
        public UnityEvent OnFullyGrownSick { get; set; }
    }
}
