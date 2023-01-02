
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
            line = input(">> ")
            parts = line.split(" ")
            command = parts[0]
            args = parts[1:]

            if command in self.commands :
                self.commands[command](*args)
            else :
                print(f"Unknown command: {command}")

    def quit ( self ) :
        import sys
        sys.exit(0)

if __name__ == "__main__" :
    cli = CLI()
    cli.run()
