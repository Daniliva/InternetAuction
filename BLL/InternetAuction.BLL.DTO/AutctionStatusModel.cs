using System.Collections.Generic;

namespace InternetAuction.BLL.DTO
{
    /// <summary>
    /// The autction status model.
    /// </summary>
    public class AutctionStatusModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name status.
        /// </summary>
        /// <value>
        /// The name status.
        /// </value>
        public string NameStatus { get; set; }

        /// <summary>
        /// Gets or sets the description status.
        /// </summary>
        /// <value>
        /// The description status.
        /// </value>
        public string DescriptionStatus { get; set; }

        /// <summary>
        /// Gets or sets the autctions.
        /// </summary>
        /// <value>
        /// The autctions.
        /// </value>
        public ICollection<AutctionModel> Autctions { get; set; }
    }
}