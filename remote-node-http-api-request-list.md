<h2>Overview</h2>

For the http/https api on your remote node, you have to enable the system if it's not the case this is inside the settings file.

**In version earlier than 0.2.8.1R:**


Open the **config.ini** file and edit the line **ENABLE_API_HTTP**:

~~~text
ENABLE_API_HTTP=Y
~~~ 

**In version equal or more than 0.2.8.1R:**


Open the **config.json** file and edit the following line containing **"enable_api_http"**:

~~~text
"enable_api_http": true,
~~~ 



<h2>Default port:</h2>

**API HTTP:** 18001 (Can be used by website or other apps that use http protocols).

<h2>API Command line:</h2>

| Command | Description |
| ------- | -------- |
| /get_coin_name |  return Xiropht |
| /get_coin_min_name |  return XIR |
| /get_coin_max_supply |  return max supply |
| /get_coin_circulating |  return total coin circulating |
| /get_coin_total_fee |  return total fee |
| /get_coin_total_mined |  return total coin mined |
| /get_coin_blockchain_height |  return blockchain height |
| /get_coin_total_block_mined |  return total block mined |
| /get_coin_total_block_left |  return total block left |
| /get_coin_network_difficulty |  return current network difficulty |
| /get_coin_network_hashrate |  return current network hashrate |
| /get_coin_network_full_stats |  return all stats of the network |
| /get_coin_block_per_id |  return a block information from a block id, for example: http://127.0.0.1:18001/get_coin_block_per_id=1 |
| /get_coin_block_per_hash |  return a block information from a block hash selected, for example: http://127.0.0.1:18001/get_coin_block_per_hash=hash_selected |
| /get_coin_transaction_per_id |  return a transaction information per a transaction id example: http://127.0.0.1:18001/get_coin_transaction_per_id=1 |
| /get_coin_transaction_per_hash |  return a transaction information per a transaction hash, for example: http://127.0.0.1:18001/get_coin_transaction_per_hash=hash_selected |
| /get_last_blocktemplate | return the last blocktemplate |

<h2>Responses informations: </h2>

__On version earlier than **0.2.8.2R**:__

__Every json responses sent by the API, returns every value in the string type.__

-------------------------------------------------------------------------------------------------------------------

On version equal or higher than **0.2.8.2R**:

<h3>1. /get_coin_name </h3>

Return the coin name.

| Field | type | Description |
| ----- | ---- | ----------- |
| result | string | Return the coin name. |
| version | string | Return the version of the remote node tool . |

-------------------------------------------------------------------------------------------------------------------

<h3>2. /get_coin_min_name </h3>

Return the ticker name of the coin.

| Field | type | Description |
| ----- | ---- | ----------- |
| result | string | Return the ticker name of the coin. |
| version | string | Return the version of the remote node tool . |

-------------------------------------------------------------------------------------------------------------------

<h3>3. /get_coin_max_supply</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | double | Return the Max Supply of the coin. |

-------------------------------------------------------------------------------------------------------------------

<h3>4. /get_coin_circulating</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | double | Return the current amount of coin circulating. |

-------------------------------------------------------------------------------------------------------------------

<h3>5. /get_coin_total_fee</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | double | Return the current amount of fee accumulated. |

-------------------------------------------------------------------------------------------------------------------

<h3>6. /get_coin_total_mined</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | double | Return the current amount of coin mined. |

-------------------------------------------------------------------------------------------------------------------

<h3>7. /get_coin_blockchain_height</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | long | Return the current blockchain height. |

-------------------------------------------------------------------------------------------------------------------

<h3>8. /get_coin_total_block_mined</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | long | Return the current amount of blocks mined. |

-------------------------------------------------------------------------------------------------------------------

<h3>9. /get_coin_total_block_left</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | long | Return the current amount of blocks left to mining. |

-------------------------------------------------------------------------------------------------------------------

<h3>10. /get_coin_network_difficulty</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | double | Return the current network difficulty. |

-------------------------------------------------------------------------------------------------------------------

<h3>11. /get_coin_network_hashrate</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| result | double | Return the current network hashrate. |

-------------------------------------------------------------------------------------------------------------------

<h3>12. /get_coin_network_full_stats</h3>

