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
        /// <summary>
        /// LevelGenerator seed
        /// </summary>
        public int Seed;

        /// <summary>
        /// Container for all sections in hierarchy
        /// </summary>
        public Transform SectionContainer;

        /// <summary>
        /// Maximum level size measured in sections
        /// </summary>
        public int MaxLevelSize;

        /// <summary>
        /// Maximum allowed distance from the original section
        /// </summary>
        public int MaxAllowedOrder;

        /// <summary>
        /// Spawnable section prefabs
        /// </summary>
        public Section[] Sections;

        /// <summary>
        /// Spawnable dead ends
        /// </summary>
        public DeadEnd[] DeadEnds;

        /// <summary>
        /// Tags that will be taken into consideration when building the first section
        /// </summary>
        public string[] InitialSectionTags;
        
        /// <summary>
        /// Special section rules, limits and forces the amount of a specific tag
        /// </summary>
        public TagRule[] SpecialRules;

        protected List<Section> registeredSections = new List<Section>();
        
        public int LevelSize { get; private set; }
        public Transform Container => SectionContainer != null ? SectionContainer : transform;

        protected IEnumerable<Collider2D> RegisteredColliders => registeredSections.SelectMany(s => s.Bounds.Colliders).Union(DeadEndColliders);
        protected List<Collider2D> DeadEndColliders = new List<Collider2D>();
        protected bool HalfLevelBuilt => registeredSections.Count > LevelSize;

        protected void Start()
        {
            if (Seed != 0)
                RandomService.SetSeed(Seed);
            else
                Seed = RandomService.Seed;
            
            CheckRuleIntegrity();
            LevelSize = MaxLevelSize;
            CreateInitialSection();
            DeactivateBounds();
        }

        protected void CheckRuleIntegrity()
        {
            foreach (var ruleTag in SpecialRules.Select(r => r.Tag))
            {
                if (SpecialRules.Count(r => r.Tag.Equals(ruleTag)) > 1)
                    throw new InvalidRuleDeclarationException();
            }
        }

        protected void CreateInitialSection() => Instantiate(PickSectionWithTag(InitialSectionTags, Sections), transform).Initialize(this, 0);

        public void AddSectionTemplate() => Instantiate(Resources.Load("SectionTemplate"), Vector3.zero, Quaternion.identity);
        public void AddDeadEndTemplate() => Instantiate(Resources.Load("DeadEndTemplate"), Vector3.zero, Quaternion.identity);

        public bool IsSectionValid(Bounds newSection, Bounds sectionToIgnore)
        {
            List<Collider2D> newSectionColliders = newSection.Colliders.ToList<Collider2D>();
            foreach(var col in newSectionColliders)
            {
                foreach(var k in RegisteredColliders.Except(sectionToIgnore.Colliders))
                {
                    if (k.bounds.Intersects(col.bounds))
                        return false;
                }
            }
            return true;
        }

        public void RegisterNewSection(Section newSection)
        {
            registeredSections.Add(newSection);

            if(SpecialRules.Any(r => newSection.Tags.Contains(r.Tag)))
                SpecialRules.First(r => newSection.Tags.Contains(r.Tag)).PlaceRuleSection();

            LevelSize--;
        }

        public void RegistrerNewDeadEnd(IEnumerable<Collider2D> colliders) => DeadEndColliders.AddRange(colliders);

        public Dictionary<Section,List<Transform>> PickSections(string[] tags, ExitDirection direction) 
            => GetSectionsInDirection(GetSectionsWithTag(tags, Sections), direction);
        
        private IEnumerable<Section> GetSectionsWithTag(string[] tags, IEnumerable<Section> sections)
        {
            if (RulesContainTargetTags(tags) && HalfLevelBuilt)
            {
                foreach (var rule in SpecialRules.Where(r => r.NotSatisfied))
                {
                    if (tags.Contains(rule.Tag))
                        return Sections.Where(x => x.Tags.Contains(rule.Tag));
                }
            }
            var pickedTag = PickFromExcludedTags(tags);
            return sections.Where(x => x.Tags.Contains(pickedTag));
        }

        private Section PickSectionWithTag(string[] tags, IEnumerable<Section> sections) => GetSectionsWithTag(tags, sections).PickOne();

        public Dictionary<Section,List<Transform>> GetSectionsInDirection(IEnumerable<Section> sections, ExitDirection direction)
        {
            Dictionary<Section,List<Transform>> dic = new();
            foreach(var section in sections)
            {
                List<Transform> exits = new();
                foreach(var exit in section.Exits.ExitSpots)
                {
                    if(ExitMethods.AreDirectionsOpposite(exit.GetComponent<AdvancedExit>().exitDirection, direction))
                        exits.Add(exit);
                }
                if (exits.Count != 0)
                    dic.Add(section, exits);
            }
            return dic;
        }

        public Section PickSectionInDirectionWithTag(string[] tags, ExitDirection direction)
        {
            List<Section> sections = new();

            foreach(var section in Sections)
            {
                foreach(var exit in section.Exits.ExitSpots)
                {
                    if(ExitMethods.AreDirectionsOpposite(exit.GetComponent<AdvancedExit>().exitDirection, direction) && !sections.Contains(section))
                        sections.Add(section);
                }
            }
            return PickSectionWithTag(tags, sections);
        }

        
        protected string PickFromExcludedTags(string[] tags)
        {
            var tagsToExclude = SpecialRules.Where(r => r.Completed).Select(rs => rs.Tag);
            return tags.Except(tagsToExclude).PickOne();
        }

        protected bool RulesContainTargetTags(string[] tags) => tags.Intersect(SpecialRules.Where(r => r.NotSatisfied).Select(r => r.Tag)).Any();

        protected void DeactivateBounds()
        {
            foreach (var c in RegisteredColliders)
                c.enabled = false;
        }
    }
}