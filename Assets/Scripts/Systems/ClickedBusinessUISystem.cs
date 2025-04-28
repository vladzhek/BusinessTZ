using Components;
using Init;
using Leopotam.EcsLite;
using Extensions;

namespace Systems
{
    public class ClickedBusinessUISystem : IEcsRunSystem
    {
        private readonly GameData _data;

        public ClickedBusinessUISystem(GameData gameData)
        {
            _data = gameData;
        }

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var balancePool = world.GetPool<BalanceComponent>();
            var balanceEntity = world.Filter<BalanceComponent>().End().FirstOrDefault();

            foreach (var eventEntity in world.Filter<ClickEvent>().End())
            {
                ref var click = ref world.GetPool<ClickEvent>().Get(eventEntity);

                if (click.Entity.Unpack(world, out var UIEntity))
                {
                    ref var business = ref world.GetPool<Business>().Get(UIEntity);
                    var config = _data.BusinessConfig.GetBusinessData(business.ID);
                    ref var balance = ref balancePool.Get(balanceEntity);

                    switch (click.Type)
                    {
                        case ButtonType.LevelUp:
                            var cost = (business.Level + 1) * business.BaseCost;
                            
                            if (cost > balance.Value)
                            {
                                world.GetPool<ClickEvent>().Del(eventEntity);
                                return;
                            }
                            
                            if(business.Level == 0) world.GetPool<IncomeProgress>().Add(eventEntity) = new IncomeProgress { Timer = 0f }; 

                            balance.Value -= cost;
                            business.Level++;
                            break;
                        case ButtonType.Upgrade1:
                            if (config.Upgrade1.Cost > balance.Value)
                            {
                                world.GetPool<ClickEvent>().Del(eventEntity);
                                return;
                            }
                            
                            balance.Value -= config.Upgrade1.Cost;
                            business.Upgrade1 = true;
                            break;
                        case ButtonType.Upgrade2:
                            if (config.Upgrade2.Cost > balance.Value)
                            {
                                world.GetPool<ClickEvent>().Del(eventEntity);
                                return;
                            }
                            
                            balance.Value -= config.Upgrade2.Cost;
                            business.Upgrade2 = true;
                            break;
                    }
                    
                    business.Income = business.Level * business.BaseIncome * (1 +
                                      (business.Upgrade1 ? config.Upgrade1.IncomeMultiplier : 0f) +
                                      (business.Upgrade2 ? config.Upgrade2.IncomeMultiplier : 0f));


                    world.GetPool<UpdateUIEvent>().Add(UIEntity);
                }

                world.GetPool<ClickEvent>().Del(eventEntity);
            }
        }
    }
}