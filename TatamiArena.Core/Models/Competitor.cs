using System;
using System.Collections.Generic;

namespace TatamiArena.Core.Models
{
    public class Competitor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public Sex Sex { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CompetitionParticipant> Participations { get; set; } = new List<CompetitionParticipant>();

        public bool IsValidAge()
        {
            return Age > 0 && Age < 100;
        }

        public bool IsValidWeight()
        {
            return Weight > 0 && Weight < 300; // Assuming 300kg as a reasonable maximum
        }
    }
} 