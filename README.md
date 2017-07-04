Jarloo.Sojurn
=============

![alt tag](/images/mainWindow2.png)

About Sojurn
------------

Sojurn keeps track of what TV shows you've watched, and which ones you haven't. It features a backlog so you know what episodes you haven't watched yet, and a timeline that lets you see when your favorite shows will air next. 

You can explore your favorite shows, browsing by season and episode.

It's simple to add new shows or remove ones you don't watch anymore.

To add a show just click the + icon in the top left and a new window appears where you can enter the name of the show and search. Just click the show from the search window and Sojurn adds it to your list of shows, downloads all the images, figures out when it will be on next and much more!

You can view all the seasons and episdoes of a show just by clicking on it.

![alt tag](/images/episodeWindow.png)

And Sojurn makes it easy to mark an episode as watched or unwatched. See that little eye icon above a show? Click that and it marks it as watched. Click again and it marks it as unwatched. The episodes that are the dimmed in the image above have been watched already. 

Don't code but want to use this?
--------------------------------

Click on releases on GitHub and download the latest release. All you need to do is unzip the file and put in in a folder somewhere you want, then run Jarloo.Sojurn.exe. 

Sojurn will create a two new folders. A "Data" folder and an "Images" folder.

The images holds images Sojurn downloads for show titles and episodes. 

The "Data" folder is where your list of shows and the status of what shows you've watched is stored. They are stored in a file called index.json. You will notice there are other JSON files there, these are just backups of your index.json file, stored there in the event something goes wrong, so you can go back to a specific point in time.

Backups
-------

If you want to back up your history, such as shows added and thier viewed status, all you need to do is make a copy of the index.json file in your Data directory. 

Then if you install Sojurn on a new machine just copy and paste you index.json file into the Data folder and Sojurn will be completely updated from when you left it. It will download all the images for the shows and when you click on a show it will also download the images for the episodes.

Keep in mind to get a Data Folder you must either create it or run Sojurn. Sojurn will make these for you once you run the program.

