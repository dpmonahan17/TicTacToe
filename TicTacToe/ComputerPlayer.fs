namespace TicTacToe

module PCTools =
    let printPosition (position:GridPosition) =
    match position with
        | (Left, Top) -> printf "Position is Left, Top"
        | (Left, Center) -> printf "Position is Left, Center"
        | (Left, Bottom) -> printf "Position is Left, Bottom"
        | (Middle, Top) -> printf "Position is Middle, Top"
        | (Middle, Center) -> printf "Position is Middle, Center"
        | (Middle, Bottom) -> printf "Position is Middle, Bottom"
        | (Right, Top) -> printf "Position is Right, Top"
        | (Right, Center) -> printf "Position is Right, Center"
        | (Right, Bottom) -> printf "Position is Right, Bottom"

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

type IMoveCalculator = 
    abstract member GetBestScore : grid:Grid -> player:Player -> depth:int -> maximizingPlayer:bool -> int

type Algorithm(gameUtils : IGameUtilities) = class

    member x.gameUtils = gameUtils
    
    member x.GetOpponent (player:Player) =
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
        (x :> IMoveCalculator).GetBestScore (grid:Grid) (player:Player) (depth:int) (maximizingPlayer:bool)

    member x.Max (grid:Grid) (player:Player) (depth:int) =
        let bestScore = -1000
        let maxScore = 
            (x.gameUtils.GetPossibleMoves grid)
            |> List.map(fun move ->
                let markedMove = (x.gameUtils.MarkMove move player)
                x.GetBestScore (x.gameUtils.UpdateGrid grid markedMove) player (depth + 1) false 
                ) 
            |> List.max
        max bestScore maxScore

    member x.Min(grid:Grid) (player:Player) (depth:int)= 
        let bestScore = 1000
        let minScore = 
            (x.gameUtils.GetPossibleMoves grid)
            |> List.map(fun move ->
                let markedMove = (x.gameUtils.MarkMove move (x.GetOpponent player))
                x.GetBestScore (x.gameUtils.UpdateGrid grid markedMove) player (depth + 1) true
                )
            |> List.min
        min bestScore minScore

    interface IMoveCalculator with

        member x.GetBestScore (grid:Grid) (player:Player) (depth:int) (currentPlayer:bool) =
            if x.gameUtils.CheckForWin grid || x.gameUtils.CheckForTie grid then
                x.GetScore grid player depth
            else
                 match currentPlayer with
                 | true -> x.Max grid player depth
                 | false -> x.Min grid player depth

end



type IComputerPlayer = 
    abstract member GetNextMove : grid:Grid -> player:Player -> Space

type ComputerPlayer(gameUtils : IGameUtilities, algorithm : IMoveCalculator) = class
    
    member x.GameUtilities = gameUtils
    member x.Calc = algorithm

    member x.GetNextMove (grid:Grid) (player:Player) =
        (x :> IComputerPlayer).GetNextMove grid player

    member x.GetMoveScore grid move player=
        let currentGrid = x.GameUtilities.UpdateGrid grid move
        x.Calc.GetBestScore currentGrid player 0 false

    interface IComputerPlayer with

        member x.GetNextMove (grid:Grid) (player:Player) =
            let possibleMoves = x.GameUtilities.GetPossibleMoves grid
            let scoreList = possibleMoves |> List.map(fun i -> ((x.GetMoveScore grid (x.GameUtilities.MarkMove i player) player), i))
            let bestMove = scoreList |> List.maxBy(fun (x,y) -> x)
            let space = match bestMove with 
                        | (a,b) -> x.GameUtilities.MarkMove b player
            space

end
