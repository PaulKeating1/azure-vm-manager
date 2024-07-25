namespace AzureVmManager.DataObjects
{
    public class Image
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required string Publisher { get; set; }
        public required string Offer { get; set; }
        public required string Sku { get; set; }
    }
}
