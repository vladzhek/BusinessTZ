using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

namespace View
{
    public class BalanceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _balanceText;
        
        private EcsPackedEntity _entity;
    
        public void SetEntity(EcsPackedEntity entity)
        {
            _entity = entity;
        }

        public void SetText(float amount)
        {
            _balanceText.text = $"Баланс: {amount:0.0}$";
        }
    }
}