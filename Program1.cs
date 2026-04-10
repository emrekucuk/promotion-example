// using System;
// using System.Collections.Generic;
// using System.Linq;

// // ─── Enums ───────────────────────────────────────────────────────────────────

// enum CampaignType
// {
//     BuyXGetYFree,
//     BuyXPayForY,
//     PercentageDiscount,
//     FixedAmountDiscount,
//     CartAmountDiscount,
//     CartPercentageDiscount,
//     ProductBundleDiscount,
//     SecondItemDiscount,
//     CheapestItemFree,
//     CategoryDiscount,
//     CrossCategoryDiscount,
//     LoyaltyDiscount,
//     FirstPurchaseDiscount,
//     BirthdayDiscount,
//     FlashSale,
//     HappyHour,
//     SeasonalDiscount,
//     CashDiscount,
//     InstallmentDiscount,
//     SpecificCardDiscount,
//     FreeShipping,
//     GiftWithPurchase,
//     FreeItemWithAmount,
//     MultiBuyDiscount,
//     PackageDiscount
// }

// enum RuleType
// {
//     MinAmount,
//     MinQuantity,
//     ProductId,
//     CategoryId,
//     CustomerId,
//     PaymentMethod,
//     TimeRange,
//     FirstOrder
// }

// enum Operator
// {
//     Equal,
//     GreaterThan,
//     GreaterThanOrEqual,
//     LessThan,
//     In,
//     Between,
//     FirstOrder
// }

// enum RewardType
// {
//     FreeItem,
//     PercentageDiscount,
//     FixedDiscount,
//     FreeShipping,
//     GiftProduct
// }

// enum TargetType
// {
//     Cart,
//     Product,
//     Category,
//     CheapestItem,
//     MostExpensiveItem
// }

// // ─── Models ──────────────────────────────────────────────────────────────────

// class CartItem
// {
//     public Guid ProductId { get; set; }
//     public string Name { get; set; }
//     public string Category { get; set; }
//     public decimal Price { get; set; }
//     public int Quantity { get; set; }
//     public decimal TotalPrice => Price * Quantity;
// }

// class Cart
// {
//     public Guid CustomerId { get; set; }
//     public bool IsFirstOrder { get; set; }
//     public string PaymentMethod { get; set; }
//     public List<CartItem> Items { get; set; } = new();
//     public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
//     public int TotalQuantity => Items.Sum(i => i.Quantity);
// }

// class CampaignRule
// {
//     public RuleType RuleType { get; set; }
//     public Operator Operator { get; set; }
//     public string Value { get; set; }
//     public string Value2 { get; set; }
// }

// class CampaignReward
// {
//     public RewardType RewardType { get; set; }
//     public decimal Value { get; set; }
//     public TargetType TargetType { get; set; }
//     public Guid? TargetId { get; set; }
//     public int MaxUsageCount { get; set; } = int.MaxValue;
//     public int MaxUsagePerUser { get; set; } = int.MaxValue;
// }

// class Campaign
// {
//     public Guid Id { get; set; } = Guid.NewGuid();
//     public string Name { get; set; }
//     public CampaignType CampaignType { get; set; }
//     public DateTime StartDate { get; set; }
//     public DateTime EndDate { get; set; }
//     public bool IsActive { get; set; }
//     public int Priority { get; set; }
//     public bool IsStackable { get; set; }
//     public List<CampaignRule> Rules { get; set; } = new();
//     public CampaignReward Reward { get; set; }
// }

// class DiscountResult
// {
//     public string CampaignName { get; set; }
//     public decimal DiscountAmount { get; set; }
//     public string Description { get; set; }
// }

// // ─── Engine ──────────────────────────────────────────────────────────────────

// class CampaignEngine
// {
//     private readonly List<Campaign> _campaigns;

//     public CampaignEngine(List<Campaign> campaigns)
//     {
//         _campaigns = campaigns;
//     }

//     public List<DiscountResult> Apply(Cart cart)
//     {
//         var results = new List<DiscountResult>();

