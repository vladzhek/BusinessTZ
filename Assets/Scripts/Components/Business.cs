using System;
using Data;

namespace Components
{
    [Serializable]
    public struct Business
    {
        public string Title;
        public string ID;
        public int Level;
        public float BaseIncome;
        public float BaseCost;
        public float Income;
        public bool Upgrade1;
        public bool Upgrade2;
        public UpgradeData UpgradeData1;
        public UpgradeData UpgradeData2;
        public float IncomeDelay;
    }
}