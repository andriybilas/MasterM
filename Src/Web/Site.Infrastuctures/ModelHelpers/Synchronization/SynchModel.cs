using System;

namespace Site.Infrastuctures.ModelHelpers.Synchronization
{
    public class SynchModel
    {
        public DateTime? StartSynchDate { get; set; }
        public DateTime? EndSynchDate { get; set; }
        public bool FromLastSynch { get; set; }
    }
}
