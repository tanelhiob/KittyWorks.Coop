# KittyWorks.Coop
Example functional architecture in C#

<img width="586" alt="image" src="https://github.com/tanelhiob/KittyWorks.Coop/assets/7113198/2b6d89db-9ef2-4cc8-a855-f34c5ad7ec5b">

Instead of injecting dependency classes into other classes, this architecture injects application IO functions into static business functions.
Doing so has multiple benefits
- DI validates compile time
- much simpler unit tests due to static business functions
- runtime performance boost due to static business functions
- 
Tradeoffs are
- large ApplicationContext class
- separate interface code for every business function
- use of unknown and unproved architecture

The trick here is to have one large ApplicationContext class that implements all the business function interfaces.
This way we can pass that large context to any business function and it cuts the slice it needs.
When unit testing, we can only implement the small slice the function uses, drastically reducing mocking code and making unit tests faster and easier to add.
Even though C# is a clumsy language for this sort of architecture, the end result is still rather robust and composable.
The main complexity is managing the large code size in ApplicationContext.
