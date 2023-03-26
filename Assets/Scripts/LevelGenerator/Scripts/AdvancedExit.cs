using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class AdvancedExit : MonoBehaviour
    {
        /// <summary>
        /// Tags that this section can annex
        /// </summary>
        public string[] CreatesTags;
        public ExitDirection exitDirection;
        public bool IsBusy = false;
    }
}
