using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class UpdateBusinessProgressSystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
  
            foreach (var eventEntity in world.Filter<IncomeProgress>().End())
            {
                ref var business = ref world.GetPool<Business>().Get(eventEntity);
                ref var businessViewRef = ref world.GetPool<BusinessViewRef>().Get(eventEntity);
                ref var progress = ref world.GetPool<IncomeProgress>().Get(eventEntity);
                
                businessViewRef.View.UpdateProgress(progress.Timer / business.IncomeDelay);
            }
        }
    }
}