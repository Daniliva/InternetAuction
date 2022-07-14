using System;
using System.Collections.Generic;

namespace InternetAuction.WEB.Domain
{
    public class LotInfo
    {
        public int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual decimal CostMin { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<string> SelecteStatus { get; set; } = new List<string>();

        public List<string> SelectedLotCategory { get; set; } = new List<string>();
        public virtual string Category { get; set; }
    }
}