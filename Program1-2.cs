// using System;
// using System.Collections.Generic;
// using CampaignEngine;

// class Program
// {
//     static void Main(string[] args)
//     {
//         var gazoz = Guid.Parse("235c0031-b98d-426a-a4b3-6e3855bd21f9");
//         var yogurt = Guid.Parse("c674752f-becf-4281-b90f-2705f7813112");
//         var kola = Guid.Parse("51855889-0705-47e9-8983-4fa26703447f");
//         var ayran = Guid.Parse("7c483d12-5d61-4429-be6b-025fbd470c3f");
//         var simit = Guid.Parse("2ee9f76c-fcf6-4348-802e-5429947c9873");
//         var pogaca = Guid.Parse("0b0dbf46-24c8-479a-8582-150ab652a7ef");
//         var cay = Guid.Parse("2e1c36f6-293e-4b00-873c-aac41395b112");
//         var ekmek = Guid.Parse("6889c813-082d-4466-a0a7-8a425e3b9cb6");
//         var kurabiye = Guid.Parse("eef97af1-ba0d-43b5-bbf7-66c234d56570");
//         var kahve = Guid.Parse("1fabd85e-42e7-4053-a77c-3fd12e491a9f");
//         var kalem = Guid.Parse("e7c72c78-1fb5-411b-86fe-cadd44586cfe");
//         var ulkerGofret = Guid.Parse("97c2d204-328e-4a80-922e-709c79264c40");

//         var sale = new Sale
//         {
//             Id = Guid.Parse("25a5c681-5f82-492d-a104-aeb9d89c77f8"),
//             SaleItems = new List<SaleItem>
//     {
//         new SaleItem { ProductId = gazoz, Quantity = 3, SalePrice = 20, PriceTotal = 60 },
//         new SaleItem { ProductId = yogurt, Quantity = 2, SalePrice = 30, PriceTotal = 60 },
//         new SaleItem { ProductId = kola, Quantity = 1, SalePrice = 40, PriceTotal = 40 },
//         new SaleItem { ProductId = ayran, Quantity = 1, SalePrice = 15, PriceTotal = 15 },
//         new SaleItem { ProductId = cay, Quantity = 1, SalePrice = 30, PriceTotal = 30 },
//         new SaleItem { ProductId = simit, Quantity = 2, SalePrice = 10, PriceTotal = 20 },
//         new SaleItem { ProductId = ekmek, Quantity = 1, SalePrice = 15, PriceTotal = 15 },
//         new SaleItem { ProductId = kahve, Quantity = 1, SalePrice = 50, PriceTotal = 50 },
//         new SaleItem { ProductId = kalem, Quantity = 1, SalePrice = 40, PriceTotal = 40 },
//         new SaleItem { ProductId = ulkerGofret, Quantity = 1, SalePrice = 25, PriceTotal = 25 }
//     }
//         };

//         // 🔥 CampaignType ile oluşturuyoruz
//         var campaigns = new List<Campaign>
//         {
//             // CampaignFactory.CreateBuyXPayYCampaign(productA, 3, 2),
//             // CampaignFactory.CreatePercentageCampaign(productB, 10),
//             // CampaignFactory.CreateFreeProductCampaign(productC, 1)
//             new Campaign
//             {
//                 Id = Guid.Parse("8ad251ca-c772-46e7-9a5a-7a3041b89987"),
//                 Name = "Mega Campaign",
//                 IsActive = true,
//                 StartDate = DateTime.Now.AddDays(-1),
//                 EndDate = DateTime.Now.AddDays(1),
//                 Promotions =new List<Promotion>
//                 {
//                     // new Promotion
//                     // {
//                     //     Targets = new List<PromotionTarget>
//                     //     {
//                     //         new PromotionTarget { TargetType = TargetType.Product, ProductId = kahve }
//                     //     },
//                     //     Conditions = new List<PromotionCondition>
//                     //     {
//                     //         new PromotionCondition
//                     //         {
//                     //             Type = ConditionType.ProductExists,
//                     //             ProductId = ekmek
//                     //         },
//                     //         new PromotionCondition
//                     //         {
//                     //             Type = ConditionType.ProductExists,
//                     //             ProductId = simit
//                     //         }
//                     //     },
//                     //     Benefits = new List<PromotionBenefit>
//                     //     {
//                     //         new PromotionBenefit
//                     //         {
//                     //             Type = BenefitType.PercentageDiscount,
//                     //             Value = 10
//                     //         }
//                     //     }
//                     // },
//                 }               

