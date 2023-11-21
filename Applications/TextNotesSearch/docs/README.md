# About TNS

TNS stands for Text Notes Search

## TODO

- Change prompt to "notes :> ", "content :> " and "filename :> "
- Change result selection format from "{file_number}/{line_number}" to "{file_number} {line_number}"
- Change keyworld separator charactor for content search from ',' to space

## Releases

### v3.0.0-working

- Upgrade to .net7

### v2.1.0

- Type none-numeric string to do a new search in the "open" mode of "mcn notes"
- Add file name filter support when do "mcn notes" search

### v2.0.0

- Upgrade to .net6
- Add note piece search by subcommand: mcn notes
- Add alias for "content" and "name" subcommands

### v1.0.0

Implement the following commands:

- name "keyword1,keyworld2"
- content "keyword1,keyworld2"
