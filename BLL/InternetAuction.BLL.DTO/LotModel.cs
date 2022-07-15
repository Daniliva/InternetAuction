using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
    /// <summary>
    /// The lot model.
    /// </summary>
    public class LotModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public virtual UserModel Author { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual LotCategoryModel Category { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = "Lot's name")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Required(ErrorMessage = "Lot's Description")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the cost minimum.
        /// </summary>
        /// <value>
        /// The cost minimum.
        /// </value>
        [Range(0, float.MaxValue, ErrorMessage = "CostMin must be more than 0")]
        [DisplayFormat(DataFormatString = "{0:#####.##}")]
        public virtual decimal CostMin { get; set; }

        /// <summary>
        /// Gets or sets the photo current.
        /// </summary>
        /// <value>
        /// The photo current.

        public virtual AutctionModel Autction { get; set; }

        /// <summary>
        /// Gets or sets the autction reference.
        /// </summary>
        /// <value>
        /// The autction reference.
        /// </value>
        public virtual int AutctionRef { get; set; }
    }
}