//                 // Promotions = new List<Promotion>
//                 // {
//                 //     // 🟢 Gazoz 3 al 2 öde
//                 //     new Promotion
//                 //     {
//                 //         Priority = 10,
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = gazoz }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.ProductQuantity,
//                 //                 ProductId = gazoz,
//                 //                 Operator = OperatorType.GreaterOrEqual,
//                 //                 Value = 3
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.BuyXPayY,
//                 //                 ProductId = gazoz,
//                 //                 BuyQuantity = 3,
//                 //                 PayQuantity = 2
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 Yoğurt %5
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = yogurt }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.ProductExists,
//                 //                 ProductId = yogurt
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.PercentageDiscount,
//                 //                 Value = 5
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 Kola alana ayran bedava
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = ayran }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.ProductExists,
//                 //                 ProductId = kola
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.FreeProduct,
//                 //                 ProductId = ayran,
//                 //                 Quantity = 1
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 2 simit veya pogaca → çay 10 TL indirim
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = cay }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.ProductQuantity,
//                 //                 ProductId = simit,
//                 //                 Operator = OperatorType.GreaterOrEqual,
//                 //                 Value = 2
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.FixedAmountDiscount,
//                 //                 Value = 10
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 ekmek veya kurabiye → kahve %10
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = kahve }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.ProductExists,
//                 //                 ProductId = ekmek
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.PercentageDiscount,
//                 //                 Value = 10
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 Sepet ≥100 → kalem 25 TL
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = kalem }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.CartTotal,
//                 //                 Operator = OperatorType.GreaterOrEqual,
//                 //                 Value = 100
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.FixedPrice,
//                 //                 Value = 25
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 Ülker gofret 15 TL
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Product, ProductId = ulkerGofret }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.ProductExists,
//                 //                 ProductId = ulkerGofret
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.FixedPrice,
//                 //                 Value = 15
//                 //             }
//                 //         }
//                 //     },

//                 //     // 🟢 Sepet ≥500 → %10 ama max 1000 TL
//                 //     new Promotion
//                 //     {
//                 //         Targets = new List<PromotionTarget>
//                 //         {
//                 //             new PromotionTarget { TargetType = TargetType.Cart }
//                 //         },
//                 //         Conditions = new List<PromotionCondition>
//                 //         {
//                 //             new PromotionCondition
//                 //             {
//                 //                 Type = ConditionType.CartTotal,
//                 //                 Operator = OperatorType.GreaterOrEqual,
//                 //                 Value = 500
//                 //             }
//                 //         },
//                 //         Benefits = new List<PromotionBenefit>
//                 //         {
//                 //             new PromotionBenefit
//                 //             {
//                 //                 Type = BenefitType.PercentageDiscount,
//                 //                 Value = 10,
//                 //                 MaxApplicableAmount = 1000
//                 //             }
//                 //         }
//                 //     }
//                 // }


//             }
//         };

//         var engine = new PromotionEngine();
//         engine.ApplyPromotions(sale, campaigns);

//         Console.WriteLine("===== SONUÇ =====");

//         foreach (var item in sale.SaleItems)
//         {
//             Console.WriteLine($"Ürün: {item.ProductId}");
//             Console.WriteLine($"Adet: {item.Quantity}");
//             Console.WriteLine($"Promosyon Tipi: {item.DiscountCampaignType}");
//             Console.WriteLine($"PriceTotal: {item.PriceTotal}");
//             Console.WriteLine($"SalePrice: {item.SalePrice}");
//             Console.WriteLine($"İndirim: {item.DiscountTotal}");
//             Console.WriteLine($"NonEligible: {item.NonEligableQuantity}");
//             Console.WriteLine("----------------");
//         }

//         Console.WriteLine($"TOPLAM İNDİRİM: {sale.DiscountTotal}");

//     }
// }
