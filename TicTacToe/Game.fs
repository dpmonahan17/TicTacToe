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

type GridStatus = Filled | NotFilled

type Space = {
    Position : GridPosition
    Marked : Mark
}

type Grid = {
    grid: Space List
}

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


module IGameControls =

    let PrintCurrentGrid grid =
        printf "grid"

    let PrintPlayerOptions grid =
        printf "options"

    let GetPlayerInput grid player =
        printf "get Player input"
        //IGameUtilities.updateBoa
        grid

    let PrintGameResults (results:GameResult) (player:Player) =
        match results with
        | Win -> printf "Player %s has won!" player.name
        | Tie -> printf "Game has ended in a tie"

    let GetTurn grid player=
        PrintCurrentGrid grid
        PrintPlayerOptions grid
        GetPlayerInput grid player

module IGameUtilities =

    let UpdateGrid (grid:Grid) player selection =
        grid.grid |> List.find(fun i -> i.Position = selection)


    let CheckForTie (grid:Grid) =
        false

    let CheckForWin (grid:Grid) =
        false
    let hasThreeInARow spaces = 
        No

    let updateBoard spaces =
        {grid = spaces} //Circular Reference Need to implement check against spaces and use real check everytime, don't store in flags.

    let buildBlankBoard =
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
        {grid = spaces; completedRow = No; fillStatus = NotFilled}


module Game =

    let printResults board =
        match board with
        | XTurn _ ->
            board
        | OTurn _ ->
            board
        | Completed {player=player; result=result} ->
            IGameControls.PrintGameResults result player
            board
        

    let checkForTie board =
        match board with 
        | XTurn {grid=xGrid; players=players} ->
            if IGameUtilities.CheckForTie xGrid 
            then Completed {result = Tie; player = players.PlayerX}
            else board
        | OTurn {grid=oGrid; players=players} ->
            if IGameUtilities.CheckForTie oGrid 
            then Completed {result = Tie; player = players.PlayerO}
            else board
        | Completed _ ->
            board
        
    let checkForWin board =
        match board with 
        | XTurn {grid=xGrid; players=players} ->
            if IGameUtilities.CheckForWin xGrid 
            then Completed {result = Win; player = players.PlayerX}
            else board
        | OTurn {grid=oGrid; players=players} ->
            if IGameUtilities.CheckForWin oGrid 
            then Completed {result = Win; player = players.PlayerO}
            else board
        | Completed _ ->
            board

    let performTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            XTurn {grid = IGameControls.GetTurn xGrid players.PlayerX; players = players}
        | OTurn {grid=oGrid; players=players} ->
            OTurn {grid = IGameControls.GetTurn oGrid players.PlayerO; players = players}
        | Completed _ ->
            board

    let rec nextTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            board |> performTurn |> checkForWin |> checkForTie |> nextTurn
        | OTurn {grid=oGrid; players=players} ->
            board |> performTurn |> checkForWin |> checkForTie |> nextTurn
        | Completed _ ->
            board |> printResults

    let start() =
        printf "started"


module Program =
    
    let main =
        printf "game"