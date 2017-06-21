namespace TicTacToe


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
        printf "Test"

end


module Program =
    
    let main =
        let game = Game(GameControls(GameUtilities()), GameUtilities())
        game.start()