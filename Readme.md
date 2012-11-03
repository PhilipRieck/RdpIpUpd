RDPIPUPD - The remote desktop file IP address updater
=====================================================

Monitors your dynamic IP address and updates an RDP file with it.


Wait, what use is that?
-----------------------

To me, it's *very* useful.  I have a RDP file stored in my [dropbox](http://dropbox.com) folder that 
easily allows me to connect to my work machine with all the proper options set.   Only one problem; My work 
machine gets random IP addresses from DHCP!.   Enter the RDPIPUpd.

I have it set to run as a startup program, and point it at the RDP file in dropbox.  Now from any other machine
that has dropbox on it I can doubleclick to connect.  Combine it with VPN and it's a no-hassle virtual presence


Usage
-----

1. Create an RDP file by saving a connection with the remote desktop application.  
   Much more useful if this file is on a share or in dropbox or skydrive, or the like.
2. Run RdpIpUpd
3. Click the tray icon and choose _Configure_
4. Point it at your RDP file, and save.
5. For extra usefulnes, set RdpIpUpd to run at startup.

Enjoy.


The Code
--------

I wanted to look at [Caliburn.Micro](http://caliburnmicro.codeplex.com/) one afternoon, and decided to use it to write a utility.  This is the result.  My only constraint was that I wanted to make sure Caliburn.Micro didn't enforce a WPF shell or some IoC constraints, so this seemed like a good trial.


#License Stuff
Only because I have been told ( somewhat harshly ) that "dumping code into github isn't providing a license".  Sorry.  

This work is "AS IS" and without warranty of any kind, licensed under the [Creative Commons CC0 License](http://creativecommons.org/publicdomain/zero/1.0/)
