using System;
using System.Collections.Generic;
using Components;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public bool isFirstLaunch;
        public float Balance;
        public List<ProgressData> ProgressDatas = new();

        public PlayerProgress()
        {
            isFirstLaunch = true;
            Balance = 0;
        }
    }

    [Serializable]
    public class ProgressData
    {
        public Business data;
        public IncomeProgress Timer;
    }
}