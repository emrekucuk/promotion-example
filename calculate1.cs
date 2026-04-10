
// namespace CampaignEngine
// {
//     public static class CampaignFactory
//     {
//         public static Campaign CreateBuyXPayYCampaign(Guid productId, int buy, int pay)
//         {
//             return new Campaign
//             {
//                 Id = Guid.NewGuid(),
//                 Name = $"{buy} Al {pay} Öde",
//                 IsActive = true,
//                 StartDate = DateTime.Now.AddDays(-1),
//                 EndDate = DateTime.Now.AddDays(1),
//                 Promotions = new List<Promotion>
//                 {
//                     new Promotion
//                     {
//                         Id = Guid.NewGuid(),
//                         Name = "BuyXPayY Promo",
//                         Priority = 10,
//                         Targets = new List<PromotionTarget>
//                         {
//                             new PromotionTarget
//                             {
//                                 TargetType = TargetType.Product,
//                                 ProductId = productId
//                             }
//                         },
//                         Conditions = new List<PromotionCondition>
//                         {
//                             new PromotionCondition
//                             {
//                                 Type = ConditionType.ProductQuantity,
//                                 ProductId = productId,
//                                 Operator = OperatorType.GreaterOrEqual,
//                                 Value = buy
//                             }
//                         },
//                         Benefits = new List<PromotionBenefit>
//                         {
//                             new PromotionBenefit
//                             {
//                                 Type = BenefitType.BuyXPayY,
//                                 ProductId = productId,
//                                 BuyQuantity = buy,
//                                 PayQuantity = pay
//                             }
//                         }
//                     }
//                 }
//             };
//         }

//         public static Campaign CreatePercentageCampaign(Guid productId, double percent)
//         {
//             return new Campaign
//             {
//                 Id = Guid.NewGuid(),
//                 Name = $"%{percent} İndirim",
//                 IsActive = true,
//                 StartDate = DateTime.Now.AddDays(-1),
//                 EndDate = DateTime.Now.AddDays(1),
//                 Promotions = new List<Promotion>
//                 {
//                     new Promotion
//                     {
//                         Id = Guid.NewGuid(),
//                         Name = "Percentage Promo",
//                         Priority = 5,
//                         Targets = new List<PromotionTarget>
//                         {
//                             new PromotionTarget
//                             {
//                                 TargetType = TargetType.Product,
//                                 ProductId = productId
//                             }
//                         },
//                         Conditions = new List<PromotionCondition>
//                         {
//                             new PromotionCondition
//                             {
//                                 Type = ConditionType.ProductExists,
//                                 ProductId = productId
//                             }
//                         },
//                         Benefits = new List<PromotionBenefit>
//                         {
//                             new PromotionBenefit
//                             {
//                                 Type = BenefitType.PercentageDiscount,
//                                 Value = percent
//                             }
//                         }
//                     }
//                 }
//             };
//         }

//         public static Campaign CreateFreeProductCampaign(Guid productId, int quantity)
//         {
//             return new Campaign
//             {
//                 Id = Guid.NewGuid(),
//                 Name = $"Hediye Ürün",
//                 IsActive = true,
//                 StartDate = DateTime.Now.AddDays(-1),
//                 EndDate = DateTime.Now.AddDays(1),
//                 Promotions = new List<Promotion>
//                 {
//                     new Promotion
//                     {
//                         Id = Guid.NewGuid(),
//                         Name = "Free Product Promo",
//                         Priority = 8,
//                         Targets = new List<PromotionTarget>
//                         {
//                             new PromotionTarget
//                             {
//                                 TargetType = TargetType.Product,
//                                 ProductId = productId
//                             }
//                         },
//                         Conditions = new List<PromotionCondition>
//                         {
//                             new PromotionCondition
//                             {
//                                 Type = ConditionType.ProductExists,
//                                 ProductId = productId,
//                                 Quantity =2,
//                             }
//                         },
//                         Benefits = new List<PromotionBenefit>
//                         {
//                             new PromotionBenefit
//                             {
//                                 Type = BenefitType.FreeProduct,
//                                 ProductId = productId,
//                                 Quantity = quantity
//                             }
//                         }
//                     }
//                 }
//             };
//         }
//     }
//     public class Sale
//     {
//         public Guid Id { get; set; }

//         public double DiscountTotal { get; set; }

//         public List<SaleItem> SaleItems { get; set; } = new();
//     }

