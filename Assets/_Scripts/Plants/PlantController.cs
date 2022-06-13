using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Plants
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class PlantController : MonoBehaviour, IGrowable
    {
        [field: SerializeField] public bool IsGrowing { get; set; }
        [field: SerializeField] public bool IsSick { get; set; }
        [field: SerializeField] public bool IsHarvestable { get; set; }
        [field: SerializeField] public PlantScriptableObject PlantScriptableObject { get; set; }
        [field: SerializeField] public UnityEvent OnGrowingHealthy { get; set; }
        [field: SerializeField] public UnityEvent OnGetCured { get; set; }
        [field: SerializeField] public UnityEvent OnGrowingSick { get; set; }
        [field: SerializeField] public UnityEvent OnFullyGrownHealthy { get; set; }
        [field: SerializeField] public UnityEvent OnFullyGrownSick { get; set; }

        private string _plantName;
        private int _amountOfDaysToGrow, _amountOfPlantStages;
        private float _baseSickness;
        private float maximumScale = 3f;
        private List<string> _positivePlantEffects, _negativePlantEffects;
        private Mesh _plantMesh;
        private Material _plantMaterial;

        protected IEnumerator StartToGrow()
        {
            var timer = 0f;
            var startScale = transform.localScale;
            var harvestableScale = new Vector2(maximumScale, maximumScale);
            IsGrowing = true;
            do
            {
                transform.localScale = Vector3.Lerp(startScale, harvestableScale, timer / _amountOfDaysToGrow);
                timer += Time.deltaTime;
                yield return null;
            } while (timer < _amountOfDaysToGrow && IsGrowing && !IsHarvestable);
            if((Vector2)transform.localScale == harvestableScale)
                IsHarvestable = true;
        }
        
        private void Start()
        {
            InitializePlantData();
        }

        private void InitializePlantData()
        {
            SetPlantData();
            gameObject.GetComponent<MeshFilter>().mesh = _plantMesh;
            gameObject.GetComponent<MeshRenderer>().material = _plantMaterial;
            gameObject.GetComponent<MeshCollider>().convex = true;
            gameObject.GetComponent<MeshCollider>().sharedMesh = _plantMesh;
        }

        private void SetPlantData()
        {
            _plantName = PlantScriptableObject.PlantName;
            _amountOfDaysToGrow = PlantScriptableObject.AmountOfDaysToGrow;
            _amountOfPlantStages = PlantScriptableObject.AmountOfPlantStages;
            _baseSickness = PlantScriptableObject.BaseSicknessChance;
            _positivePlantEffects = PlantScriptableObject.PositivePlantEffects;
            _negativePlantEffects = PlantScriptableObject.NegativePlantEffects;
            _plantMesh = PlantScriptableObject.PlantMesh;
            _plantMaterial = PlantScriptableObject.PlantMaterial;
        }
    }
}