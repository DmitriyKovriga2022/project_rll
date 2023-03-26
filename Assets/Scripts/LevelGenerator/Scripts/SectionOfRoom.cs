using System.Collections.Generic;
using System.Linq;
using LevelGenerator.Scripts.Helpers;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class SectionOfRoom : Section
    {
        protected override void GenerateSection(Transform exit)
        {
            AdvancedExit e = exit.GetComponent<AdvancedExit>();
            e.IsBusy = true;
            
            if (IsCorridorFoud(exit)) return;

            var candidate = BuildSectionFromExit(e);
                
            if (candidate == null) PlaceDeadEnd(exit, e.exitDirection);
            else if (LevelGenerator.IsSectionValid(candidate.Bounds, Bounds))
                candidate.Initialize(LevelGenerator, order);
            else
            {
                Destroy(candidate.gameObject);
                PlaceDeadEnd(exit, e.exitDirection);
            }
        }

        private bool IsCorridorFoud(Transform exit)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(exit.position, (Vector2)exit.position + ExitMethods.ConvertDirectioToVector2(exit.GetComponent<AdvancedExit>().exitDirection)*6);

            if (hits.Length != 0)
            {
                foreach (var col in hits)
                {
                    if (col.transform.GetComponentInParent<SectionOfCorridor>() != null) 
                        return true;
                }
            }
            return false;
        }
        

        private Section BuildSectionFromExit(AdvancedExit exit)
        {
            var s = LevelGenerator.PickSectionInDirectionWithTag(exit.CreatesTags, exit.GetComponent<AdvancedExit>().exitDirection);
            var section = Instantiate(s, exit.transform).GetComponent<Section>();
            Transform entrance = Entrance(section, exit.GetComponent<AdvancedExit>().exitDirection);
            
            section.transform.position -= entrance.transform.localPosition;
            return section;
        }

        private Transform Entrance(Section section, ExitDirection direction)
        {
            foreach(var e in section.Exits.ExitSpots)
            {
                AdvancedExit exit = e.GetComponent<AdvancedExit>();
                if(ExitMethods.AreDirectionsOpposite(direction, exit.exitDirection))
                {
                    return e;
                }
            }
            return null;
        }
    }
}