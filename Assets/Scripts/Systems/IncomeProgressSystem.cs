using Components;
using Extensions;
using Init;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class IncomeProgressSystem : IEcsRunSystem
    {
        private int _balanceEntity = -1;
        
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<Business>().Inc<IncomeProgress>().End();
            var businessPool = world.GetPool<Business>();
            var progressPool = world.GetPool<IncomeProgress>();
            
            var balancePool = world.GetPool<BalanceComponent>();
            _balanceEntity = world.Filter<BalanceComponent>().End().FirstOrDefault();

            foreach (var entity in filter)
            {
                ref var business = ref businessPool.Get(entity);
                ref var progress = ref progressPool.Get(entity);
                
                if (business.Level > 0)
                {
                    ref var businessViewRef = ref world.GetPool<BusinessViewRef>().Get(entity);
                    
                    progress.Timer += Time.deltaTime;
                    businessViewRef.View.UpdateProgress(progress.Timer);
                    
                    var businessData = GameManager.Instance._businesData[business.ID]; 
                    businessData.Timer.Timer = progress.Timer;
                    GameManager.Instance._businesData[business.ID] = businessData;
                    
                    if (progress.Timer >= business.IncomeDelay)
                    {
                        ref var balance = ref balancePool.Get(_balanceEntity);
                        balance.Value += business.Income;
                        progress.Timer = 0f;
                    }
                }
            }
        }
    }
}