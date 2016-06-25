## X.UI framework for Universal Apps

Quickly build rich UI's in your Universal Application .. 


[IMPORTANT : UAP SQLITE has a bug ...  (SQLite.UAP.2015, Version=3.10.2)   for x64 fails to pass thru the 64 bit dll's. RUN the application as an x86 platform app for the time being till the extension is resolved]


Components still being fleshed out...

* LiteTab
* RichButton
* Path
* RichInput
* EffectLayer
* Chrome
* RichTab
* OneBox
* RichScrollViewer
* LiteDataGrid - A grid that lets you define several different DataTemplates and then alternate between them with a simple index value.
* Viewer.* - Content is rendered by these viewers.
* Toolbar
* UserCard
* ColorPicker

The following components are less generic, possibly designed with the intention of modularizing the monolithic X.Browser app.

* AddTab
* MoreTab
* Extensions.*
* OneBox
* ViewModels

The following components are reused across one or more components

* CoreLib
* Win32
* Services.*
* Extensions.*


And implementing them in a demo app "X.Browser" .. Which is a demonstration browser application, that lets you browse web pages, media content, filesystem... 

![X.Browser 001](https://pbs.twimg.com/media/Cc3G7ufUUAQS6qK.jpg:large)


- Below image shows how each tab can be "pinned" to the start menu and "docked" in the tab list on the left (like Edge)

![lastrefreshed](https://pbs.twimg.com/media/CegWtACWEAABrgZ.jpg:large)


- Below video shows the startup time of a .NET Native compiled x64 version of the X.Browser app

[![.NET Native startup time](http://img.youtube.com/vi/j_8Bx6TEX4w/0.jpg)](https://www.youtube.com/watch?v=j_8Bx6TEX4w ".NET Native startup time")


- Below video shows how the extensions model works in the sample. Preparing for using the new extensions feature of the "aniversary" uwp application model

[![Third-party Extensions model](http://img.youtube.com/vi/PP1sNBbdQx8/0.jpg)](https://www.youtube.com/watch?v=PP1sNBbdQx8 "Third-party Extensions model")

- Below video shows the SketchFlow app(viewer), will be used to create extensions/plugins all in browser (like the F12 developer tool but for designers/developers)

[![SketchFlow Viewer](http://img.youtube.com/vi/9-cE2lF04cQ/0.jpg)](https://www.youtube.com/watch?v=9-cE2lF04cQ "Third-party Extensions model")

- Below shows another sample HOST UWP app that loads 2 sample extensions Flickr/Twitter

![lastrefreshed](https://pbs.twimg.com/media/ClwZkfFUoAAvSP6.jpg:large)

- Below shows the X.Browser Host UWP loading the same two extensions as X.Player (Twitter/Flickr)

![lastrefreshed](https://pbs.twimg.com/media/Clwm-snUoAAmvBz.jpg:large)
