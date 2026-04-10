// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace CampaignEngine
// {
//     public class PromotionEngine
//     {
//         public void ApplyPromotions(Sale sale, List<Campaign> campaigns)
//         {
//             if (sale == null || !sale.SaleItems.Any())
//                 return;

//             ResetSale(sale);

//             foreach (var campaign in campaigns.Where(IsCampaignValid))
//             {
//                 foreach (var promo in campaign.Promotions.OrderByDescending(x => x.Priority))
//                 {
//                     if (!CheckConditionGroups(sale, promo))
//                         continue;

//                     ApplyPromotion(sale, promo);
//                 }
//             }

//             // TOPLAM İNDİRİM: sadece item.DiscountTotal toplamı
//             sale.DiscountTotal = sale.SaleItems.Sum(x => x.DiscountTotal);
//         }

//         private void ResetSale(Sale sale)
//         {
//             foreach (var item in sale.SaleItems)
//             {
//                 item.DiscountTotal = 0;
//                 item.NonEligableQuantity = 0;
//             }
//             sale.DiscountTotal = 0;
//         }

//         private bool IsCampaignValid(Campaign campaign)
//         {
//             var now = DateTime.Now;
//             return campaign.IsActive &&
//                    campaign.StartDate <= now &&
//                    campaign.EndDate >= now;
//         }

//         // =========================
//         // CONDITIONS
//         // =========================
//         private bool CheckConditionGroups(Sale sale, Promotion promo)
//         {
//             if (!promo.ConditionGroups.Any())
//                 return true;

//             return promo.ConditionGroups.All(g => EvaluateConditionGroup(sale, g));
//         }

//         private bool EvaluateConditionGroup(Sale sale, PromotionConditionGroup group)
//         {
//             if (group.IsAggregated)
//             {
//                 var totalQty = sale.SaleItems
//                     .Where(i => group.Conditions.Any(c => c.ProductId == i.ProductId))
//                     .Sum(i => i.Quantity);

//                 return totalQty >= (group.MinQuantity ?? 0);
//             }

//             var results = group.Conditions.Select(c => EvaluateCondition(sale, c));

//             return group.Operator == LogicalOperator.AND
//                 ? results.All(x => x)
//                 : results.Any(x => x);
//         }

//         private bool EvaluateCondition(Sale sale, PromotionCondition cond)
//         {
//             switch (cond.Type)
//             {
//                 case ConditionType.ProductExists:
//                     return sale.SaleItems.Any(x => x.ProductId == cond.ProductId);

//                 case ConditionType.ProductQuantity:
//                     return sale.SaleItems
//                         .Where(x => x.ProductId == cond.ProductId)
//                         .Sum(x => x.Quantity) >= (cond.MinQuantity ?? 0);

//                 case ConditionType.CartTotal:
//                     return sale.SaleItems.Sum(x => x.PriceTotal) >= (cond.Value ?? 0);
//             }

//             return false;
//         }

//         // =========================
//         // APPLY PROMOTION
//         // =========================
//         private void ApplyPromotion(Sale sale, Promotion promo)
//         {
//             foreach (var group in promo.BenefitGroups)
//             {
//                 ApplyBenefitGroup(sale, group);
//             }
//         }

//         // =========================
//         // BENEFIT GROUP
//         // =========================
//         private void ApplyBenefitGroup(Sale sale, PromotionBenefitGroup group)
//         {
//             if (group.Operator == LogicalOperator.AND)
//             {
//                 foreach (var benefit in group.Benefits)
//                     ApplyBenefit(sale, benefit);

//                 return;
//             }

//             // OR → en yüksek indirimli benefit seç
//             var best = group.Benefits
//                 .Select(b => new
//                 {
//                     Benefit = b,
//                     Discount = SimulateBenefit(sale, b)
//                 })
//                 .OrderByDescending(x => x.Discount)
//                 .FirstOrDefault();

//             if (best != null && best.Discount > 0)
//             {
//                 ApplyBenefit(sale, best.Benefit);
//             }
//         }

//         // =========================
//         // SIMULATE
//         // =========================
//         private double SimulateBenefit(Sale sale, PromotionBenefit benefit)
//         {
//             var items = GetItems(sale, benefit);

