using System.Collections.Generic;

namespace InternetAuction.DAL.Entities.MSSQL
{
    /// <summary>
    /// The autction status.
    /// </summary>
    public class AutctionStatus : BaseEntity
    {
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

        public ICollection<Autction> Autctions { get; set; }
    }
}