namespace TicTacToe

type public Game(gameControls :IGameControls, gameUtils: IGameUtilities) = class

    member x.GameControls = gameControls
    member x.GameUtilities = gameUtils
    
    member x.setupOpponent (player:Player) =
        match player.Mark with
        | X -> {Name = "PC Player"; Mark = O}
        | O -> {Name = "PC Player"; Mark = X}


    member x.PrintResults board =
        match board with
        | XTurn _ ->
            board
        | OTurn _ ->
            board
        | Completed {grid = grid; player=player; result=result} ->
            x.GameControls.PrintGameResults grid  result player
            board
      
    member x.CheckForTie board =
        match board with 
        | XTurn {grid=xGrid; players=players} ->
            if x.GameUtilities.CheckForTie xGrid 
            then Completed {grid = xGrid; result = Tie; player = players.PlayerX}
            else board
        | OTurn {grid=oGrid; players=players} ->
            if x.GameUtilities.CheckForTie oGrid 
            then Completed {grid = oGrid; result = Tie; player = players.PlayerO}
            else board
        | Completed _ ->
            board
        
    member x.CheckForWin board =
        match board with 
        | XTurn {grid=xGrid; players=players} ->
            if x.GameUtilities.CheckForWin xGrid 
            then Completed {grid = xGrid; result = Win; player = players.PlayerX}
            else board
        | OTurn {grid=oGrid; players=players} ->
            if x.GameUtilities.CheckForWin oGrid 
            then Completed {grid = oGrid; result = Win; player = players.PlayerO}
            else board
        | Completed _ ->
            board

    member x.PerformTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            XTurn {grid = x.GameControls.GetTurn xGrid players.PlayerX; players = players}
        | OTurn {grid=oGrid; players=players} ->
            OTurn {grid = x.GameControls.GetTurn oGrid players.PlayerO; players = players}
        | Completed _ ->
            board

    member x.NextTurn board =
        match board with
        | XTurn {grid=xGrid; players=players} ->
            OTurn {grid = xGrid; players = players}
                |> x.PerformTurn |> x.CheckForWin |> x.CheckForTie |> x.NextTurn
        | OTurn {grid=oGrid; players=players} ->
            XTurn {grid = oGrid; players = players}
                |> x.PerformTurn |> x.CheckForWin |> x.CheckForTie |> x.NextTurn
        | Completed _ ->
            board |> x.PrintResults

    member x.Start() =
        let player1 = x.GameControls.GetPlayerData
        let player2 = x.setupOpponent player1

        match player1.Mark with
        | X -> OTurn {grid = x.GameUtilities.BuildBlankBoard; players = {PlayerX = player1; PlayerO = player2}} |> x.NextTurn
        | O -> XTurn {grid = x.GameUtilities.BuildBlankBoard; players = {PlayerX = player2; PlayerO = player1}} |> x.NextTurn

        x.Start()

end
