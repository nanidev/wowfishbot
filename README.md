<div align="center">

# wowfishbot

<p align="center">WoW Fish Bot is a configurable Windows desktop automation tool for World of Warcraft fishing. It combines sound-triggered catch detection, bobber recognition, and customizable timing controls to help streamline fishing in both classic and newer WoW versions. The app includes Interact With Target mode for modern clients, bobber pattern calibration, optional bobber validation, selectable search areas, pixel tolerance settings, custom cast and click inputs, and saved settings for repeat use. It also supports catch sound recording, built-in logs, optional UI hide/show hotkeys, and an emergency stop with ESC, making it a practical and easy-to-configure fishing assistant for players who want a reliable automated workflow.
</p>

[![](https://dcbadge.limes.pink/api/server/b94AzAJg8p)](https://discord.gg/b94AzAJg8p)

[![Stars](https://img.shields.io/github/stars/nanidev/wowfishbot?style=flat-square)](https://github.com/nanidev/wowfishbot/stargazers) [![Forks](https://img.shields.io/github/forks/nanidev/wowfishbot?style=flat-square)](https://github.com/nanidev/wowfishbot/network) [![Issues](https://img.shields.io/github/issues/nanidev/wowfishbot?style=flat-square)](https://github.com/nanidev/wowfishbot/issues) [![Watchers](https://img.shields.io/github/watchers/nanidev/wowfishbot?style=flat-square)](https://github.com/nanidev/wowfishbot/watchers) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](https://opensource.org/licenses/MIT)

![C#](https://img.shields.io/badge/-C%23-555?style=flat-square&logo=c%23)

[🐛 Report Bug](https://github.com/nanidev/wowfishbot/issues) · [✨ Request Feature](https://github.com/nanidev/wowfishbot/issues)


### [❤️ Donate](https://ko-fi.com/nanidev)

</div>

---

⚠️ IMPORTANT: This project, WowfishBot, is developed strictly for educational and research purposes. It is not intended for commercial use, and the user assumes all risk when utilizing it. Please use it responsibly and ethically.

## 📋 Table of Contents

- [✨ Features](#features)
- [📝 Latest Release](#latest-release)
- [✅ Supported World of Warcraft Clients](#Supported-World-of-Warcraft-Clients)
- [🚀 Installation](#installation)
- [💻 Preview](#preview)
- [📄 License](#license)


## ✨ Features

- ✅ **Sound-triggered automation**
- ✅ **Support for newer WoW versions**
- ✅ **Classic mode support**
- ✅ **Interact With Target mode**
- ✅ **Configurable cast, click, and performance delays**
- ✅ **Bobber pattern calibration**
- ✅ **Optional bobber validation**
- ✅ **Selectable bobber search area**
- ✅ **Pixel tolerance and match settings**
- ✅ **Catch sound recording and playback**
- ✅ **Window selection and process filtering**
- ✅ **Custom cast key and click button support**
- ✅ **Optional UI hide/show hotkey**
- ✅ **Save and load settings**
- ✅ **Built-in logs and status display**
- ✅ **Emergency stop with `ESC`**


## 📝 Latest Release

### `v0.2` — Newer WoW Client Version Support

This update adds support for newer WoW clients through **Interact With Target** mode.

#### What changed
- Added **Interact With Target** mode for newer WoW versions.
- The configured cast key is used to cast and then used again after the catch sound to reel in.
- Bobber scanning and mouse clicking are skipped in this mode.
- Fixed a startup crash caused by a missing `Performance Delay` control.
- Improved build reliability by updating generated designer files for nullable context handling.

#### Important
- Classic fishing mode still works as before.
- If using a newer WoW client, enable **Interact With Target** in the app settings.

For the full history, see the [GitHub Releases](https://github.com/nanidev/wowfishbot/releases) page.

## ✅ Supported World of Warcraft Clients

- ✅ **World of Warcraft 1.12.1** — Vanilla wow (Tested and working)
- ⬜ **World of Warcraft 2.4.3** — The Burning Crusade
- ⬜ **World of Warcraft 3.3.5a** — Wrath of the Lich King
- ⬜ **World of Warcraft 4.3.4** — Cataclysm
- ⬜ **World of Warcraft 5.4.8** — Mists of Pandaria
- ⬜ **World of Warcraft 6.2.4** — Warlords of Draenor
- ⬜ **World of Warcraft 7.3.5** — Legion
- ⬜ **World of Warcraft 8.3.7** — Battle for Azeroth
- ⬜ **World of Warcraft 9.2.7** — Shadowlands
- ⬜ **World of Warcraft 10.2.7** — Dragonflight
- ⬜ **World of Warcraft 11.x** — The War Within / current retail builds

### Help Test Other Clients
Only **1.12.1** has been confirmed so far. If you test the bot on any other client version, your feedback helps improve compatibility and expand support for more World of Warcraft versions.

## 🚀 Installation

1. Prerequisites:      
       - Windows that can run .NET 9 (Some already have pre-installed)     
       - .NET 9 (https://dotnet.microsoft.com/en-us/download/dotnet/9.0)            
2. Installation:         
       - Download and unzip release files.          
       - Run wowfishbot.exe to start the program 
  
## 💻 Preview
![Applicaion preview](images/preview.png)



## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

---

[![Donate](images/supp.png)](https://ko-fi.com/nanidev)

<div align="center">Made with ❤️ by nanidev</div>
