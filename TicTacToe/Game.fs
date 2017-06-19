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

type public GameControls() = class
    let PrintCurrentGrid grid =
        printf "grid"

    let PrintPlayerOptions grid =
        printf "options"

    let GetPlayerInput (grid:Grid) player =
        printf "get Player input"
        //IGameUtilities.updateBoard
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

type IGameUtilities =
    abstract member CheckForTie : grid:Grid -> bool
    abstract member CheckForWin : grid:Grid -> bool
    abstract member BuildBlankBoard : Grid


type public GameUtilities() = class

    member x.UpdateGrid (grid:Grid) player selection =
        grid.grid |> List.find(fun i -> i.Position = selection)
           
    member x.hasThreeInARow (spaces:List<Space>) = 
        No

    member x.updateBoard (spaces:List<Space>) =
        {grid = spaces} : Grid //Circular Reference Need to implement check against spaces and use real check everytime, don't store in flags.


    interface IGameUtilities with
        member x.CheckForTie (grid:Grid) =
            false
        member x.CheckForWin (grid:Grid) =
            false
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
        

    member x.checkForTie board =
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
        
    member x.checkForWin board =
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

    member x.performTurn board =
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
            board |> x.performTurn |> x.checkForWin |> x.checkForTie |> x.nextTurn
        | OTurn {grid=oGrid; players=players} ->
            board |> x.performTurn |> x.checkForWin |> x.checkForTie |> x.nextTurn
        | Completed _ ->
            board |> x.printResults

    member x.start() =
        printf "started"

end


module Program =
    
    let main =
        printf "game"