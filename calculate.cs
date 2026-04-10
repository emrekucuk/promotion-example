namespace CampaignEngine;

public class PromotionEngine
{

    public void ApplyPromotions(Sale sale, List<Campaign> campaigns)
    {
        if (sale == null || !sale.SaleItems.Any())
            return;

        ResetSale(sale);


        // foreach (var campaign in campaigns.Where(IsCampaignValid)) // buraya gelen zaten dogrulanmis kampanya
        foreach (var campaign in campaigns)
        {

            foreach (var promo in campaign.Promotions.OrderByDescending(x => x.Priority))
            {
                // if (nonCombinableApplied) break;

                if (!CheckConditionGroups(sale, promo))
                    continue;

                ApplyPromotion(sale, promo);
            }
        }

        // Ürün bazlı indirimler
        var itemDiscounts = sale.SaleItems.Sum(x => x.DiscountTotal);


        // Toplam indirim = ürün + sepet
        sale.DiscountTotal = itemDiscounts + sale.CartTotalDiscount;
    }

    private void ResetSale(Sale sale)
    {
        sale.DiscountTotal = 0;
        sale.CartTotalDiscount = 0; // <--- sepette sıfırla

        foreach (var item in sale.SaleItems)
        {
            item.DiscountTotal = 0;
            item.NonEligableQuantity = 0;
        }
    }

    private bool IsCampaignValid(Campaign campaign)
    {
        var now = DateTime.Now;
        return campaign.IsActive &&
               campaign.StartDate <= now &&
               campaign.EndDate >= now;
    }

    // =========================
    // CONDITIONS
    // =========================
    private bool CheckConditionGroups(Sale sale, Promotion promo)
    {
        if (!promo.ConditionGroups.Any())
            return true;

        var a = promo.ConditionGroups.All(g => EvaluateConditionGroup(sale, g));
        return a;
    }

    private bool EvaluateConditionGroup(Sale sale, PromotionConditionGroup group)
    {
        if (group.IsAggregated)
        {
            var totalQty = sale.SaleItems
                .Where(i => group.Conditions.Any(c => c.ProductId == i.ProductId))
                .Sum(i => i.Quantity);

            if (totalQty < (group.MinQuantity ?? 0))
                return false;

            if (group.MaxQuantity.HasValue && totalQty > group.MaxQuantity.Value)
                return false;

            return true;
        }

        var results = group.Conditions.Select(c => EvaluateCondition(sale, c));

        return group.Operator == LogicalOperator.AND
            ? results.All(x => x)
            : results.Any(x => x);
    }

    private bool EvaluateCondition(Sale sale, PromotionCondition cond)
    {
        switch (cond.Type)
        {
            case ConditionType.ProductExists:
                return sale.SaleItems.Any(x => x.ProductId == cond.ProductId);

            case ConditionType.ProductQuantity:
                return sale.SaleItems
                    .Where(x => x.ProductId == cond.ProductId)
                    .Sum(x => x.Quantity) >= (cond.MinQuantity ?? 0);

            case ConditionType.CartTotal:
                System.Console.WriteLine("");
                //TODO: (emre.kucuk) burada total price yerine sale'deki total'e de bakilabilir
                var cartTotal = sale.TotalPrice;
                return cond.Operator switch
                {
                    OperatorType.GreaterThan => cartTotal > (cond.Value ?? 0),
                    OperatorType.GreaterOrEqual => cartTotal >= (cond.Value ?? 0),
                    OperatorType.LessThan => cartTotal < (cond.Value ?? 0),
                    OperatorType.LessOrEqual => cartTotal <= (cond.Value ?? 0),
                    OperatorType.Equal => cartTotal == (cond.Value ?? 0),
                    _ => cartTotal >= (cond.Value ?? 0)
                };
        }

        return false;
    }

    // =========================
    // APPLY PROMOTION
    // =========================
    private void ApplyPromotion(Sale sale, Promotion promo)
    {
        foreach (var group in promo.BenefitGroups)
        {
            ApplyBenefitGroup(sale, group, promo.MaxUsagePerCart);
        }
    }

    private void ApplyBenefitGroup(Sale sale, PromotionBenefitGroup group, object maxUsagePerCart)
    {
        throw new NotImplementedException();
    }

    // =========================
    // BENEFIT GROUP
    // =========================
    private void ApplyBenefitGroup(Sale sale, PromotionBenefitGroup group, int? maxUsagePerCart)
    {
        if (group.Operator == LogicalOperator.AND)
        {
            foreach (var benefit in group.Benefits)
                ApplyBenefit(sale, benefit, maxUsagePerCart);

            return;
        }

        // OR → en iyi seç
        var best = group.Benefits
            .Select(b => new
            {
                Benefit = b,
                Discount = SimulateBenefit(sale, b, maxUsagePerCart)
            })
            // .OrderByDescending(x => x.Discount) // en yuksek indirimi sec
            .OrderBy(x => x.Discount) // en dusuk indirimi sec
            .FirstOrDefault();

        if (best != null && best.Discount > 0)
        {
            ApplyBenefit(sale, best.Benefit, maxUsagePerCart);
        }
    }

