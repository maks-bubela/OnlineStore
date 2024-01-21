namespace OnlineStore.Models
{
    public class ProductsInfoModel
    {
        /// <summary>
        /// Id of products.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Name of products.
        /// </summary>
        public string ProductName { set; get; }
        /// <summary>
        /// Quantity of products.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Description of products.
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        /// Quantity of products.
        /// </summary>
        public long Quantity { get; set; }
    }
}
