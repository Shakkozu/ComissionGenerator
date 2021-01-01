# ComissionGenerator
This project purpose is to create open-source helpful Commission Generator.

# WPF 
The user will enter his company data, which will be stored locally as serialized json file, and loaded during application starts.
Application has built-in local database which stores information about clients (user can add/remove/load client from database by using buttons).
New entry is inserted into database, during commission generation.

In 'Settings' page user can choose template file, which will be edited using tags presented below.

After entering information about client, wares and company, user can generate new document.
If user is using template file, he can decide whether to replace values or full sentences (i.e. 'Email: example@google.com / example@google.com')

My first thought was to write this application in UWP (Universal Windows Platform), however, due to problems with file permissions i decided to port this app to WPF (Windows Presentation Foundation).

# ASP.NET MVC
I decided, to port this application also to ASP.NET MVC5 project.

During this port I decided to swtich from saving informations as files, to use database.

I decided to use SQLite, because this task doesn't need any more advanced database engine.


# Demo
Demo application is available on: https://commissiongeneratorshakkozu.azurewebsites.net/

During testing application please notice that database needs some time to 'wake up'. After few proceeded requests, database query execution time should be acceptable.


With project i also include prepared file template, which may be extended.

Tag system used within documents:

# Wares
<b>To Insert wares info into Table, Table have to has caption = 'WARES_TABLE'!!</b> <br/>
\<ItemId\> <br/>
\<ItemName\> <br/>
\<ItemQuantity\> - Acceptable formats ('x', 'xm')<br/> 
\<ItemPrice\> > - '$', 'PLN'<br/>
\<ItemTotalPrice\> <br/>

# Creator
\<CreatorInfo\> - "{CreatorFullName}\nEmail: {CreatorEmailAddress}\nPhone Number: {CreatorPhoneNumber}" </br>
\<CreatorName\> - "{CreatorFullName}" <br/>
\<CreatorEmail\> - "etc" <br/> 
\<CreatorPhoneNumber\> - "etc" <br/>
  
# Client
\<ClientInfo\> - "{FullName}\n{Address}\n{EmailAddress}\n{PhoneNumber}\n{footer}" (footer == {NIP} if client is company) <br/>
\<ClientAddress\> - "{Street}\n{PostalCode}, {City}" <br/>
\<ClientAddressCity\> <br/>
\<ClientAddressStreet\> <br/>
\<ClientAddressPostalCode\> <br/>
\<ClientName\> <br/>
\<ClientEmail\> <br/>
\<ClientPhoneNumber\> <br/>
\<ClientNIP\> <br/>

# Company
Same as Client tags +<br/>
\<CompanyREGON\>

# MISC
\<TodayDate\> </br>
\<TotalPrice\> Cost of all items from Wares Table<\br>
