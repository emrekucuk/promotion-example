// namespace KoopPos.Common.Infrastructure.Promotion;

// public class CampaignService
// {
//     private readonly KoopPosModuleDbContext _dbContext;
//     public CampaignService(KoopPosModuleDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }

//     public void ApplyPromotions(Sale sale, List<Campaign> campaigns)
//     {
//         if (sale == null || !sale.SaleItems.Any())
//             return;

//         ResetSale(sale);

//         foreach (var campaign in campaigns)
//         {
//             foreach (var promo in campaign.Promotions.OrderByDescending(x => x.Priority))
//             {
//                 if (!CheckConditionGroups(sale, promo))
//                     continue;

//                 ApplyPromotion(sale, promo);
//             }
//         }

//         // Ürün bazlı indirimler
//         var itemDiscounts = sale.SaleItems.Sum(x => x.TotalDiscount);

//         // Toplam indirim = ürün + sepet
//         sale.TotalDiscount = itemDiscounts + sale.CartDiscountTotal;
//     }

//     private void ResetSale(Sale sale)
//     {
//         sale.TotalDiscount = 0;
//         sale.CartDiscountTotal = 0;

//         foreach (var item in sale.SaleItems)
//         {
//             item.TotalDiscount = 0;
//             item.NonEligableQuantity = 0;
//         }
//     }

//     private bool CheckConditionGroups(Sale sale, Domain.Entities.Promotion promo)
//     {
//         if (!promo.ConditionGroups.Any())
//             return true;

//         return promo.ConditionGroups.All(g => EvaluateConditionGroup(sale, g));
//     }

//     private bool EvaluateConditionGroup(Sale sale, PromotionConditionGroup group)
//     {
//         if (group.IsAggregated)
//         {
//             var totalQty = sale.SaleItems
//                 .Where(i => group.Conditions.Any(c => c.ProductId == i.ProductId))
//                 .Sum(i => i.Quantity);

//             return totalQty >= (group.MinQuantity ?? 0);
//         }

//         var results = group.Conditions.Select(c => EvaluateCondition(sale, c));

//         return group.Operator == LogicalOperator.AND
//             ? results.All(x => x)
//             : results.Any(x => x);
//     }

//     private bool EvaluateCondition(Sale sale, PromotionCondition cond)
//     {
//         switch (cond.Type)
//         {
//             case ConditionType.ProductExists:
//                 return sale.SaleItems.Any(x => x.ProductId == cond.ProductId);

//             case ConditionType.ProductQuantity:
//                 return sale.SaleItems
//                     .Where(x => x.ProductId == cond.ProductId)
//                     .Sum(x => x.Quantity) >= (cond.MinQuantity ?? 0);

//             case ConditionType.CartTotal:
//                 return sale.SaleItems.Sum(x => x.TotalPrice) >= (cond.Value ?? 0);
//         }

//         return false;
//     }

//     private void ApplyPromotion(Sale sale, Domain.Entities.Promotion promo)
//     {
//         foreach (var group in promo.BenefitGroups)
//         {
//             ApplyBenefitGroup(sale, group);
//         }
//     }

//     private void ApplyBenefitGroup(Sale sale, PromotionBenefitGroup group)
//     {
//         if (group.Operator == LogicalOperator.AND)
//         {
//             foreach (var benefit in group.Benefits)
//                 ApplyBenefit(sale, benefit);
//             return;
//         }

//         // OR → en yüksek indirimi seç
//         var best = group.Benefits
//             .Select(b => new { Benefit = b, Discount = SimulateBenefit(sale, b) })
//             .OrderByDescending(x => x.Discount)
//             .FirstOrDefault();

//         if (best != null && best.Discount > 0)
//             ApplyBenefit(sale, best.Benefit);
//     }

//     private double SimulateBenefit(Sale sale, PromotionBenefit benefit)
//     {
//         var items = GetItems(sale, benefit);
//         double total = 0;

//         foreach (var item in items)
//         {
//             var qty = item.Quantity - item.NonEligableQuantity;
//             if (qty <= 0) continue;

//             switch (benefit.Type)
//             {
//                 case BenefitType.PercentageDiscount:
//                     total += item.SalePrice * qty * (benefit.Value ?? 0) / 100;
//                     break;

//                 case BenefitType.FixedAmountDiscount:
//                     total += (benefit.Value ?? 0) * qty;
//                     break;

//                 case BenefitType.FixedPrice:
//                     total += Math.Max(0, (item.SalePrice - (benefit.Value ?? item.SalePrice)) * qty);
//                     break;

//                 case BenefitType.FreeProduct:
//                     total += item.SalePrice * (benefit.Quantity ?? 1);
//                     break;

//                 case BenefitType.BuyXPayY:
//                     var set = qty / benefit.BuyQuantity.Value;
//                     var freeQty = set * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);
//                     total += freeQty * item.SalePrice;
//                     break;
//             }
//         }

//         if (!benefit.ProductId.HasValue)
//         {
//             var totalCart = sale.SaleItems.Sum(x => x.TotalPrice);
//             if (benefit.Type == BenefitType.PercentageDiscount)
//                 total += totalCart * (benefit.Value ?? 0) / 100;
//             else if (benefit.Type == BenefitType.FixedAmountDiscount)
//                 total += (benefit.Value ?? 0);
//         }

//         return total;
//     }

//     private void ApplyBenefit(Sale sale, PromotionBenefit benefit)
//     {
//         if (!benefit.ProductId.HasValue)
//         {
//             var total = sale.SaleItems.Sum(x => x.TotalPrice);
//             if (benefit.Type == BenefitType.PercentageDiscount)
//                 sale.CartDiscountTotal += total * (benefit.Value ?? 0) / 100;
//             else if (benefit.Type == BenefitType.FixedAmountDiscount)
//                 sale.CartDiscountTotal += (benefit.Value ?? 0);
//             return;
//         }

//         var items = GetItems(sale, benefit);

//         foreach (var item in items)
//         {
//             var qty = item.Quantity - item.NonEligableQuantity;
//             if (qty <= 0) continue;

//             switch (benefit.Type)
//             {
//                 case BenefitType.PercentageDiscount:
//                     item.TotalDiscount += item.SalePrice * qty * (benefit.Value ?? 0) / 100;
//                     break;

//                 case BenefitType.FixedAmountDiscount:
//                     item.TotalDiscount += (benefit.Value ?? 0) * qty;
//                     break;

//                 case BenefitType.FixedPrice:
//                     item.TotalDiscount += Math.Max(0, (item.SalePrice - (benefit.Value ?? item.SalePrice)) * qty);
//                     break;

//                 case BenefitType.FreeProduct:
//                     var freeQty = benefit.Quantity ?? 1;
//                     item.TotalDiscount += item.SalePrice * freeQty;
//                     item.NonEligableQuantity += freeQty;
//                     break;

//                 case BenefitType.BuyXPayY:
//                     var set = qty / benefit.BuyQuantity.Value;
//                     var free = set * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);
//                     item.TotalDiscount += free * item.SalePrice;
//                     item.NonEligableQuantity += free;
//                     break;
//             }
//         }
//     }

//     private List<SaleItem> GetItems(Sale sale, PromotionBenefit benefit)
//     {
//         if (benefit.ProductId.HasValue)
//             return sale.SaleItems.Where(x => x.ProductId == benefit.ProductId.Value).ToList();

//         return new List<SaleItem>();
//     }

//     public async Task<List<Campaign>> CheckCampaignEligable(Sale sale)
//     {
//         var now = DateTime.Now;

//         var regionId = await _dbContext.AppCooperatives
//             .Where(c => c.Id == sale.CooperativeId && !c.IsDeleted)
//             .Select(c => c.RegionId)
//             .FirstOrDefaultAsync();

//         var customer = await _dbContext.AppTkCustomers
//             .Where(c => c.TcknOrVkn == sale.TcknOrVkn && sale.TcknOrVkn != "11111111111" && !c.IsDeleted)
//             .FirstOrDefaultAsync();

//         return await _dbContext.KoopPosCampaigns
//             .AsNoTracking()
//             .Include(c => c.CampaignProduct)
//             .Where(c => !c.IsDeleted
//                         && c.IsActive
//                         && c.StartDate <= now
//                         && c.EndDate >= now
//                         && (
//                             (!c.CampaignCooperatives.Any()
//                              && !c.CampaignTkRegions.Any()
//                              && !c.CampaignTkCustomers.Any()
//                              && !c.CampaignTkCustomerTypes.Any())
//                             || c.CampaignCooperatives.Any(cc => cc.CooperativeId == sale.CooperativeId)
//                             || c.CampaignTkRegions.Any(cr => cr.TkRegionId == regionId)
//                             || (customer != null && c.CampaignTkCustomers.Any(ct => ct.TkCustomerId == customer.Id))
//                             || (customer != null && c.CampaignTkCustomerTypes.Any(ct => ct.CustomerType == customer.CustomerType))
//                         ))
//             .ToListAsync();
//     }
// }