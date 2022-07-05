using System.Collections.Generic;

namespace InternetAuction.BLL.DTO
{
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

        public ICollection<AutctionModel> Autctions { get; set; }
    }
}