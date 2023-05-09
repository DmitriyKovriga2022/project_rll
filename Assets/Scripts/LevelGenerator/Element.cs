using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class Element : MonoBehaviour
    {
        public IEnumerable<AdvancedExit> ExitSpots => GetComponentsInChildren<AdvancedExit>().Where(t => t != null);
        public Vector2Int Position;
        [NonSerialized] public Vector2Int PositionOnMap;
        
        public Section Section => GetComponentInParent<Section>();

        public AdvancedExit GetExitInDirection(ExitDirection direction)
        {
            foreach (var exit in ExitSpots)
            {
                if (exit.ExitDirection == direction)
                    return exit;
            }
            return null;
        }
    }
}
