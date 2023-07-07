
class CLI :

    def __init__ ( self ) :
        self.commands = {}
        self.add_command("quit", self.quit)

    def add_command ( self , name , func ) :
        if name in self.commands :
            print(f"Warning: Overwriting command {name}")
        self.commands[name] = func

    def run ( self ) :
        while True :
            try :
                line = input(">> ")
            except KeyboardInterrupt:
                print("")
                self.quit()
            parts = line.split(" ")
            command = parts[0]
            args = " ".join(parts[1:])

            if command in self.commands :
                self.commands[command](args)
            else :
                print(f"Unknown command: {command}")

    def quit ( args ) :
        import sys
        sys.exit(0)

def read_mrc ( filename ) :
    with open(filename, "rb") as f :
        data = f.read()
    print(f"Read {len(data)} bytes from {filename}")
    return data

def hex_dump_file ( filename ) :
    data = read_mrc(filename)
    hex_dump(data, 0, 0x100)

def hex_dump ( data, offset=0, length=None, width=32 ) :
    """
        address | hex dump, bars every 4 bytes | ascii dump (non-printable chars replaced with dots)
    """
    if length is not None :
        data = data[offset:offset+length]
    for i in range(0, len(data), width) :
        hexdump = ""
        asciidump = ""
        for j in range(width) :
            if i+j < len(data) :
                hexdump += f"{data[i+j]:02x} "
                if j % 4 == 3 :
                    hexdump += "| "
                if data[i+j] < 32 or data[i+j] > 126 :
                    asciidump += "."
                else :
                    asciidump += chr(data[i+j])
            else :
                hexdump += "   "
                if j % 4 == 3 :
                    hexdump += "| "
                asciidump += " "
        print(f"{i:08x} | {hexdump}{asciidump}")

if __name__ == "__main__" :
    cli = CLI()
    cli.add_command("read", read_mrc)
    cli.add_command("hexfile", hex_dump_file)
    cli.run()
