using System;
using System.Linq;
using UnityEngine;

namespace _Scripts.Plants
{
    [RequireComponent(typeof(PlantController),typeof(Animator), typeof(Outline))]
    public class Plant : PlantController
    {
        public int _currentGrowthStage = 0;
        public int _daysPassed = 0;
        public void Growing()
        {
            if (IsSick || IsHarvestable) return;
            _daysPassed++;
            IsGrowing = true;
            if (_daysPassed >= AmountOfDaysToGrow)
                IsHarvestable = true;
        }

        public void ChangeOutlineColor()
        {
            if(IsSick)
                GetComponent<Outline>().OutlineColor = Color.red;
            if(IsGrowing)
                GetComponent<Outline>().OutlineColor = Color.yellow;
            if(IsHarvestable)
                GetComponent<Outline>().OutlineColor = Color.green;
        }
        public void BecomeSick()
        {
            if (IsSick) return;
            IsGrowing = false;
            IsSick = true;
        }

        public void Harvest()
        {
            if (!IsHarvestable) return;
            Destroy(gameObject);
        }
        public void GoToNextGrowStage()
        {
            for (int i = 0; i < AmountOfGrowthStages.Count; i++)
            {
                if (AmountOfGrowthStages[i] == _daysPassed)
                {
                    _currentGrowthStage = i;
                }
            }

            if (_currentGrowthStage == 0) return;
            gameObject.GetComponent<MeshFilter>().mesh = PlantMeshes[_currentGrowthStage];
            gameObject.GetComponent<MeshRenderer>().material = PlantMaterial;
            gameObject.GetComponent<MeshCollider>().convex = true;
            gameObject.GetComponent<MeshCollider>().sharedMesh = PlantMeshes[_currentGrowthStage];
        }
    }
}