using System.Collections.Generic;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class Elements : MonoBehaviour
    {
        public IEnumerable<Element> ContainedElements => GetComponentsInChildren<Element>();
    }
}
