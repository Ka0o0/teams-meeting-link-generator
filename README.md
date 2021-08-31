# Teams Meeting [Link] Generator (tmg)

This application allows anyone working with MS Teams to create meeting links that can be shared with colleagues and external partners.

```
tmg generate -s "banana phone meeting"
```

## Installation

Install the program anywhere in your PATH.

The program will look for any of the configuration files: `~/.tmg.json` or `/etc/tmg.json`.
It will try to load the configuration files in order and use the first one that it can parse. 
In case of a invalid JSON, access problems or any loading issues the program will try to use the next configuration file.
The config file needs to provide the app id and the tenant id (the MS Domain id).

```
{
        "appId": "....",
        "tenantId": "..."
}
```

## Usage

Follow the steps described in Installation.
Call `tmg -h` to show the help menu which will further guide you.