//     public class SaleItem
//     {
//         public Guid ProductId { get; set; }
//         public Guid ProductVersionId { get; set; }
//         public double Quantity { get; set; }
//         public double TotalTax { get; set; }
//         public double PriceTotal { get; set; } // Örn: 150 3 al 2 öde 
//         public double GrandTotal { get; set; } //100
//         public double DiscountTotal { get; set; }
//         public double SalePrice { get; set; }
//         public double TaxRate { get; set; }
//         public string ProductName { get; set; }
//         public int Index { get; set; }
//         public BenefitType DiscountCampaignType { get; set; }
//         public bool HasDiscount { get; set; }
//         public double NonEligableQuantity { get; set; }
//         public double DiscountTotalNonEligable { get; set; }
//         public string ExtraCampaignName { get; set; }
//     }

//     // =========================
//     // CAMPAIGN (Container)
//     // =========================
//     /// <summary>
//     /// Kampanya ana container’ı. Bir kampanya bir veya birden fazla promotion içerir.
//     /// </summary>
//     public class Campaign
//     {
//         public Guid Id { get; set; }

//         /// <summary>
//         /// Kampanya adı
//         /// </summary>
//         public string Name { get; set; }

//         /// <summary>
//         /// Kampanya açıklaması
//         /// </summary>
//         public string Description { get; set; }

//         /// <summary>
//         /// Kampanya başlangıç tarihi
//         /// </summary>
//         public DateTime StartDate { get; set; }

//         /// <summary>
//         /// Kampanya bitiş tarihi
//         /// </summary>
//         public DateTime EndDate { get; set; }

//         /// <summary>
//         /// Kampanya aktif mi pasif mi
//         /// </summary>
//         public bool IsActive { get; set; }

//         /// <summary>
//         /// Kampanya içindeki tüm promotionlar
//         /// </summary>
//         public List<Promotion> Promotions { get; set; } = new List<Promotion>();
//     }

//     // =========================
//     // PROMOTION (Execution Unit)
//     // =========================
//     /// <summary>
//     /// Kampanya içindeki uygulama birimi. 
//     /// Örn: indirim, hediye, 3 al 2 öde gibi kurallar burada tanımlanır.
//     /// </summary>
//     public class Promotion
//     {
//         public Guid Id { get; set; }

//         /// <summary>
//         /// Hangi kampanya için
//         /// </summary>
//         public Guid CampaignId { get; set; }
//         public Campaign Campaign { get; set; }

//         /// <summary>
//         /// Promotion adı
//         /// </summary>
//         public string Name { get; set; }

//         /// <summary>
//         /// Çakışma yönetimi: Öncelik
//         /// </summary>
//         public int Priority { get; set; } = 0;

//         /// <summary>
//         /// Başka promotionlarla kombinlenebilir mi
//         /// </summary>
//         public bool IsCombinable { get; set; } = true;

//         /// <summary>
//         /// Kullanıcı başına maksimum kullanım limiti
//         /// </summary>
//         public int? MaxUsagePerUser { get; set; }

//         /// <summary>
//         /// Hedef kitle/ürün vs
//         /// </summary>
//         public List<PromotionTarget> Targets { get; set; } = new List<PromotionTarget>();

//         /// <summary>
//         /// Koşullar (IF kısmı)
//         /// </summary>
//         public List<PromotionCondition> Conditions { get; set; } = new List<PromotionCondition>();

//         /// <summary>
//         /// Faydalar/indirimler (THEN kısmı)
//         /// </summary>
//         public List<PromotionBenefit> Benefits { get; set; } = new List<PromotionBenefit>();
//     }

//     // =========================
//     // TARGET (Scope)
//     // =========================
//     /// <summary>
//     /// Promotion hedefi (ürün, kategori veya sepet)
//     /// </summary>
//     public class PromotionTarget
//     {
//         public Guid Id { get; set; }

//         /// <summary>
//         /// Hangi promotion için
//         /// </summary>
//         public Guid PromotionId { get; set; }
//         public Promotion Promotion { get; set; }

//         /// <summary>
//         /// Hedef türü (ürün, kategori, sepet)
//         /// </summary>
//         public TargetType TargetType { get; set; }

//         /// <summary>
//         /// Hedef ürün (varsa)
//         /// </summary>
//         public Guid? ProductId { get; set; }

//         /// <summary>
//         /// Hedef kategori (varsa)
//         /// </summary>
//         public Guid? CategoryId { get; set; }
//     }

