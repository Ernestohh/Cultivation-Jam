using System;
using UnityEngine;

namespace _Scripts.Plants
{
    [RequireComponent(typeof(PlantController),typeof(Animator))]
    public class Plant : PlantController
    {
        private void Update()
        {
            if(Input.GetAxisRaw("Horizontal") > 0 && !IsGrowing)
                OnGrowingHealthy?.Invoke();
            if(Input.GetAxisRaw("Vertical") > 0 && IsGrowing)
                OnGrowingSick?.Invoke();
        }

        public void Growing()
        {
            if (IsSick || IsHarvestable) return;
            AmountOfDaysToGrow--;
            if (AmountOfDaysToGrow <= 0)
                IsHarvestable = true;
        }
        public void BecomeSick()
        {
            if (IsSick) return;
            StopCoroutine(StartToGrow());
            IsGrowing = false;
            IsSick = true;
        }
    }
}