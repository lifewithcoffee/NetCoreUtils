# About RCM

RCM stands for Robocopy Configuration Manager

## ToDo

### Current

- When add/rename a group, the name shall be unique

### Waiting

- Need a "simulating" flag in the config file. When this flag is on, the actual robocopy command won't be executed.
- Before doing backup, need to validate the configuration first

### More Thoughts

- Need a RestoreCommand, which uses the same backup config but sync from the target to the source
- Group copy?
- Support alias for source/target pathes
  - Use path alias to update backup items

## Done

## Releases

## v1.0.0

Implement the following commands:

- add `group-name`
- list
- list `group-name`
- remove `group-name`
- remove `group-name` `item-name`
- rename `group-name`
- rename `group-name` `item-name`
- backup `group-name`
- update `group-name` `item-name` `source-path` `target-path`
