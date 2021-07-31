# Curl|sh

A C# script to provide a (basic) overview of the functions of a shell script. Avoids piping directly to sh.

## Description

Piping curl (or wget) directly to sh, especially as root, is generally considered bad practice. 
The usual advise is to read any scrips you plan on running.

This is generally infeasable for most users, as they either don't understand more complex scripts or don't have the time to read and analyze what a script does.

Curl|sh aims to provide a basic overview of a shell script, providing the user with a list of URLs the script references (includes comments), changes to /etc/apt/sources.list, and removed/added files/dirs.


## Getting Started

### Dependencies

* dotnet-sdk-5.0
* curl


### Installing

* Clone repo
* `dotnet build`

### Executing program

* `dotnet run`

To execute outside of build env: 
* `curlsh <script url>` 
```
-a / --args: Passes given arguments to script.
-d / --dontrun: Don't actually run the script, just run analysis. If this isn't set, you'll still be prompted before running the script.
```

## Authors

[lew](https://github.com/lewisdoesstuff)


## Version History

* 0.2
    * Added script running
* 0.1
    * Initial Release

## License

This project is licensed under the GPL v3 License - see the LICENSE.md file for details

## Inspiration
I've seen too many people complain about `curl | sudo sh` without providing an alternate solution to reading the script.
