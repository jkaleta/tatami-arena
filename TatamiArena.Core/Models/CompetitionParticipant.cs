using System;

namespace TatamiArena.Core.Models
{
    public class CompetitionParticipant
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public int CategoryId { get; set; }
        public int CompetitorId { get; set; }
        public DateTime RegisteredAt { get; set; }

        public Competition Competition { get; set; }
        public Category Category { get; set; }
        public Competitor Competitor { get; set; }
    }
} 