//     // =========================
//     // CONDITION (IF)
//     // =========================
//     /// <summary>
//     /// Promotion için koşullar. Örn: ürün adedi, sepet toplamı vb.
//     /// </summary>
//     public class PromotionCondition
//     {
//         public Guid Id { get; set; }

//         /// <summary>
//         /// Hangi promotion için
//         /// </summary>
//         public Guid PromotionId { get; set; }
//         public Promotion Promotion { get; set; }

//         /// <summary>
//         /// Koşul türü (ürün adedi, ürün varlığı, sepet tutarı)
//         /// </summary>
//         public ConditionType Type { get; set; }

//         /// <summary>
//         /// Hedef ürün (varsa)
//         /// </summary>
//         public Guid? ProductId { get; set; }

//         /// <summary>
//         /// Koşul değeri (örn: sepet toplamı veya sabit değer)
//         /// </summary>
//         public double? Value { get; set; }

//         /// <summary>
//         /// Karşılaştırma operatörü (>=, <=, = vb.)
//         /// </summary>
//         public OperatorType? Operator { get; set; }

//         /// <summary>
//         /// Maksimum quantity limiti (opsiyonel)
//         /// </summary>
//         public int? MaxQuantity { get; set; }

//         /// <summary>
//         /// Minimum quantity limiti (opsiyonel)
//         /// </summary>
//         public int? MinQuantity { get; set; }

//         /// <summary>
//         /// Örn: ürün adedi veya sepet toplamı
//         /// </summary>
//         public int? Quantity { get; set; }
//     }

//     // =========================
//     // BENEFIT (THEN)
//     // =========================
//     /// <summary>
//     /// Promotion sonrası uygulanacak faydalar / indirimler
//     /// </summary>
//     public class PromotionBenefit
//     {
//         public Guid Id { get; set; }

//         /// <summary>
//         /// Hangi promotion için
//         /// </summary>
//         public Guid PromotionId { get; set; }
//         public Promotion Promotion { get; set; }

//         /// <summary>
//         /// Fayda tipi (indirim, hediye, 3 al 2 öde vb.)
//         /// </summary>
//         public BenefitType Type { get; set; }

//         /// <summary>
//         /// İndirim değeri (% veya sabit)
//         /// </summary>
//         public double? Value { get; set; }

//         /// <summary>
//         /// Maksimum indirim limiti
//         /// </summary>
//         public double? MaxDiscount { get; set; }

//         /// <summary>
//         /// Maksimum uygulanabilir tutar
//         /// </summary>
//         public double? MaxApplicableAmount { get; set; }

//         /// <summary>
//         /// Hedef ürün (hediye vb.)
//         /// </summary>
//         public Guid? ProductId { get; set; }

//         /// <summary>
//         /// Hedef ürün adedi
//         /// </summary>
//         public int? Quantity { get; set; }

//         /// <summary>
//         /// Buy X Pay Y için alım adedi
//         /// </summary>
//         public int? BuyQuantity { get; set; }

//         /// <summary>
//         /// Buy X Pay Y için ödenecek adedi
//         /// </summary>
//         public int? PayQuantity { get; set; }
//     }

//     // =========================
//     // ENUMS
//     // =========================

//     /// <summary>
//     /// Promotion hedef türleri
//     /// </summary>
//     public enum TargetType
//     {
//         Product = 1,    // Tek bir ürün
//         Category = 2,   // Kategori bazlı
//         Cart = 3        // Sepet bazlı
//     }

//     /// <summary>
//     /// Promotion koşul türleri
//     /// </summary>
//     public enum ConditionType
//     {
//         ProductQuantity = 1,  // Ürün adedi
//         ProductExists = 2,    // Ürün sepette var mı
//         CartTotal = 3         // Sepet toplamı
//     }

//     /// <summary>
//     /// Koşul karşılaştırma operatörleri
//     /// </summary>
//     public enum OperatorType
//     {
//         GreaterThan = 1,      // >
//         GreaterOrEqual = 2,   // >=
//         LessThan = 3,         // <
//         LessOrEqual = 4,      // <=
//         Equal = 5             // =
//     }

//     /// <summary>
//     /// Promotion fayda türleri
//     /// </summary>
//     public enum BenefitType
//     {
//         None = 0,
//         PercentageDiscount = 1, // Yüzdelik indirim
//         FixedAmountDiscount = 2, // Sabit tutar indirim
//         FixedPrice = 3,         // Sabit fiyat uygulaması
//         FreeProduct = 4,        // Ücretsiz ürün
//         BuyXPayY = 5            // 3 al 2 öde vb.
//     }

