using System;
using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
    /// <summary>
    /// The bidding model.
    /// </summary>
    public class BiddingModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        [Range(0, float.MaxValue, ErrorMessage = "CostMin must be more than 0")]
        [DisplayFormat(DataFormatString = "{0:#####.##}")]
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