using System.Collections.Generic;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class Bounds : MonoBehaviour
    {
        public IEnumerable<Collider2D> Colliders => GetComponentsInChildren<Collider2D>();
    }
}
