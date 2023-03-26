using System.Collections.Generic;
using System.Linq;
using LevelGenerator.Scripts.Helpers;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class Section : MonoBehaviour
    {
        /// <summary>
        /// Section tags
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// Tags that this section can annex
        /// </summary>
        public string[] CreatesTags;

        /// <summary>
        /// Exits node in hierarchy
        /// </summary>
        public Exits Exits;

        /// <summary>
        /// Bounds node in hierarchy
        /// </summary>
        public Bounds Bounds;

        /// <summary>
        /// Chances of the section spawning a dead end
        /// </summary>
        public int DeadEndChance;

        protected LevelGenerator LevelGenerator;
        protected int order;
        
        public void Initialize(LevelGenerator levelGenerator, int sourceOrder)
        {
            LevelGenerator = levelGenerator;
            transform.SetParent(LevelGenerator.Container);
            LevelGenerator.RegisterNewSection(this);
            order = sourceOrder + 1;

            GenerateAnnexes();
        }

        protected void GenerateAnnexes()
        {
            if (CreatesTags.Any())
            {
                List<Transform> exits = Exits.ExitSpots.ToList<Transform>();
                while (exits.Count > 0)
                {
                    var e = exits.PickOne();
                    exits.Remove(e);
                    if (e.GetComponent<AdvancedExit>().IsBusy) continue;
                    if (LevelGenerator.LevelSize > 0 && order < LevelGenerator.MaxAllowedOrder)
                        if (RandomService.RollD100(DeadEndChance))
                            PlaceDeadEnd(e, e.GetComponent<AdvancedExit>().exitDirection);
                        else
                            GenerateSection(e);
                    else
                        PlaceDeadEnd(e, e.GetComponent<AdvancedExit>().exitDirection);
                }
            }
        }

        protected virtual void GenerateSection(Transform exit) {}

        protected virtual Section BuildSectionFromExit(Section section, Transform exit, Transform entrance) 
            => Instantiate(section, exit.position - entrance.localPosition, exit.rotation, exit).GetComponent<Section>();

        protected virtual void PlaceDeadEnd(Transform exit, ExitDirection dir) 
        {
            List<DeadEnd> ends = new List<DeadEnd>();
            foreach(var end in LevelGenerator.DeadEnds)
                if(dir == end.exitDirection) { ends.Add(end); }
            Instantiate(ends.PickOne(), exit).Initialize(LevelGenerator);
        }
    }
}