| Field | type | Description |
| ----- | ---- | ----------- |
| coin_name | string | Return the name of the coin. |
| coin_min_name | string | Return the ticker name of the coin. |
| coin_max_supply | double | Return the max supply of the coin. |
| coin_circulating | double | Return the current amount of coin circulating. |
| coin_total_fee | double | Return the current amount of accumulated fee. |
| coin_total_mined | double | Return the current amount of coin mined. |
| coin_blockchain_height | long | Return the current blockchain height. |
| coin_total_block_mined | long | Return the current amount of blocks mined. |
| coin_total_block_left | long | Return the current amount of blocks left to mining. |
| coin_network_difficulty | double | Return the current network difficulty. |
| coin_network_hashrate | double | Return the current network hashrate. |
| coin_total_transaction | long | Return the current total transaction synced. |

-------------------------------------------------------------------------------------------------------------------

<h3>13. /get_coin_block_per_id</h3>

Return a block information from a block id, for example: http://127.0.0.1:18001/get_coin_block_per_id=1

| Field | type | Description |
| ----- | ---- | ----------- |
| block_id | long | Return the block id. |
| block_hash | string | Return the block hash. |
| block_transaction_hash | string | Return the block transaction hash. |
| block_timestamp_create | long | Return the timestamp create of the block. |
| block_timestamp_found | long | Return the timestamp found of the block. |
| block_difficulty | double | Return the block difficulty. |
| block_reward | double | Return the block reward. |

<h4>14. /get_coin_block_per_hash</h4>

Return a block information from a block hash, for example: http://127.0.0.1:18001/get_coin_block_per_hash=block_hash

| Field | type | Description |
| ----- | ---- | ----------- |
| block_id | long | Return the block id. |
| block_hash | string | Return the block hash. |
| block_transaction_hash | string | Return the block transaction hash. |
| block_timestamp_create | long | Return the timestamp create of the block. |
| block_timestamp_found | long | Return the timestamp found of the block. |
| block_difficulty | double | Return the block difficulty. |
| block_reward | double | Return the block reward. |

<h4>15. /get_coin_transaction_per_id</h4>

Return a transaction information per a transaction id example: http://127.0.0.1:18001/get_coin_transaction_per_id=1 

| Field | type | Description |
| ----- | ---- | ----------- |
| transaction_id | long | Return the block id. |
| transaction_id_sender | string | Return the unique of the sender. |
| transaction_fake_amount | double | Return the fake amount of transaction. |
| transaction_fake_fee | double | Return the fake fee of transaction. |
| transaction_id_receiver | string | Return the unique of the receiver. |
| transaction_timestamp_sended | long | Return the timestamp of sending of the transaction. |
| transaction_hash | string | Return the transaction hash. |
| transaction_timestamp_received | long | Return the timestamp of receive of the transaction. |

**Note:**

the result of **transaction_id_sender** can be different of a number, please follow the list of specific tag of transaction:

List of specific tag of transaction:

~~~text
m = blockchain 
f = dev fee
r = remote node fee 
~~~

<h4>16. /get_coin_transaction_per_hash</h4>

Return a transaction information per a transaction hash example: http://127.0.0.1:18001/get_coin_transaction_per_hash=transaction_hash 

| Field | type | Description |
| ----- | ---- | ----------- |
| transaction_id | long | Return the transaction id. |
| transaction_id_sender | string | Return the unique of the sender. |
| transaction_fake_amount | double | Return the fake amount of transaction. |
| transaction_fake_fee | double | Return the fake fee of transaction. |
| transaction_id_receiver | string | Return the unique of the receiver. |
| transaction_timestamp_sended | long | Return the timestamp of sending of the transaction. |
| transaction_hash | string | Return the transaction hash. |
| transaction_timestamp_received | long | Return the timestamp of receive of the transaction. |

**Note:**

the result of **transaction_id_sender** can be different of a number, please follow the list of the specific tag of the transaction:

List of specific tag of transaction:

~~~text
m = blockchain 
f = dev fee
r = remote node fee 
~~~

<h4>17. /get_last_blocktemplate</h4>

| Field | type | Description |
| ----- | ---- | ----------- |
| block_id | long | return the block id. |
| block_hash | string | return the block hash. |
| block_algorithm | string | return the algorithm. |
| block_size | int | return the block size. |
| block_method | string | return the mining method of the block. |
| block_job | string | return the range of the block. |
| block_min_range | double | return the min range of the block. |
| block_max_range | double | return the max range of the block. |
| block_reward | double | return the block reward of the block. |
| block_difficulty | double | return the block difficulty. |
| block_network_hashrate | double | return the network hashrate. |
| block_timestamp_create | long | return the timestamp create of the block. |
| block_hash_indication | string | return the block hash indication. |
| block_lifetime | int | return the block lifetime. |

