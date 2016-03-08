using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentiaTools.Models
{
    public interface IDiscountHelper
    {
        //将一个折扣运用于一个十进制的值
        decimal ApplyDiscount(decimal totalParam);
    }

    public class DefaultDiscountHelper : IDiscountHelper
    {
        public decimal discountSize;

        public DefaultDiscountHelper(decimal discountParam)
        {
            discountSize = discountParam;
        }
        public decimal ApplyDiscount(decimal totalParam)
        {
            return (totalParam - (discountSize / 100m * totalParam));
        }
    }
}