# Xiropht-Remote-Node
Xiropht Remote Node version 0.2.3.1b compatible with Windows with Netframework 4.6 or higher and other OS who use Mono.


**In production, we suggest to compile in Release Mode for disable log files.**

[Windows]:

For compile the remote node source, you have to use Visual Studio 2013 to 2017 and support Netframework 4.6.

The Xiropht-Connector-All is required, you find the release and source here:

https://github.com/XIROPHT/Xiropht-Connector-All

For execute the remote node with arguments on a .bat file instead to follow each time instructions: 

- For start a public remote node:
Xiropht-RemoteNode.exe your_wallet_address Y 

- For start a private remote node:

Xiropht-RemoteNode.exe your_wallet_address N

[Linux]:

For compile the remote node source, you have to use Mono-Complete.
The Xiropht-Connector-All is required, you find the release and source here: 

https://github.com/XIROPHT/Xiropht-Connector-All

You can also directly execute the .exe program from a released binary with that command line once you have install mono-complete package: 

-> mono Xiropht-RemoteNode.exe

For compile the executable windows into a linux binary you should use that command: 

-> mkbundle Xiropht-RemoteNode.exe -o Xiropht-RemoteNode Xiropht-Connector-All.dll --deps -z --static

Then you will get Xiropht-Remote2 binary file if you follow this command line.

-> For execute that binary you have just to use for example: ./Xiropht-RemoteNode


[Advantage to compile the remote node into a binary]

1. You don't have to keep everytime every library like Xiropht-Connector-All.dll because the previously command for make your binary file, compile every dependencies together into a unique file.

2. Normaly the same OS version don't have to install mono-complete package for run it.

3. A better performance.

**Default port:**

-> API TCP - 18002 (Used by seed nodes if you want to be listed on the public, and by wallets).

-> API HTTP/HTTPS - 18001 (Can be used by website).

**API HTTP Command line:**

get_coin_name ->  return Xiropht

get_coin_min_name -> return XIR

get_coin_max_supply -> return max supply

get_coin_circulating -> return total coin circulating

get_coin_total_fee -> return total fee

get_coin_total_mined -> return total coin mined

get_coin_blockchain_height -> return blockchain height

get_coin_total_block_mined -> return total block mined

get_coin_total_block_left -> return total block left

get_coin_network_difficulty -> return current network difficulty

get_coin_network_hashrate -> return current network hashrate

get_coin_network_full_stats -> return all stats of the network

get_coin_block_per_id -> return a block information per id example: http://remote_node_ip:18001/get_coin_block_per_id=1

get_coin_transaction_per_id -> return a transaction information per id example: http://remote_node_ip:18001/get_coin_transaction_per_id=1

example for use a command line: http://remote_node_ip:18001/get_coin_name

**Newtonsoft.Json library is used since version 0.2.2.8b for the API HTTP/HTTPS system: https://github.com/JamesNK/Newtonsoft.Json**
