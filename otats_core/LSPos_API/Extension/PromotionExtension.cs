using System;
using BaseBusiness;
using BusinessLayer.Model.Sell;

namespace BusinessLayer.Extension
{
	public static class PromotionExtension
	{
		public static bool HasPromotion(this PromotionModel pro, decimal ratePrice, int quantity)
		{
			bool value = false;
			if (pro.SessionType == "TYPE_SESSION_SERVICE" && pro.StartTime.HasValue && pro.StartTime.Value.TimeOfDay < DateTime.Now.TimeOfDay)
			{
				return value;
			}
			if (pro.SessionType == "TYPE_SESSION_SERVICE" && pro.EndTime.HasValue && pro.EndTime.Value.TimeOfDay < DateTime.Now.TimeOfDay)
			{
				return value;
			}
			switch (pro.PromoType)
			{
			case "DISCOUNT_TOTAL_COST":
				value = true;
				break;
			case "DISCOUNT_PER_TICKET":
				return true;
			case "BOOK_X_NUMBER_TICKET":
				if (!(quantity >= pro.MinOrder))
				{
					break;
				}
				return true;
			case "DISCOUNT_EARLY_BIRD":
				return true;
			case "DISCOUNT_LAST_MINUTE":
				return true;
			default:
				return false;
			}
			return false;
		}

		public static RatePromotionModel GetDiscount(this PromotionModel pro, decimal ratePrice, int quantity)
		{
			bool discount = pro.DiscountOrSurcharge ?? true;
			bool percent = pro.IsPercent;
			decimal promotionPrice = default(decimal);
			int unitQuantity = 0;
			switch (pro.PromoType)
			{
			case "DISCOUNT_TOTAL_COST":
			{
				decimal totalPrice = ratePrice * (decimal)quantity;
				promotionPrice = GetDiscountAmount(totalPrice, pro.DiscountAmount, percent, discount);
				unitQuantity = 1;
				break;
			}
			case "DISCOUNT_PER_TICKET":
				promotionPrice = GetDiscountAmount(ratePrice, pro.DiscountAmount, percent, discount);
				unitQuantity = quantity;
				break;
			case "BOOK_X_NUMBER_TICKET":
				if (quantity >= pro.MinOrder)
				{
					quantity -= pro.FreeForMinOrder;
					promotionPrice = ratePrice;
					unitQuantity = quantity;
				}
				break;
			case "DISCOUNT_EARLY_BIRD":
				promotionPrice = GetDiscountAmount(ratePrice, pro.DiscountAmount, percent, discount);
				unitQuantity = quantity;
				break;
			case "DISCOUNT_LAST_MINUTE":
				promotionPrice = GetDiscountAmount(ratePrice, pro.DiscountAmount, percent, discount);
				unitQuantity = quantity;
				break;
			default:
				promotionPrice = default(decimal);
				unitQuantity = 0;
				break;
			}
			return new RatePromotionModel
			{
				PromotionPrice = promotionPrice,
				Quantity = unitQuantity
			};
		}

		private static decimal GetDiscountAmount(decimal price, decimal discount, bool percent, bool isDiscount)
		{
			decimal prefix = ((!isDiscount) ? 1 : (-1));
			return GetDiscountAmount(price, discount, percent) * prefix;
		}

		private static decimal GetDiscountAmount(decimal price, decimal discount, bool percent)
		{
			return percent ? (discount * price / 100m) : discount;
		}
	}
}
