namespace TicTacToe

type MoveData = {
    grid : Grid
    moves : Space List
    player : Player
    score : int
    depth : int
}

type ScoringState =
| NextMove of MoveData
| FinalMove of MoveData
| BestMove of Space

type IAlgorithm = 
    abstract member GetBestScore : grid:Grid -> player:Player -> depth:int -> maximizingPlayer:bool -> int

type Algorithm(gameUtils : IGameUtilities) = class

    member x.gameUtils = gameUtils
    
    member x.GetOppenent (player:Player) =
        match player.Mark with
        | X -> {Name = "o"; Mark = O}
        | O -> {Name = "x"; Mark = X}

    member x.GetScore (grid:Grid) (player:Player) (depth:int) : int =
        match player.Mark with
        | X -> 
            if x.gameUtils.CheckForXWin grid then 10 - depth
            else if x.gameUtils.CheckForOWin grid then -10 + depth
            else 0
        | O -> 
            if x.gameUtils.CheckForXWin grid then -10 + depth
            else if x.gameUtils.CheckForOWin grid then 10 - depth
            else 0

    member x.GetBestScore (grid:Grid) (player:Player) (depth:int) (maximizingPlayer:bool) =
        (x :> IAlgorithm).GetBestScore (grid:Grid) (player:Player) (depth:int) (maximizingPlayer:bool)

    member x.Max (grid:Grid) (player:Player) (depth:int) =
        let bestScore = -1000
        let maxScore = 
            (x.gameUtils.GetPossibleMoves grid) |> List.map(fun move ->
                x.GetBestScore (x.gameUtils.UpdateGrid grid (x.gameUtils.MarkMove move player)) player (depth + 1) false) |> List.max
        printf "Max Move Score = %i and depth is = %i\n" maxScore depth
        printf "Current Player is %s\n" player.Name
        max bestScore maxScore
    member x.Min(grid:Grid) (player:Player) (depth:int)= 
        let bestScore = 1000
        let minScore = 
            (x.gameUtils.GetPossibleMoves grid) |> List.map(fun move ->
                x.GetBestScore (x.gameUtils.UpdateGrid grid (x.gameUtils.MarkMove move player)) player (depth + 1) true) |> List.min
        printf "Min Move Score = %i and depth is = %i\n" minScore depth
        printf "Current Player is %s\n" player.Name
        min bestScore minScore

    interface IAlgorithm with

        member x.GetBestScore (grid:Grid) (player:Player) (depth:int) (currentPlayer:bool) =
            if x.gameUtils.CheckForWin grid || x.gameUtils.CheckForTie grid then
                x.GetScore grid player depth
            else
                match currentPlayer with
                | true -> x.Max grid player depth
                | false -> x.Min grid (x.GetOppenent player) depth
            
end



type IComputerPlayer = 
    abstract member GetNextMove : grid:Grid -> player:Player -> Space

type ComputerPlayer(gameUtils : IGameUtilities, algorithm : IAlgorithm) = class
    
    member x.GameUtilities = gameUtils
    member x.Calc = algorithm

    member x.GetNextMove (grid:Grid) (player:Player) =
        (x :> IComputerPlayer).GetNextMove grid player

    member x.GetMoveScore grid move player=
        let currentGrid = x.GameUtilities.UpdateGrid grid move
        let score = x.Calc.GetBestScore currentGrid player 0 true
        printf "Final Move Score is: %i\n" score
        score


    interface IComputerPlayer with

        member x.GetNextMove (grid:Grid) (player:Player) =
            let possibleMoves = x.GameUtilities.GetPossibleMoves grid
            let scoreList = possibleMoves |> List.map(fun i -> ((x.GetMoveScore grid i player), i))
            let bestMove = scoreList |> List.minBy(fun (x,y) -> x)
            let space = match bestMove with 
                        | (a,b) -> x.GameUtilities.MarkMove b player
            space

end
