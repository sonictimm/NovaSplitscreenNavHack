# NovaSplitscreenNavHack
Hacking the Nova UI System for Unity to make it work with 4 Players


I am working on a Unity game with Nova UI as my UI Framework.
My game is a 4-Player Splitscreen game.  Some parts of the game require each player to control their own UI menus.  
Nova has an excellent Focus & Navigation system, but it doesn't seem to support multiple focuses.

So I made a Class, NovaNavHacker, and an Interface, ISplitscreenPlayerFocusable, to address this.

It's clunky, but it retains almost full functionality, while allowing me to configure buttons to be navigable by Only Player 1, 2, 3, or 4.
Each player can have their own focus, press their own buttons, etc.

The only drawback (aside from convoluted setup) is that Auto Navigation doesn't work-- You have to manually set each nav target.

Using this in my game DRIFT BOOM BOOM.  Uploading it now before it gets messy.  Works well right now.

I do not plan to update it, but please ask me if you have questions about how to implement it.
I might make an example project if enough people ask, but it would require Nova and Rewired to work. I will not redistribute them.

**NovaNavHacker** manages the navigation and focus for each player
**ISpitscreenPlayerFocusable** Is an interface that must be implemented on anything that you want to be navigable by a single player (but not by any player).

You may Set a player's Nav Focus to null to disable navigation.

**NovaNavInput_4Players** is where the inputs are channeled, using the Rewired Input System for Unity.


Nova seems like an excellent package, but I was a bit miffed that there's no 4-player UI input out of the box.
And since the source code is not available, I could not modify it... hence this ugly hack.


Nova UI System: https://novaui.io/index.html
https://assetstore.unity.com/packages/tools/gui/nova-226304

Rewired Input System: https://guavaman.com/projects/rewired/
https://assetstore.unity.com/packages/tools/utilities/rewired-21676?aid=1011l3LkF&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker
