using System.Collections.Generic;
using System.Linq;
using LevelGenerator.Scripts.Exceptions;
using LevelGenerator.Scripts.Helpers;
using LevelGenerator.Scripts.Structure;
using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private int _seed;
        [SerializeField] private Vector2Int _levelSize;
        [SerializeField] private Vector2Int _roomSize;
        [SerializeField] private Vector2Int _startRoomPosition;
        [SerializeField] private Section[] _sections;
        [SerializeField] private InitialSection _initialSection;
        [SerializeField] private TagRule[] _specialRules;
        public CameraController CameraController;

        private List<Section> _registeredSections = new List<Section>();
        private InitialSection _registeredInitialSection;
        private Element[,] _map;
        private RoomController _startRoom;

        public Transform Container => transform;

        private void Start()
        {
            if (_seed != 0)
                RandomService.SetSeed(_seed);
            else
                _seed = RandomService.Seed;
            
            CheckRuleIntegrity();
            _map = new Element[_levelSize.x, _levelSize.y];
            CreateInitialSection();
            _startRoom.SetRoomActive();
        }

        private void CheckRuleIntegrity()
        {
            foreach (var ruleTag in _specialRules.Select(r => r.Tag))
            {
                if (_specialRules.Count(r => r.Tag.Equals(ruleTag)) > 1)
                    throw new InvalidRuleDeclarationException();
            }
        }

        private void CreateInitialSection()
        {
            Section room = Instantiate(_initialSection, (Vector2)(_startRoomPosition * _roomSize), Quaternion.Euler(0,0,0), transform);
            _startRoom = room.RoomController;
            room.ActivateRoomController(CameraController);
            room.Initialize(this, _startRoomPosition);
        }

        public bool IsSectionValid(AdvancedExit exit, Section newSection, Element initialElement, out Vector2Int newSectionPos)
        {
            var startElementPosOnMap = exit.GetComponentInParent<Element>().PositionOnMap;
            var initialElementPosOnMap = startElementPosOnMap + ExitMethods.ConvertDirectioToVector2(exit.ExitDirection);
            newSectionPos = Vector2Int.zero;
            foreach (var element in newSection.Elements.ContainedElements)
            {
                var elementPosOnMap = new Vector2Int(element.Position.x - initialElement.Position.x + initialElementPosOnMap.x, element.Position.y - initialElement.Position.y + initialElementPosOnMap.y);
                if (element.Position == Vector2Int.zero) newSectionPos = elementPosOnMap;
                if (element == initialElement) continue;
                if (elementPosOnMap.x < 0 || elementPosOnMap.x > (_levelSize.x - 1) || elementPosOnMap.y < 0 || elementPosOnMap.y > (_levelSize.y - 1)
                    || _map[elementPosOnMap.x, elementPosOnMap.y] != null) return false;
            }
            return true;
        }

        public void RegisterInitialSection(InitialSection initialSection)
        {
            _registeredInitialSection = initialSection;
            initialSection.Elements.ContainedElements.First().PositionOnMap = _startRoomPosition;
        }

        public void RegisterNewSection(Section newSection, Vector2Int sectionPositionOnMap)
        {
            foreach (var element in newSection.Elements.ContainedElements)
            {
                element.PositionOnMap = sectionPositionOnMap + element.Position;
                _map[element.PositionOnMap.x, element.PositionOnMap.y] = element;
            }
            _registeredSections.Add(newSection);

            if(_specialRules.Any(r => newSection.Tags.Contains(r.Tag)))
                _specialRules.First(r => newSection.Tags.Contains(r.Tag)).PlaceRuleSection();
        }

        public Section PickSectionWithTag(string[] tags, IEnumerable<Section> sectionsToIgnore)
        {
            if (RulesContainTargetTags(tags))
            {
                foreach (var rule in _specialRules.Where(r => r.NotSatisfied))
                {
                    if (tags.Contains(rule.Tag))
                        return _sections.Where(x => x.Tags.Contains(rule.Tag)).PickOne();
                }
            }
            var pickedTag = PickFromExcludedTags(tags);
            return _sections.Where(x => x.Tags.Contains(pickedTag)).Except(sectionsToIgnore).PickOne();
        }
        
        private string PickFromExcludedTags(string[] tags)
        {
            var tagsToExclude = _specialRules.Where(r => r.Completed).Select(rs => rs.Tag);
            return tags.Except(tagsToExclude).PickOne();
        }

        private bool RulesContainTargetTags(string[] tags) => tags.Intersect(_specialRules.Where(r => r.NotSatisfied).Select(r => r.Tag)).Any();

        public bool IsExitExtreme(AdvancedExit e)
        {
            Element element = e.GetComponentInParent<Element>();
            if ((element.PositionOnMap.x == 0 && e.ExitDirection == ExitDirection.Left) || (element.PositionOnMap.x == (_levelSize.x - 1) && e.ExitDirection == ExitDirection.Right)
                || (element.PositionOnMap.y == 0 && e.ExitDirection == ExitDirection.Down) || (element.PositionOnMap.y == (_levelSize.y - 1) && e.ExitDirection == ExitDirection.Up))
            {
                e.CurrentState = AdvancedExit.State.close;
                return true;
            }
            return false;
        }

        public Element GetNeighborElement(AdvancedExit exit)
        {
            var elementPosition = exit.GetComponentInParent<Element>().PositionOnMap;
            var exitDirection = ExitMethods.ConvertDirectioToVector2(exit.ExitDirection);
            return _map[elementPosition.x + exitDirection.x, elementPosition.y + exitDirection.y];
        }

        public Vector2 SetWorldPosition(Vector2Int sectionPositionOnMap) => new Vector2(sectionPositionOnMap.x * _roomSize.x, sectionPositionOnMap.y * _roomSize.y);
    }
}