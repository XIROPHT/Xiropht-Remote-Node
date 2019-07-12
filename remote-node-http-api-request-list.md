<h2>Overview</h2>

For the http/https api on your remote node, you have to enable the system if it's not case this is inside the setting file.

**In version earlier than 0.2.8.1R:**


Open the **config.ini** file and edit the line **ENABLE_API_HTTP** by:

~~~text
ENABLE_API_HTTP=Y
~~~ 

**In version equal or more than 0.2.8.1R:**


Open the **config.json** file and edit the following line containing **"enable_api_http"** by:

~~~text
"enable_api_http": true,
~~~ 



<h2>Default port:</h2>

**API HTTP:** 18001 (Can be used by website or other apps who use http protocols).

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


<h3>Example:</h3>

Get full network stats: ``` https://api.xiropht.com/get_coin_network_full_stats```

Result: 

```
{"coin_name":"Xiropht","coin_min_name":"XIRO","coin_max_supply":"26000004,41048858","coin_circulating":"364172.08874191","coin_total_fee":"397.91125809","coin_total_mined":"364580.00000000","coin_blockchain_height":"36459","coin_total_block_mined":"36458","coin_total_block_left":"2563542","coin_network_difficulty":"578034378","coin_network_hashrate":"693641253.6","coin_total_transaction":"133136","version":"0.2.8.1"}
```

