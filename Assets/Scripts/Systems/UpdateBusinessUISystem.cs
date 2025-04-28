using Components;
using Init;
using Leopotam.EcsLite;

namespace Systems
{
    public class UpdateBusinessUISystem : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
  
            foreach (var eventEntity in world.Filter<UpdateUIEvent>().End())
            {
                ref var business = ref world.GetPool<Business>().Get(eventEntity);
                ref var businessViewRef = ref world.GetPool<BusinessViewRef>().Get(eventEntity);
                businessViewRef.View.UpdateDisplay(business);
                GameManager.Instance._businesData[business.ID].data = business;
                world.GetPool<UpdateUIEvent>().Del(eventEntity); 
            }
        }
    }
}