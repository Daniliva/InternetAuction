using System;
using System.Collections.Generic;

namespace InternetAuction.BLL.DTO
{
    /// <summary>
    /// The autction.
    /// </summary>
    public class AutctionModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
        public AutctionStatusModel Status { get; set; }

        /// <summary>
        /// Gets or sets the biddings.
        /// </summary>
        /// <value>
        /// The biddings.
        /// </value>
        public ICollection<BiddingModel> Biddings { get; set; }

        /// <summary>
        /// Gets or sets the lots.
        /// </summary>
        /// <value>
        /// The lots.
        /// </value>
        public LotModel Lot { get; set; }
    }
}