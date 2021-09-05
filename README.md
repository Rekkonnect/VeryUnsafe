# VeryUnsafe
This librarty contains a collection of various dangerous operations. Using `unsafe` may be less than necessary in some cases, mind your steps.

## Disclaimer
Carefully read the doucmentation's remarks, for the operations are really unsafe in nature.

## Features
- `GetObjectHandleAddress`, to get the address of the type handle of an object
- `ChangeType`, to change the type handle of an object at runtime, without requiring reallocation

## Usages
- Pro-/demote objects at runtime
- Mimic another type (may or may not be heavily inspired by [Seb-stian/GetVoid](https://github.com/Seb-stian/GetVoid))
- Corrupt the managed heap in one more way

# Stay safe!
