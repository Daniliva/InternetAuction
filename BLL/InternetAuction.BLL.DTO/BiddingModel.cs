using System;

namespace InternetAuction.BLL.DTO
{
    public class BiddingModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Gets or sets the autction.
        /// </summary>
        /// <value>
        /// The autction.
        /// </value>
        public AutctionModel Autction { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }
    }
}