# Xiropht-Remote-Node
<h3>Xiropht Remote Node version 0.3.010R compatible with Windows Netframework 4.6.1 or higher or other OS like Linux that need to use Mono.</h3>

**In production, we suggest to compile source in Release Mode to disable log files and debug mode.**

<h4>Please check our wiki to get help about the remote node tool:</h4>

https://github.com/XIROPHT/Xiropht-Remote-Node/wiki

<h4>Supported Firewall on the API Ban system</h4>

-> PF [FreeBSD, OpenBSD and others BSD that support PacketFilter]

-> iptables [Linux usually]

-> Windows Firewall (automaticaly used once the tool is launched on Windows)

<h3>Installation Instructions</h3>

On Linux OS (Work also Raspbian OS for Raspberry):

- sudo wget https://github.com/XIROPHT/Xiropht-Remote-Node/releases/download/0.3.0.1R/Xiropht-RemoteNode-0.3.0.1R-Linux-64bit.zip

or:

- sudo wget https://github.com/XIROPHT/Xiropht-Remote-Node/releases/download/0.3.0.1R/Xiropht-RemoteNode-0.3.0.1R-Raspberry.zip

- sudo unzip Xiropht-RemoteNode-0.3.0.1R-Linux-64bit.zip

or:

- sudo unzip Xiropht-RemoteNode-0.3.0.1R-Raspberry.zip

- sudo chmod 0777 Xiropht-RemoteNode-Ubuntu-18.04-x64 or Xiropht-RemoteNode-Raspberry

- sudo ./Xiropht-RemoteNode-Ubuntu-18.04-x64 or sudo ./Xiropht-RemoteNode-Raspberry

**Newtonsoft.Json library is used since version 0.2.2.8b for the API HTTP/HTTPS system: https://github.com/JamesNK/Newtonsoft.Json**

**Developers:**

- Xiropht (Sam Segura)
