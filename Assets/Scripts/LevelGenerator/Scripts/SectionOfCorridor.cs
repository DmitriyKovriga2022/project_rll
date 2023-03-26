using System.Collections.Generic;
using System.Linq;
using LevelGenerator.Scripts.Helpers;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class SectionOfCorridor : Section
    {
        protected override void GenerateSection(Transform exit)
        {
            exit.GetComponent<AdvancedExit>().IsBusy = true;
            if (IsRoomFoud(exit)) return;

            if (!TryBuildSection(exit))
                RemoveCorridor(exit);
            
        }

        private bool IsRoomFoud(Transform exit)
        {
            AdvancedExit advExit = exit.GetComponent<AdvancedExit>();
            RaycastHit2D[] hits = Physics2D.LinecastAll(exit.position, (Vector2)exit.position + ExitMethods.ConvertDirectioToVector2(advExit.exitDirection)*5);

            if (hits.Length != 0)
            {
                foreach (var col in hits)
                {
                    if (col.transform.GetComponentInParent<DeadEnd>() != null) 
                        Destroy(col.transform.GetComponentInParent<DeadEnd>().gameObject);
                }
                return true;
            }
            return false;
        }

        private bool TryBuildSection(Transform exit)
        {
            AdvancedExit advExit = exit.GetComponent<AdvancedExit>();
            
            Dictionary<Section,List<Transform>> candidates = LevelGenerator.PickSections(advExit.CreatesTags, advExit.exitDirection);
            while(candidates.Count > 0)
            {
                var section = candidates.Keys.PickOne();

                while(candidates[section].Count > 0)
                {
                    var entrance = candidates[section].PickOne();
                    var candidate = Instantiate(section, exit.position - entrance.localPosition, exit.rotation, exit).GetComponent<Section>();
                    if (LevelGenerator.IsSectionValid(candidate.Bounds, Bounds))
                    {
                        TakeEntrance(candidate, entrance);
                        candidate.Initialize(LevelGenerator, order);
                        return true;
                    }
                    Destroy(candidate.gameObject);
                    candidates[section].Remove(entrance);
                }
                candidates.Remove(section);
            }
            return false;
        }

        private void TakeEntrance(Section section, Transform entrance)
        {
            foreach(var ex in section.Exits.ExitSpots)
            {
                if(ex.localPosition == entrance.localPosition)
                {
                    ex.GetComponent<AdvancedExit>().IsBusy = true;
                    break;
                }
            }
        }

        private void RemoveCorridor(Transform exit)
        {
            var end = Exits.ExitSpots.Except((IEnumerable<Transform>)exit).First();
            PlaceDeadEnd(end, ExitMethods.GetOppositeDirection(end.GetComponent<AdvancedExit>().exitDirection));
            Destroy(this.gameObject);
        }
    }
}
