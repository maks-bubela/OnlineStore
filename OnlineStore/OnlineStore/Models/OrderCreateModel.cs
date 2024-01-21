using OnlineStore.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore.Models
{
    public class OrderCreateModel
    {
        /// <summary>
        /// Id of products.
        /// </summary>
        public long ProductsId { get; set; }
        /// <summary>
        /// Quantity of products.
        /// </summary>
        public long QuantityProducts { get; set; }
        /// <summary>
        /// Payment type.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentType PaymentTypeEnum { get; set; }
        /// <summary>
        /// Delivery type.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeliveryType DeliveryTypeEnum { get; set; }
    }
}
