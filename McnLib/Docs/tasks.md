# McnLib

## Tasks

- [ ] Create a cache/AST object
- [ ] Load from file
- [ ] Save to file
- [ ] Generate note id index file & query by node id

## Design Desicion Record

- 2022-05-25 Remove parsing note sections  
  Reason:  
  Child structures (aka. note sections) are not used as a search unit and
  unnecessary to display a note's structure, so there is no use case for note
  sections.
