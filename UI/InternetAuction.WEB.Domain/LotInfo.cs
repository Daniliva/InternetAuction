using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternetAuction.WEB.Domain
{

    /// <summary>
    /// The lot info.
    /// </summary>
    public class LotInfo
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
        [Required(ErrorMessage = "Minimal cost lot ")]
        public virtual decimal CostMin { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the selecte status.
        /// </summary>
        /// <value>
        /// The selecte status.
        /// </value>
        [Required(ErrorMessage = "Selecte lot's status ")]
        public List<string> SelecteStatus { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the selected lot category.
        /// </summary>
        /// <value>
        /// The selected lot category.
        /// </value>
        [Required(ErrorMessage = "Selecte lot's category")]
        public List<string> SelectedLotCategory { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual string Category { get; set; }
    }
}