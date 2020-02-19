namespace Xiropht_RemoteNode.Api.Object
{
    public class ClassApiTransactionObject
    {
        public long transaction_id;
        public string transaction_id_sender;
        public decimal transaction_fake_amount;
        public decimal transaction_fake_fee;
        public string transaction_id_receiver;
        public long transaction_timestamp_sended;
        public string transaction_hash;
        public long transaction_timestamp_received;
    }
}
