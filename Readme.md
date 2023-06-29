### It is my personal demo project was created with next technologies:

- C# 
- .NET(Standart 2.0, Core 7.0)
- Xamarin.Forms,
- ASP.NET Core,
- Prism.Unit,
- Microsoft.Datasync,
- SQLite(code-based) and others

- git(?)

Here developed application for notes. Not multi-usered but with synchronization function via few instance of app

### Functions of application:

- directions creation and removing
- records creation, editing and removing
- everyminutes avtosaving of record
- syncroniazation with server`s DB

### Functions of server

- syncroniazation betwen server`s DB and local DB

### Features

- multiplatform(tested on Droid and UWP, not optimized for iOS)

### Screens of application:

Add diretion on UWP
![add direction on uwp screen](images/AddDirectionUWP.png)

List of directions
![list of directions on uwp screen](images/DiretionsPageUWP.png)

Write record
![add/edit record on uwp screen](images/AddEditPageUWP.png)

List of records
![list of records on uwp screen](images/RecordsPageUWP.png)

Confirm and run synchronization
![confirm and run synchronization popup on uwp](images/ConfirmSyncUWP.png)

Add direction on mobile
![add direction on mobile screen](images/AddDirectionMobile.jpg)

Run syncronization on mobile also
![confirm and run synchronization popup on mobile](images/ConfirmSyncMobile.jpg)

List of directions
![list of directions on mobile screen](images/DirectionsPageMobile.jpg)

Write record
![add/edit record on mobile screen](images/AddEditPageMobile.jpg)

List of records
![list of records on mobile screen](images/RecordsPageMobile.jpg)

### Negative moments:
- I had to build DLLs of datasync client and directly refers to them instead nuget packages(official releases restrict connections via https only)