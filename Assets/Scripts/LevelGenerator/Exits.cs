using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class Exits : MonoBehaviour
    {
        public IEnumerable<AdvancedExit> ExitSpots => GetComponentsInChildren<AdvancedExit>().Where(t => t != null);
    }
}
