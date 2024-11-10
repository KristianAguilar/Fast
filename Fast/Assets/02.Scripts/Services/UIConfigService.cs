using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class UIConfigService : MonoBehaviour
    {
        public static UIConfigService Instance { get; private set; }

        /// <summary>
        /// List with a sprite for each category
        /// </summary>
        public List<CategoryDesign> CategoryDesigns;

        /// <summary>
        /// Icon related to task points
        /// </summary>
        public Sprite pointsIcon;
        /// <summary>
        /// Reward close icon sprite
        /// </summary>
        public Sprite rewardIconClose;
        /// <summary>
        /// Reward open icon sprite
        /// </summary>
        public Sprite rewardIconOpen;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }    
        }

        /// <summary>Return a sprite for the request category</summary>
        public CategoryDesign GetCategoryDesign(TaskCategory category)
        {
            return CategoryDesigns.Find((pair) => pair.category == category);
        }
    }

    [System.Serializable]
    public class CategoryDesign
    {
        public TaskCategory category;
        public Sprite sprite;
        public Color color;
    }
}