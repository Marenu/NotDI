# NotDI: Not a Dependency Injector

A Proof of Concept for a library which combines Inversion of Control with a Service Locator pattern

## Idea

There are 2 reasons why I don't like Dependency Injectors and have created a Proof of Concept
* You have to pass the services as parameters, which means more writing effort
* The instances of the services are created by black magic
* If the services are accessed from outside, either the Dependency Injector must be initialized or all dependencies must be resolved manually

## Differences / Disadvantages

### Resolve everything at once

It is not possible to resolve a service line by line of code, because the resolver does not know the next service to be resolved (if available) and therefore the scoped instances cannot be shared with the next line. This can be avoided by either using the resolving method to resolve multiple objects at once or by opening a session of multiple lines of code containing the instances.

### Instance Storage

If the instance of the Services is created by the user instead of a Dependency Injector, the instances must be saved in a static variable to be able to transfer the instances between the resolving Sessions.

### Pull mapping information instead of pushing it

If a service is accessed from outside, it should still have the mapping information available. Therefore, when the services are resolved, the mapping information is searched for instead of passing it to the library as with Dependency Injection Libraries. 

## Future

* Integrating benchmarks
* Allow temporary changes to mapping information to facilitate testing
* Writing tests