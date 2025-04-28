using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct BusinessData
    {
        [Header("Base Settings")]
        public string ID;
        public string Title;
        public float IncomeDelay; //Время получение прибыли
        public int BaseCost; // Стоимость покупки бизнеса
        public float BaseIncome; // Базовая прибыль

        [Header("Upgrades")]
        public UpgradeData Upgrade1;
        public UpgradeData Upgrade2;
    }

    [System.Serializable]
    public struct UpgradeData
    {
        public string Title;
        public int Cost;
        [Range(0f, 5f)] public float IncomeMultiplier;
    }
}