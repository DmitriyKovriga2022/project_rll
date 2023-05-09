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

        protected LevelGenerator LevelGenerator;
        protected Vector2Int PositionOnMap;
        
        public virtual void Initialize(LevelGenerator levelGenerator, Vector2Int sectionPositionOnMap)
        {
            LevelGenerator = levelGenerator;
            PositionOnMap = sectionPositionOnMap;
            RegisterInGenerator();
            GenerateAnnexes();
        }

        protected virtual void RegisterInGenerator() => LevelGenerator.RegisterNewSection(this, PositionOnMap);

        public void ActivateRoomController(CameraController controller) => RoomController.Initialize(controller);

        protected virtual void GenerateAnnexes()
        {
            if (CreatesTags.Any())
            {
                List<AdvancedExit> exits = GetAllExits();
                while (exits.Count > 0)
                {
                    var exit = exits.PickOne();
                    exits.Remove(exit);
                    if (exit.CurrentState != AdvancedExit.State.free) continue;
                    if (LevelGenerator.IsExitExtreme(exit)) 
                    {
                        exit.SetDeadEnd();
                        continue;
                    }
                    if (IsRoomFoud(exit)) continue;
                    GenerateSection(exit);
                }
            }
        }

        protected List<AdvancedExit> GetAllExits()
        {
            List<AdvancedExit> exits = new();
            foreach (var element in Elements.ContainedElements)
                exits.AddRange(element.ExitSpots);
            return exits;
        }

        protected bool IsRoomFoud(AdvancedExit exit)
        {
            Element neighborElement = LevelGenerator.GetNeighborElement(exit);
            if (neighborElement != null)
            {
                foreach(var entrance in neighborElement.ExitSpots)
                {
                    if (ExitMethods.AreDirectionsOpposite(exit.ExitDirection, entrance.ExitDirection))
                    {
                        SetExits(exit, entrance, neighborElement.Section);
                        return true;
                    }
                }
            }
            return false;
        }

        protected void GenerateSection(AdvancedExit exit) 
        {
            List<Section> sectionsToIgnore = new();
            while(true)
            {
                try
                {
                    var candidate = LevelGenerator.PickSectionWithTag(CreatesTags, sectionsToIgnore);
                    List<Element> elements = candidate.Elements.ContainedElements.ToList();
                    while(elements.Count > 0)
                    {
                        var element = elements.PickOne();
                        Vector2Int sectionPosOnMap;
                        if (element.GetExitInDirection(ExitMethods.GetOppositeDirection(exit.ExitDirection)) != null)
                        {
                            if (LevelGenerator.IsSectionValid(exit, candidate, element, out sectionPosOnMap))
                            {
                                var newSection = BuildSection(candidate, sectionPosOnMap);
                                newSection.ActivateRoomController(LevelGenerator.CameraController);
                                TakeEntrance(newSection, element.Position, exit);
                                newSection.Initialize(LevelGenerator, sectionPosOnMap);
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


        protected void TakeEntrance(Section newSection, Vector2Int elementPosition, AdvancedExit exit)
        {
            foreach(var element in newSection.Elements.ContainedElements)
            {
                if(element.Position == elementPosition)
                {
                    foreach (var entrance in element.ExitSpots)
                    {
                        if (ExitMethods.AreDirectionsOpposite(entrance.ExitDirection, exit.ExitDirection))
                        {
                            SetExits(exit, entrance, newSection);
                            return;
                        }
                    }
                }
            }
        }

        protected void SetExits(AdvancedExit exit, AdvancedExit entrance, Section newSection)
        {
            exit.SetExit(entrance, RoomController);
            entrance.SetExit(exit, newSection.RoomController);
        }

        protected Section BuildSection(Section section, Vector2Int sectionPositionOnMap) => Instantiate(section, LevelGenerator.SetWorldPosition(sectionPositionOnMap), Quaternion.Euler(0,0,0), LevelGenerator.Container);
    }
}