//     ////////////////
//     /// 
//     /// 
//     public class PromotionEngine
//     {
//         public void ApplyPromotions(Sale sale, List<Campaign> campaigns)
//         {
//             if (sale == null || !sale.SaleItems.Any())
//                 return;

//             ResetSale(sale);

//             // Campaign bazlı çalışıyoruz
//             foreach (var campaign in campaigns.Where(c => c.IsActive))
//             {
//                 if (!IsCampaignValid(campaign))
//                     continue;

//                 foreach (var promo in campaign.Promotions.OrderByDescending(x => x.Priority))
//                 {
//                     if (!CheckConditions(sale, promo))
//                         continue;

//                     ApplyPromotion(sale, promo);
//                 }
//             }

//             sale.DiscountTotal = sale.SaleItems.Sum(x => x.DiscountTotal);
//         }

//         private void ResetSale(Sale sale)
//         {
//             sale.DiscountTotal = 0;

//             foreach (var item in sale.SaleItems)
//             {
//                 item.DiscountTotal = 0;
//                 item.NonEligableQuantity = 0;
//                 item.DiscountTotalNonEligable = 0;
//             }
//         }

//         private bool IsCampaignValid(Campaign campaign)
//         {
//             var now = DateTime.Now;

//             return campaign.IsActive &&
//                    campaign.StartDate <= now &&
//                    campaign.EndDate >= now;
//         }


//         // =========================
//         // CONDITION
//         // =========================
//         private bool CheckConditions(Sale sale, Promotion promo)
//         {
//             foreach (var cond in promo.Conditions)
//             {
//                 double value = 0;

//                 switch (cond.Type)
//                 {
//                     case ConditionType.ProductQuantity:
//                         var item = sale.SaleItems.FirstOrDefault(x => x.ProductId == cond.ProductId);
//                         if (item == null) return false;

//                         value = item.Quantity;
//                         break;

//                     case ConditionType.ProductExists:
//                         if (!sale.SaleItems.Any(x => x.ProductId == cond.ProductId))
//                             return false;
//                         break;

//                     case ConditionType.CartTotal:
//                         value = sale.SaleItems.Sum(x => x.PriceTotal);
//                         break;
//                 }

//                 // min/max quantity
//                 if (cond.MinQuantity.HasValue && value < cond.MinQuantity.Value)
//                     return false;

//                 if (cond.MaxQuantity.HasValue && value > cond.MaxQuantity.Value)
//                     return false;

//                 // operator
//                 if (cond.Operator.HasValue && cond.Value.HasValue)
//                 {
//                     if (!Compare(value, cond.Value.Value, cond.Operator.Value))
//                         return false;
//                 }
//             }

//             return true;
//         }

//         private bool Compare(double actual, double target, OperatorType op)
//         {
//             return op switch
//             {
//                 OperatorType.GreaterThan => actual > target,
//                 OperatorType.GreaterOrEqual => actual >= target,
//                 OperatorType.LessThan => actual < target,
//                 OperatorType.LessOrEqual => actual <= target,
//                 OperatorType.Equal => actual == target,
//                 _ => false
//             };
//         }

//         // =========================
//         // APPLY PROMOTION
//         // =========================
//         private void ApplyPromotion(Sale sale, Promotion promo)
//         {
//             var targetItems = GetTargetItems(sale, promo);

//             foreach (var benefit in promo.Benefits)
//             {
//                 switch (benefit.Type)
//                 {
//                     case BenefitType.PercentageDiscount:
//                         ApplyPercentage(sale, targetItems, benefit);
//                         break;

//                     case BenefitType.FixedAmountDiscount:
//                         ApplyFixedAmount(sale, targetItems, benefit);
//                         break;

//                     case BenefitType.FixedPrice:
//                         ApplyFixedPrice(sale, targetItems, benefit);
//                         break;

//                     case BenefitType.FreeProduct:
//                         ApplyFreeProduct(sale, targetItems, benefit);
//                         break;

//                     case BenefitType.BuyXPayY:
//                         ApplyBuyXPayY(sale, targetItems, benefit);
//                         break;
//                 }
//             }
//         }

//         // =========================
//         // TARGET FILTER
//         // =========================
//         private List<SaleItem> GetTargetItems(Sale sale, Promotion promo)
//         {
//             var result = new List<SaleItem>();

