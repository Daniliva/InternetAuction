using System.Collections.Generic;

namespace InternetAuction.DAL.Entities.MSSQL
{
    /// <summary>
    /// The lot category.
    /// </summary>
    public class LotCategory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name category.
        /// </summary>
        /// <value>
        /// The name category.
        /// </value>
        public string NameCategory { get; set; }

        /// <summary>
        /// Gets or sets the description category.
        /// </summary>
        /// <value>
        /// The description category.
        /// </value>
        public string DescriptionCategory { get; set; }

        /// <summary>
        /// Gets or sets the lots.
        /// </summary>
        /// <value>
        /// The lots.
        /// </value>
        public ICollection<Lot> Lots { get; set; }
    }
}