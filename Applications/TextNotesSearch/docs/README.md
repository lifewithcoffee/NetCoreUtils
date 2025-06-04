# About TNS

TNS stands for Text Notes Search

## TODO

Working:

- Search: `_working_ find notes only by file title`
- Search: `_working_ refactoring in CommandService.cs`

---
More:
- Make up usage documents
- Change prompt to "notes :> ", "content :> " and "filename :> "
- Change result selection format from "{file_number}/{line_number}" to "{file_number} {line_number}"
- Change keyworld separator charactor for content search from ',' to space

## Releases

### v3.0 ~ working

- Find by file title only

Done:
- Upgrade to .net9
- Update to use gvim 9.1
- Able to use 'r' command in 'open' prompt mode

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