//             foreach (var target in promo.Targets)
//             {
//                 switch (target.TargetType)
//                 {
//                     case TargetType.Product:
//                         result.AddRange(sale.SaleItems
//                             .Where(x => x.ProductId == target.ProductId));
//                         break;

//                     case TargetType.Category:
//                         // result.AddRange(sale.SaleItems
//                         //     .Where(x => x.CategoryId == target.CategoryId));
//                         break;

//                     case TargetType.Cart:
//                         result.AddRange(sale.SaleItems);
//                         break;
//                 }
//             }

//             return result.Distinct().ToList();
//         }

//         // =========================
//         // BENEFITS
//         // =========================

//         private void ApplyPercentage(Sale sale, List<SaleItem> items, PromotionBenefit benefit)
//         {
//             foreach (var item in items)
//             {
//                 var eligibleQty = item.Quantity - item.NonEligableQuantity;
//                 if (eligibleQty <= 0) continue;

//                 double discount = item.SalePrice * eligibleQty * (benefit.Value ?? 0) / 100;

//                 discount = ApplyMaxLimits(discount, benefit);

//                 item.DiscountTotal += discount;
//                 item.DiscountCampaignType = BenefitType.PercentageDiscount;
//             }
//         }

//         private void ApplyFixedAmount(Sale sale, List<SaleItem> items, PromotionBenefit benefit)
//         {
//             foreach (var item in items)
//             {
//                 var eligibleQty = item.Quantity - item.NonEligableQuantity;
//                 if (eligibleQty <= 0) continue;

//                 double discount = (benefit.Value ?? 0) * eligibleQty;

//                 discount = ApplyMaxLimits(discount, benefit);

//                 item.DiscountTotal += discount;
//                 item.DiscountCampaignType = BenefitType.FixedPrice;

//             }
//         }

//         private void ApplyFixedPrice(Sale sale, List<SaleItem> items, PromotionBenefit benefit)
//         {
//             foreach (var item in items)
//             {
//                 var eligibleQty = item.Quantity - item.NonEligableQuantity;
//                 if (eligibleQty <= 0) continue;

//                 double original = item.SalePrice * eligibleQty;
//                 double fixedTotal = (benefit.Value ?? item.SalePrice) * eligibleQty;

//                 double discount = Math.Max(0, original - fixedTotal);

//                 item.DiscountTotal += discount;
//                 item.DiscountCampaignType = BenefitType.FixedPrice;

//             }
//         }

//         private void ApplyFreeProduct(Sale sale, List<SaleItem> items, PromotionBenefit benefit)
//         {
//             foreach (var item in items)
//             {
//                 int freeQty = benefit.Quantity ?? 1;

//                 var eligibleQty = Math.Min(freeQty, item.Quantity);
//                 double discount = eligibleQty * item.SalePrice;

//                 item.DiscountTotal += discount;

//                 item.NonEligableQuantity += eligibleQty;
//                 item.DiscountTotalNonEligable += discount;
//                 item.DiscountCampaignType = BenefitType.FreeProduct;

//             }
//         }

//         private void ApplyBuyXPayY(Sale sale, List<SaleItem> items, PromotionBenefit benefit)
//         {
//             if (!benefit.BuyQuantity.HasValue || !benefit.PayQuantity.HasValue)
//                 return;

//             foreach (var item in items)
//             {
//                 var eligibleQty = item.Quantity - item.NonEligableQuantity;
//                 if (eligibleQty < benefit.BuyQuantity.Value)
//                     continue;

//                 double setCount = eligibleQty / benefit.BuyQuantity.Value;

//                 var freeQty = setCount * (benefit.BuyQuantity.Value - benefit.PayQuantity.Value);

//                 double discount = freeQty * item.SalePrice;

//                 item.DiscountTotal += discount;

//                 item.NonEligableQuantity += freeQty;
//                 item.DiscountTotalNonEligable += discount;
//                 item.DiscountCampaignType = BenefitType.BuyXPayY;

//             }
//         }

//         private double ApplyMaxLimits(double discount, PromotionBenefit benefit)
//         {
//             if (benefit.MaxDiscount.HasValue)
//                 discount = Math.Min(discount, benefit.MaxDiscount.Value);

//             if (benefit.MaxApplicableAmount.HasValue)
//                 discount = Math.Min(discount, benefit.MaxApplicableAmount.Value);

//             return discount;
//         }
//     }
// }