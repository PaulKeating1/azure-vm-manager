namespace AzureVmManager.DataObjects
{
    public class ResourceGroup
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
    }
}