//         var activeCampaigns = _campaigns
//             .Where(c => c.IsActive
//                      && c.StartDate <= DateTime.Now
//                      && c.EndDate >= DateTime.Now)
//             .OrderBy(c => c.Priority)
//             .ToList();

//         foreach (var campaign in activeCampaigns)
//         {
//             if (!results.Any() || campaign.IsStackable)
//             {
//                 if (EvaluateRules(campaign, cart))
//                 {
//                     var discount = CalculateReward(campaign, cart);
//                     if (discount != null)
//                         results.Add(discount);
//                 }
//             }
//         }

//         return results;
//     }

//     private bool EvaluateRules(Campaign campaign, Cart cart)
//     {
//         foreach (var rule in campaign.Rules)
//         {
//             if (!EvaluateRule(rule, cart))
//                 return false;
//         }
//         return true;
//     }

//     private bool EvaluateRule(CampaignRule rule, Cart cart)
//     {
//         switch (rule.RuleType)
//         {
//             case RuleType.MinAmount:
//                 var minAmount = decimal.Parse(rule.Value);
//                 return rule.Operator switch
//                 {
//                     Operator.GreaterThan => cart.TotalAmount > minAmount,
//                     Operator.GreaterThanOrEqual => cart.TotalAmount >= minAmount,
//                     _ => false
//                 };

//             case RuleType.MinQuantity:
//                 var minQty = int.Parse(rule.Value);
//                 return rule.Operator switch
//                 {
//                     Operator.GreaterThan => cart.TotalQuantity > minQty,
//                     Operator.GreaterThanOrEqual => cart.TotalQuantity >= minQty,
//                     _ => false
//                 };

//             case RuleType.FirstOrder:
//                 return cart.IsFirstOrder;

//             case RuleType.PaymentMethod:
//                 return cart.PaymentMethod?.Equals(rule.Value, StringComparison.OrdinalIgnoreCase) ?? false;

//             default:
//                 return true;
//         }
//     }

//     private DiscountResult CalculateReward(Campaign campaign, Cart cart)
//     {
//         var reward = campaign.Reward;

//         switch (reward.RewardType)
//         {
//             // ── 1. PERCENTAGE DISCOUNT ────────────────────────────────────────
//             // TargetType'a göre indirim uygulanacak tutar belirlenir:
//             //   Cart            → sepet geneli toplam
//             //   CheapestItem    → sepetteki en düşük birim fiyatlı ürün
//             //   MostExpensiveItem → sepetteki en yüksek birim fiyatlı ürün
//             //   Product         → TargetId ile eşleşen ürünlerin satır toplamı
//             //   Category        → TargetId (CategoryId) ile eşleşen satır toplamı
//             case RewardType.PercentageDiscount:
//                 {
//                     decimal baseAmount = reward.TargetType switch
//                     {
//                         TargetType.CheapestItem =>
//                             cart.Items.Min(i => i.Price),

//                         TargetType.MostExpensiveItem =>
//                             cart.Items.Max(i => i.Price),

//                         TargetType.Product when reward.TargetId.HasValue =>
//                             cart.Items
//                                 .Where(i => i.ProductId == reward.TargetId.Value)
//                                 .Sum(i => i.TotalPrice),

//                         // Category senaryosunda TargetId, CategoryId'yi temsil eder;
//                         // CartItem.Category string olduğu için burada ProductId üzerinden
//                         // filtrelenmiştir — ileride CategoryId alanı eklenirse güncellenir.
//                         TargetType.Category when reward.TargetId.HasValue =>
//                             cart.Items
//                                 .Where(i => i.ProductId == reward.TargetId.Value)
//                                 .Sum(i => i.TotalPrice),

//                         _ => cart.TotalAmount   // TargetType.Cart veya tanımsız
//                     };

//                     var discount = Math.Round(baseAmount * (reward.Value / 100m), 2);

