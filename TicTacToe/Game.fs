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


type IGameControls() = class

    static member PrintCurrentGrid grid =
        printf "grid"

    static member PrintPlayerOptions grid =
        printf "options"

    static member GetPlayerInput grid player =
        printf "get Player input"
        //IGameUtilities.updateBoard
        grid

    member x.PrintGameResults (results:GameResult) (player:Player) =
        match results with
        | Win -> printf "Player %s has won!" player.name
        | Tie -> printf "Game has ended in a tie"

    static member GetTurn grid player=
        IGameControls.PrintCurrentGrid grid
        IGameControls.PrintPlayerOptions grid
        IGameControls.GetPlayerInput grid player

end

type IGameUtilities() = class

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
        {grid = spaces}

end

type Game(gameControls :IGameControls, gameUtils: IGameUtilities) = class

    member x.gameControls = gameControls
    member x.gameUtilities = gameUtils

    static member printResults board =
        match board with
        | XTurn _ ->
            board
        | OTurn _ ->
            board
        | Completed {player=player; result=result} ->
            gameControls  PrintGameResults result player
            board
        

    static member checkForTie board =
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
        
    static member checkForWin board =
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

    static member performTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            XTurn {grid = IGameControls.GetTurn xGrid players.PlayerX; players = players}
        | OTurn {grid=oGrid; players=players} ->
            OTurn {grid = IGameControls.GetTurn oGrid players.PlayerO; players = players}
        | Completed _ ->
            board

    static member nextTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            board |> Game.performTurn |> Game.checkForWin |> Game.checkForTie |> Game.nextTurn
        | OTurn {grid=oGrid; players=players} ->
            board |> Game.performTurn |> Game.checkForWin |> Game.checkForTie |> Game.nextTurn
        | Completed _ ->
            board |> Game.printResults

    static member start() =
        printf "started"

end


module Program =
    
    let main =
        printf "game"