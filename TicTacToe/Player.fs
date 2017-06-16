namespace TicTacToe

open TicTacToe

type public Player(name: string, mark: char) = class

    member x.Name = name
    member x.Mark = mark

    member x.GetNextPlay =
        System.Console.ReadLine

end