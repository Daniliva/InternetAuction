using System.Collections.Generic;

namespace InternetAuction.BLL.DTO
{
    /// <summary>
    /// The lot category model.
    /// </summary>
    public class LotCategoryModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
        public ICollection<LotModel> Lots { get; set; }
    }
}