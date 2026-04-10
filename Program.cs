using System;
using System.Collections.Generic;
using System.Linq;
using CampaignEngine;

class Program
{
    static void Main()
    {
        // =========================
        // PRODUCT IDS
        // =========================
        var gazoz = Guid.Parse("00a35da7-9f3c-47a3-826c-30b244c19be9");
        var yogurt = Guid.Parse("5555ff9f-0b61-4296-a95f-a3817c633d97");
        var kola = Guid.Parse("d8115396-01d0-497e-bff8-e082180981d7");
        var ayran = Guid.Parse("59988083-ff82-4215-b7bc-cd79ae21c6f8");
        var simit = Guid.Parse("1a3ad151-078b-4e86-9c0a-270daf2c1656");
        var pogaca = Guid.Parse("a8ee2863-c458-4d3c-8c6a-73a5eb06ab2e");
        var cay = Guid.Parse("98e31b7f-338c-453b-8b50-73b3f7cc22e1");
        var su = Guid.Parse("bb7e5465-7368-489b-94b7-cc7ca3c6bb6d");
        var ekmek = Guid.Parse("5ac92dfc-aedf-41cb-a360-019bbab6c422");
        var kurabiye = Guid.Parse("4538fb96-f28d-4fef-a188-b0439a49d46d");
        var kahve = Guid.Parse("21a1c991-6037-4cee-a343-faa2bd83815a");
        var kalem = Guid.Parse("04ea9106-ed87-4f08-94a1-5aa26dbcc28d");
        var ulker = Guid.Parse("44f29311-6ab1-4fd2-958d-6ed94f127854");
        var sepet = Guid.Parse("46049707-e8a1-4d6d-9171-67a1828a6316");
        var patos = Guid.Parse("fb6e8d24-34c0-4816-8551-a2246eddf9bc");

        // =========================
        // SALE
        // =========================
        var sale = new Sale
        {
            SaleItems = new List<SaleItem>
            {
                // new SaleItem { ProductId = gazoz, Quantity = 3, SalePrice = 20, PriceTotal =60},
                // new SaleItem { ProductId = yogurt, Quantity = 2, SalePrice = 30 , PriceTotal =60},
                // new SaleItem { ProductId = kola, Quantity = 1, SalePrice = 40 , PriceTotal =40},
                // new SaleItem { ProductId = ayran, Quantity = 1, SalePrice = 15 , PriceTotal =15},
                // new SaleItem { ProductId = simit, Quantity = 2, SalePrice = 10, PriceTotal =20},
                // new SaleItem { ProductId = cay, Quantity = 1, SalePrice = 15 , PriceTotal =15},
                // new SaleItem { ProductId = su, Quantity = 1, SalePrice = 25, PriceTotal = 25},
                // new SaleItem { ProductId = ekmek, Quantity = 1, SalePrice = 12 , PriceTotal =12},
                // new SaleItem { ProductId = kahve, Quantity = 1, SalePrice = 50 , PriceTotal = 50},
                // new SaleItem { ProductId = kalem, Quantity = 1, SalePrice = 40 , PriceTotal = 40},
                // new SaleItem { ProductId = ulker, Quantity = 1, SalePrice = 20 , PriceTotal = 20},
                // new SaleItem { ProductId = sepet, Quantity = 2, SalePrice = 2000 , PriceTotal = 4000},
                new SaleItem { ProductId = patos, Quantity = 4, SalePrice = 10 , PriceTotal = 20},
            },
        };
        sale.TotalPrice = sale.SaleItems.Sum(x => x.SalePrice * x.Quantity);

        // =========================
        // CAMPAIGN
        // =========================
        var campaign = new Campaign
        {
            Id = Guid.Parse("b444aa63-58f7-436a-b2b5-6dbe764945e4"),
            Name = "Mega Kampanya",
            StartDate = DateTime.Now.AddDays(-1),
            EndDate = DateTime.Now.AddDays(1),
            IsActive = true,
            Promotions = new List<Promotion>
            {
                // 1️⃣ Gazoz 3 al 2 öde
                new Promotion
                {
                    Priority = 10,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.ProductQuantity,
                                    ProductId = gazoz,
                                    MinQuantity = 3
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.BuyXPayY,
                                    ProductId = gazoz,
                                    BuyQuantity = 3,
                                    PayQuantity = 2
                                }
                            }
                        }
                    }
                },

                // 2️⃣ Yoğurt %5
                new Promotion
                {
                    Priority = 9,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.ProductExists,
                                    ProductId = yogurt,
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.PercentageDiscount,
                                    ProductId = yogurt,
                                    Value = 5
                                }
                            }
                        }
                    }
                },

                // 3️⃣ Kola alana ayran bedava
                new Promotion
                {
                    Priority = 8,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.ProductQuantity,
                                    ProductId = kola,
                                    MinQuantity = 1,
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.FreeProduct,
                                    ProductId = ayran,
                                    Quantity = 1
                                }
                            }
                        }
                    }
                },

                // 4️⃣ 2 simit veya 2 poğaça → çay veya su 10 TL
                new Promotion
                {
                    Priority = 7,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            IsAggregated = true,
                            MinQuantity = 2,
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition { ProductId = simit },
                                new PromotionCondition { ProductId = pogaca }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Operator = LogicalOperator.OR,
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.FixedPrice,
                                    ProductId = cay,
                                    Value = 10
                                },
                                new PromotionBenefit
                                {
                                    Type = BenefitType.FixedPrice,
                                    ProductId = su,
                                    Value = 10
                                }
                            }
                        }
                    }
                },

                // 5️⃣ Ekmek veya kurabiye → kahve %10
                new Promotion
                {
                    Priority = 6,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Operator = LogicalOperator.OR,
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition { ProductId = ekmek, Type = ConditionType.ProductExists },
                                new PromotionCondition { ProductId = kurabiye, Type = ConditionType.ProductExists }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.PercentageDiscount,
                                    ProductId = kahve,
                                    Value = 10
                                }
                            }
                        }
                    }
                },

                // 6️⃣ Sepet 100 TL → kalem 25 TL
                new Promotion
                {
                    Priority = 5,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.CartTotal,
                                    Operator = OperatorType.GreaterOrEqual,
                                    Value = 100
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.FixedPrice,
                                    ProductId = kalem,
                                    Value = 25
                                }
                            }
                        }
                    }
                },

                // 7️⃣ Ülker gofret 15 TL
                new Promotion
                {
                    Priority = 4,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.ProductExists,
                                    ProductId = ulker
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.FixedPrice,
                                    ProductId = ulker,
                                    Value = 15
                                }
                            }
                        }
                    }
                },

                // 8️⃣ Sepet 100 TL → %10 (max 1000 TL'ye kadar)
                new Promotion
                {
                    Priority = 3,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.CartTotal,
                                    Operator = OperatorType.GreaterOrEqual,
                                    Value = 100
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.PercentageDiscount,
                                    Value = 10,
                                    MaxDiscount = 300
                                }
                            }
                        }
                    }
                },
                // 9 - patos 1 alana 1 bedava

                new Promotion
                {
                    Priority = 8,
                    MaxUsagePerCart = 1,
                    ConditionGroups = new List<PromotionConditionGroup>
                    {
                        new PromotionConditionGroup
                        {
                            Conditions = new List<PromotionCondition>
                            {
                                new PromotionCondition
                                {
                                    Type = ConditionType.ProductQuantity,
                                    ProductId = patos,
                                    MinQuantity = 2,
                                }
                            }
                        }
                    },
                    BenefitGroups = new List<PromotionBenefitGroup>
                    {
                        new PromotionBenefitGroup
                        {
                            Benefits = new List<PromotionBenefit>
                            {
                                new PromotionBenefit
                                {
                                    Type = BenefitType.FreeProduct,
                                    ProductId = patos,
                                    Quantity = 1
                                }
                            }
                        }
                    }
                },
            }
        };

        // =========================
        // ENGINE
        // =========================
        var engine = new PromotionEngine();
        engine.ApplyPromotions(sale, new List<Campaign> { campaign });

        // =========================
        // OUTPUT
        // =========================
        Console.WriteLine("==== SONUÇ ====");


        foreach (var item in sale.SaleItems)
        {
            Console.WriteLine($"Ürün: {item.ProductId}");
            Console.WriteLine($"Adet: {item.Quantity}");
            Console.WriteLine($"İndirim: {item.DiscountTotal}");
            Console.WriteLine("----------------------");
        }

        System.Console.WriteLine($"Sepet Tutari: {sale.TotalPrice}");
        Console.WriteLine($"SEPET İNDİRİMI: {sale.CartTotalDiscount}");
        Console.WriteLine($"TOPLAM İNDİRİM: {sale.DiscountTotal}");
        Console.WriteLine($"TOPLAM Odenecek Tutar: {sale.TotalPrice - sale.DiscountTotal}");
    }
}