    // =========================
    // SIMULATE
    // =========================
    private double SimulateBenefit(Sale sale, PromotionBenefit benefit, int? maxUsagePerCart)
    {
        var items = GetItems(sale, benefit);

        double total = 0;

        foreach (var item in items)
        {
            var qty = item.Quantity;

            switch (benefit.Type)
            {
                case BenefitType.PercentageDiscount:
                    total += item.SalePrice * qty * (benefit.Value ?? 0) / 100;
                    break;

                case BenefitType.FixedAmountDiscount:
                    total += (benefit.Value ?? 0) * qty;
                    break;

                case BenefitType.FixedPrice:
                    total += Math.Max(0,
                        (item.SalePrice - (benefit.Value ?? item.SalePrice)) * qty);
                    break;

                case BenefitType.FreeProduct:
                    var freeQty = Math.Min(benefit.Quantity ?? 1, maxUsagePerCart ?? int.MaxValue);
                    total += item.SalePrice * freeQty;
                    break;

                case BenefitType.BuyXPayY:
                    var maxSets = maxUsagePerCart ?? int.MaxValue;
                    var set = Math.Min((int)(qty / benefit.BuyQuantity.Value), maxSets);
                    var freeCount = set * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);
                    total += freeCount * item.SalePrice;
                    break;
            }
        }

        // Sepet bazlı indirimleri de simule et
        if (!benefit.ProductId.HasValue)
        {
            var totalCart = sale.SaleItems.Sum(x => x.PriceTotal);
            if (benefit.Type == BenefitType.PercentageDiscount)
            {
                var discount = totalCart * (benefit.Value ?? 0) / 100;
                if (benefit.MaxDiscount.HasValue)
                    discount = Math.Min(discount, benefit.MaxDiscount.Value);
                total += discount;
            }
            else if (benefit.Type == BenefitType.FixedAmountDiscount)
            {
                var discount = benefit.Value ?? 0;
                if (benefit.MaxDiscount.HasValue)
                    discount = Math.Min(discount, benefit.MaxDiscount.Value);
                total += discount;
            }
        }