//                     string targetDesc = reward.TargetType switch
//                     {
//                         TargetType.CheapestItem => "en ucuz ürüne",
//                         TargetType.MostExpensiveItem => "en pahalı ürüne",
//                         TargetType.Product => "ürüne",
//                         TargetType.Category => "kategoriye",
//                         _ => "sepete"
//                     };

//                     return new DiscountResult
//                     {
//                         CampaignName = campaign.Name,
//                         DiscountAmount = discount,
//                         Description = $"%{reward.Value} indirim ({targetDesc}) → -{discount:C2}"
//                     };
//                 }

//             // ── 2. FIXED DISCOUNT ─────────────────────────────────────────────
//             // reward.Value: indirim tutarı (TL)
//             // Sabit indirim hiçbir zaman sepet toplamını aşamaz.
//             // TargetType; açıklama metnini zenginleştirmek için kullanılır;
//             // tutarın üst sınırı her durumda cart.TotalAmount'tır.
//             case RewardType.FixedDiscount:
//                 {
//                     decimal cap = reward.TargetType switch
//                     {
//                         TargetType.CheapestItem =>
//                             cart.Items.Min(i => i.Price),

//                         TargetType.MostExpensiveItem =>
//                             cart.Items.Max(i => i.Price),

//                         TargetType.Product when reward.TargetId.HasValue =>
//                             cart.Items
//                                 .Where(i => i.ProductId == reward.TargetId.Value)
//                                 .Sum(i => i.TotalPrice),

//                         TargetType.Category when reward.TargetId.HasValue =>
//                             cart.Items
//                                 .Where(i => i.ProductId == reward.TargetId.Value)
//                                 .Sum(i => i.TotalPrice),

//                         _ => cart.TotalAmount
//                     };

//                     var discount = Math.Min(reward.Value, cap);

//                     string targetDesc = reward.TargetType switch
//                     {
//                         TargetType.CheapestItem => "en ucuz ürüne",
//                         TargetType.MostExpensiveItem => "en pahalı ürüne",
//                         TargetType.Product => "ürüne",
//                         TargetType.Category => "kategoriye",
//                         _ => "sepete"
//                     };

//                     return new DiscountResult
//                     {
//                         CampaignName = campaign.Name,
//                         DiscountAmount = discount,
//                         Description = $"{discount:C2} sabit indirim ({targetDesc})"
//                     };
//                 }

//             // ── 3. FREE SHIPPING ──────────────────────────────────────────────
//             // reward.Value: standart kargo ücreti (örn. 29.90 TL).
//             // 0 bırakılırsa kargo bedava olarak işaretlenir; tutar bilinmez.
//             case RewardType.FreeShipping:
//                 {
//                     var shippingFee = reward.Value > 0 ? reward.Value : 0m;

//                     return new DiscountResult
//                     {
//                         CampaignName = campaign.Name,
//                         DiscountAmount = shippingFee,
//                         Description = shippingFee > 0
//                             ? $"Ücretsiz kargo → -{shippingFee:C2} kargo bedava"
//                             : "Ücretsiz kargo"
//                     };
//                 }

//             // ── 4. FREE ITEM ──────────────────────────────────────────────────
//             // Sepetteki mevcut bir ürünü bedava verir; fiyatı indirim olarak düşer.
//             //   CheapestItem      → en ucuz ürünün birim fiyatı
//             //   MostExpensiveItem → en pahalı ürünün birim fiyatı
//             //   Product           → TargetId ile eşleşen ürünün birim fiyatı
//             //   Cart / diğer      → reward.Value (sabit tutar; harici hediye fiyatı)
//             case RewardType.FreeItem:
//                 {
//                     decimal freeItemPrice = reward.TargetType switch
//                     {
//                         TargetType.CheapestItem =>
//                             cart.Items.Min(i => i.Price),

//                         TargetType.MostExpensiveItem =>
//                             cart.Items.Max(i => i.Price),

//                         TargetType.Product when reward.TargetId.HasValue =>
//                             cart.Items
//                                 .FirstOrDefault(i => i.ProductId == reward.TargetId.Value)
//                                 ?.Price ?? 0m,

