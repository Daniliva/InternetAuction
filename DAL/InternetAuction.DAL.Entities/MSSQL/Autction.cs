using System;
using System.Collections.Generic;

namespace InternetAuction.DAL.Entities.MSSQL

{
    /// <summary>
    /// The autction.
    /// </summary>
    public class Autction : BaseEntity
    {
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public AutctionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the biddings.
        /// </summary>
        /// <value>
        /// The biddings.
        /// </value>
        public ICollection<Bidding> Biddings { get; set; }

        /// <summary>
        /// Gets or sets the lots.
        /// </summary>
        /// <value>
        /// The lots.
        /// </value>
        public Lot Lot { get; set; }
    }
}