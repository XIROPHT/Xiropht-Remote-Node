# Xiropht-Remote-Node
<h3>Xiropht Remote Node version 0.2.6.8R compatible with Windows with Netframework 4.6.1 or higher or other OS like Linux who need to use Mono.</h3>

**In production, we suggest to compile source in Release Mode to disable log files and debug mode.**

<h4>Please check our wiki for get help about the remote node tool:</h4>

https://github.com/XIROPHT/Xiropht-Remote-Node/wiki

<h4>Supported Firewall on the API Ban system</h4>

-> PF [FreeBSD, OpenBSD and others BSD who support PacketFilter]

-> iptables [Linux usually]

-> Windows Firewall (Automaticaly used once the tool is launched on Windows)

<h3>Installation Instructions</h3>

On Linux OS (Work also Raspbian OS for Raspberry):

- sudo wget https://github.com/XIROPHT/Xiropht-Remote-Node/releases/download/0.2.6.8R/Xiropht-RemoteNode-0.2.6.8R-Linux.zip 

or:

- sudo wget https://github.com/XIROPHT/Xiropht-Remote-Node/releases/download/0.2.6.8R/Xiropht-RemoteNode-0.2.6.8R-Raspberry.zip

- sudo unzip Xiropht-RemoteNode-0.2.6.8R-Linux.zip

or:

- sudo unzip Xiropht-RemoteNode-0.2.6.8R-Raspberry.zip

- sudo chmod 0777 Xiropht-RemoteNode-Linux or Xiropht-RemoteNode-Raspberry

- sudo ./Xiropht-RemoteNode-Linux or sudo ./Xiropht-RemoteNode-Raspberry

**Newtonsoft.Json library is used since version 0.2.2.8b for the API HTTP/HTTPS system: https://github.com/JamesNK/Newtonsoft.Json**

**Developers:**

- Xiropht (Sam Segura)
