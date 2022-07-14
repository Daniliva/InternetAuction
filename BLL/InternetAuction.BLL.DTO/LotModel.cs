namespace InternetAuction.BLL.DTO
{
    public class LotModel
    {
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
        /// Gets or sets the photo current.
        /// </summary>
        /// <value>
        /// The photo current.

        //public byte[] PhotoCurrent { get; set; }

        public virtual AutctionModel Autction { get; set; }
        public virtual int AutctionRef { get; set; }
    }
}