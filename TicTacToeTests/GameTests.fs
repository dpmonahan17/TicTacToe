namespace TicTacToeTests

    module GameModuleTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    

        type fakeGameUtilities(tieResult:bool, winResult:bool) = class
            let mutable tieCallCount = ref 0
            let mutable winCallCount = ref 0
            let mutable tieResult = tieResult
            let mutable winResult = winResult
            
            member x.TieCallCount:int ref = tieCallCount
            member x.WinCallCount:int ref = winCallCount
            member x.TieResult:bool = tieResult            
            member x.WinResult:bool = winResult

            interface IGameUtilities with
                member x.CheckForTie board =
                    incr tieCallCount
                    tieResult
                member x.CheckForWin board =
                    incr winCallCount
                    winResult
                member x.BuildBlankBoard = 
                    let grid:Grid =
                        {grid = []}
                    grid
                    
        end

        type fakeGameControls() = class
            
            let mutable callCount = ref 0
            let mutable callGetTurnCount = ref 0
                        
            member x.CallCount:int ref = callCount
            member x.CallGetTurnCount = callGetTurnCount

            interface IGameControls with
                            
                member x.PrintGameResults (results:GameResult) (player:Player) =
                    incr callCount
                    
                member x.GetTurn grid player=
                    incr callGetTurnCount
                    {grid = [
                        {Position = (Left, Top); Marked = No};
                        {Position = (Left, Center); Marked = No};
                        {Position = (Left, Bottom); Marked = X}]
                    }

        end


        let buiLdXTurn =
            let spaces = [
                    {Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = No}]
            let players = {PlayerX = {name = "John Doe"; mark = X};
                           PlayerO = {name = "John Did"; mark = O}}
            XTurn {grid = {grid = spaces}; players = players}
        
        let buiLdOTurn =
            let spaces = [
                    {Position = (Left, Top); Marked = No};
                    {Position = (Left, Center); Marked = No}]
            let players = {PlayerX = {name = "John Doe"; mark = X};
                           PlayerO = {name = "John Did"; mark = O}}
            OTurn {grid = {grid = spaces}; players = players}
        

        [<Test>]
        let ``Game should print game end results if game is completed`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)                    
            let board = 
                Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            let result = board |> game.printResults 
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should equal board

            fake.CallCount.Value |> should equal 1
        
        [<Test>]
        let ``Game should not print end game results if its X's turn`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)
            let board = buiLdXTurn
            
            let result = board |> game.printResults 
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal board
            | _ -> result |> should equal board

            fake.CallCount.Value |> should equal 0

        [<Test>]
        let ``Game should not print end game results if its O's turn`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)
            let board = buiLdOTurn
            
            let result = board |> game.printResults 
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal board
            | _ -> result |> should equal board

            fake.CallCount.Value |> should equal 0

        [<Test>]
        let ``Game CheckForTie step should return Completed if X Turn and game is tied`` () =
            let fakeUtils = fakeGameUtilities((true:bool),(false:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdXTurn
            
            let result = board |> game.CheckForTie
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal Tie
            | _ -> result |> should equal board

            fakeUtils.TieCallCount.Value |> should equal 1

        [<Test>]
        let ``Game CheckForTie step should return original board if game is not tied`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdOTurn
            
            let result = board |> game.CheckForTie
            match result with
            | Completed {player=player; result=resultData} -> resultData |> should equal Tie
            | _ -> result |> should equal board

            fakeUtils.TieCallCount.Value |> should equal 1

        [<Test>]
        let ``Game CheckForWin step should return original board if game is not Won`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdOTurn
            
            let result = board |> game.CheckForWin
            match result with
            | Completed {player=player; result=resultData} -> 
                resultData |> should equal Tie
                player.mark |> should equal X
            | _ -> result |> should equal board

            fakeUtils.WinCallCount.Value |> should equal 1

        [<Test>]
        let ``Game CheckForWin step should return Completed With win and player if game is Won`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(true:bool))
            let game = Game(fakeGameControls(), fakeUtils)
            let board = buiLdXTurn
            
            let result = board |> game.CheckForWin
            match result with
            | Completed {player=player; result=resultData} -> 
                resultData |> should equal Win
                player.mark |> should equal X
            | _ -> board |> should equal board

            fakeUtils.WinCallCount.Value |> should equal 1

        [<Test>]
        let ``Game PerformTurn step should return Board with updated Grid if player turn`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let fakeControls = fakeGameControls()
            let game = Game(fakeControls, fakeUtils)
            let board = buiLdXTurn
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 1

        [<Test>]
        let ``Game PerformTurn step should return Board if already Completed`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let fakeControls = fakeGameControls()
            let game = Game(fakeControls, fakeUtils)
            let board = Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 0

        [<Test>]
        let ``Game NextTurn step call the other steps`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let fakeControls = fakeGameControls()
            let game = Game(fakeControls, fakeUtils)
            let board = Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            let result = board |> game.PerformTurn
            match result with
            | Completed {player=player; result=resultData} -> result |> should equal board
            | _ -> result |> should not' (equal board)

            fakeControls.CallGetTurnCount.Value |> should equal 0


        [<Test>]
        let ``Scorer does things`` () =
            let fakeUtils = fakeGameUtilities((false:bool),(false:bool))
            let scorer = GameScorer(fakeUtils)
            4 |> should equal 4

        [<Test>]
        let ``GetPossibleMoves returns grid of valid spaces`` () =
            let utilities = GameUtilities()
            let grid :Grid= {grid = [
                {Position = (Left, Top); Marked = No};
                {Position = (Left, Center); Marked = No};
                {Position = (Left, Bottom); Marked = X};
                {Position = (Middle, Top); Marked = O};
                {Position = (Middle, Center); Marked = X};
                {Position = (Middle, Bottom); Marked = O};
                {Position = (Right, Top); Marked = X};
                {Position = (Right, Center); Marked = O};
                {Position = (Right, Bottom); Marked = X};
            ]}
            
            let newGrid : Grid = utilities.GetPossibleMoves grid
            
            newGrid.grid.Length |> should equal 2
            (newGrid.grid |> List.find(fun i -> i.Position = (Left, Top))).Marked |> should equal No
            (newGrid.grid |> List.find(fun i -> i.Position = (Left, Center))).Marked |> should equal No


