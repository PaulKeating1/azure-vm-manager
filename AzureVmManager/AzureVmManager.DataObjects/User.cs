namespace AzureVmManager.DataObjects
{
    public class User
    {
        public required string ObjectId { get; set; }
        public required string DisplayName { get; set; }
        public required string GivenName { get; set; }
        public required string Surname { get; set; }
    }
}
