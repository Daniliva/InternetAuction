using System;

namespace InternetAuction.DAL.Entities.MSSQL
{
    /// <summary>
    /// The bidding.
    /// </summary>
    public class Bidding : BaseEntity
    {
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the autction.
        /// </summary>
        /// <value>
        /// The autction.
        /// </value>
        public Autction Autction { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }
    }
}