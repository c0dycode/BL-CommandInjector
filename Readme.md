# NOTE
This is a beta for now. It is possible that it causes crashes, bugs or similiar. I'm not responsible if you lose progress in your game or anything else :BabyRage:

# WARNING
If you are a Destiny 2 player and you want to use this together with "Borderlands Commander", make ABSOLUTELY sure you close Borderlands Commander BEFORE launching D2. We have received reports that Borderlands Commander running while playing D2 will result in a ban from D2.

We are trying to reach out to Bungie to hopefully get this resolved!

# About
CommandInjector is a little NamedPipe-based "API" DLL-Plugin written in C++ for Borderlands 2 and Borderlands TPS.

This is only useful to developers. This is **NOT** going to do anything on it's own.

If you want to know how to utilize it from C#, look at the Project in this repo, the "Needed Code.txt" in the Zip-File or 
you can also use/look at mopioid's [BLIO](https://github.com/mopioid/BLIO).

# Known Issues
- If you use multiple tools that utilize CommandInjector at the same time & they send commands basically at the same time, there's a chance that one of the commands is "eaten" and you'll need to wait 30 seconds for the event to timeout. I'm trying to figure out a way to fix this without making usage more difficult.

# Requirements
- [PluginLoader](https://github.com/c0dycode/BorderlandsPluginLoader)


# Current Features
- Sending Commands to the game and receive ConsoleOutput
- Get LevelTransitionData (From Map -> To Map) (Currently in Testing)

# Projects utilizing CommandInjector
- [Borderlands ReadOnly Detector](https://github.com/FromDarkHell/BorderlandsReadOnlyDetector)
- [Borderlands Chatbot](https://github.com/mopioid/Borderlands-Chatbot)
- [Borderlands Commander](https://github.com/mopioid/Borderlands-Commander)
- [Borderlands SuperHot](https://github.com/blacktavius/BLSuperHot)
- [Borderlands Twitch Integration](https://github.com/mopioid/Borderlands-Twitch-Integration)
- You tell me :P

# Changelog
* 26th August, 2018
- Fixed :tm: an issue which would cause "Borderlands Commander" feedback messages, for example, to be displayed from someone else in Coop

# Having issues?
Feel free to open up a new issue. 
If you believe CommandInjector caused the game to crash, check if you have a "CommandInjector.dmp" in your Win32-folder and if so, send it to me.

# Special thanks to
- mopioid for testing, working with and enjoying it
- FromDarkHell for testing and having issues :BabyRage:
 
# Support
If you enjoy my work and would like to support me, feel free to do so here :)

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CRVHLK9MURS9Q)
