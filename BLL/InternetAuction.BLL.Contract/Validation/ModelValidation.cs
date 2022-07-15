using InternetAuction.DAL.Entities.MSSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternetAuction.BLL.Contract.Validation
{
    /// <summary>
    /// The model validation.
    /// </summary>
    public static class ModelValidation
    {
        /// <summary>
        /// Objects the null check.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static bool ObjectNullCheck(object o)
        {
            return o == (null);
        }

        /// <summary>
        /// Data the check.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A bool.</returns>
        public static bool DataCheck(DateTime startDate, DateTime endDate)
        {
            return endDate > startDate;
        }

        /// <summary>
        /// Data the by period check.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="betweenDate">The between date.</param>
        /// <returns>A bool.</returns>
        public static bool DataByPeriodCheck(DateTime startDate, DateTime endDate, DateTime betweenDate)
        {
            return betweenDate > startDate && betweenDate < endDate;
        }

        /// <summary>
        /// Auctions the check.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public static bool AuctionCheck(Autction product)
        {
            return !(ObjectNullCheck(product) &&
                ObjectNullCheck(product.Lot) &&
                ObjectNullCheck(product.Status));
        }

        /// <summary>
        /// Users the check.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public static bool UserCheck(User product)
        {
            return !(ObjectNullCheck(product) && ObjectNullCheck(product.RoleUsers) && ObjectNullCheck(product.UserName) && ObjectNullCheck(product.Email))
        }

        /// <summary>
        /// Lots the check.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public static bool LotCheck(Lot product)
        {
            return !(ObjectNullCheck(product) && ObjectNullCheck(product.Author) && ObjectNullCheck(product.Autction))
        }
    }
}