//                         _ => reward.Value   // harici / önceden belirlenmiş fiyat
//                     };

//                     string itemDesc = reward.TargetType switch
//                     {
//                         TargetType.CheapestItem => "en ucuz ürün bedava",
//                         TargetType.MostExpensiveItem => "en pahalı ürün bedava",
//                         TargetType.Product => "seçili ürün bedava",
//                         _ => "hediye ürün"
//                     };

//                     return new DiscountResult
//                     {
//                         CampaignName = campaign.Name,
//                         DiscountAmount = freeItemPrice,
//                         Description = $"Ücretsiz ürün ({itemDesc}) → -{freeItemPrice:C2}"
//                     };
//                 }

//             // ── 5. GIFT PRODUCT ───────────────────────────────────────────────
//             // Sepete fiziksel olarak yeni bir hediye ürün eklenir.
//             // reward.Value: hediye ürünün katalog fiyatı.
//             // DiscountAmount olarak yansıtılır; envanter düşümü servis katmanında yapılır.
//             case RewardType.GiftProduct:
//                 {
//                     var giftPrice = reward.Value;

//                     return new DiscountResult
//                     {
//                         CampaignName = campaign.Name,
//                         DiscountAmount = giftPrice,
//                         Description = $"Hediye ürün → -{giftPrice:C2} değerinde ürün sepete eklendi"
//                     };
//                 }

//             default:
//                 return null;
//         }
//     }
// }

// // ─── Program ─────────────────────────────────────────────────────────────────

// class Program
// {
//     static void Main()
//     {
//         Console.OutputEncoding = System.Text.Encoding.UTF8;

//         // Kampanyaları tanımla
//         var campaigns = new List<Campaign>
//         {
//             // // 3 Al 2 Öde → en ucuz ürün %33 indirim
//             new Campaign
//             {
//                 Name = "3 Al 2 Öde",
//                 CampaignType = CampaignType.BuyXPayForY,
//                 StartDate = DateTime.Now.AddDays(-1),
//                 EndDate = DateTime.Now.AddDays(30),
//                 IsActive = true,
//                 Priority = 1,
//                 IsStackable = false,
//                 Rules = new List<CampaignRule>
//                 {
//                     new CampaignRule
//                     {
//                         RuleType = RuleType.MinQuantity,
//                         Operator = Operator.GreaterThanOrEqual,
//                         Value = "3"
//                     }
//                 },
//                 Reward = new CampaignReward
//                 {
//                     RewardType = RewardType.PercentageDiscount,
//                     Value = 33.33m,
//                     TargetType = TargetType.CheapestItem
//                 }
//             },

//             // 3 Al 1 Öde → en ucuz ürün %33 indirim
//             // new Campaign
//             // {
//             //     Name = "4 Al 3 Öde",
//             //     CampaignType = CampaignType.BuyXPayForY,
//             //     StartDate = DateTime.Now.AddDays(-1),
//             //     EndDate = DateTime.Now.AddDays(30),
//             //     IsActive = true,
//             //     Priority = 1,
//             //     IsStackable = false,
//             //     Rules = new List<CampaignRule>
//             //     {
//             //         new CampaignRule
//             //         {
//             //             RuleType = RuleType.MinQuantity,
//             //             Operator = Operator.GreaterThanOrEqual,
//             //             Value = "4"
//             //         }
//             //     },
//             //     Reward = new CampaignReward
//             //     {
//             //         RewardType = RewardType.FreeItem,
//             //         Value = 1,
//             //         TargetType = TargetType.Product,
//             //         TargetId = ,
//             //     }
//             // },

