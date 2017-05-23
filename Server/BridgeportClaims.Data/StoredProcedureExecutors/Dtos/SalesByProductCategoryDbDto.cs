namespace BridgeportClaims.Data.StoredProcedureExecutors.Dtos
{
    public class SalesByProductCategoryDbDto
    {
        public virtual string ProductName { get; set; }
        public virtual string OrderDate { get; set; }
        public virtual int OrderYear { get; set; }
        public virtual decimal GrandTotalSold { get; set; }
        public virtual int NumberOfProductsSold { get; set; }
        public virtual string DateRangeType { get; set; }
        public virtual int RowOrderID { get; set; }
    }
}
