using UnityEngine;
using System.Collections.Generic;

namespace Data
{
    [CreateAssetMenu(fileName = "BusinessConfig", menuName = "Configs/Business Config")]
    public class BusinessConfig : ScriptableObject
    {
        public List<BusinessData> Businesses = new List<BusinessData>();

        public BusinessData GetBusinessData(string id)
        {
            foreach (var business in Businesses)
            {
                if (business.ID == id)
                    return business;
            }
            Debug.LogError($"Business with ID {id} not found!");
            return default;
        }
    }
}