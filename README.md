CustomAspNetIdentity
====================

Decoupling Asp.Net Identity 2.0 from Entity Framework

The implementation provides a decoupled Asp.Net Identity 2.0 framework from Entity Framework with custom implementation. The custom framework does not include support for Claims and External Logins, although these can be very easily added just just implementing appropriate IUser*Store in the UserStore class

