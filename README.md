# Xiropht-Remote-Node
Xiropht Remote Node version 0.2.2.7b compatible with Windows with Netframework 4.6 and other OS who use Mono.


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

For execute your binary compiled with arguments:

- For start a public remote node:

-> ./Xiropht-RemoteNode your_wallet_address Y

- For start a private remote node:

-> ./Xiropht-RemoteNode your_wallet_address N

For execute the remote node without to compile it with arguments:

- For start a public remote node:

-> mono Xiropht-RemoteNode.exe your_wallet_address Y

- For start a private remote node:

-> mono Xiropht-RemoteNode.exe your_wallet_address N

[Advantage to compile the remote node into a binary]

1. You don't have to keep everytime every library like Xiropht-Connector-All.dll because the previously command for make your binary file, compile every dependencies together into a unique file.

2. Normaly the same OS version don't have to install mono-complete package for run it.

3. A better performance.
