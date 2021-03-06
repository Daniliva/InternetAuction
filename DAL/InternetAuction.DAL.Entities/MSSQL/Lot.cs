using System.Collections.Generic;

namespace InternetAuction.DAL.Entities.MSSQL
{
    /// <summary>
    /// The lot.
    /// </summary>
    public class Lot : BaseEntity
    {
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public virtual User Author { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual LotCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the cost minimum.
        /// </summary>
        /// <value>
        /// The cost minimum.
        /// </value>
        public virtual decimal CostMin { get; set; }

        /// <summary>
        /// Gets or sets the photos.
        /// </summary>
        /// <value>
        /// The photos.
        /// </value>
        public ICollection<ImageId> Photos { get; set; }

        /// <summary>
        /// Gets or sets the photo current.
        /// </summary>
        /// <value>
        /// The photo current.
        /// </value>
        public virtual ImageId PhotoCurrent { get; set; }

        public virtual Autction Autction { get; set; }
        public virtual int AutctionRef { get; set; }
    }
}