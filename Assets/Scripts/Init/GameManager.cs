using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Data;
using Leopotam.EcsLite;
using Systems;
using UnityEngine;
using View;

namespace Init
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private BusinessConfig _businessConfig;
        [SerializeField] private BusinessView _businessPrefab;
        [SerializeField] private Transform _businessContainer;
        [SerializeField] private BalanceView _balanceView;
    
        private EcsWorld _world;
        private EcsSystems _systems;
        private GameData _gameData;
        
        public EcsWorld World => _world;
        public SaveLoadService SaveLoadService = new();
        public Dictionary<string, ProgressData> _businesData = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            SaveLoadService.LoadProgress();
        }

        private void Start()
        {
            _gameData = new GameData
            {
                BusinessConfig = _businessConfig,
            };
            _systems
                .Add(new ClickedBusinessUISystem(_gameData))
                .Add(new UpdateBusinessUISystem())
                .Add(new UpdateBusinessUISystem())
                .Add(new UpdateBusinessProgressSystem())
                .Add(new IncomeProgressSystem())
                .Add(new UpdateBalanceSystem())
                .Init();

            // Загрузка прогресса или создание по конфигу
            if (SaveLoadService.PlayerProgress.isFirstLaunch) CreateBusinessesFromConfig();
            else LoadBusinessesProgress();
            
            
            var balanceEntity = _world.NewEntity();
            _world.GetPool<BalanceComponent>().Add(balanceEntity) = new BalanceComponent {
                Value = SaveLoadService.PlayerProgress.Balance };
            
            _balanceView.SetEntity(_world.PackEntity(balanceEntity));
            _balanceView.SetText(SaveLoadService.PlayerProgress.Balance);
            
            ref var balanceViewRef = ref _world.GetPool<BalanceViewRef>().Add(balanceEntity);
            balanceViewRef.View = _balanceView;
        }

        private void CreateBusinessesFromConfig()
        {
            SaveLoadService.PlayerProgress.isFirstLaunch = false;
            foreach (var businessData in _businessConfig.Businesses)
            {
                var view = Instantiate(_businessPrefab, _businessContainer);
                var entity = _world.NewEntity();
                
                ref var businessViewRef = ref _world.GetPool<BusinessViewRef>().Add(entity);
                businessViewRef.View = view;
                ref var business = ref _world.GetPool<Business>().Add(entity);
                business.Title = businessData.Title;
                business.ID = businessData.ID;
                business.Level = businessData.ID == "1" ? 1 : 0;
                business.Income = businessData.BaseIncome;
                business.BaseIncome = businessData.BaseIncome;
                business.IncomeDelay = businessData.IncomeDelay;
                business.BaseCost = businessData.BaseCost;
                business.UpgradeData1 = businessData.Upgrade1;
                business.UpgradeData2 = businessData.Upgrade2;

                view.SetEntity(_world.PackEntity(entity), business);

                _world.GetPool<UpdateUIEvent>().Add(entity);
                if (businessData.ID == "1")
                {
                    _world.GetPool<IncomeProgress>().Add(entity) = new IncomeProgress { Timer = 0f };
                }

                var data = new ProgressData();
                data.data = business;
                _businesData.Add(business.ID, data);
            }
        }

        private void LoadBusinessesProgress()
        {
            foreach (var businessData in SaveLoadService.PlayerProgress.ProgressDatas)
            {
                var view = Instantiate(_businessPrefab, _businessContainer);
                var entity = _world.NewEntity();
                
                ref var businessViewRef = ref _world.GetPool<BusinessViewRef>().Add(entity);
                businessViewRef.View = view;
                ref var business = ref _world.GetPool<Business>().Add(entity);
                business = businessData.data;

                view.SetEntity(_world.PackEntity(entity), businessData.data);

                _world.GetPool<UpdateUIEvent>().Add(entity);
                if (businessData.data.Level > 0)
                {
                    _world.GetPool<IncomeProgress>().Add(entity) = new IncomeProgress { Timer = businessData.Timer.Timer };
                }
                
                _businesData.Add(businessData.data.ID, businessData);
            }
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }

        private void OnApplicationQuit()
        {
            SaveLoadService.PlayerProgress.ProgressDatas = _businesData.Values.ToList();
            SaveLoadService.SaveProgress();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveLoadService.PlayerProgress.ProgressDatas = _businesData.Values.ToList();
                SaveLoadService.SaveProgress();
            }
        }
    }
}