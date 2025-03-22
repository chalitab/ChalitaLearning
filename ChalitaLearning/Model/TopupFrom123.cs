using Newtonsoft.Json;
using System.Text.Json;

namespace ChalitaLearning.Model
{
    public class TopupFrom123Requset
    {
        [JsonProperty("time_stamp")]
        public string TimeStamp { get; set; }

        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }

        [JsonProperty("merchant_reference")]
        public string MerchantReference { get; set; }

        [JsonProperty("payment_code")]
        public string PaymentCode { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("paid_amount")]
        public decimal? PaidAmount { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("created_date_time")]
        public string CreatedDateTime { get; set; }

        [JsonProperty("completed_date_time")]
        public string CompletedDateTime { get; set; }

        [JsonProperty("agent_code")]
        public string AgentCode { get; set; }

        [JsonProperty("channel_code")]
        public string ChannelCode { get; set; }

        [JsonProperty("transaction_status")]
        public string TransactionStatus { get; set; }

        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }

        [JsonProperty("merchant_data")]
        public string MerchantData { get; set; }

        [JsonProperty("bank_slip_url")]
        public string BankSlipUrl { get; set; }

        [JsonProperty("bank_slip_image")]
        public string BankSlipImage { get; set; }

        [JsonProperty("agent_txn_ref")]
        public string AgentTxnRef { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }
    }

    public class TopupFrom123Response() {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
