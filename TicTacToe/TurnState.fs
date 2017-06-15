namespace TicTacToe

open TicTacToe

type public TurnState(playerOne: Player, playerTwo :Player) = class

    let playerOne = playerOne
    let playerTwo = playerTwo

    member x.PlayerOne = playerOne
    member x.PlayerTwo = playerTwo

end