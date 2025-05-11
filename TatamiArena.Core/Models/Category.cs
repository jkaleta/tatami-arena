using System;
using System.Collections.Generic;

namespace TatamiArena.Core.Models
{
    public enum CategoryType
    {
        Kata,
        Kumite
    }

    public enum Sex
    {
        Male,
        Female
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public CategoryType Type { get; set; }
        public decimal? MinWeight { get; set; }
        public decimal? MaxWeight { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public List<CompetitionParticipant> Participants { get; set; } = new List<CompetitionParticipant>();

        public bool IsValidWeightRange()
        {
            if (!MinWeight.HasValue && !MaxWeight.HasValue) return true;
            if (!MinWeight.HasValue || !MaxWeight.HasValue) return true;
            return MinWeight.Value <= MaxWeight.Value;
        }

        public bool IsValidAgeRange()
        {
            if (!MinAge.HasValue && !MaxAge.HasValue) return true;
            if (!MinAge.HasValue || !MaxAge.HasValue) return true;
            return MinAge.Value <= MaxAge.Value;
        }
    }
} 