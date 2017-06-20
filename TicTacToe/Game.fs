namespace TicTacToe


type Mark = X | O | No

type GameResult = Win | Tie

type Player = {name: string
               mark : Mark}

type Players = {PlayerX : Player
                PlayerO : Player}

type GridRow = Left | Middle | Right
type GridColumn = Top | Center | Bottom
type GridPosition = GridRow * GridColumn

type Space = {
    Position : GridPosition
    Marked : Mark
}

type Grid = {
    grid: Space List
}

type MoveData = {
    grid : Grid
    move : Space
    score : int
    depth : int
}

type ScoringState = 
    | NextMove of MoveData
    | XWinMove of MoveData
    | OWinMove of MoveData
    | TieMove of MoveData

type BoardData = {
    grid : Grid
    players : Players
}

type GameResultsData = {
    player : Player
    result : GameResult
}

type IBoardState = 
    | XTurn of BoardData
    | OTurn of BoardData
    | Completed of GameResultsData


type IGameControls = 
    abstract member PrintGameResults : results:GameResult -> player:Player -> unit
    abstract member GetTurn : grid:Grid -> player:Player -> Grid

type IGameUtilities =
    abstract member CheckForTie : grid:Grid -> bool
    abstract member CheckForWin : grid:Grid -> bool
    abstract member BuildBlankBoard : Grid
    abstract member GetPossibleMoves : grid:Grid -> Grid
    abstract member GetSpace : grid:Grid -> position:GridPosition -> Space
    abstract member GetRow : grid:Grid -> index:int -> Space List
    abstract member CheckForXWin : grid:Grid -> bool
    abstract member CheckForOWin : grid:Grid -> bool
    abstract member UpdateGrid : grid:Grid -> selection:Space -> Grid


type public GameUtilities() = class

    member x.UpdateGrid (grid:Grid) (selection:Space) =
        (x :> IGameUtilities).UpdateGrid grid selection
    member x.GetPossibleMoves (grid:Grid) : Grid =
        (x :> IGameUtilities).GetPossibleMoves grid
        
    member x.GetSpace (grid:Grid) (position:GridPosition) =
        (x :> IGameUtilities).GetSpace grid position


    member x.GetRow (grid:Grid) (index:int) =
        (x :> IGameUtilities).GetRow grid index

    member x.GetScoreForSpace (space:Space) =
        match space.Marked with
        | X -> 1
        | O -> -1
        | No -> 0

    member x.CheckBoardIsFull (grid:Grid) =
        (grid.grid |> List.filter(fun i -> i.Marked = No)).Length < 1
        
    member x.GetScoreForRow (spaces:Space List) =
        spaces |> List.sumBy(fun i -> x.GetScoreForSpace i)

    member x.CheckForXWin (grid:Grid) =
        (x :> IGameUtilities).CheckForXWin grid

    member x.CheckForOWin (grid:Grid) =
        (x :> IGameUtilities).CheckForOWin grid

    member x.CheckForWin (grid:Grid) =
        (x :> IGameUtilities).CheckForWin grid

    interface IGameUtilities with
        
        member x.UpdateGrid (grid:Grid) (selection:Space) =
            let position = selection.Position
            let newGrid = 
                grid.grid |> List.map(fun i ->
                    if i.Position = position 
                    then selection
                    else i
                    )
            {grid = newGrid} :Grid

        member x.GetSpace (grid:Grid) (position:GridPosition) =
            grid.grid |> List.find(fun i -> i.Position = position)

        member x.GetRow (grid:Grid) (index:int) =
            match index with
            | 1 -> [x.GetSpace grid (Left,Top); x.GetSpace grid (Left, Center); x.GetSpace grid (Left,Bottom)]
            | 2 -> [x.GetSpace grid (Middle,Top); x.GetSpace grid (Middle, Center); x.GetSpace grid (Middle,Bottom)]
            | 3 -> [x.GetSpace grid (Right,Top); x.GetSpace grid (Right, Center); x.GetSpace grid (Right,Bottom)]
            | 4 -> [x.GetSpace grid (Right,Top); x.GetSpace grid (Middle, Top); x.GetSpace grid (Left,Top)]
            | 5 -> [x.GetSpace grid (Right,Center); x.GetSpace grid (Middle, Center); x.GetSpace grid (Left,Center)]
            | 6 -> [x.GetSpace grid (Right,Bottom); x.GetSpace grid (Middle, Bottom); x.GetSpace grid (Left,Bottom)]
            | 7 -> [x.GetSpace grid (Right,Top); x.GetSpace grid (Middle, Center); x.GetSpace grid (Left,Bottom)]
            | 8 -> [x.GetSpace grid (Left,Top); x.GetSpace grid (Middle, Center); x.GetSpace grid (Right,Bottom)]
            
        member x.CheckForXWin (grid:Grid) =
            let rows : List<List<Space>> = 
                [1;2;3;4;5;6;7;8] |> List.map(fun i -> x.GetRow grid i)
            let result = 
                rows |> List.map(fun i -> x.GetScoreForRow i)
            result |> List.contains(3)

        member x.CheckForOWin (grid:Grid) =
            let rows : List<List<Space>> = 
                [1;2;3;4;5;6;7;8] |> List.map(fun i -> x.GetRow grid i)
            let result = 
                rows |> List.map(fun i -> x.GetScoreForRow i)
            result |> List.contains(-3)

        member x.CheckForWin (grid:Grid) =
            x.CheckForXWin grid || x.CheckForOWin grid

        member x.CheckForTie (grid:Grid) =
            x.CheckBoardIsFull grid && not (x.CheckForWin grid)

        member x.GetPossibleMoves (grid:Grid) = 
            let newGrid :List<Space> = 
                grid.grid |> List.filter(fun i -> i.Marked = No)
            {grid = newGrid} : Grid
        member x.BuildBlankBoard =
            let spaces = [
                {Position = (Left, Top); Marked = No};
                {Position = (Left, Center); Marked = No};
                {Position = (Left, Bottom); Marked = No};
                {Position = (Middle, Top); Marked = No};
                {Position = (Middle, Center); Marked = No};
                {Position = (Middle, Bottom); Marked = No};
                {Position = (Right, Top); Marked = No};
                {Position = (Right, Center); Marked = No};
                {Position = (Right, Bottom); Marked = No};
            ]
            {grid = spaces} :Grid
        
