namespace TicTacToe

type IComputerPlayer = 
    abstract member GetBestMove : grid:Grid -> Grid
    abstract member GetMoveScore : grid:Grid -> move:Space -> int

type ComputerPlayer(gameUtils : IGameUtilities) = class
    
    member x.GameUtilities = gameUtils

    member x.GetMoveScore (grid:Grid) (move:Space) =
        (x :> IComputerPlayer).GetMoveScore grid move
    
    member x.GetBestMove (grid:Grid) :Grid =
        (x :> IComputerPlayer).GetBestMove grid

    member x.GetAllMovesScores (grid:Grid) (moves:Grid) =
        moves.grid |> List.map(fun i -> ((x.GetMoveScore grid i), i))

    member x.CheckTie MoveState =
        match MoveState with
        | NextMove {grid = grid; move = move; score = score; depth = depth} -> 
            if x.GameUtilities.CheckForTie grid
            then EndMove {grid = grid; move = move; score = (0 + depth); depth = depth}
            else MoveState
        | _ -> MoveState

    member x.CheckO MoveState = 
        match MoveState with
        | _ -> MoveState

    member x.CheckX MoveState =
        match MoveState with
        | _ -> MoveState

    member x.ToggleMove (move:Space) =
        match move.Marked with
        | X -> {Position = move.Position; Marked = O}
        | O -> {Position = move.Position; Marked = X}
        | _ -> {Position = move.Position; Marked = X}

    member x.IncrementDepth (depth:int) = 
        depth + 1

    member x.PerformMove MoveState =
        match MoveState with
        | NextMove {grid = grid; move = move; score = score; depth = depth} -> 
            x.GameUtilities.UpdateGrid grid move
            x.GameUtilities.GetPossibleMoves grid
            NextMove {grid = grid; move = x.ToggleMove move; score = score; depth = x.IncrementDepth depth}
        | _ -> MoveState

    member x.NextMove (MoveState:ScoringState) :ScoringState =
        match MoveState with
        | NextMove {grid = grid; move = move; score = score; depth = depth} ->  
            MoveState |> x.CheckTie |> x.CheckO |> x.CheckX |> x.PerformMove |> x.NextMove
        | _ -> MoveState

    interface IComputerPlayer with
        member x.GetBestMove (grid:Grid) =
            let moves = x.GameUtilities.GetPossibleMoves grid
            let scores = x.GetAllMovesScores grid moves
            let bestMove = scores |> List.minBy(fun (x,y) -> x)
            let space = 
                match bestMove with
                | (x,y) -> y
            x.GameUtilities.UpdateGrid grid space

         member x.GetMoveScore (grid:Grid) (move:Space) =
            let finalState = x.NextMove (NextMove {grid = grid; move = move; score = 0; depth = 0})
            match finalState with
            | EndMove {grid = grid; move = move; score = score; depth = depth}
                -> score
            | _ -> 0

end
