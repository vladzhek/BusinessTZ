using Components;
using Extensions;
using Init;
using Leopotam.EcsLite;

namespace Systems
{
    public class UpdateBalanceSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var balancePool = world.Filter<BalanceComponent>().End().FirstOrDefault();
            
            ref var balanceViewRef = ref world.GetPool<BalanceViewRef>().Get(balancePool);
            ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(balancePool);
            
            balanceViewRef.View.SetText(balanceComponent.Value);
            GameManager.Instance.SaveLoadService.PlayerProgress.Balance = balanceComponent.Value;
        }
    }
}