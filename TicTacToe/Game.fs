namespace TicTacToe

open TicTacToe



type GameState =
    | XTurn
    | OTurn
    | GameOver


type public Game() = class

    let grid = Grid()

    member x.PlayerOne = Player("Player 1", 'x')
    member x.PlayerTwo = Player("Player 2", 'o')
    
    member x.GetGrid = grid


end