        return total;
    }

    // =========================
    // APPLY BENEFIT
    // =========================
    private void ApplyBenefit(Sale sale, PromotionBenefit benefit, int? maxUsagePerCart)
    {
        // CART discount (ProductId null ise)
        if (!benefit.ProductId.HasValue)
        {
            var total = sale.SaleItems.Sum(x => x.PriceTotal);
            double discount = 0;

            if (benefit.Type == BenefitType.PercentageDiscount)
                discount = total * (benefit.Value ?? 0) / 100;
            else if (benefit.Type == BenefitType.FixedAmountDiscount)
                discount = benefit.Value ?? 0;

            if (benefit.MaxDiscount.HasValue)
                discount = Math.Min(discount, benefit.MaxDiscount.Value);

            sale.CartTotalDiscount += discount;
            return;
        }

        var items = GetItems(sale, benefit);

        foreach (var item in items)
        {
            if (item.NonEligableQuantity >= item.Quantity)
                continue;

            // item.ExtraPromotionName = benefit.BenefitGroup.Promotion.Campaign.Name;

            var eligibleQty = item.Quantity - item.NonEligableQuantity;
            if (eligibleQty <= 0)
                continue;

            var cappedQty = maxUsagePerCart.HasValue
                ? Math.Min(eligibleQty, maxUsagePerCart.Value)
                : eligibleQty;

            switch (benefit.Type)
            {
                case BenefitType.PercentageDiscount:
                    item.DiscountTotal += item.SalePrice * cappedQty * (benefit.Value ?? 0) / 100;
                    item.BenefitType = BenefitType.PercentageDiscount;
                    break;

                case BenefitType.FixedAmountDiscount:
                    item.DiscountTotal += (benefit.Value ?? 0) * cappedQty;
                    item.BenefitType = BenefitType.FixedAmountDiscount;
                    break;

                case BenefitType.FixedPrice:
                    var discount = Math.Max(0, item.SalePrice - (benefit.Value ?? item.SalePrice));
                    item.DiscountTotal += discount * cappedQty;
                    item.BenefitType = BenefitType.FixedPrice;
                    break;

                case BenefitType.FreeProduct:
                    var freeQty = Math.Min(benefit.Quantity ?? 1, maxUsagePerCart ?? int.MaxValue);
                    freeQty = (int)Math.Min(freeQty, eligibleQty);
                    item.DiscountTotal += item.SalePrice * freeQty;
                    item.NonEligableQuantity += freeQty;
                    item.BenefitType = BenefitType.FreeProduct;
                    break;

                case BenefitType.BuyXPayY:
                    var maxSets = maxUsagePerCart ?? int.MaxValue;
                    var set = Math.Min((int)(eligibleQty / benefit.BuyQuantity.Value), maxSets);
                    var free = set * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);
                    item.DiscountTotal += free * item.SalePrice;
                    item.NonEligableQuantity += Convert.ToInt32(free);
                    item.BenefitType = BenefitType.BuyXPayY;
                    break;
            }
        }
    }

    // =========================
    // GET ITEMS
    // =========================
    private List<SaleItem> GetItems(Sale sale, PromotionBenefit benefit)
    {
        if (benefit.ProductId.HasValue)
        {
            return sale.SaleItems
                .Where(x => x.ProductId == benefit.ProductId.Value)
                .ToList();
        }

        return new List<SaleItem>();
    }

    // public async Task<List<Campaign>> CheckCampaignEligable(Sale sale)
    // {
    //     var now = DateTime.Now;

    //     var regionId = await _dbContext.AppCooperatives
    //         .Where(c => c.Id == sale.CooperativeId && !c.IsDeleted)
    //         .Select(c => c.RegionId)
    //         .FirstOrDefaultAsync();

    //     var customer = await _dbContext.AppTkCustomers
    //         .Where(c => c.TcknOrVkn == sale.TcknOrVkn && sale.TcknOrVkn != "11111111111" && !c.IsDeleted)
    //         .FirstOrDefaultAsync();

    //     return await _dbContext.KoopPosCampaigns
    //         .AsNoTracking()
    //         .Include(c => c.Promotions).ThenInclude(p => p.ConditionGroups).ThenInclude(c => c.Conditions)
    //         .Include(c => c.Promotions).ThenInclude(p => p.BenefitGroups).ThenInclude(c => c.Benefits).ThenInclude(b => b.Product)
    //         .Where(c => !c.IsDeleted
    //                     && c.IsActive
    //                     && c.StartDate <= now
    //                     && c.EndDate >= now
    //                     && (
    //                         // Hiçbir kısıtlama yoksa herkese açık
    //                         (!c.CampaignCooperatives.Any()
    //                          && !c.CampaignTkRegions.Any()
    //                          && !c.CampaignTkCustomers.Any()
    //                          && !c.CampaignTkCustomerTypes.Any())
    //                         ||
    //                         c.CampaignCooperatives.Any(cc => cc.CooperativeId == sale.CooperativeId)
    //                         || c.CampaignTkRegions.Any(cr => cr.TkRegionId == regionId)
    //                         || (customer != null && c.CampaignTkCustomers.Any(ct => ct.TkCustomerId == customer.Id))
    //                         || (customer != null && c.CampaignTkCustomerTypes.Any(ct => ct.CustomerType == customer.CustomerType))
    //                     ))
    //         .ToListAsync();
    // }
    // public async Task<List<Campaign>> CheckCampaignEligable(Sale sale)
    // {
    //     var now = DateTime.Now;

    //     var regionId = await _dbContext.AppCooperatives
    //         .Where(c => c.Id == sale.CooperativeId && !c.IsDeleted)
    //         .Select(c => c.RegionId)
    //         .FirstOrDefaultAsync();

    //     var customer = await _dbContext.AppTkCustomers
    //         .Where(c => c.TcknOrVkn == sale.TcknOrVkn && sale.TcknOrVkn != "11111111111" && !c.IsDeleted)
    //         .FirstOrDefaultAsync();

    //     return await _dbContext.KoopPosCampaigns
    //         .AsNoTracking()
    //         .Include(c => c.CampaignProduct)
    //         .Where(c => !c.IsDeleted
    //                     && c.IsActive
    //                     && c.StartDate <= now
    //                     && c.EndDate >= now
    //                     && (
    //                         // Hiçbir kısıtlama yoksa herkese açık
    //                         (!c.CampaignCooperatives.Any()
    //                          && !c.CampaignTkRegions.Any()
    //                          && !c.CampaignTkCustomers.Any()
    //                          && !c.CampaignTkCustomerTypes.Any())
    //                         ||
    //                         c.CampaignCooperatives.Any(cc => cc.CooperativeId == sale.CooperativeId)
    //                         || c.CampaignTkRegions.Any(cr => cr.TkRegionId == regionId)
    //                         || (customer != null && c.CampaignTkCustomers.Any(ct => ct.TkCustomerId == customer.Id))
    //                         || (customer != null && c.CampaignTkCustomerTypes.Any(ct => ct.CustomerType == customer.CustomerType))
    //                     ))
    //         .ToListAsync();
    // }

}

