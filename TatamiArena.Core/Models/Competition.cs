using System;
using System.Collections.Generic;

namespace TatamiArena.Core.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CompetitionParticipant> Participants { get; set; } = new List<CompetitionParticipant>();
    }
} 