//             double total = 0;

//             foreach (var item in items)
//             {
//                 var qty = item.Quantity;

//                 switch (benefit.Type)
//                 {
//                     case BenefitType.PercentageDiscount:
//                         total += item.SalePrice * qty * (benefit.Value ?? 0) / 100;
//                         break;

//                     case BenefitType.FixedAmountDiscount:
//                         total += (benefit.Value ?? 0) * qty;
//                         break;

//                     case BenefitType.FixedPrice:
//                         total += Math.Max(0,
//                             (item.SalePrice - (benefit.Value ?? item.SalePrice)) * qty);
//                         break;

//                     case BenefitType.FreeProduct:
//                         total += item.SalePrice * (benefit.Quantity ?? 1);
//                         break;

//                     case BenefitType.BuyXPayY:
//                         var set = qty / benefit.BuyQuantity.Value;
//                         var freeQty = set * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);
//                         total += freeQty * item.SalePrice;
//                         break;
//                 }
//             }

//             // Cart-level % indirim için
//             if (!benefit.ProductId.HasValue && benefit.Type == BenefitType.PercentageDiscount)
//             {
//                 var totalPrice = sale.SaleItems.Sum(x => x.PriceTotal);
//                 var applicable = benefit.MaxApplicableAmount.HasValue
//                                  ? Math.Min(totalPrice, benefit.MaxApplicableAmount.Value)
//                                  : totalPrice;
//                 total = applicable * (benefit.Value ?? 0) / 100;
//             }

//             return total;
//         }

//         // =========================
//         // APPLY BENEFIT
//         // =========================
//         private void ApplyBenefit(Sale sale, PromotionBenefit benefit)
//         {
//             // Cart-level % indirim
//             if (!benefit.ProductId.HasValue)
//             {
//                 if (benefit.Type == BenefitType.PercentageDiscount)
//                 {
//                     var total = sale.SaleItems.Sum(x => x.PriceTotal);
//                     var applicable = benefit.MaxApplicableAmount.HasValue
//                                      ? Math.Min(total, benefit.MaxApplicableAmount.Value)
//                                      : total;

//                     foreach (var item in sale.SaleItems)
//                     {
//                         var portion = item.PriceTotal / total;
//                         item.DiscountTotal += Math.Round(applicable * (benefit.Value ?? 0) / 100 * portion, 2);
//                     }
//                 }
//                 return;
//             }

//             var items = GetItems(sale, benefit);

//             foreach (var item in items)
//             {
//                 if (item.NonEligableQuantity >= item.Quantity)
//                     continue;

//                 switch (benefit.Type)
//                 {
//                     case BenefitType.PercentageDiscount:
//                         item.DiscountTotal += item.SalePrice * item.Quantity * (benefit.Value ?? 0) / 100;
//                         break;

//                     case BenefitType.FixedAmountDiscount:
//                         item.DiscountTotal += (benefit.Value ?? 0) * item.Quantity;
//                         break;

//                     case BenefitType.FixedPrice:
//                         item.DiscountTotal += (item.SalePrice - (benefit.Value ?? item.SalePrice)) * item.Quantity;
//                         break;

//                     case BenefitType.FreeProduct:
//                         var freeQty = benefit.Quantity ?? 1;
//                         item.DiscountTotal += item.SalePrice * freeQty;
//                         item.NonEligableQuantity += freeQty;
//                         break;

//                     case BenefitType.BuyXPayY:
//                         var set = item.Quantity / benefit.BuyQuantity.Value;
//                         var free = set * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);
//                         item.DiscountTotal += free * item.SalePrice;
//                         item.NonEligableQuantity += free;
//                         break;
//                 }
//             }
//         }

//         // =========================
//         // GET ITEMS
//         // =========================
//         private List<SaleItem> GetItems(Sale sale, PromotionBenefit benefit)
//         {
//             if (benefit.ProductId.HasValue)
//             {
//                 return sale.SaleItems
//                     .Where(x => x.ProductId == benefit.ProductId.Value)
//                     .ToList();
//             }

//             return new List<SaleItem>();
//         }
//     }
// }