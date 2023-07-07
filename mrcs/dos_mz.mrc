# OFFSET | SIZE | NAME       | CONSTRAINTS
    0000 |    2 | e_magic    | ='MZ' # MZ header signature
    0002 |    2 | e_cblp     |       # Bytes on last page of file
    0004 |    2 | e_cp       |       # Pages in file
    0006 |    2 | e_crlc     |       # Relocations
    0008 |    2 | e_cparhdr  |       # Size of header in paragraphs
    000A |    2 | e_minalloc |       # Minimum extra paragraphs needed
    000C |    2 | e_maxalloc |       # Maximum extra paragraphs needed
    000E |    2 | e_ss       |       # Initial (relative) SS value
    0010 |    2 | e_sp       |       # Initial SP value
    0012 |    2 | e_csum     |       # Checksum
    0014 |    2 | e_ip       |       # Initial IP value
    0016 |    2 | e_cs       |       # Initial (relative) CS value
    0018 |    2 | e_lfarlc   |       # File address of relocation table
    001A |    2 | e_ovno     |       # Overlay number
    001C |    8 | e_res      |       # Reserved words
    0024 |    2 | e_oemid    |       # OEM identifier (for e_oeminfo)
    0026 |    2 | e_oeminfo  |       # OEM information; e_oemid specific
    0028 |   20 | e_res2     |       # Reserved words
    003C |    4 | e_lfanew   |       # File address of new exe header
