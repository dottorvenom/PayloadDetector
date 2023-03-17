# PayloadDetector

First draft (v. 0.1) of the tool designed for blueteams to facilitate the detection 
of payloads loaded into endpoints by the attacker.

It consists of an agent that will run as a service with system account privileges.
The agent creates a logsource where it records and stores the payloads captured on file creation, modification and deletion.

The data is sent to a command and control to be then analyzed.

There is a keylogging routine to be enabled as needed.

The agent functions are configurable through a configuration file.
#### The configuration file must be placed in the executable folder

The agent code is VB.NET while the command and control is PHP with mysql database

#### To remember is simply a first version of testing
Input checks and validations with code optimization are still to be implemented