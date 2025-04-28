using Components;
using Init;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View
{
    public class BusinessView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _income;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _upgrade1Button;
        [SerializeField] private Button _upgrade2Button;

        private TMP_Text _lvlUpText;
        private TMP_Text _upgrade1Text;
        private TMP_Text _upgrade2Text;
        
        private UnityAction _levelUpAction;
        private UnityAction _upgrade1Action;
        private UnityAction _upgrade2Action;
        
        private EcsPackedEntity _UIEntity;
        private Business _data;
    
        public void SetEntity(EcsPackedEntity entity, Business data)
        {
            _UIEntity = entity;
            _data = data;
            Subscribe();

            _lvlUpText = _levelUpButton.GetComponentInChildren<TMP_Text>();
            _upgrade1Text = _upgrade1Button.GetComponentInChildren<TMP_Text>();
            _upgrade2Text = _upgrade2Button.GetComponentInChildren<TMP_Text>();
        }

        private void Subscribe()
        {
            _levelUpAction = () => OnLevelUpClicked(ButtonType.LevelUp);
            _upgrade1Action = () => OnLevelUpClicked(ButtonType.Upgrade1);
            _upgrade2Action = () => OnLevelUpClicked(ButtonType.Upgrade2);
            
            _levelUpButton.onClick.AddListener(_levelUpAction);
            _upgrade1Button.onClick.AddListener(_upgrade1Action);
            _upgrade2Button.onClick.AddListener(_upgrade2Action);
        }

        private void OnLevelUpClicked(ButtonType type)
        {
            var world = GameManager.Instance.World;
            
            if (_UIEntity.Unpack(world, out int unpackedEntity))
            {
                ref var clickEvent = ref world.GetPool<ClickEvent>().Add(unpackedEntity);
                clickEvent.Type = type;
                clickEvent.Entity = _UIEntity;
            }
        }
        
        public void UpdateDisplay(Business data)
        {
            string buyed = "Куплено";
            _title.text = data.Title;
            _level.text = "LVL\n" + data.Level;
            _income.text = "Доход\n$" + data.Income;
            _upgrade1Button.interactable = !data.Upgrade1;
            _upgrade2Button.interactable = !data.Upgrade2;
            var cost = (data.Level + 1) * data.BaseCost;
            _lvlUpText.text = $"LVL UP\nЦена: {cost}$";
            var percentIncome1 = data.UpgradeData1.IncomeMultiplier * 100;
            var percentIncome2 = data.UpgradeData2.IncomeMultiplier * 100;
            _upgrade1Text.text = $"{data.UpgradeData1.Title}\nДоход: +{percentIncome1}%\nЦена: {(data.Upgrade1 ? "Куплено" : $"{data.UpgradeData1.Cost}$")}";
            _upgrade2Text.text = $"{data.UpgradeData2.Title}\nДоход: +{percentIncome2}%\nЦена: {(data.Upgrade2 ? "Куплено" : $"{data.UpgradeData2.Cost}$")}";
        }
        
        public void UpdateProgress(float progress)
        {
            float to = _data.IncomeDelay;
            float normalizedProgress = Mathf.Clamp(progress / to, 0f, 1f);
            
            _progressBar.value = normalizedProgress;
        }
        
        void UnsubscribeAll()
        {
            if (_levelUpButton != null) 
                _levelUpButton.onClick.RemoveListener(_levelUpAction);
    
            if (_upgrade1Button != null) 
                _upgrade1Button.onClick.RemoveListener(_upgrade1Action);
    
            if (_upgrade2Button != null) 
                _upgrade2Button.onClick.RemoveListener(_upgrade2Action);
        }

        private void OnDestroy()
        {
            UnsubscribeAll();
        }
    }
}