end

type public GameControls(gameUtils : IGameUtilities) = class

    let gameUtils = gameUtils

    let markToString (mark:Mark) =
        match mark with
        | X -> "x"
        | O -> "o"
        | No -> " "

    let getPositionPerIndex (index:int) =
        match index with
        | 1 -> (Left,Top)
        | 2 -> (Middle,Top)
        | 3 -> (Right,Top)
        | 4 -> (Left,Center)
        | 5 -> (Middle,Center)
        | 6 -> (Right,Center)
        | 7 -> (Left,Bottom)
        | 8 -> (Middle,Bottom)
        | 9 -> (Right,Bottom)

    let getMark (grid:Grid) (index:int) =
        match index with
        | 1 -> (gameUtils.GetSpace grid (Left,Top)).Marked |> markToString
        | 2 -> (gameUtils.GetSpace grid (Middle,Top)).Marked |> markToString
        | 3 -> (gameUtils.GetSpace grid (Right,Top)).Marked |> markToString
        | 4 -> (gameUtils.GetSpace grid (Left,Center)).Marked |> markToString
        | 5 -> (gameUtils.GetSpace grid (Middle,Center)).Marked |> markToString
        | 6 -> (gameUtils.GetSpace grid (Right,Center)).Marked |> markToString
        | 7 -> (gameUtils.GetSpace grid (Left,Bottom)).Marked |> markToString
        | 8 -> (gameUtils.GetSpace grid (Middle,Bottom)).Marked |> markToString
        | 9 -> (gameUtils.GetSpace grid (Right,Bottom)).Marked |> markToString

    let PrintCurrentGrid grid =
        printf "Board Index - Press Number for grid to place marker"
        printf " 1 | 2 | 3 \n"
        printf "===========\n"
        printf " 4 | 5 | 6 \n"
        printf "===========\n"
        printf " 7 | 8 | 9 \n\n\n"
        printf " %s | %s | %s \n" (getMark grid 1) (getMark grid 2) (getMark grid 3)
        printf "===========\n"
        printf " %s | %s | %s \n" (getMark grid 4) (getMark grid 5) (getMark grid 6)
        printf "===========\n"
        printf " %s | %s | %s \n" (getMark grid 7) (getMark grid 8) (getMark grid 9)

    let PrintPlayerOptions grid =
        printf "options"

    let GetPlayerInput (grid:Grid) player =
        printf "Please press number to select where to play"
        let selection = System.Console.ReadLine
        
        grid

    
    interface IGameControls with
    
    member x.PrintGameResults (results:GameResult) (player:Player) =
        match results with
        | Win -> printf "Player %s has won!" player.name
        | Tie -> printf "Game has ended in a tie"

    member x.GetTurn grid player=
        PrintCurrentGrid grid
        PrintPlayerOptions grid
        GetPlayerInput grid player

