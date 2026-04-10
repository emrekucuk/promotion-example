
namespace CampaignEngine
{
    public class Sale
    {
        public Guid Id { get; set; }

        public double DiscountTotal { get; set; }
        public double CartDiscountTotal { get; set; }

        public List<SaleItem> SaleItems { get; set; } = new();
        public double GrandTotal { get; internal set; }
    }

    public class SaleItem
    {
        public Guid ProductId { get; set; }
        public Guid ProductVersionId { get; set; }
        public double Quantity { get; set; }
        public double TotalTax { get; set; }
        public double TotalPrice { get; set; } // Örn: 150 3 al 2 öde 
        public double GrandTotal { get; set; } //100
        public double DiscountTotal { get; set; }
        public double SalePrice { get; set; }
        public double TaxRate { get; set; }
        public string ProductName { get; set; }
        public int Index { get; set; }
        public BenefitType BenefitType { get; set; }
        public bool HasDiscount { get; set; }
        public double NonEligableQuantity { get; set; }
        public double DiscountTotalNonEligable { get; set; }
        public string ExtraPromotionName { get; internal set; }
    }

    // =========================
    // CAMPAIGN (Container)
    // =========================
    /// <summary>
    /// Kampanya ana container’ı. Bir kampanya bir veya birden fazla promotion içerir.
    /// </summary>
    public class Campaign
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Kampanya adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kampanya açıklaması
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kampanya başlangıç tarihi
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Kampanya bitiş tarihi
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Kampanya aktif mi pasif mi
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Kampanya içindeki tüm promotionlar
        /// </summary>
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
    }

    // =========================
    // PROMOTION (Execution Unit)
    // =========================
    /// <summary>
    /// Kampanya içindeki uygulama birimi. 
    /// Örn: indirim, hediye, 3 al 2 öde gibi kurallar burada tanımlanır.
    /// </summary>

    public class Promotion
    {
        public Guid Id { get; set; }

        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; } = 0;
        public int? MaxUsagePerCart { get; set; }

        public List<PromotionTarget> Targets { get; set; } = new List<PromotionTarget>();

        // ❌ ESKİ
        // public List<PromotionCondition> Conditions { get; set; }

        // ❌ ESKİ
        // public List<PromotionBenefit> Benefits { get; set; }

        // ✅ YENİ
        public List<PromotionConditionGroup> ConditionGroups { get; set; } = new List<PromotionConditionGroup>();

        public List<PromotionBenefitGroup> BenefitGroups { get; set; } = new List<PromotionBenefitGroup>();
    }

    public class PromotionConditionGroup
    {
        public Guid Id { get; set; }

        public Guid PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        /// <summary>
        /// Group içindeki conditionlar nasıl bağlanacak
        /// </summary>
        public LogicalOperator Operator { get; set; } = LogicalOperator.AND;

        /// <summary>
        /// Ürünler birlikte sayılacak mı (🔥 kritik)
        /// </summary>
        public bool IsAggregated { get; set; } = false;

        /// <summary>
        /// Group için min toplam adet (aggregation için)
        /// </summary>
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Group için max toplam adet
        /// </summary>
        public int? MaxQuantity { get; set; }

        public List<PromotionCondition> Conditions { get; set; } = new List<PromotionCondition>();
    }
    // =========================
    // TARGET (Scope)
    // =========================
    /// <summary>
    /// Promotion hedefi (ürün, kategori veya sepet)
    /// </summary>
    public class PromotionTarget
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Hangi promotion için
        /// </summary>
        public Guid PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        /// <summary>
        /// Hedef türü (ürün, kategori, sepet)
        /// </summary>
        public TargetType TargetType { get; set; }

        /// <summary>
        /// Hedef ürün (varsa)
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Hedef kategori (varsa)
        /// </summary>
        public Guid? CategoryId { get; set; }
    }

    // =========================
    // CONDITION (IF)
    // =========================
    /// <summary>
    /// Promotion için koşullar. Örn: ürün adedi, sepet toplamı vb.
    /// </summary>
    public class PromotionCondition
    {
        public Guid Id { get; set; }

        public Guid ConditionGroupId { get; set; }
        public PromotionConditionGroup ConditionGroup { get; set; }

        public ConditionType Type { get; set; }

        public Guid? ProductId { get; set; }

        public double? Value { get; set; }

        public OperatorType? Operator { get; set; }

        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
    }

    public class PromotionBenefitGroup
    {
        public Guid Id { get; set; }

        public Guid PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        /// <summary>
        /// Benefitler AND mi OR mu
        /// </summary>
        public LogicalOperator Operator { get; set; } = LogicalOperator.OR;

        /// <summary>
        /// Kullanıcı mı seçer sistem mi
        /// </summary>
        public BenefitSelectionType SelectionType { get; set; } = BenefitSelectionType.Automatic;

        public List<PromotionBenefit> Benefits { get; set; } = new List<PromotionBenefit>();
    }

    // =========================
    // BENEFIT (THEN)
    // =========================
    /// <summary>
    /// Promotion sonrası uygulanacak faydalar / indirimler
    /// </summary>
    public class PromotionBenefit
    {
        public Guid Id { get; set; }

        public Guid BenefitGroupId { get; set; }
        public PromotionBenefitGroup BenefitGroup { get; set; }

        public BenefitType Type { get; set; }

        public double? Value { get; set; }

        public double? MaxDiscount { get; set; }

        public double? MaxApplicableAmount { get; set; }

        public Guid? ProductId { get; set; }

        public int? Quantity { get; set; }

        public int? BuyQuantity { get; set; }
        public int? PayQuantity { get; set; }
    }

    // =========================
    // ENUMS
    // =========================

    /// <summary>
    /// Promotion hedef türleri
    /// </summary>
    public enum TargetType
    {
        Product = 1,    // Tek bir ürün
        Category = 2,   // Kategori bazlı
        Cart = 3        // Sepet bazlı
    }

    /// <summary>
    /// Promotion koşul türleri
    /// </summary>
    public enum ConditionType
    {
        ProductQuantity = 1,  // Ürün adedi
        ProductExists = 2,    // Ürün sepette var mı
        CartTotal = 3         // Sepet toplamı
    }
    public enum LogicalOperator
    {
        AND = 1,
        OR = 2
    }

    public enum BenefitSelectionType
    {
        Automatic = 1,   // sistem seçer
        UserSelectable = 2 // kullanıcı seçer
    }

    /// <summary>
    /// Koşul karşılaştırma operatörleri
    /// </summary>
    public enum OperatorType
    {
        GreaterThan = 1,      // >
        GreaterOrEqual = 2,   // >=
        LessThan = 3,         // <
        LessOrEqual = 4,      // <=
        Equal = 5             // =
    }

    /// <summary>
    /// Promotion fayda türleri
    /// </summary>
    public enum BenefitType
    {
        None = 0,
        PercentageDiscount = 1, // Yüzdelik indirim
        FixedAmountDiscount = 2, // Sabit tutar indirim
        FixedPrice = 3,         // Sabit fiyat uygulaması
        FreeProduct = 4,        // Ücretsiz ürün
        BuyXPayY = 5,           // 3 al 2 öde vb.
        BuyXGetNthPercentOff = 6,  // X al, N. ürüne % indirim
        BuyXGetNthAmountOff = 7    // X al, N. ürüne sabit tutar indirim
    }
}