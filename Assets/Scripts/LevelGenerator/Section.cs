using System;
using System.Collections.Generic;
using System.Linq;
using LevelGenerator.Scripts.Helpers;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class Section : MonoBehaviour
    {
        public string[] Tags;
        public string[] CreatesTags;
        public Elements Elements;
        public RoomController RoomController;

        private LevelGenerator _levelGenerator;
        
        public void Initialize(LevelGenerator levelGenerator, Vector2Int sectionPositionOnMap)
        {
            _levelGenerator = levelGenerator;
            _levelGenerator.RegisterNewSection(this, sectionPositionOnMap);

            RoomController.Initialize(levelGenerator.CmConfiner);

            GenerateAnnexes();
        }

        private void GenerateAnnexes()
        {
            if (CreatesTags.Any())
            {
                List<AdvancedExit> exits = GetAllExits();
                while (exits.Count > 0)
                {
                    var exit = exits.PickOne();
                    exits.Remove(exit);
                    if (exit.CurrentState != AdvancedExit.State.free) continue;
                    if (_levelGenerator.IsExitExtreme(exit)) 
                    {
                        exit.SetDeadEnd();
                        continue;
                    }
                    if (IsRoomFoud(exit)) continue;
                    GenerateSection(exit);
                }
            }
        }

        private List<AdvancedExit> GetAllExits()
        {
            List<AdvancedExit> exits = new();
            foreach (var element in Elements.ContainedElements)
                exits.AddRange(element.ExitSpots);
            return exits;
        }

        private bool IsRoomFoud(AdvancedExit exit)
        {
            Element neighborElement = _levelGenerator.GetNeighborElement(exit);
            if (neighborElement != null)
            {
                foreach(var entrance in neighborElement.ExitSpots)
                {
                    if (ExitMethods.AreDirectionsOpposite(exit.ExitDirection, entrance.ExitDirection))
                    {
                        SetExits(exit, entrance);
                        return true;
                    }
                }
            }
            return false;
        }

        private void GenerateSection(AdvancedExit exit) 
        {
            List<Section> sectionsToIgnore = new();
            while(true)
            {
                try
                {
                    var candidate = _levelGenerator.PickSectionWithTag(Tags, sectionsToIgnore);
                    List<Element> elements = candidate.Elements.ContainedElements.ToList();
                    while(elements.Count > 0)
                    {
                        var element = elements.PickOne();
                        Vector2Int sectionPosOnMap;
                        if (element.GetExitInDirection(ExitMethods.GetOppositeDirection(exit.ExitDirection)) != null)
                        {
                            if (_levelGenerator.IsSectionValid(exit, candidate, element, out sectionPosOnMap))
                            {
                                var newSection = BuildSection(candidate, sectionPosOnMap);
                                TakeEntrance(newSection, element.Position, exit);
                                newSection.Initialize(_levelGenerator, sectionPosOnMap);
                                return;
                            }
                        }
                        elements.Remove(element);
                    }
                    sectionsToIgnore.Add(candidate);
                }
                catch (System.Exception) 
                { 
                    exit.SetDeadEnd();
                    break; 
                }
            }
        }


        private void TakeEntrance(Section newSection, Vector2Int elementPosition, AdvancedExit exit)
        {
            foreach(var element in newSection.Elements.ContainedElements)
            {
                if(element.Position == elementPosition)
                {
                    foreach (var entrance in element.ExitSpots)
                    {
                        if (ExitMethods.AreDirectionsOpposite(entrance.ExitDirection, exit.ExitDirection))
                        {
                            SetExits(exit, entrance);
                            return;
                        }
                    }
                }
            }
        }

        private void SetExits(AdvancedExit exit, AdvancedExit entrance)
        {
            exit.SetExit(entrance, RoomController);
        }

        private Section BuildSection(Section section, Vector2Int sectionPositionOnMap) => Instantiate(section, _levelGenerator.SetWorldPosition(sectionPositionOnMap), Quaternion.Euler(0,0,0), _levelGenerator.Container);
    }
}