end

type IGameScorer = 
    abstract member GetBestMove : grid:Grid -> Grid
    abstract member GetMoveScore : grid:Grid -> move:Space -> int

type GameScorer(gameUtils : IGameUtilities) = class
    
    member x.GameUtilities = gameUtils

    member x.GetMoveScore (grid:Grid) (move:Space) =
        (x :> IGameScorer).GetMoveScore grid move
    
    member x.GetBestMove (grid:Grid) :Grid =
        (x :> IGameScorer).GetBestMove grid

    member x.GetAllMovesScores (grid:Grid) (moves:Grid) =
        moves.grid |> List.map(fun i -> ((x.GetMoveScore grid i), i))

    member x.CheckTie MoveState =
        match MoveState with
        | NextMove {grid = grid; move = move; score = score; depth = depth} -> 
            if x.GameUtilities.CheckForTie grid
            then TieMove {grid = grid; move = move; score = score; depth = depth} |> x.NextMove
            else MoveState
        | _ -> MoveState
        // | NextMove -> MoveState
        // | XWinMove -> MoveState
        // | OWinMove -> MoveState
        // | TieMove -> MoveState

    member x.CheckO MoveState = 
        match MoveState with
        | _ -> MoveState
        // | NextMove -> MoveState
        // | XWinMove -> MoveState
        // | OWinMove -> MoveState
        // | TieMove -> MoveState

    member x.CheckX MoveState =
        match MoveState with
        | _ -> MoveState
        // | NextMove -> MoveState
        // | XWinMove -> MoveState
        // | OWinMove -> MoveState
        // | TieMove -> MoveState

    member x.NextMove (MoveState:ScoringState) :ScoringState =
        match MoveState with
        | NextMove {grid = grid; move = move; score = score; depth = depth} ->  
            MoveState |> x.CheckTie |> x.CheckO |> x.CheckX |> x.NextMove
        | _ -> MoveState
        // | XWinMove -> MoveState
        // | OWinMove -> MoveState
        // | TieMove -> MoveState

    interface IGameScorer with
        member x.GetBestMove (grid:Grid) =
            let moves = x.GameUtilities.GetPossibleMoves grid
            let scores = x.GetAllMovesScores grid moves
            let bestMove = scores |> List.minBy(fun (x,y) -> x)
            let space = 
                match bestMove with
                | (x,y) -> y
            x.GameUtilities.UpdateGrid grid space

         member x.GetMoveScore (grid:Grid) (move:Space) =
            -10

end

type ComputerPlayer(gameScorer:IGameScorer) = class

    member x.GameScorer = gameScorer



end

type public Game(gameControls :IGameControls, gameUtils: IGameUtilities) = class

    member x.gameControls = gameControls
    member x.gameUtilities = gameUtils

    member x.printResults board =
        match board with
        | XTurn _ ->
            board
        | OTurn _ ->
            board
        | Completed {player=player; result=result} ->
            x.gameControls.PrintGameResults result player
            board
      
    member x.CheckForTie board =
        match board with 
        | XTurn {grid=xGrid; players=players} ->
            if x.gameUtilities.CheckForTie xGrid 
            then Completed {result = Tie; player = players.PlayerX}
            else board
        | OTurn {grid=oGrid; players=players} ->
            if x.gameUtilities.CheckForTie oGrid 
            then Completed {result = Tie; player = players.PlayerO}
            else board
        | Completed _ ->
            board
        
    member x.CheckForWin board =
        match board with 
        | XTurn {grid=xGrid; players=players} ->
            if x.gameUtilities.CheckForWin xGrid 
            then Completed {result = Win; player = players.PlayerX}
            else board
        | OTurn {grid=oGrid; players=players} ->
            if x.gameUtilities.CheckForWin oGrid 
            then Completed {result = Win; player = players.PlayerO}
            else board
        | Completed _ ->
            board

    member x.PerformTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            XTurn {grid = x.gameControls.GetTurn xGrid players.PlayerX; players = players}
        | OTurn {grid=oGrid; players=players} ->
            OTurn {grid = x.gameControls.GetTurn oGrid players.PlayerO; players = players}
        | Completed _ ->
            board

    member x.nextTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            board |> x.PerformTurn |> x.CheckForWin |> x.CheckForTie |> x.nextTurn
        | OTurn {grid=oGrid; players=players} ->
            board |> x.PerformTurn |> x.CheckForWin |> x.CheckForTie |> x.nextTurn
        | Completed _ ->
            board |> x.printResults

    member x.start() =
        printf "started"

end


module Program =
    
    let main =
        printf "game"