//             // 500TL üzeri %10 indirim
//             // new Campaign
//             // {
//             //     Name = "500TL Üzeri %10 İndirim",
//             //     CampaignType = CampaignType.CartPercentageDiscount,
//             //     StartDate = DateTime.Now.AddDays(-1),
//             //     EndDate = DateTime.Now.AddDays(30),
//             //     IsActive = true,
//             //     Priority = 2,
//             //     IsStackable = true,
//             //     Rules = new List<CampaignRule>
//             //     {
//             //         new CampaignRule
//             //         {
//             //             RuleType = RuleType.MinAmount,
//             //             Operator = Operator.GreaterThan,
//             //             Value = "500"
//             //         }
//             //     },
//             //     Reward = new CampaignReward
//             //     {
//             //         RewardType = RewardType.PercentageDiscount,
//             //         Value = 10,
//             //         TargetType = TargetType.Cart
//             //     }
//             // },

//             // İlk alışverişe 50TL indirim
//             // new Campaign
//             // {
//             //     Name = "İlk Alışveriş 50TL İndirim",
//             //     CampaignType = CampaignType.FirstPurchaseDiscount,
//             //     StartDate = DateTime.Now.AddDays(-1),
//             //     EndDate = DateTime.Now.AddDays(30),
//             //     IsActive = true,
//             //     Priority = 3,
//             //     IsStackable = true,
//             //     Rules = new List<CampaignRule>
//             //     {
//             //         new CampaignRule
//             //         {
//             //             RuleType = RuleType.FirstOrder,
//             //             Operator = Operator.FirstOrder
//             //         }
//             //     },
//             //     Reward = new CampaignReward
//             //     {
//             //         RewardType = RewardType.FixedDiscount,
//             //         Value = 50,
//             //         TargetType = TargetType.Cart
//             //     }
//             // }
//         };

//         // Test sepeti
//         var cart = new Cart
//         {
//             CustomerId = Guid.NewGuid(),
//             IsFirstOrder = true,
//             PaymentMethod = "CreditCard",
//             Items = new List<CartItem>
//             {
//                 new CartItem { Name = "Ürün A", Category = "Elektronik", Price = 200, Quantity = 2 },
//                 new CartItem { Name = "Ürün B", Category = "Giyim",      Price = 150, Quantity = 1 },
//                 new CartItem { Name = "Ürün C", Category = "Giyim",      Price = 100, Quantity = 1 },
//                 new CartItem { Name = "Ürün D", Category = "Elektronik",      Price = 50, Quantity = 4 },
//             }
//         };

//         // Sonuçları göster
//         Console.WriteLine("════════════════════════════════════");
//         Console.WriteLine("         KAMPANYA ENGİNE            ");
//         Console.WriteLine("════════════════════════════════════");
//         Console.WriteLine("\n📦 SEPET İÇERİĞİ:");
//         foreach (var item in cart.Items)
//             Console.WriteLine($"  - {item.Name} x{item.Quantity} = {item.TotalPrice:C2}");

//         Console.WriteLine($"\n  Toplam Adet : {cart.TotalQuantity}");
//         Console.WriteLine($"  Toplam Tutar: {cart.TotalAmount:C2}");
//         Console.WriteLine($"  İlk Sipariş : {cart.IsFirstOrder}");

//         var engine = new CampaignEngine(campaigns);
//         var results = engine.Apply(cart);

//         Console.WriteLine("\n🎯 UYGULANAN KAMPANYALAR:");
//         if (!results.Any())
//         {
//             Console.WriteLine("  Uygulanabilir kampanya bulunamadı.");
//         }
//         else
//         {
//             foreach (var r in results)
//             {
//                 Console.WriteLine($"  ✅ [{r.CampaignName}] → {r.Description}");
//             }
//         }

//         var totalDiscount = results.Sum(r => r.DiscountAmount);
//         var finalAmount = cart.TotalAmount - totalDiscount;

//         Console.WriteLine("\n════════════════════════════════════");
//         Console.WriteLine($"  Sepet Tutarı   : {cart.TotalAmount:C2}");
//         Console.WriteLine($"  Toplam İndirim : -{totalDiscount:C2}");
//         Console.WriteLine($"  Ödenecek Tutar : {finalAmount:C2}");
//         Console.WriteLine("════════════════════════════════════");
//     }
// }
